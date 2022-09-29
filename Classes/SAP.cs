using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pigeon.Classes
{
    internal class SAP
    {
        public string Assignment { get; set; }
        public string DocumentNo { get; set; }
        public string BusA { get; set; }
        public string Type { get; set; }
        public DateOnly DocDate { get; set; }
        public string PK { get; set; }
        public decimal AmountInLocalCur { get; set; }
        public string LCurr { get; set; }
        public string Text { get; set; }
        public string InterXBank { get; set; }
        public string? Store { get; set; }
    }
}
