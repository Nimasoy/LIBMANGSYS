using Application.Commands;
using Application.DTOs;
using Application.Interfaces;
using Application.Queries;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Any;

namespace Application.Services;

public class UserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly ILogger<UserService> _logger;
    private readonly ICacheService _cache;


    public UserService(IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher<User> passwordHasher, ITokenService tokenService, ILogger<UserService> logger, ICacheService cache)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _logger = logger;
        _cache = cache;
    }

    public async Task<string> LoginAsync(CreateLoginCommand request)
    {
        _logger.LogInformation("Login attempt for: {Email}", request.Email);
        var user = await _unitOfWork.Users.GetByEmailAsync(request.Email);
        if (user is null)
        {
            _logger.LogWarning("Login failed: user not found. Email={Email}", request.Email);
            throw new Exception("Invalid credentials");
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, request.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            _logger.LogWarning("Login failed: wrong password. Email={Email}", request.Email);
            throw new Exception("Invalid credentials");
        }
        _logger.LogInformation("Login success for: {Email}", request.Email);
        return _tokenService.GenerateToken(user, user.Role);
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        const string key = "user_list";
        var cached = await _cache.GetAsync<IEnumerable<UserDto>>(key);
        if (cached is not null)
        {
            _logger.LogInformation("Users returned from cache");
            return cached;
        }
        var users = await _unitOfWork.Users.GetUsersAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto> GetUserByIdAsync(int id)
    {
        var key = $"user_{id}";
        var cached = await _cache.GetAsync<UserDto>(key);
        if (cached is not null)
        {
            _logger.LogInformation("User {Id} returned from cache", id);
            return cached;
        }

        var user = await _unitOfWork.Users.GetUserByIdAsync(id);
        if (user == null) throw new Exception("User not found");
        var dto = _mapper.Map<UserDto>(user);
        await _cache.SetAsync(key, dto);
        return dto;
    }

    public async Task<UserDto> CreateUserAsync(CreateUserCommand request)
    {
        _logger.LogInformation("User registration attempt: {Email}", request.Email);
        if (await _unitOfWork.Users.GetByEmailAsync(request.Email) is not null) {
            _logger.LogWarning("User registration failed: Email exists. Email={Email}", request.Email);
            throw new Exception("Email already exists");
        }
        var user = new User
        {
            Email = request.Email,
            UserName = request.UserName,
            Role = request.Role
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        await _unitOfWork.Users.AddUserAsync(user);
        await _unitOfWork.SaveChangesAsync();

        await _cache.RemoveAsync("user_list");

        _logger.LogInformation("User registered: {Email}", user.Email);
        return _mapper.Map<UserDto>(user);
    }

    public async Task UpdateUserAsync(UpdateUserCommand request)
    {
        var user = await _unitOfWork.Users.GetUserByIdAsync(request.Id);
        if (user == null) throw new Exception("User not found.");
        user.UserName = request.UserName;
        user.Email = request.Email;
        await _unitOfWork.Users.UpdateUserAsync(user);
        await _unitOfWork.SaveChangesAsync();
        await _cache.RemoveAsync($"user_{request.Id}");
        await _cache.RemoveAsync("user_list");
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _unitOfWork.Users.GetUserByIdAsync(id);
        if (user == null) throw new Exception("User not found.");
        await _unitOfWork.Users.DeleteUserAsync(user);
        await _unitOfWork.SaveChangesAsync();
        await _cache.RemoveAsync($"user_{id}");
        await _cache.RemoveAsync("user_list");
    }
}
