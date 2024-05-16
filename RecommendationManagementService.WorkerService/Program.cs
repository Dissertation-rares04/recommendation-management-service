using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RecommendationManagementService.Business.Implementation;
using RecommendationManagementService.Business.Interface;
using RecommendationManagementService.Core.AppSettings;
using RecommendationManagementService.Data.Implementation;
using RecommendationManagementService.Data.Interface;
using RecommendationManagementService.WorkerService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.Configure<KafkaSettings>(hostContext.Configuration.GetSection("Kafka"));
        services.Configure<KafkaTopics>(hostContext.Configuration.GetSection("KafkaTopics"));
        services.Configure<Auth0Settings>(hostContext.Configuration.GetSection("Auth0"));
        services.Configure<MongoDbSettings>(hostContext.Configuration.GetSection("MongoDb"));

        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.AddTransient<IUserResolver, UserResolver>();

        services.AddTransient<IBaseServiceDataAccess, BaseServiceDataAccess>();
        services.AddTransient<IRecommendationServiceDataAccess, RecommendationServiceDataAccess>();
        services.AddTransient<IUserInteractionDataAccess, UserInteractionDataAccess>();

        //services.AddTransient<IBaseService, BaseService>();
        services.AddTransient<IUserInteractionService, UserInteractionService>();
        services.AddTransient<IWorkerRecommendationService, WorkerRecommendationService>();

        services.AddSingleton<IMessageHandler, UserInteractionMessageHandler>();

        services.AddHostedService<KafkaConsumerWorker>();
    })
    .Build();

await host.RunAsync();
