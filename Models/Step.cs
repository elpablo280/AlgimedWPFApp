using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgimedWPFApp.Models
{
    public class Step
    {
        public int ID { get; set; }
        public int ModeId { get; set; }
        public Mode Mode { get; set; }
        public TimeSpan Timer { get; set; }
        public string Destination { get; set; }
        public int Speed { get; set; }
        public string Type { get; set; }
        public double Volume { get; set; }
    }
}
