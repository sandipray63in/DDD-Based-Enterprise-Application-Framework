using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Infrastructure.DI;

namespace Infrastructure.Utilities
{
    public static class ContainerUtility
    {
        public static TType CheckRegistrationAndGetInstance<TType,TResolvedType>(params ParameterTypeOverrideData[] parameterTypeOverrideDataValues)
            where TResolvedType : TType
        {
            if (!Container.Instance.IsRegistered<TType>())
            {
                if (parameterTypeOverrideDataValues.IsNullOrEmpty())
                {
                    Container.Instance.RegisterType<TType, TResolvedType>();
                }
                else
                {
                    var injectionParameterTypes = parameterTypeOverrideDataValues.Select(x => new InjectionParameter(x.ParameterType, x.ParameterValue)).ToArray();
                    Container.Instance.RegisterType<TType, TResolvedType>(new InjectionConstructor(injectionParameterTypes));
                }
            }
            return (TType)ContainerUtility.Resolve(typeof(TType), parameterTypeOverrideDataValues.Select(x => x as ParameterOverrideData).ToList());
        }

        public static TType CheckRegistrationAndGetInstance<TType, TResolvedType>(string name, params ParameterTypeOverrideData[] parameterTypeOverrideDataValues)
            where TResolvedType : TType
        {
            if (!Container.Instance.IsRegistered<TType>(name))
            {
                if (parameterTypeOverrideDataValues.IsNullOrEmpty())
                {
                    Container.Instance.RegisterType<TType, TResolvedType>(name);
                }
                else
                {
                    var injectionParameterTypes = parameterTypeOverrideDataValues.Select(x => new InjectionParameter(x.ParameterType, x.ParameterValue)).ToArray();
                    Container.Instance.RegisterType<TType, TResolvedType>(name,new InjectionConstructor(injectionParameterTypes));
                }
            }
            return (TType)ContainerUtility.Resolve(typeof(TType),name, parameterTypeOverrideDataValues.Select(x => x as ParameterOverrideData).ToList());
        }

        public static object Resolve(Type type,IList<ParameterOverrideData> parameterOverrideDataList = null)
        {
            ParameterOverride[] parameterOverrides = null;
            if (parameterOverrideDataList.IsNotNullOrEmpty())
            {
                parameterOverrides = parameterOverrideDataList.Where(x => x.ParameterValue.IsNotNull()).Select(x => new ParameterOverride(x.ParameterName, x.ParameterValue)).ToArray();
            }
            return parameterOverrides.IsNotEmpty() ? Container.Instance.Resolve(type)
                :
                Container.Instance.Resolve(type, parameterOverrides);
        }

        public static object Resolve(Type type,string name, IList<ParameterOverrideData> parameterOverrideDataList=null)
        {
            ParameterOverride[] parameterOverrides = null;
            if (parameterOverrideDataList.IsNotNullOrEmpty())
            {
                parameterOverrides = parameterOverrideDataList.Where(x => x.ParameterValue.IsNotNull()).Select(x => new ParameterOverride(x.ParameterName, x.ParameterValue)).ToArray();
            }
            return parameterOverrides.IsNotEmpty() ? Container.Instance.Resolve(type,name)
                :
                Container.Instance.Resolve(type,name, parameterOverrides);
        }
    }
}
