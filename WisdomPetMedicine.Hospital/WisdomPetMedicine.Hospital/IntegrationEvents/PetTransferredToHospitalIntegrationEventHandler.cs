
using Azure.Messaging.ServiceBus;
using System.Text.Json;
using WisdomPetMedicine.Hospital.Api.Infrastructure;
using WisdomPetMedicine.Hospital.Domain.Entities;
using WisdomPetMedicine.Hospital.Domain.Repositories;
using WisdomPetMedicine.Hospital.Domain.ValueObjects;

namespace WisdomPetMedicine.Hospital.Api.IntegrationEvents
{
    public class PetTransferredToHospitalIntegrationEventHandler : BackgroundService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger<PetTransferredToHospitalIntegrationEventHandler> logger;
        private readonly IPatientAggregateStore patientAggregateStore;
        private readonly ServiceBusClient client;
        private readonly ServiceBusProcessor processor;

        public PetTransferredToHospitalIntegrationEventHandler( IConfiguration configuration, 
                                                                IServiceScopeFactory serviceScopeFactory,
                                                                ILogger<PetTransferredToHospitalIntegrationEventHandler> logger,
                                                                IPatientAggregateStore patientAggregateStore)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.logger = logger;
            this.patientAggregateStore = patientAggregateStore;

            var vm = configuration["ServiceBus:ConnectionString"];
            client = new ServiceBusClient(configuration["ServiceBus:ConnectionString"]);

            processor = client.CreateProcessor(configuration["ServiceBus:TopicName"], configuration["ServiceBus:SubscriptionName"]);
            processor.ProcessMessageAsync += Proccesor_ProcessMessageAsync;
            processor.ProcessErrorAsync += Proccesor_ProcessErrorAsync;

        }
        private async Task Proccesor_ProcessMessageAsync(ProcessMessageEventArgs arg)
        {
            var body = arg.Message.Body.ToString();
            var theEvent = JsonSerializer.Deserialize<PetTransferredToHospitalIntegrationEvent>(body);
            await arg.CompleteMessageAsync(arg.Message);
            logger?.LogInformation(body);

            using var scope = serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<HospitalDbContext>();

            var existingPatient = await dbContext.PatientsMetadata.FindAsync(theEvent.Id);
            if(existingPatient == null)
            {
                dbContext.PatientsMetadata.Add(theEvent);
                await dbContext.SaveChangesAsync();
            }

            var patientId = PatientId.Create(theEvent.Id);
            var patient = new Patient(patientId);
            await patientAggregateStore.SaveAsync(patient);
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
