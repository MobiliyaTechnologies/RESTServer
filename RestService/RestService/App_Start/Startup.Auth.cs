namespace RestService
{
    using System.Configuration;
    using System.IdentityModel.Tokens;
    using Microsoft.Owin.Security.Jwt;
    using Microsoft.Owin.Security.OAuth;
    using Owin;
    using RestService.Utilities;

    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseOAuthBearerAuthentication(this.CreateBearerOptionsFromPolicy(ApiConfiguration.B2cSignUpPolicy));
            app.UseOAuthBearerAuthentication(this.CreateBearerOptionsFromPolicy(ApiConfiguration.B2cSignInPolicy));
        }

        public OAuthBearerAuthenticationOptions CreateBearerOptionsFromPolicy(string policy)
        {
            TokenValidationParameters tvps = new TokenValidationParameters
            {
                // This is where you specify that your API only accepts tokens from its own clients
                ValidAudience = ApiConfiguration.B2cClientId,
                AuthenticationType = policy,
            };

            return new OAuthBearerAuthenticationOptions
            {
                // This SecurityTokenProvider fetches the Azure AD B2C meta-data & signing keys from the OpenIDConnect meta data endpoint
                AccessTokenFormat = new JwtFormat(tvps, new OpenIdConnectCachingSecurityTokenProvider(string.Format(ApiConfiguration.B2cAadInstance, ApiConfiguration.B2cTenant, policy))),
            };
        }
    }
}
