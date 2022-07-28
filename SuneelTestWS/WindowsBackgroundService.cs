using Services;
using SuneelTestWS.Service;
using System.Text;

namespace SuneelTestWS
{
    public class WindowsBackgroundService : BackgroundService
    {
        private readonly ILogger<WindowsBackgroundService> _logger;
        private readonly IConfiguration configuration;
        private readonly IPowerTradeAggregiation powerTradeAggregiation;

        int iRunMinutes;
        int iMaxRetryCount = 10;
        public WindowsBackgroundService(ILogger<WindowsBackgroundService> logger, IConfiguration configuration, Service.IPowerTradeAggregiation powerTradeAggregiation)
        {
            _logger = logger;
            this.configuration = configuration;
            this.powerTradeAggregiation = powerTradeAggregiation;

            iRunMinutes = Convert.ToInt32(configuration["ServiceTriggerMinutes"]);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    //Calling the method that contains the code to generate the CSV files
                    TriggerTheTradeAggJob();

                    await Task.Delay((1000 * 1) * iRunMinutes, stoppingToken);


                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured during executing the Background Process. {ex.Message}");

                //Making sure the Windows Service is not breaking, due to this Exception, exiting with true
                Environment.Exit(1);
            }
        }

        //Seperated this code from ExecuteAsync Method
        private void TriggerTheTradeAggJob()
        {
            int iRetryCount = 0;
            bool bJobRunStatus = true;
            do
            {
                DateTime jobStarted = DateTime.Now;
                _logger.LogInformation($"Background process started at: {jobStarted} ...");

                if (iRetryCount > 0) _logger.LogInformation($"******** Re-running the job # {iRetryCount}, as pervious run failed...");

                try
                {
                    DateTime dateTime = DateTime.Now;


                    string stOutputFileName = "";
                    //Calling the PowerTrageAggregiation in Dependency Injection model
                    string stStatus = powerTradeAggregiation.WriteOutputToCSV(dateTime, out stOutputFileName);
                    if (stStatus != "Success")
                    {
                        bJobRunStatus = false;
                        _logger.LogError("Error while retrieving the data. " + stStatus);
                    }
                    else
                    {
                        bJobRunStatus = true;
                        _logger.LogInformation($"CSV File {stOutputFileName}, is generated with status {stStatus}");
                    }
                }
                catch (Exception ex)
                {
                    bJobRunStatus = false;
                    _logger.LogError($"Unhandled exception occured during job run. Details : {ex.Message}");
                }


                //Calculating the time taken and displaying in the log
                TimeSpan timeSpan = DateTime.Now - jobStarted;
                _logger.LogInformation($"Background process completed in {timeSpan} time.");

                if (!bJobRunStatus) iRetryCount++;

                if (iRetryCount > iMaxRetryCount)
                {
                    _logger.LogError($"Retry count exceed the limit {iMaxRetryCount}. Exiting the job execution.");

                    //Wait for a second before re-triggering
                    Task.Delay(1000);
                    break;
                }

            } while (!bJobRunStatus);
        }
    }
}