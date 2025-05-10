using Application.DTOs;
using Application.Interfaces;
using Application.Queries;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class GetUserOverdueBooksQueryHandler : IRequestHandler<GetUserOverdueBooksQuery, IEnumerable<UserOverdueDto>>
    {
        private readonly IUserService _userService;

        public GetUserOverdueBooksQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IEnumerable<UserOverdueDto>> Handle(GetUserOverdueBooksQuery request, CancellationToken cancellationToken)
        {
            return await _userService.GetUserOverdueBooksAsync(request.UserId);
        }
    }

}
