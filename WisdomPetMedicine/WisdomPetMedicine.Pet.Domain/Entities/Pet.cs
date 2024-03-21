using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WisdomPetMedicine.Pet.Domain.Events;
using WisdomPetMedicine.Pet.Domain.Exceptions;
using WisdomPetMedicine.Pet.Domain.ValueObjects;

namespace WisdomPetMedicine.Pet.Domain.Entities
{
    public class Pet
    {
        public Guid Id { get; set; }
        public PetName Name { get; set; }
        public PetBreed Breed { get; set; }
        public SexOfPet SexOfPet { get; set; }
        public PetColor Color { get; set; }
        public PetDateOfBirth DateOfBirth { get; set; }
        public PetSpecies Species { get; set; }

        public Pet(PetId id)
        {
            Id = id;
        }

        public Pet()
        {
            
        }
        public void SetName(PetName name)
        {
            Name = name;
        }

        public void SetBreed(PetBreed breed)
        {
            Breed = breed;
        }

        public void SetSex(SexOfPet sex)
        {
            SexOfPet = sex;
        }

        public void SetColor(PetColor color)
        {
            Color = color;
        }

        public void SetDateOfBirth(PetDateOfBirth dateOfBirth)
        {
            DateOfBirth = dateOfBirth;
        }

        public void SetSpecies(PetSpecies species)
        {
            Species = species;
        }

        public void FlagForAdoption() //Esta funcion lo que hace es que mediante el Patron Observator se envian los eventos de dominio.
        {
            ValidateStateForAdoption();
            DomainEvents.PetFlaggedForAdoption.Publish(new PetFlaggedForAdoption(Id, Name, Breed, SexOfPet, Color, DateOfBirth, Species));
        }

        public void TransferToHospital()
        {
            ValidateStateForTransfer();
            DomainEvents.PetTransferredToHospital.Publish(new PetTransferredToHospital(Id, Name, Breed, SexOfPet, Color, DateOfBirth, Species));
        }
        private void ValidateStateForAdoption()
        {
            if (Name == null)
            {
                throw new InvalidPetStateException("Name is missing");
            }
            if (Breed == null)
            {
                throw new InvalidPetStateException("Breed is missing");
            }
            if (SexOfPet == null)
            {
                throw new InvalidPetStateException("Sex of pet is missing");
            }
            if (Color == null)
            {
                throw new InvalidPetStateException("Color is missing");
            }
            if (DateOfBirth == null)
            {
                throw new InvalidPetStateException("Date of birth is missing");
            }
            if (Species == null)
            {
                throw new InvalidPetStateException("Species is missing");
            }
        }

        private void ValidateStateForTransfer()
        {
            if (Name == null)
            {
                throw new InvalidPetStateException("Name is missing");
            }
            if (Breed == null)
            {
                throw new InvalidPetStateException("Breed is missing");
            }
            if (SexOfPet == null)
            {
                throw new InvalidPetStateException("Sex of pet is missing");
            }
            if (Color == null)
            {
                throw new InvalidPetStateException("Color is missing");
            }
            if (DateOfBirth == null)
            {
                throw new InvalidPetStateException("Date of birth is missing");
            }
            if (Species == null)
            {
                throw new InvalidPetStateException("Species is missing");
            }
        }
    }
}
