using WisdomPetMedicine.Rescue.Domain.ValueObjects;

namespace WisdomPetMedicine.Rescue.Commands
{
    public record SetAdopterPhoneNumberCommand(Guid Id, string PhoneNumber)
    {
    }
}
