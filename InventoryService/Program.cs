using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using InventoryService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        var fullyQualifiedNamespace = Environment.GetEnvironmentVariable("AZURE_SERVICE_BUS_NAMESPACE");
        if(string.IsNullOrEmpty(fullyQualifiedNamespace))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Please ensure the environment variable [AZURE_SERVICE_BUS_NAMESPACE] is set!");
            System.Environment.Exit(0);
        }

        var sbAdminClient = new ServiceBusAdministrationClient(fullyQualifiedNamespace, new DefaultAzureCredential());
        var sbClient = new ServiceBusClient(fullyQualifiedNamespace, new DefaultAzureCredential());

        services.AddHostedService<Worker>();
        services.AddSingleton<ServiceBusAdministrationClient>(implementationInstance: sbAdminClient);
        services.AddSingleton<ServiceBusClient>(implementationInstance: sbClient);
    })
    .Build();

await host.RunAsync();
