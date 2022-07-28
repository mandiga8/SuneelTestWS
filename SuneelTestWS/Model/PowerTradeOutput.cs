using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuneelTestWS.Model
{
    public class PowerTradeOutput
    {
        public DateTime TradeDate { get; set; }
        public DateTime TimeOfTrade { get; set; }
        public double AggVolume { get; set; }
    }
}
