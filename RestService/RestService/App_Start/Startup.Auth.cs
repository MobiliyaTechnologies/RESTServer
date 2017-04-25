namespace RestService
{
    using System.Configuration;
    using System.IdentityModel.Tokens;
    using Microsoft.Owin.Security.Jwt;
    using Microsoft.Owin.Security.OAuth;
    using Owin;

    public partial class Startup
    {
        private string aadInstance;
        private string tenant;
        private string clientId;
        private string signUpSignInPolicy;
        private string editProfilePolicy;
        private string signInPolicy;

        public void ConfigureAuth(IAppBuilder app)
        {
            this.aadInstance = ConfigurationManager.AppSettings["b2c:AadInstance"];
            this.tenant = ConfigurationManager.AppSettings["b2c:Tenant"];
            this.clientId = ConfigurationManager.AppSettings["b2c:ClientId"];
            this.signUpSignInPolicy = ConfigurationManager.AppSettings["b2c:SignUpSignInPolicyId"];
           this.signInPolicy = ConfigurationManager.AppSettings["b2c:SignInPolicyId"];
         this.editProfilePolicy = ConfigurationManager.AppSettings["b2c:UserProfilePolicyId"];

            app.UseOAuthBearerAuthentication(this.CreateBearerOptionsFromPolicy(this.signUpSignInPolicy));
            app.UseOAuthBearerAuthentication(this.CreateBearerOptionsFromPolicy(this.signInPolicy));
            app.UseOAuthBearerAuthentication(this.CreateBearerOptionsFromPolicy(this.editProfilePolicy));
        }

        public OAuthBearerAuthenticationOptions CreateBearerOptionsFromPolicy(string policy)
        {
            TokenValidationParameters tvps = new TokenValidationParameters
            {
                // This is where you specify that your API only accepts tokens from its own clients
                ValidAudience = this.clientId,
                AuthenticationType = policy,
            };

            return new OAuthBearerAuthenticationOptions
            {
                // This SecurityTokenProvider fetches the Azure AD B2C meta-data & signing keys from the OpenIDConnect meta data endpoint
                AccessTokenFormat = new JwtFormat(tvps, new OpenIdConnectCachingSecurityTokenProvider(string.Format(this.aadInstance, this.tenant, policy))),
            };
        }
    }
}
