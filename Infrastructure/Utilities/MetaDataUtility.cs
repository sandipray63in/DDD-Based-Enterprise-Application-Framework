using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructure.Utilities
{
    public static class MetaDataUtility
    {
        /// <summary>
        /// Gets the type of the class from the supplied class name and assembly name
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static Type GetType(string assemblyName, string className)
        {
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(x => x.GetName().Name.Equals(assemblyName,StringComparison.InvariantCultureIgnoreCase));
            ContractUtility.Requires<ArgumentNullException>(assembly.IsNotNull(), assemblyName + " not found");
            Type type = assembly.GetTypes().SingleOrDefault(x => x.Name.Equals(className, StringComparison.InvariantCultureIgnoreCase));
            ContractUtility.Requires<ArgumentNullException>(type.IsNotNull(), className + " not found within " + assemblyName);
            return type;
        }

        /// <summary>
        /// Gets the type of the generic class from the supplied open generic type(e.g typeof(Task<>)), genericArgumentsAssemblyName and genericArgumentsClassNames
        /// </summary>
        /// <param name="genericOpenType"></param>
        /// <param name="genericArgumentsAssemblyName"></param>
        /// <param name="genericArgumentsClassNames"></param>
        /// <returns></returns>
        public static Type GetGenericType(Type genericOpenType,string genericArgumentsAssemblyName, string[] genericArgumentsClassNames,Type[] extraExplicitGenericArgumentTypes = null)
        {
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(x => x.GetName().Name.Equals(genericArgumentsAssemblyName, StringComparison.InvariantCultureIgnoreCase));
            ContractUtility.Requires<ArgumentNullException>(assembly.IsNotNull(), genericArgumentsAssemblyName + " not found");
            Type[] genericArgumentTypes = assembly.GetTypes().Where(x => genericArgumentsClassNames.Contains(x.Name)).ToArray();
            ContractUtility.Requires<ArgumentNullException>(genericArgumentTypes.IsNotNull(), genericArgumentsAssemblyName + " does not contain " + genericArgumentsClassNames.Aggregate((a,b) => a + " or " + b));
            ContractUtility.Requires<ArgumentOutOfRangeException>(genericArgumentTypes.IsNotEmpty(), genericArgumentsAssemblyName + " does not contain " + genericArgumentsClassNames.Aggregate((a, b) => a + " or " + b));
            if(extraExplicitGenericArgumentTypes.IsNotNullOrEmpty())
            {
                genericArgumentTypes.AddRangeIfNotContains(extraExplicitGenericArgumentTypes);
            }
            return genericOpenType.MakeGenericType(genericArgumentTypes);
        }

        /// <summary>
        /// e.g. Policy.Handle<TException>()(this method can return a PolicyBuilder object) 
        /// </summary>
        /// <typeparam name="TType">Policy in the example mentioned above</typeparam>
        /// <typeparam name="TMethodReturnType">PolicyBuilder in the example mentioned above</typeparam>
        /// <param name="methodName">"Handle" method in the example mentioned above</param>
        /// <param name="methodGenericArgumentTypes">TException in the example mentioned above</param>
        /// <param name="methodAruments">In the example mentioned above, no method arguments used but there can be</param>
        /// <returns></returns>
        public static TMethodReturnType InvokeStaticMethod<TType,TMethodReturnType>(string methodName, Type[] methodGenericArgumentTypes = null, params object[] methodAruments)
        {
            MethodCallExpression methodCallExpression = Expression.Call(typeof(TType), methodName, methodGenericArgumentTypes, methodAruments.Select(x => Expression.Constant(x)).ToArray()) ;
            return Expression.Lambda<Func<TMethodReturnType>>(methodCallExpression).Compile()();
        }

        /// <summary>
        /// PolicyBuilder(Instance).Or<TException>()((this method can return a PolicyBuilder object))
        /// </summary>
        /// <typeparam name="TType">PolicyBuilder in the example mentioned above</typeparam>
        /// <typeparam name="TMethodReturnType">PolicyBuilder in the example mentioned above</typeparam>
        /// <param name="methodName">"Or" method in the example mentioned above</param>
        /// <param name="methodGenericArgumentTypes">TException in the example mentioned above</param>
        /// <param name="methodAruments">In the example mentioned above, no method arguments used but there can be</param>
        /// <returns></returns>
        public static TMethodReturnType InvokeInstanceMethod<TType, TMethodReturnType>(TType instance,string methodName, Type[] methodGenericArgumentTypes = null, params object[] methodAruments)
        {
            MethodCallExpression methodCallExpression = Expression.Call(Expression.Constant(instance), methodName, methodGenericArgumentTypes, methodAruments.Select(x => Expression.Constant(x)).ToArray());
            return Expression.Lambda<Func<TMethodReturnType>>(methodCallExpression).Compile()();
        }
    }
}
