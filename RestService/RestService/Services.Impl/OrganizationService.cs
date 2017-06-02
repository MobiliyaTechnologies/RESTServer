namespace RestService.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RestService.Entities;
    using RestService.Enums;
    using RestService.Mappings;
    using RestService.Models;
    using RestService.Utilities;

    public sealed class OrganizationService : IOrganizationService, IDisposable
    {
        private readonly PowerGridEntities dbContext;
        private readonly IContextInfoAccessorService context;
        private readonly IBlobStorageService blobStorageService;

        public OrganizationService()
        {
            this.dbContext = new PowerGridEntities();
            this.context = new ContextInfoAccessorService();
            this.blobStorageService = new BlobStorageService();
        }

        OrganizationModel IOrganizationService.GetOrganization()
        {
            var organization = this.dbContext.Organization.WhereActiveOrganization().FirstOrDefault();
            var organizationModel = new OrganizationModelMapping().Map(organization);

            if (organizationModel != null && organization.OrganizationLogo != null)
            {
                var blobStorageModel = new BlobStorageModel
                {
                    BlobName = organization.OrganizationLogo,
                    StorageContainer = ApiConfiguration.BlobPublicContainer,
                };

                organizationModel.OrganizationLogoUri = this.blobStorageService.GetBlobUri(blobStorageModel);
            }

            return organizationModel;
        }

        ResponseModel IOrganizationService.AddOrganization(OrganizationModel model)
        {
            var organization = this.dbContext.Organization.FirstOrDefault();
            var responseMessage = string.Empty;

            if (organization != null)
            {
                if (!string.IsNullOrWhiteSpace(model.OrganizationName))
                {
                    organization.OrganizationName = model.OrganizationName;
                }

                if (!string.IsNullOrWhiteSpace(model.OrganizationDesc))
                {
                    organization.OrganizationDesc = model.OrganizationDesc;
                }

                if (!string.IsNullOrWhiteSpace(model.OrganizationAddress))
                {
                    organization.OrganizationAddress = model.OrganizationAddress;
                }

                if (!string.IsNullOrWhiteSpace(model.OrganizationLogoName))
                {
                    organization.OrganizationLogo = model.OrganizationLogoName;
                }

                organization.ModifiedBy = this.context.Current.UserId;
                organization.ModifiedOn = DateTime.UtcNow;

                responseMessage = "Organization Updated successfully.";
            }
            else
            {
                if (string.IsNullOrWhiteSpace(model.OrganizationName))
                {
                    return new ResponseModel { Message = "Organization name required to create new organization", Status_Code = (int)StatusCode.Error };
                }

                organization = new Organization();
                organization.OrganizationName = model.OrganizationName;
                organization.OrganizationDesc = model.OrganizationDesc;
                organization.OrganizationAddress = model.OrganizationAddress;
                organization.OrganizationLogo = model.OrganizationLogoName;
                organization.CreatedBy = this.context.Current.UserId;
                organization.CreatedOn = DateTime.UtcNow;
                organization.ModifiedBy = this.context.Current.UserId;
                organization.ModifiedOn = DateTime.UtcNow;
                organization.IsActive = true;
                organization.IsDeleted = false;

                this.dbContext.Organization.Add(organization);

                responseMessage = "Organization added successfully.";
            }

            this.dbContext.SaveChanges();

            if (model.OrganizationLogo != null && model.OrganizationLogoContentType != null && model.OrganizationLogoName != null)
            {
                var blobStorageModel = new BlobStorageModel
                {
                    BlobName = model.OrganizationLogoName,
                    BlobType = model.OrganizationLogoContentType,
                    Blob = model.OrganizationLogo,
                    StorageContainer = ApiConfiguration.BlobPublicContainer,
                    IsPublicContainer = true
                };

                this.blobStorageService.UploadBlob(blobStorageModel);
            }

            return new ResponseModel { Message = responseMessage, Status_Code = (int)StatusCode.Ok };
        }

        ResponseModel IOrganizationService.AddPremisesToOrganization(int organizationId, List<int> premiseIds)
        {
            var premises = this.dbContext.Premise.WhereActivePremise(c => premiseIds.Any(i => i == c.PremiseID));

            if (premises.Count() != premiseIds.Count() || premises.Count() == 0)
            {
                return new ResponseModel { Status_Code = (int)StatusCode.Error, Message = "Premise does not exists for given ids" };
            }

            var organization = this.dbContext.Organization.WhereActiveOrganization(u => u.OrganizationID == organizationId);

            if (organization == null)
            {
                return new ResponseModel { Status_Code = (int)StatusCode.Error, Message = string.Format("Organization does not exists for id - {0}", organizationId) };
            }

            var newPremiseToAdd = premises.Where(c => c.OrganizationID != organizationId);

            foreach (var premise in newPremiseToAdd)
            {
                premise.OrganizationID = organizationId;
            }

            this.dbContext.SaveChanges();

            return new ResponseModel { Status_Code = (int)StatusCode.Ok, Message = "Premises added to organization successfully." };
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            if (this.dbContext != null)
            {
                this.dbContext.Dispose();
            }
        }
    }
}