using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WisdomPetMedicine.Common.Implementations;

namespace WisdomPetMedicine.Pet.Domain.Events
{
    public class DomainEvents
    {
        public static DomainEvent<PetFlaggedForAdoption> PetFlaggedForAdoption = new();
        public static DomainEvent<PetTransferredToHospital> PetTransferredToHospital = new();
    }
}
