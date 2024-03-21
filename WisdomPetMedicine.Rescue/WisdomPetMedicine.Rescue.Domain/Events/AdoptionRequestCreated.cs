using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WisdomPetMedicine.Rescue.Domain.Events
{
    public record AdoptionRequestCreated(Guid RescuedAnimalId, Guid AdopterId);

}
