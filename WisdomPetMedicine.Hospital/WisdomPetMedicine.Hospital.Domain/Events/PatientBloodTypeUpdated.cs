using System;
using WisdomPetMedicine.Common;
using WisdomPetMedicine.Common.Interfaces;

namespace WisdomPetMedicine.Hospital.Domain.Events
{
    public record PatientBloodTypeUpdated(Guid Id, string BloodType) : IDomainEvent { }
}