namespace Bookzilla.Admin.Contracts.Activation;

public interface IActivationHandler
{
    bool CanHandle();

    Task HandleAsync();
}
