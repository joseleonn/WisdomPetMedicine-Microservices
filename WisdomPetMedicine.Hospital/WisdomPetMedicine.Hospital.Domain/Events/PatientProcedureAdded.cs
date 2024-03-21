using System;
using WisdomPetMedicine.Common;
using WisdomPetMedicine.Common.Interfaces;

namespace WisdomPetMedicine.Hospital.Domain.Events
{
    public record PatientProcedureAdded(Guid PatientId, Guid Id, string ProcedureName) : IDomainEvent { }
}
