using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pigeon.Classes
{
    internal class Slip
    {
        public DateOnly TrxDate { get; set; }
        public TimeOnly TrxTime { get; set; }
        public DateOnly CutoffDate { get; set; }
        public decimal Amount { get; set; }
        public string? Store { get; set; }
    }
}
