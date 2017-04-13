using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using RestService.Models;
using System.Configuration;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Jwt;
using System.IdentityModel.Tokens;

namespace RestService
{
    public partial class Startup
    {
        public static string aadInstance;
        public static string tenant;
        public static string clientId;
        public static string signUpPolicy;
        public static string signInPolicy;
        public static string editProfilePolicy;
        public static string MFAPolicyName;

        public void ConfigureAuth(IAppBuilder app)
        {
            aadInstance = ConfigurationManager.AppSettings["b2c:AadInstance"];
            tenant = ConfigurationManager.AppSettings["b2c:Tenant"];
            clientId = ConfigurationManager.AppSettings["b2c:ClientId"];
            signUpPolicy = ConfigurationManager.AppSettings["b2c:SignUpPolicyId"];
            signInPolicy = ConfigurationManager.AppSettings["b2c:SignInPolicyId"];
            editProfilePolicy = ConfigurationManager.AppSettings["b2c:UserProfilePolicyId"];
            MFAPolicyName = ConfigurationManager.AppSettings["b2c:MFAPolicyName"];

            // app.UseOAuthBearerAuthentication(CreateBearerOptionsFromPolicy(signUpPolicy));
            app.UseOAuthBearerAuthentication(CreateBearerOptionsFromPolicy(signInPolicy));
        }

        public OAuthBearerAuthenticationOptions CreateBearerOptionsFromPolicy(string policy)
        {
            TokenValidationParameters tvps = new TokenValidationParameters
            {
                // This is where you specify that your API only accepts tokens from its own clients
                ValidAudience = clientId,
                AuthenticationType = policy,
            };

            return new OAuthBearerAuthenticationOptions
            {
                // This SecurityTokenProvider fetches the Azure AD B2C metadata & signing keys from the OpenIDConnect metadata endpoint
                AccessTokenFormat = new JwtFormat(tvps,
                new OpenIdConnectCachingSecurityTokenProvider(String.Format(aadInstance, tenant, policy))),
            };
        }
    }
}