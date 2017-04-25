namespace RestService
{
    using System.Collections.Generic;
    using System.IdentityModel.Tokens;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.IdentityModel.Protocols;

    // This class is necessary because the OAuthBearer Middle-ware does not leverage
    // the OpenID Connect meta-data endpoint exposed by the STS by default.
    public class OpenIdConnectCachingSecurityTokenProvider : Microsoft.Owin.Security.Jwt.IIssuerSecurityTokenProvider
    {
        private readonly string metadataEndpoint;
        private readonly ReaderWriterLockSlim synclock = new ReaderWriterLockSlim();
        private readonly ConfigurationManager<OpenIdConnectConfiguration> configManager;
        private string issuer;
        private IEnumerable<SecurityToken> tokens;

        public OpenIdConnectCachingSecurityTokenProvider(string metadataEndpoint)
        {
            this.metadataEndpoint = metadataEndpoint;
            this.configManager = new ConfigurationManager<OpenIdConnectConfiguration>(metadataEndpoint);

            this.RetrieveMetadata();
        }

        /// <summary>
        /// Gets the issuer the credentials are for.
        /// </summary>
        /// <value>
        /// The issuer the credentials are for.
        /// </value>
        public string Issuer
        {
            get
            {
                this.RetrieveMetadata();
                this.synclock.EnterReadLock();
                try
                {
                    return this.issuer;
                }
                finally
                {
                    this.synclock.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// Gets all known security tokens.
        /// </summary>
        /// <value>
        /// All known security tokens.
        /// </value>
        public IEnumerable<SecurityToken> SecurityTokens
        {
            get
            {
                this.RetrieveMetadata();
                this.synclock.EnterReadLock();
                try
                {
                    return this.tokens;
                }
                finally
                {
                    this.synclock.ExitReadLock();
                }
            }
        }

        private void RetrieveMetadata()
        {
            this.synclock.EnterWriteLock();
            try
            {
                OpenIdConnectConfiguration config = Task.Run(this.configManager.GetConfigurationAsync).Result;
                this.issuer = config.Issuer;
                this.tokens = config.SigningTokens;
            }
            finally
            {
                this.synclock.ExitWriteLock();
            }
        }
    }
}
