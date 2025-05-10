using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UserOverdueDto
    {
        public int LendingId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public DateTime DueAt { get; set; }
        public int DaysOverdue => (int)(DateTime.UtcNow - DueAt).TotalDays;
    }
}
