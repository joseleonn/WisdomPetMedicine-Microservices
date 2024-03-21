using System;
using WisdomPetMedicine.Common;
using WisdomPetMedicine.Common.Interfaces;

namespace WisdomPetMedicine.Hospital.Domain.Events
{
    public record PatientWeightUpdated(Guid Id, decimal Value) : IDomainEvent { }
}