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
        private readonly T1 _dataT1;

        public BaseEntityComposite(TId id) : base(id)
        {
            _dataT1 = new T1(); 
        }

        public T1 T1Data { get; private set; }
    }

    public abstract class BaseEntityComposite<TId, T1, T2> : BaseEntityComposite<TId, T1>
        where TId : struct
        where T1 : IAddOnObject, new()
        where T2 : IAddOnObject, new()
    {
        private readonly T2 _dataT2;

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
            _dataT2 = new T2();
        }

        public T2 T2Data { get; private set; }
    }

    public abstract class BaseEntityComposite<TId, T1, T2,T3> : BaseEntityComposite<TId, T1,T2>
        where TId : struct
        where T1 : IAddOnObject, new()
        where T2 : IAddOnObject, new()
        where T3 : IAddOnObject, new()
    {
        private readonly T3 _dataT3;

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
            _dataT3 = new T3();
        }

        public T3 T3Data { get; private set; }
    }

    public abstract class BaseEntityComposite<TId, T1, T2, T3,T4> : BaseEntityComposite<TId, T1, T2,T3>
        where TId : struct
        where T1 : IAddOnObject, new()
        where T2 : IAddOnObject, new()
        where T3 : IAddOnObject, new()
        where T4 : IAddOnObject, new()
    {
        private readonly T4 _dataT4;

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
            _dataT4 = new T4();
        }

        public T4 T4Data { get; private set; }
    }

    public abstract class BaseEntityComposite<TId, T1, T2, T3, T4,T5> : BaseEntityComposite<TId, T1, T2, T3,T4>
        where TId : struct
        where T1 : IAddOnObject, new()
        where T2 : IAddOnObject, new()
        where T3 : IAddOnObject, new()
        where T4 : IAddOnObject, new()
        where T5 : IAddOnObject, new()
    {
        private readonly T5 _dataT5;

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
            _dataT5 = new T5();
        }

        public T5 T5Data { get; private set; }
    }
}
