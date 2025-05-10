using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class OverdueDto
    {
        public int LendingId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public DateTime DueAt { get; set; }
        public int DaysOverdue => (int)(DateTime.UtcNow - DueAt).TotalDays;
    }
}

