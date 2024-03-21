using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WisdomPetMedicine.Rescue.Domain.ValueObjects
{
    public record AdopterId
    {
        public Guid Value { get; init; }
        internal AdopterId(Guid value)
        {
            Value = value;
        }

        public static implicit operator Guid(AdopterId id)
        {
            return id.Value;
        }

        public static AdopterId Create(Guid value)
        {
            return new AdopterId(value);
        }
    }
}
