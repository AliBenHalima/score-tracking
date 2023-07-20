using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Models
{
    public class Base
    {
        public int Id { get; set; }
        public DateTimeOffset Created { get; set; } = DateTime.UtcNow;
        public DateTimeOffset Updated { get; set; } = DateTime.UtcNow;
    }
}
