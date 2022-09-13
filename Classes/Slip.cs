using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pigeon.Classes
{
    internal class Slip
    {
        public DateOnly date { get; set; }
        public TimeOnly time { get; set; }
        public decimal amount { get; set; }
        public string? store { get; set; }
    }
}
