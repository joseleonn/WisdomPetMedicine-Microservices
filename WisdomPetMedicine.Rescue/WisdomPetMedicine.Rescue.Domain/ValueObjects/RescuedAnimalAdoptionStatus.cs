using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WisdomPetMedicine.Rescue.Domain.ValueObjects
{
    public enum RescuedAnimalAdoptionStatus
    {
        None,
        PendingReview,
        Accepted,
        Rejected
    }
}
