using Microsoft.Extensions.Configuration;
using SuneelTestWS.Service;

namespace SuneelTestWS.Test
{

    public class PowerTradeAggregationTest
    {
        IPowerTradeAggregiation powerTrageAgg;
        public PowerTradeAggregationTest()
        {
            TestData testData = new TestData();
            powerTrageAgg = new PowerTradeAggregiation(testData.powerService, testData.configuration);
        }

        [Fact]
        public void GetAggregiatedResults_Test()
        {
            List<Model.PowerTradeOutput> oResult = powerTrageAgg.GetAggregiatedResults(DateTime.Now);

            //Verify if results exits
            Assert.NotNull(oResult);

            //Verify if number of rows are 24
            Assert.Equal(24, oResult.Count);

        }

        [Fact]
        public void WriteOutputToCSV_Test()
        {
            string stCSVFileName = "";
            string stResult = powerTrageAgg.WriteOutputToCSV(System.DateTime.Now, out stCSVFileName);

            //Verify if the status is Success
            Assert.Equal("Success", stResult);

            //Check if File got created
            Assert.True(System.IO.File.Exists(stCSVFileName));

        }
    }
}