namespace RestService.Services.Impl
{
    using System.Net.Http;
    using System.Web;
    using RestService.Models;

    public class ContextInfoAccessorService : IContextInfoAccessorService
    {
        UserModel IContextInfoAccessorService.Current
        {
            get
            {
                var request = (HttpRequestMessage)HttpContext.Current.Items["MS_HttpRequestMessage"];
                var userModel = (UserModel)request.Properties["Context"];
                return userModel;
            }
        }
    }
}