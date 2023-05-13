using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pigeon.Classes
{
    internal class Result
    {
        public string Store { get; set; }
        public DateOnly CutoffDate { get; set; }
        public string? SRCBank { get; set; }
        public string Comparer1 { get; set; }
        public string? Comparer2 { get; set; }
        public decimal? Comparer1Amount { get; set; }
        public decimal? Comparer2Amount  { get; set; }
    }
}
