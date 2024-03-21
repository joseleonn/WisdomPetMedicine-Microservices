using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Threading.Tasks;
using WisdomPetMedicine.Common;
using WisdomPetMedicine.Common.Interfaces;
using WisdomPetMedicine.Hospital.Domain.Entities;
using WisdomPetMedicine.Hospital.Domain.Repositories;
using WisdomPetMedicine.Hospital.Domain.ValueObjects;
using Container = Microsoft.Azure.Cosmos.Container;
using Newtonsoft.Json;

namespace WisdomPetMedicine.Hospital.Infrastructure
{
    public class PatientAggregateStore : IPatientAggregateStore
    {
        
        private readonly CosmosClient cosmosClient;
        private readonly Container patientContainer;
        public PatientAggregateStore(IConfiguration configuration)
        {
            var connectionString = configuration["CosmosDb:ConnectionString"];
            var databaseId = configuration["CosmosDb:DatabaseId"];
            var containerId = configuration["CosmosDb:ContainerId"];

            var clientOptions = new CosmosClientOptions()
            {
                SerializerOptions = new CosmosSerializationOptions()
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                }
            };

            cosmosClient = new CosmosClient(connectionString, clientOptions);
            patientContainer = cosmosClient.GetContainer(databaseId, containerId);
        }
        public async Task<Patient> LoadAsync(PatientId patientId)
        {
            //funcion para rehidratar los eventos de dominio a partir del almacen de eventos de CosmosDB, se le pasa por parametro
            //una id de paciente y trae todos los eventos relacionados al mismo.
            if (patientId == null)
            {
                throw new ArgumentNullException(nameof(patientId));
            }

            var aggregateId = $"Patient-{patientId.Value}";
            var sqlQueryText = $"SELECT * FROM c WHERE c.aggregateId = '{aggregateId}'";
            var queryDefinition = new QueryDefinition(sqlQueryText);
            var queryResultSetIterator = patientContainer.GetItemQueryIterator<CosmosEventData>(queryDefinition);
            var allEvents = new List<CosmosEventData>();

            while (queryResultSetIterator.HasMoreResults)
            {
                var currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (CosmosEventData item in currentResultSet)
                {
                    allEvents.Add(item);
                }
            }

            var domainEvents = allEvents.Select(e =>
            {
                var assemblyQualifiedName = JsonConvert.DeserializeObject<string>(e.AssemblyQualifiedName);
                var eventType = Type.GetType(assemblyQualifiedName);
                var data = JsonConvert.DeserializeObject(e.Data, eventType);
                return data as IDomainEvent;
            });

            var aggregate = new Patient();
            aggregate.Load(domainEvents);

            return aggregate;
        }

        public async Task SaveAsync(Patient patient)
        {
            //este metodo lo que hace es que mediante el AggregateRoot (el cual proporciona la logica para saber si hubo eventos de dominio)
            //obtiene los cambios de dominio y esta funcion los almacena en CosmosDB (almacen de eventos) 
            if (patient == null)
            {
                throw new ArgumentNullException(nameof(patient));
            }

            var changes = patient.GetChanges()
              .Select(e => new CosmosEventData(Guid.NewGuid(),
                                               $"Patient-{patient.Id}",
                                               e.GetType().Name,
                                              JsonConvert.SerializeObject(e),
                                              JsonConvert.SerializeObject(e.GetType().AssemblyQualifiedName)))
              .AsEnumerable();

            if (!changes.Any())
            {
                return;
            }

            foreach (var item in changes)
            {
               
                    Console.WriteLine($"Event EventName: {item.EventName}");
                    Console.WriteLine($"Data: {item.Data}");
                    Console.WriteLine();
                
                await patientContainer.CreateItemAsync(item);
            }

            patient.ClearChanges();
        }
    }
}