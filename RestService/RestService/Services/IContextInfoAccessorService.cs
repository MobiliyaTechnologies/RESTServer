namespace RestService.Services
{
    using RestService.Models;

    public interface IContextInfoAccessorService
    {
        UserModel Current { get; }
    }
}
