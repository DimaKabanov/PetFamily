namespace PetFamily.SharedKernel.Interfaces;

public interface ISoftDeletable
{
    void Delete();
    void Restore();
}