using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Infrastructure.DI;

namespace Infrastructure.Utilities
{
    public static class ContainerUtility
    {
        public static TType CheckRegistrationAndGetInstance<TType,TResolvedType>(params object[] parameterValues)
            where TResolvedType : TType
        {
            if (!Container.Instance.IsRegistered<TType>())
            {
                if (parameterValues.IsNullOrEmpty())
                {
                    Container.Instance.RegisterType<TType, TResolvedType>();
                }
                else
                {
                    Container.Instance.RegisterType<TType, TResolvedType>(new InjectionConstructor(parameterValues));
                }
            }
            return Container.Instance.Resolve<TType>();
        }

        public static TType CheckRegistrationAndGetInstance<TType, TResolvedType>(string name, params object[] parameterValues)
            where TResolvedType : TType
        {
            if (!Container.Instance.IsRegistered<TType>(name))
            {
                if (parameterValues.IsNullOrEmpty())
                {
                    Container.Instance.RegisterType<TType, TResolvedType>(name);
                }
                else
                {
                    Container.Instance.RegisterType<TType, TResolvedType>(name,new InjectionConstructor(parameterValues));
                }
            }
            return Container.Instance.Resolve<TType>(name);
        }

        public static object Resolve(Type type,IList<ParameterOverrideData> parameterOverrideDataList = null)
        {
            ParameterOverride[] parameterOverrides = null;
            if (parameterOverrideDataList.IsNotNullOrEmpty())
            {
                parameterOverrides = parameterOverrideDataList.Select(x => new ParameterOverride(x.ParameterName, x.ParameterValue)).To<ParameterOverride[]>();
            }
            return parameterOverrides.IsNotEmpty() ? Container.Instance.Resolve(type)
                :
                Container.Instance.Resolve(type, parameterOverrides);
        }

        public static object Resolve(Type type,string name, IList<ParameterOverrideData> parameterOverrideDataList)
        {
            ParameterOverride[] parameterOverrides = null;
            if (parameterOverrideDataList.IsNotNullOrEmpty())
            {
                parameterOverrides = parameterOverrideDataList.Select(x => new ParameterOverride(x.ParameterName, x.ParameterValue)).To<ParameterOverride[]>();
            }
            return parameterOverrides.IsNotEmpty() ? Container.Instance.Resolve(type,name)
                :
                Container.Instance.Resolve(type,name, parameterOverrides);
        }
    }
}
