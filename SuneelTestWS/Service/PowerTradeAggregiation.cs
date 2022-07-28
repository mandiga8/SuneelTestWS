using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuneelTestWS.Service
{
    /// <summary>
    /// This Class has methods to talks to the PowerService Library and perform required operations
    /// </summary>
    public class PowerTradeAggregiation : IPowerTradeAggregiation
    {
        private readonly IPowerService powerService;
        private readonly IConfiguration configuration;
        private string stCSVFileNameStarts = "PowerPosition";

        public PowerTradeAggregiation(IPowerService powerService, IConfiguration configuration)
        {
            this.powerService = powerService;
            this.configuration = configuration;
        }


        /// <summary>
        /// This method Talks PowerService for a selected Date and returns the aggregiated data as a raw List. 
        /// This method converts the provided Date to local time before talking to PowerService
        /// </summary>
        /// <param name="dateTime">Date of Trade</param>
        /// <returns></returns>
        public List<Model.PowerTradeOutput> GetAggregiatedResults(DateTime dateTime)
        {
            //Convert the Datetime to Local Time
            DateTime dtLocalTime = dateTime.ToLocalTime();
            DateTime dStartTimeOfPeriod = new DateTime(dtLocalTime.Year, dtLocalTime.Month, dtLocalTime.Day-1, 23, 0, 0);

            //Create empty List
            List<Model.PowerTradeOutput> powerTradeOutput = new List<Model.PowerTradeOutput>();

            //Retrieve the Trades data from PowerService Library
            IEnumerable<PowerTrade> powerTrades = powerService.GetTrades(dtLocalTime);

            if (powerTrades != null)
            {
                //Loop through the data for 24 Periods
                for (int iPeriod = 0; iPeriod <= 23; iPeriod++)
                {
                    Model.PowerTradeOutput oAgg = new Model.PowerTradeOutput();
                    oAgg.TradeDate = dtLocalTime;
                    oAgg.TimeOfTrade = dStartTimeOfPeriod.AddHours(iPeriod);
                    
                    //Loop through the list of results produced in above results for current Period, from abouve loop
                    foreach (PowerTrade powerTrade in powerTrades)
                    {
                        //Add the Volume for samae Date and Period
                        oAgg.AggVolume += powerTrade.Periods[iPeriod].Volume;
                    }

                    //Add the aggregiated data to List
                    powerTradeOutput.Add(oAgg);
                }
            }

            //Return the final aggregiated data to List
            return powerTradeOutput;
        }

        /// <summary>
        /// This method Talks GetAggregiatedResults method to get the date and writes the data in CSV file, wehre the path and name was specified in the configuraiton
        /// </summary>
        /// <param name="dateTime">Date of Trade</param>
        /// <returns></returns>
        public string WriteOutputToCSV(DateTime dateTime, out string outputCSVFileName)
        {
            outputCSVFileName = System.IO.Path.Combine( configuration["OutputCSVPath"], $"{stCSVFileNameStarts}_{DateTime.Now.ToString("yyyyMMdd_HHmm")}.csv");
            try {
                //Retrieve the data from the GetAggregiatedResults method
                List<Model.PowerTradeOutput> powerTradeOutputs = GetAggregiatedResults(dateTime);


                //Generating the Data in the CSV Format
                StringBuilder sbOutput = new StringBuilder();
                sbOutput.AppendLine("Local Time,Volume");
                foreach (Model.PowerTradeOutput powerTradeOutput in powerTradeOutputs)
                {
                    sbOutput.AppendLine($"{powerTradeOutput.TimeOfTrade.ToString("HH:mm")},{Math.Round(powerTradeOutput.AggVolume, 0)}");
                }

                //Writing the data to File, to create CSV file
                System.IO.File.WriteAllText(outputCSVFileName, sbOutput.ToString());

                return "Success";
            }
            catch(Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
    }
}
