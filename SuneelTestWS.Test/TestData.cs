using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuneelTestWS.Test
{
    internal class TestData
    {
        private Dictionary<string, string> myConfiguration = new Dictionary<string, string>
            {
                {"ServiceTriggerMinutes", "1"},
                {"OutputCSVPath", @"D:\Working\temp_files_test"},
            };


        public IConfigurationRoot configuration;
        public Services.PowerService powerService;

        public TestData()
        {
            //Initializing the mock iconfiguration 
            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            //Initialized the actual PowerService, as mocking this may take time
            powerService = new Services.PowerService();
        }
    }
}
