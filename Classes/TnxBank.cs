using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pigeon.Classes
{
    internal class TnxBank
    {
        public DateTime TnxDateTime { get; set; }
        public DateOnly CutoffDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public string TnxCCY { get; set; }
        public string RefPrimary { get; set; }
        public string SettleStatus { get; set; }
        public string SRCBank { get; set; }
        public string InterXBank { get; set; }
        public string? Store { get; set; }
    }
}
