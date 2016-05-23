using System.Data.Entity.ModelConfiguration.Configuration;
using Domain.Base;

namespace EFDomainMaps.Base.EFBase
{
    /// <summary>
    /// To use this class, inherit it in child classes and override SetEntitySpecificProperties but within that first call
    /// base.SetEntitySpecificProperties() and then code the specific implementations for the child classes.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class BaseIdentityAndAuditableAndSoftDeleteableAggregateRootMap<TEntity> : BaseIdentityAndAuditableAggregateRootMap<TEntity> where TEntity : BaseIdentityAndAuditableAndSoftDeleteableCommandAggregateRoot
    {
        protected virtual string IsDeletedColumnName { get; } = "col_is_deleted";
        protected virtual string DeletedOnColumnName { get; } = "col_deleted_on";

        protected override void SetEntitySpecificProperties()
        {
            ExtendIsDeletedPropertyWithOtherConfigurations(Property(p => p.IsDeleted).HasColumnName(IsDeletedColumnName));
            ExtendDeletedOnPropertyWithOtherConfigurations(Property(p => p.DeletedOn).HasColumnName(DeletedOnColumnName));
        }

        #region Overrideable Configuration Extensions

        protected virtual void ExtendIsDeletedPropertyWithOtherConfigurations(PrimitivePropertyConfiguration propertyConfig) { }

        protected virtual void ExtendDeletedOnPropertyWithOtherConfigurations(DateTimePropertyConfiguration propertyConfig) { }

        #endregion
    }
}
