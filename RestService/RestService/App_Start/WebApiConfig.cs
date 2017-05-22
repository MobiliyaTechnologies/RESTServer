namespace RestService
{
    using System.Web.Http;
    using System.Web.Http.Cors;
    using RestService.Filters;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // temporary added for development, it will be configure on azure app service.
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // added temporary to expose error outside domain for testing.
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            config.Filters.Add(new CustomAuthorizeAttribute());
        }
    }
}
