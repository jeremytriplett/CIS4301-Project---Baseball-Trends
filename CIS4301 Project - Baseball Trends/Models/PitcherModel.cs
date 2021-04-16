using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIS4301_Project___Baseball_Trends.Models
{
    public class PitcherModel
    {
        public string playerId { get; set; }
        public string yearId { get; set; }
        public int stint { get; set; }
        public string teamId { get; set; }
        public int wins { get; set; }
        public int losses { get; set; }
        public int ipOuts { get; set; }
        public double era { get; set; }
    }
}
