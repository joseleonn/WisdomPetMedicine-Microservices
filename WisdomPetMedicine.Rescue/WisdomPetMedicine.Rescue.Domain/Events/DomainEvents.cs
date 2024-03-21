using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WisdomPetMedicine.Common.Implementations;

namespace WisdomPetMedicine.Rescue.Domain.Events
{
    public static class DomainEvents
    {
        public static readonly DomainEvent<AdoptionRequestCreated> AdoptionRequestCreated = new();
    }
}
