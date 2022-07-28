using SuneelTestWS.Model;

namespace SuneelTestWS.Service
{
    public interface IPowerTradeAggregiation
    {
        List<PowerTradeOutput> GetAggregiatedResults(DateTime dateTime);
        string WriteOutputToCSV(DateTime dateTime, out string OutputCSVFileName);
    }
}