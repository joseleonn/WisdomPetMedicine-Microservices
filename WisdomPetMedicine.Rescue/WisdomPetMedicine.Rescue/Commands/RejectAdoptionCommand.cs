namespace WisdomPetMedicine.Rescue.Commands
{
    public record RejectAdoptionCommand(Guid PetId, Guid AdopterId);

}
