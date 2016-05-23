using System;
using Microsoft.Practices.Unity;

namespace Infrastructure.Extensions
{
    public static class UnityContainerExtensions
    {
        public static bool IsTypeOrSomeBaseTypeRegistered<TType>(this IUnityContainer unityContainer,string nameToCheck = null, int maxDepthToCheckForBaseType = 0)
        {
            return IsTypeOrSomeBaseTypeRegistered(unityContainer, typeof(TType), nameToCheck, maxDepthToCheckForBaseType);
        }

        public static bool IsTypeOrSomeBaseTypeRegistered(this IUnityContainer unityContainer,object instance, string nameToCheck = null, int maxDepthToCheckForBaseType = 0)
        {
            return IsTypeOrSomeBaseTypeRegistered(unityContainer, instance.GetType(), nameToCheck, maxDepthToCheckForBaseType);
        }

        public static bool IsTypeOrSomeBaseTypeRegistered(this IUnityContainer unityContainer, Type type, string nameToCheck = null, int maxDepthToCheckForBaseType = 0)
        {
            return IsTypeOrSomeBaseTypeRegistered(unityContainer, type, nameToCheck, maxDepthToCheckForBaseType);
        }

        private static bool IsTypeOrSomeBaseTypeRegistered(IUnityContainer unityContainer, Type type, string nameToCheck = null, int maxDepthToCheckForBaseType = 0, int currentCountToCheckForBaseType = 0)
        {
            if ((nameToCheck.IsNullOrWhiteSpace() && unityContainer.IsRegistered(type))
                ||
                (nameToCheck.IsNotNullOrWhiteSpace() && unityContainer.IsRegistered(type,nameToCheck)) 
               )
            {
                return true;
            }
            if (maxDepthToCheckForBaseType > 0 && currentCountToCheckForBaseType > maxDepthToCheckForBaseType)
            {
                return false;
            }
            var baseType = type.BaseType;
            if (baseType.IsNull())
            {
                return false;
            }
            else
            {
                if (maxDepthToCheckForBaseType > 0)
                {
                    ++currentCountToCheckForBaseType;
                }
                return IsTypeOrSomeBaseTypeRegistered(unityContainer, baseType, nameToCheck, maxDepthToCheckForBaseType, currentCountToCheckForBaseType);
            }
        }
    }
}
