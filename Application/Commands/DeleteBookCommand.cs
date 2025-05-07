using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
    public class DeleteBookCommand : IRequest<Unit>
    {
        public int Id { get; }
        public DeleteBookCommand(int id)
        {
            Id = id;
        }
    }

}
