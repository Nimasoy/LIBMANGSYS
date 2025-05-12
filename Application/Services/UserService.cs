using Application.Commands.Login;
using Application.Commands.Users;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Polly.Registry;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepo;
    private readonly ILendingRepository _lendingRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly ILogger<UserService> _logger;
    private readonly ICacheService _cache;
    private readonly ResiliencePipelineProvider<string> _pipelineProvider;

    public UserService(
        IUserRepository userRepo,
        ILendingRepository lendingRepo,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IPasswordHasher<User> passwordHasher,
        ITokenService tokenService,
        ILogger<UserService> logger,
        ICacheService cache,
        ResiliencePipelineProvider<string> pipelineProvider)
    {
        _userRepo = userRepo;
        _lendingRepo = lendingRepo;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _logger = logger;
        _cache = cache;
        _pipelineProvider = pipelineProvider;
    }

    public async Task<string> LoginAsync(CreateLoginCommand request)
    {
        _logger.LogInformation("Login attempt for: {Email}", request.Email);
        var user = await _userRepo.GetByEmailAsync(request.Email);
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
        var pipeline = _pipelineProvider.GetPipeline("read-pipeline");
        return await pipeline.ExecuteAsync(async _ =>
        {
            const string key = "user_list";
            var cached = await _cache.GetAsync<IEnumerable<UserDto>>(key);
            if (cached is not null)
            {
                _logger.LogInformation("Users returned from cache");
                return cached;
            }

            var users = await _userRepo.GetUsersAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        });
    }

    public async Task<UserDto> GetUserByIdAsync(int id)
    {
        var pipeline = _pipelineProvider.GetPipeline("read-pipeline");
        return await pipeline.ExecuteAsync(async _ =>
        {
            var key = $"user_{id}";
            var cached = await _cache.GetAsync<UserDto>(key);
            if (cached is not null)
            {
                _logger.LogInformation("User {Id} returned from cache", id);
                return cached;
            }

            var user = await _userRepo.GetUserByIdAsync(id);
            if (user == null) throw new Exception("User not found");
            var dto = _mapper.Map<UserDto>(user);
            await _cache.SetAsync(key, dto);
            return dto;
        });
    }

    public async Task<UserDto> CreateUserAsync(CreateUserCommand request)
    {
        _logger.LogInformation("User registration attempt: {Email}", request.Email);
        if (await _userRepo.GetByEmailAsync(request.Email) is not null)
        {
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
        await _userRepo.AddUserAsync(user);
        await _unitOfWork.SaveChangesAsync();

        await _cache.RemoveAsync("user_list");
        _logger.LogInformation("User registered: {Email}", user.Email);
        return _mapper.Map<UserDto>(user);
    }

    public async Task UpdateUserAsync(UpdateUserCommand request)
    {
        var user = await _userRepo.GetUserByIdAsync(request.Id);
        if (user == null) throw new Exception("User not found.");
        user.UserName = request.UserName;
        user.Email = request.Email;
        await _userRepo.UpdateUserAsync(user);
        await _unitOfWork.SaveChangesAsync();
        await _cache.RemoveAsync($"user_{request.Id}");
        await _cache.RemoveAsync("user_list");
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _userRepo.GetUserByIdAsync(id);
        if (user == null) throw new Exception("User not found.");
        await _userRepo.DeleteUserAsync(user);
        await _unitOfWork.SaveChangesAsync();
        await _cache.RemoveAsync($"user_{id}");
        await _cache.RemoveAsync("user_list");
    }
    public async Task<IEnumerable<BorrowingHistoryDto>> GetUserBorrowingHistoryAsync(int userId)
    {
        var history = await _userRepo.GetUserBorrowingHistoryAsync(userId);
        return _mapper.Map<IEnumerable<BorrowingHistoryDto>>(history);
    }
    public async Task<IEnumerable<UserOverdueDto>> GetUserOverdueBooksAsync(int userId)
    {
        var lendings = await _lendingRepo.GetUserOverdueLendingsAsync(userId);
        return _mapper.Map<IEnumerable<UserOverdueDto>>(lendings);
    }


}
