namespace DryIocCore2.Services
{
    public interface ITransientService
    {
        IDependentService DependentService { get; }
    }
}