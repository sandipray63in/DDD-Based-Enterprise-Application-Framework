using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using Domain.Base.Entities;
using Infrastructure.Utilities;

namespace DomainContextsAndMaps.Base.EFBase
{
    public abstract class BaseEntityRootMap<TId,TEntity> : EntityTypeConfiguration<TEntity>
        where TId : struct
        where TEntity : BaseEntity<TId>
    {
        protected abstract string IDColumnName { get; }

        protected virtual DatabaseGeneratedOption DbIdGenerationOption { get; } = DatabaseGeneratedOption.None;

        public BaseEntityRootMap()
        {
            ContractUtility.Requires<ArgumentNullException>(IDColumnName.IsNotNullOrWhiteSpace(), "IDColumnName cannot be null or empty.");
            ExtendKeyIDWithOtherConfigurations(HasKey(p => p.Id));
            ExtendPropertyIDWithOtherConfigurations(Property(p => p.Id).HasColumnName(IDColumnName).HasDatabaseGeneratedOption(DbIdGenerationOption));
            SetEntitySpecificProperties();
            ToTable(TableName);
        }

        protected abstract void SetEntitySpecificProperties();

        protected abstract string TableName { get; }

        #region Overrideable Configuration Extensions

        protected virtual void ExtendKeyIDWithOtherConfigurations(EntityTypeConfiguration<TEntity> entityConfig) { }

        protected virtual void ExtendPropertyIDWithOtherConfigurations(PrimitivePropertyConfiguration propertyConfig) { }

        #endregion 
    }
}
