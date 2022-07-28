using SuneelTestWS;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        //Adding the Custom Classes to Dependency Injection
        services.AddSingleton<Services.IPowerService, Services.PowerService>();
        services.AddSingleton<SuneelTestWS.Service.IPowerTradeAggregiation, SuneelTestWS.Service.PowerTradeAggregiation>();

        //Adding the Background Service as Hosted Service
        services.AddHostedService<WindowsBackgroundService>();
    })
    .Build();


await host.RunAsync();
