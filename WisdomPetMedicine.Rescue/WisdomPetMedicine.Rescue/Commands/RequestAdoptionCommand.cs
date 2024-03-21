namespace WisdomPetMedicine.Rescue.Commands
{
    public record RequestAdoptionCommand(Guid PetId, Guid AdopterId);

}
