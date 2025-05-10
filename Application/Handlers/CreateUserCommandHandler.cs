using Application.Commands;
using Application.DTOs;
using Domain.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using MediatR;
using AutoMapper;
using Application.Interfaces;

namespace Application.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IUserService _userService;

        public CreateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            return await _userService.CreateUserAsync(request);
        }
    }


}
