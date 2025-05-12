using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Tags
{
    public class DeleteTagCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public DeleteTagCommand(int id) => Id = id;
    }



}
