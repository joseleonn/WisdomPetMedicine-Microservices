using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WisdomPetMedicine.Common.Interfaces;

namespace WisdomPetMedicine.Pet.Domain.Events
{
 
      public record PetTransferredToHospital(Guid Id, string Name, string Breed, int Sex, string Color, DateTime DateOfBirth, string Species) : IDomainEvent { }

    
}
