﻿using Domain.Interfaces;
using Application.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using Application.Interfaces;
using AutoMapper;
using Domain.Enums;
using Domain.Entities;

namespace Application.Handlers
{
    public class ReserveBookCommandHandler : IRequestHandler<ReserveBookCommand, Unit>
    {
        private readonly IBookService _bookService;

        public ReserveBookCommandHandler(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<Unit> Handle(ReserveBookCommand request, CancellationToken cancellationToken)
        {
            await _bookService.ReserveBookAsync(request);
            return Unit.Value;
        }
    }


}