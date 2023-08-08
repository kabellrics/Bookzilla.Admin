namespace Bookzilla.Admin.Contracts.Services;

public interface IPersistAndRestoreService
{
    void RestoreData();

    void PersistData();
}
