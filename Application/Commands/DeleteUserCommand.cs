﻿using MediatR;

namespace Application.Commands
{
    public class DeleteUserCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public DeleteUserCommand(int id) => Id = id;
    }

}
