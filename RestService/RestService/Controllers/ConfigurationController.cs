namespace RestService.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using RestService.Utilities;

    /// <summary>
    /// Provides configuration APIs.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [AllowAnonymous]
    [RoutePrefix("api")]
    public class ConfigurationController : ApiController
    {
        /// <summary>
        /// Gets the B2C configuration.
        /// </summary>
        /// <returns>The b2c configuration dictionary.</returns>
        [Route("GetMobileConfiguration")]
        public IDictionary<string, string> GetMobileConfiguration()
        {
            var b2cConfiguration = new Dictionary<string, string>();

            b2cConfiguration.Add("B2cTenant", ApiConfiguration.B2cTenant);
            b2cConfiguration.Add("B2cClientId", ApiConfiguration.B2cClientId);
            b2cConfiguration.Add("B2cClientSecret", ApiConfiguration.B2cClientSecret);
            b2cConfiguration.Add("B2cSignUpPolicy", ApiConfiguration.B2cSignUpPolicy);
            b2cConfiguration.Add("B2cSignInPolicy", ApiConfiguration.B2cSignInPolicy);
            b2cConfiguration.Add("B2cChangePasswordPolicy", ApiConfiguration.B2cChangePasswordPolicy);

            b2cConfiguration.Add("B2cAuthorizeURL", ApiConfiguration.B2cMobileAuthorizeURL);
            b2cConfiguration.Add("B2cTokenURL", ApiConfiguration.B2cMobileTokenURL);
            b2cConfiguration.Add("B2cTokenURLIOS", ApiConfiguration.B2cMobileTokenURLIOS);
            b2cConfiguration.Add("B2cChangePasswordURL", ApiConfiguration.B2cMobileChangePasswordURL);
            b2cConfiguration.Add("B2cRedirectUrl", ApiConfiguration.B2cMobileRedirectUrl);

            return b2cConfiguration;
        }
    }
}
