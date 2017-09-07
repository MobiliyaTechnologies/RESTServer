namespace RestService
{
    using System.Web.Http;
    using System.Web.Http.Cors;
    using System.Web.Http.ExceptionHandling;
    using Microsoft.ApplicationInsights;
    using RestService.ErrorHandler;
    using RestService.Filters;
    using RestService.Utilities;

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

            // set application insights instrumentation key.
            Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active.InstrumentationKey = ApiConfiguration.ApplicationInsightsInstrumentationKey;

            config.Services.Add(typeof(IExceptionLogger), new AiExceptionLogger(new TelemetryClient()));
        }
    }
}
