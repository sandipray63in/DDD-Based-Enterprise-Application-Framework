using System;
using System.Data.Entity.ModelConfiguration.Configuration;
using Infrastructure.Utilities;
using Domain.Base.AddOnObjects;
using Domain.Base.Entities.Composites;

namespace DomainContextsAndMaps.Base.EFBase
{
    public abstract class BaseEntityWithAuditInfoRootMap<TEntity, TId> : BaseEntityRootMap<TEntity, TId>
        where TEntity : BaseEntityComposite<TId, AuditInfo>
        where TId : struct
    {
        #region Actual Audit Columns Overridables

        protected virtual string CreatedByColumnName { get; } = "col_created_by";
        protected virtual string CreationDateColumnName { get; } = "col_creation_date";
        protected virtual string LastUpdatedByColumnName { get; } = "col_last_updated_by";
        protected virtual string LastUpdationDateColumnName { get; } = "col_last_updation_date";

        #endregion

        protected sealed override void SetEntitySpecificProperties()
        {
            ContractUtility.Requires<ArgumentNullException>(CreatedByColumnName.IsNotNullOrWhiteSpace(), "CreatedByColumnName cannot be null or empty.");
            ContractUtility.Requires<ArgumentNullException>(CreationDateColumnName.IsNotNullOrWhiteSpace(), "CreationDateColumnName cannot be null or empty.");
            ContractUtility.Requires<ArgumentNullException>(LastUpdatedByColumnName.IsNotNullOrWhiteSpace(), "LastUpdatedByColumnName Name cannot be null or empty.");
            ContractUtility.Requires<ArgumentNullException>(LastUpdationDateColumnName.IsNotNullOrWhiteSpace(), "LastUpdationDateColumnName Name cannot be null or empty.");

            SetSpecificPropertiesForEntity();

            ExtendCreatedByPropertyWithOtherConfigurations(Property(p => p.T1Data.CreatedBy).HasColumnName(CreatedByColumnName));
            ExtendCreationDatePropertyWithOtherConfigurations(Property(p => p.T1Data.CreatedOn).HasColumnName(CreationDateColumnName));
            ExtendLastUpdatedByPropertyWithOtherConfigurations(Property(p => p.T1Data.LastUpdatedBy).HasColumnName(LastUpdatedByColumnName));
            ExtendLastUpdatedDatePropertyWithOtherConfigurations(Property(p => p.T1Data.LastUpdateOn).HasColumnName(LastUpdationDateColumnName));
        }

        protected abstract void SetSpecificPropertiesForEntity();

        #region Overrideable Configuration Extensions

        protected virtual void ExtendCreatedByPropertyWithOtherConfigurations(StringPropertyConfiguration stringPropertyConfig) { }

        protected virtual void ExtendCreationDatePropertyWithOtherConfigurations(DateTimePropertyConfiguration datePropertyConfig) { }

        protected virtual void ExtendLastUpdatedByPropertyWithOtherConfigurations(StringPropertyConfiguration stringPropertyConfig) { }

        protected virtual void ExtendLastUpdatedDatePropertyWithOtherConfigurations(DateTimePropertyConfiguration datePropertyConfig) { }

        #endregion 
    }
}
