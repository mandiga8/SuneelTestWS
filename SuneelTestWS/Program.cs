using SuneelTestWS;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = "SuneelTestWS";
    })

    .ConfigureServices(services =>
    {
        //Adding the Custom Classes to Dependency Injection
        services.AddSingleton<Services.IPowerService, Services.PowerService>();
        services.AddSingleton<SuneelTestWS.Service.IPowerTradeAggregiation, SuneelTestWS.Service.PowerTradeAggregiation>();

        //Adding the Background Service as Hosted Service
        services.AddHostedService<WindowsBackgroundService>();
    })
    .ConfigureLogging((context, logging) =>
    {
        logging.AddConfiguration(
            context.Configuration.GetSection("Logging"));
    })
    .Build();


await host.RunAsync();
