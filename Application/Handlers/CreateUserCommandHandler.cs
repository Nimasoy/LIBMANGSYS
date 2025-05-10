using Application.Commands;
using Application.DTOs;
using Domain.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using MediatR;
using AutoMapper;
using Application.Services;

namespace Application.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly UserService _userService;

        public CreateUserCommandHandler(UserService userService)
        {
            _userService = userService;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            return await _userService.CreateUserAsync(request);
        }
    }


}
