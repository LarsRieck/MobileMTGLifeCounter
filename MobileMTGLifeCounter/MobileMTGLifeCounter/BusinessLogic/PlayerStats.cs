using System;
using System.Collections.Generic;
using System.Text;

namespace MobileMTGLifeCounter.BusinessLogic
{
    public class PlayerStats
    {
        public string Player { get; set; }
        public int LifePoints { get; set; } = 40;
        public TimeSpan Countdown { get; set; } = new TimeSpan(1, 0, 0);

    }
}
