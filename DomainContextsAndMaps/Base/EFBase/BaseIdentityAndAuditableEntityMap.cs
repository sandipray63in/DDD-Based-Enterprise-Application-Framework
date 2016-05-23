using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using Domain.Base;
using Infrastructure.Utilities;

namespace EFDomainMaps.Base.EFBase
{
    public abstract class BaseIdentityAndAuditableAggregateRootMap<TEntity> : EntityTypeConfiguration<TEntity> where TEntity : BaseIdentityAndAuditableEntity
    { 
        protected virtual bool HasIDKeyColumn { get; } = true;

        protected virtual bool HasAuditColumns { get; } = true;

        protected virtual string IDColumnName { get; }

        protected virtual DatabaseGeneratedOption DbIdGenerationOption { get; } = DatabaseGeneratedOption.None;

        #region Actual Audit Columns Overridables

        protected virtual string CreatedByColumnName { get; } = "col_created_by";
        protected virtual string CreationDateColumnName { get; } = "col_creation_date";
        protected virtual string LastUpdatedByColumnName { get; } = "col_last_updated_by";
        protected virtual string LastUpdationDateColumnName { get; } = "col_last_updation_date";

        #endregion

        public BaseIdentityAndAuditableAggregateRootMap()
        {
            ContractUtility.Requires<ArgumentNullException>(!HasIDKeyColumn || IDColumnName.IsNotNullOrWhiteSpace(), "IDColumnName cannot be null or empty.If ID Column is not there " +
           "then override HasIDKeyColumn property and set it to false");
            ContractUtility.Requires<ArgumentNullException>(!HasAuditColumns || CreatedByColumnName.IsNotNullOrWhiteSpace(), "CreatedByColumnName cannot be null or empty.If Created By Column is not there " +
                   "then override HasIDKeyColumn property and set it to false");
            ContractUtility.Requires<ArgumentNullException>(!HasAuditColumns || CreationDateColumnName.IsNotNullOrWhiteSpace(), "CreationDateColumnName cannot be null or empty.If Creation Date Column is not there " +
               "then override HasIDKeyColumn property and set it to false");
            ContractUtility.Requires<ArgumentNullException>(!HasAuditColumns || LastUpdatedByColumnName.IsNotNullOrWhiteSpace(), "LastUpdatedByColumnName Name cannot be null or empty.If Last Updated By Column is not there " +
               "then override HasIDKeyColumn property and set it to false");
            ContractUtility.Requires<ArgumentNullException>(!HasAuditColumns || LastUpdationDateColumnName.IsNotNullOrWhiteSpace(), "LastUpdationDateColumnName Name cannot be null or empty.If Last Updation Date Column is not there " +
               "then override HasIDKeyColumn property and set it to false");

            if (HasIDKeyColumn)
            {
                ExtendKeyIDWithOtherConfigurations(HasKey(p => p.Id));
                ExtendPropertyIDWithOtherConfigurations(Property(p => p.Id).HasColumnName(IDColumnName).HasDatabaseGeneratedOption(DbIdGenerationOption));
            }

            SetEntitySpecificProperties();

            if (HasAuditColumns)
            {
                ExtendCreatedByPropertyWithOtherConfigurations(Property(p => p.CreatedBy).HasColumnName(CreatedByColumnName));
                ExtendCreationDatePropertyWithOtherConfigurations(Property(p => p.CreatedOn).HasColumnName(CreationDateColumnName));
                ExtendLastUpdatedByPropertyWithOtherConfigurations(Property(p => p.LastUpdatedBy).HasColumnName(LastUpdatedByColumnName));
                ExtendLastUpdatedDatePropertyWithOtherConfigurations(Property(p => p.LastUpdateOn).HasColumnName(LastUpdationDateColumnName));
            }

            ToTable(TableName);
        }
         
        protected abstract void SetEntitySpecificProperties();

        protected abstract string TableName { get; }

        #region Overrideable Configuration Extensions

        protected virtual void ExtendKeyIDWithOtherConfigurations(EntityTypeConfiguration<TEntity> entityConfig) { }

        protected virtual void ExtendPropertyIDWithOtherConfigurations(PrimitivePropertyConfiguration propertyConfig) { }

        protected virtual void ExtendCreatedByPropertyWithOtherConfigurations(StringPropertyConfiguration stringPropertyConfig) { }

        protected virtual void ExtendCreationDatePropertyWithOtherConfigurations(DateTimePropertyConfiguration datePropertyConfig) { }

        protected virtual void ExtendLastUpdatedByPropertyWithOtherConfigurations(StringPropertyConfiguration stringPropertyConfig) { }

        protected virtual void ExtendLastUpdatedDatePropertyWithOtherConfigurations(DateTimePropertyConfiguration datePropertyConfig) { }

        #endregion 

    }
}
