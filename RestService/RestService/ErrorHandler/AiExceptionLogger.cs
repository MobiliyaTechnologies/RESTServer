namespace RestService.ErrorHandler{
    using System;
    using System.Web.Http.ExceptionHandling;
    using Microsoft.ApplicationInsights;
    /// <summary>
    ///  Represents an unhandled exception logger.
    /// </summary>
    /// <seealso cref="System.Web.Http.ExceptionHandling.ExceptionLogger" />
    public class AiExceptionLogger : ExceptionLogger, IDisposable
    {
        private readonly TelemetryClient telemetryClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="AiExceptionLogger"/> class.
        /// </summary>
        /// <param name="telemetryClient">The telemetry client.</param>
        public AiExceptionLogger(TelemetryClient telemetryClient)
        {
            this.telemetryClient = telemetryClient;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// Flushes the telemetry client.
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (this.telemetryClient != null)
                {
                    this.telemetryClient.Flush();
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// logs the exception synchronously.
        /// </summary>
        /// <param name="context">The exception logger context.</param>
        public override void Log(ExceptionLoggerContext context)
        {
            try
            {
                if (context != null && context.Exception != null)
                {
                    this.telemetryClient.TrackException(context.Exception);
                }

                base.Log(context);
            }
            catch (Exception)
            {
                // ignore to avoid original exception overriding.
            }
        }
    }}