using System;
using Domain.Base.AddOnObjects;

namespace Domain.Base.Entities.Composites
{
    /// <summary>
    /// Hopefully, 5 levels of generic arguments should suffice.
    /// </summary>
    public abstract class BaseEntityComposite<TId, T1> : BaseEntity<TId>
        where TId : struct
        where T1 : IAddOnObject, new()
    {
        public T1 T1Data { get; private set; }

        /// <summary>
        /// Entity Framework needs a default constructor while fetching data 
        /// from external data source
        /// </summary>
        public BaseEntityComposite() { }

        public BaseEntityComposite(TId id) : base(id)
        {
            T1Data = new T1(); 
        }
    }

    public abstract class BaseEntityComposite<TId, T1, T2> : BaseEntityComposite<TId, T1>
        where TId : struct
        where T1 : IAddOnObject, new()
        where T2 : IAddOnObject, new()
    {
        public T2 T2Data { get; private set; }

        /// <summary>
        /// Entity Framework needs a default constructor while fetching data 
        /// from external data source
        /// </summary>
        public BaseEntityComposite() { }

        public BaseEntityComposite(TId id) : base(id)
        {
            DomainContractUtility.Requires<ArgumentException>(typeof(T1) != typeof(T2),"The generic arcguments cannot be of the same type");
            DomainContractUtility.Requires<ArgumentException>(typeof(INoSqlAddOnObject).IsAssignableFrom<T1>()
                                                              ||
                                                              !typeof(INoSqlAddOnObject).IsAssignableFrom<T2>(), "Any AddOn object which is " +  
                                                              "of NoSql type can only be set as the 1st generic argument i.e. T1");
            DomainContractUtility.Requires<ArgumentException>(!typeof(INoSqlAddOnObject).IsAssignableFrom<T1>()
                                                              &&
                                                              typeof(T2) != typeof(AuditInfo) , "If T1 is not of NoSql type then "+
                                                              "it should be of type AuditInfo");
            T2Data = new T2();
        }
    }

    public abstract class BaseEntityComposite<TId, T1, T2,T3> : BaseEntityComposite<TId, T1,T2>
        where TId : struct
        where T1 : IAddOnObject, new()
        where T2 : IAddOnObject, new()
        where T3 : IAddOnObject, new()
    {
        public T3 T3Data { get; private set; }

        /// <summary>
        /// Entity Framework needs a default constructor while fetching data 
        /// from external data source
        /// </summary>
        public BaseEntityComposite() { }

        public BaseEntityComposite(TId id) : base(id)
        {
            DomainContractUtility.Requires<ArgumentException>(typeof(T1) != typeof(T2) && typeof(T2) != typeof(T3) 
                                                              && typeof(T3) != typeof(T1), 
                                                              "The generic arcguments cannot be of the same type");
            DomainContractUtility.Requires<ArgumentException>(typeof(INoSqlAddOnObject).IsAssignableFrom<T1>()
                                                             ||
                                                             !typeof(INoSqlAddOnObject).IsAssignableFrom<T2>(), "Any AddOn object which is " +
                                                             "of NoSql type can only be set as the 1st generic argument i.e. T1");
            DomainContractUtility.Requires<ArgumentException>(!typeof(INoSqlAddOnObject).IsAssignableFrom<T1>()
                                                              &&
                                                              typeof(T2) != typeof(AuditInfo), "If T1 is not of NoSql type then " +
                                                              "it should be of type AuditInfo");
            T3Data = new T3();
        }
    }

    public abstract class BaseEntityComposite<TId, T1, T2, T3,T4> : BaseEntityComposite<TId, T1, T2,T3>
        where TId : struct
        where T1 : IAddOnObject, new()
        where T2 : IAddOnObject, new()
        where T3 : IAddOnObject, new()
        where T4 : IAddOnObject, new()
    {
        public T4 T4Data { get; private set; }

        /// <summary>
        /// Entity Framework needs a default constructor while fetching data 
        /// from external data source
        /// </summary>
        public BaseEntityComposite() { }

        public BaseEntityComposite(TId id) : base(id)
        {
            DomainContractUtility.Requires<ArgumentException>(typeof(T1) != typeof(T2) && typeof(T2) != typeof(T3)
                                                              && typeof(T3) != typeof(T4) && typeof(T4) != typeof(T1),
                                                              "The generic arcguments cannot be of the same type");
            DomainContractUtility.Requires<ArgumentException>(typeof(INoSqlAddOnObject).IsAssignableFrom<T1>()
                                                             ||
                                                             !typeof(INoSqlAddOnObject).IsAssignableFrom<T2>(), "Any AddOn object which is " +
                                                             "of NoSql type can only be set as the 1st generic argument i.e. T1");
            DomainContractUtility.Requires<ArgumentException>(!typeof(INoSqlAddOnObject).IsAssignableFrom<T1>()
                                                              &&
                                                              typeof(T2) != typeof(AuditInfo), "If T1 is not of NoSql type then " +
                                                              "it should be of type AuditInfo");
            T4Data = new T4();
        }

    }

    public abstract class BaseEntityComposite<TId, T1, T2, T3, T4,T5> : BaseEntityComposite<TId, T1, T2, T3,T4>
        where TId : struct
        where T1 : IAddOnObject, new()
        where T2 : IAddOnObject, new()
        where T3 : IAddOnObject, new()
        where T4 : IAddOnObject, new()
        where T5 : IAddOnObject, new()
    {
        public T5 T5Data { get; private set; }

        /// <summary>
        /// Entity Framework needs a default constructor while fetching data 
        /// from external data source
        /// </summary>
        public BaseEntityComposite(){}

        public BaseEntityComposite(TId id) : base(id)
        {
            DomainContractUtility.Requires<ArgumentException>(typeof(T1) != typeof(T2) && typeof(T2) != typeof(T3)
                                                              && typeof(T3) != typeof(T4) && typeof(T4) != typeof(T5)
                                                              && typeof(T5) != typeof(T1),
                                                              "The generic arcguments cannot be of the same type");
            DomainContractUtility.Requires<ArgumentException>(typeof(INoSqlAddOnObject).IsAssignableFrom<T1>()
                                                             ||
                                                             !typeof(INoSqlAddOnObject).IsAssignableFrom<T2>(), "Any AddOn object which is " +
                                                             "of NoSql type can only be set as the 1st generic argument i.e. T1");
            DomainContractUtility.Requires<ArgumentException>(!typeof(INoSqlAddOnObject).IsAssignableFrom<T1>()
                                                              &&
                                                              typeof(T2) != typeof(AuditInfo), "If T1 is not of NoSql type then " +
                                                              "it should be of type AuditInfo");
            T5Data = new T5();
        }
    }
}
