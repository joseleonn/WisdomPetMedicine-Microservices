
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using WisdomPetMedicine.Rescue.Api.IntegrationEvents;
using WisdomPetMedicine.Rescue.Domain.Entities;
using WisdomPetMedicine.Rescue.Domain.Repositories;
using WisdomPetMedicine.Rescue.Domain.ValueObjects;
using WisdomPetMedicine.Rescue.Infraestructure;

namespace WisdomPetMedicine.Rescue.IntegrationEvents
{
    public class PetFlaggedForAdoptionIntegrationEventHandler : BackgroundService //clase para suscribirse a azure service bus 
    {
        private readonly ILogger<PetFlaggedForAdoptionIntegrationEventHandler> logger;
        private readonly ServiceBusClient client;
        private readonly ServiceBusProcessor processor;
        private readonly IServiceScopeFactory serviceScopeFactory;
        public PetFlaggedForAdoptionIntegrationEventHandler(IConfiguration configuration, ILogger<PetFlaggedForAdoptionIntegrationEventHandler> logger, IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.logger = logger;
            client = new ServiceBusClient(configuration["ServiceBus:ConnectionString"]);
            processor = client.CreateProcessor(configuration["ServiceBus:TopicName"], configuration["ServiceBus:SubscriptionName"]);
            processor.ProcessMessageAsync += Proccesor_ProcessMessageAsync;
            processor.ProcessErrorAsync += Proccesor_ProcessErrorAsync;
        }

        private async Task Proccesor_ProcessMessageAsync(ProcessMessageEventArgs arg)
        {           
            var body = arg.Message.Body.ToString();
            var theEvent = JsonConvert.DeserializeObject<PetFlaggedForAdoptionIntegrationEvent>(body);
            await arg.CompleteMessageAsync(arg.Message);
            logger?.LogInformation(body);

            using var scope = serviceScopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IRescueRepository>();
            var dbContext = scope.ServiceProvider.GetRequiredService<RescueDbContext>();
            dbContext.RescuedAnimalsMetadata.Add(theEvent); //se almacena los metadatos del evento
            var rescuedAnimal = new RescuedAnimal(RescuedAnimalId.Create(theEvent.Id));
            await repo.AddRescuedAnimalAsync(rescuedAnimal);
        }
        private Task Proccesor_ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            logger?.LogError(arg.Exception.ToString());
            return Task.CompletedTask;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await processor.StartProcessingAsync(stoppingToken);
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await processor.StopProcessingAsync(stoppingToken);
        }
    }
}
