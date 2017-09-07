namespace RestService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;
    using RestService.Enums;
    using RestService.Filters;
    using RestService.Models;
    using RestService.Services;
    using RestService.Services.Impl;
    using RestService.Utilities;

    [RoutePrefix("api")]   
    public class OrganizationController : ApiController
    {
        private readonly IOrganizationService organizationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationController"/> class.
        /// </summary>
        public OrganizationController()
        {
            this.organizationService = new OrganizationService();
        }

        /// <summary>
        /// Gets organization.
        /// This API is accessible to only super admin user.
        /// </summary>
        /// <returns>The organization detail.</returns>
        [Route("GetOrganization")]
        [ResponseType(typeof(OrganizationModel))]
        public HttpResponseMessage GetOrganization()
        {
            var data = this.organizationService.GetOrganization();

            if (data == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Organization does not exist.");
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Adds the organization if not exist or update existing organization.
        /// This API is accessible to only super admin user.
        /// It receive only multipart/form-data content-type.
        /// Required fields for new organization - OrganizationName.
        /// For update only modified fields require.
        /// Sample request -
        /// {  "OrganizationName": "",  "OrganizationDesc": "", "OrganizationAddress": ""}.
        /// Organization logo file with any name but must be image.
        /// </summary>
        /// <param name="organizationModel">The organization model.</param>
        /// <returns>The organization added confirmation, or bad request error response if invalid parameters.</returns>
        [Route("AddOrganization")]
        [HttpPost]
        [ResponseType(typeof(ResponseModel))]
        [CustomAuthorize(UserRole = UserRole.SuperAdmin)]
        [OverrideAuthorization]
        public HttpResponseMessage AddOrganization()
        {
            var organizationModel = this.GetOrganizationModelFromRequest().Result;

            var data = this.organizationService.AddOrganization(organizationModel);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Adds the premises to organization.
        /// This API is accessible to only super admin user.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="premiseIds">The premise ids.</param>
        /// <returns>The premises added to organization confirmation, or bad request error response if invalid parameters.</returns>
        [Route("AddPremisesToOrganization/{organizationId}")]
        [HttpPut]
        [ResponseType(typeof(ResponseModel))]
        [CustomAuthorize(UserRole = UserRole.SuperAdmin)]
        [OverrideAuthorization]
        public HttpResponseMessage AddPremisesToOrganization(int organizationId, [FromBody] List<int> premiseIds)
        {
            if (organizationId < 1 || premiseIds.Count() < 1 || premiseIds.Any(c => c < 1))
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid organization id or premise ids.");
            }

            var premiseAddedConfirmation = this.organizationService.AddPremisesToOrganization(organizationId, premiseIds);
            return this.Request.CreateResponse(HttpStatusCode.OK, premiseAddedConfirmation);
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                (this.organizationService as IDisposable).Dispose();
            }

            base.Dispose(disposing);
        }

        private async Task<OrganizationModel> GetOrganizationModelFromRequest()
        {
            // Check if the request contains multipart/form-data.
            if (!this.Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var organizationModel = new OrganizationModel();

            var contents = await this.Request.Content.ReadAsMultipartAsync();

            var fileContent = contents.Contents.FirstOrDefault(c => c.Headers.ContentType != null && c.Headers.ContentType.MediaType.StartsWith("image") && c.Headers.ContentLength > 0);
            var formDataContents = contents.Contents.Where(c => c.Headers.ContentType == null || (c.Headers.ContentDisposition != null && c.Headers.ContentDisposition.FileName == null));

            if (fileContent != null)
            {
                organizationModel.OrganizationLogo = await fileContent.ReadAsStreamAsync();
                organizationModel.OrganizationLogoContentType = fileContent.Headers.ContentType.ToString();
                organizationModel.OrganizationLogoName = "OrganizationLogo." + fileContent.Headers.ContentDisposition.FileName.Trim('"').Split('.')[1];
            }

            organizationModel.OrganizationAddress = this.GetFormDataValue(formDataContents, "OrganizationAddress");
            organizationModel.OrganizationName = this.GetFormDataValue(formDataContents, "OrganizationName");
            organizationModel.OrganizationDesc = this.GetFormDataValue(formDataContents, "OrganizationDesc");

            return organizationModel;
        }

        private string GetFormDataValue(IEnumerable<HttpContent> formDataContents, string name)
        {
            var formDataContent = formDataContents.FirstOrDefault(f => f.Headers.ContentDisposition.Name.Trim('"').Equals(name, StringComparison.InvariantCultureIgnoreCase));

            return formDataContent != null ? formDataContent.ReadAsStringAsync().Result : null;
        }
    }
}