using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WisdomPetMedicine.Pet.Domain.ValueObjects
{
    public record SexOfPet
    {
        public SexesOfPets Value { get; init; }
        internal SexOfPet(SexesOfPets value)
        {
            Value = value;
        }

        public static SexOfPet Create(SexesOfPets value)
        {
            return new SexOfPet(value);
        }

        public static implicit operator int(SexOfPet value)
        {
            return (int)value.Value;
        }

        public static SexOfPet Male = new SexOfPet(SexesOfPets.Male);
        public static SexOfPet Female = new SexOfPet(SexesOfPets.Female);
    }

    public enum SexesOfPets
    {
        Male,
        Female
    }
}
