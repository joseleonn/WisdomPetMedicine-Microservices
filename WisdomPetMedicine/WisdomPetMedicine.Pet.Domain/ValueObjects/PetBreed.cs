using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WisdomPetMedicine.Pet.Domain.Services;

namespace WisdomPetMedicine.Pet.Domain.ValueObjects
{
    public record PetBreed //Se hace de tipo record para que sea inmutable una vez creado y mantener la integridad de los datos.
    {
        public string Value { get; init; } //valor real de la raza

        internal PetBreed(string value) //es internal para ganrangizar que solo se pueda crear un PetBreed valido dentro del do,minio de la app.
        {
            Value = value;
        }

        public static PetBreed Create(string value, IBreedService breedService) //metodo para crear PetBreeds
        {
            Validate(value, breedService);
            return new PetBreed(value);
        }

        public static implicit operator string(PetBreed breed)
        {
            return breed.Value;
        }

        private static void Validate(string value, IBreedService breedService)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Breed cannot be empty or null", nameof(value));
            }

            var breed = breedService?.Find(value);
            if (breed == null)
            {
                throw new ArgumentException("Breed specified is not valid");
            }
        }
    }
}
