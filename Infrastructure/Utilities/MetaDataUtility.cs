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
        public static Type GetTypeFromClassName(string assemblyName, string className)
        {
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(x => x.GetName().Name.Equals(assemblyName,StringComparison.InvariantCultureIgnoreCase));
            ContractUtility.Requires<ArgumentNullException>(assembly.IsNotNull(), assemblyName + " not found");
            Type type = assembly.GetTypes().SingleOrDefault(x => x.Name.Equals(className, StringComparison.InvariantCultureIgnoreCase));
            ContractUtility.Requires<ArgumentNullException>(type.IsNotNull(), className + " not found within " + assemblyName);
            return type;
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
            ParameterExpression parameterExpression = Expression.Parameter(typeof(object[]));
            MethodCallExpression methodCallExpression = Expression.Call(typeof(TType), methodName, methodGenericArgumentTypes, methodAruments.Select(x => Expression.Constant(x)).ToArray()) ;
            return Expression.Lambda<TMethodReturnType>(methodCallExpression, parameterExpression).Compile();
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
            ParameterExpression parameterExpression = Expression.Parameter(typeof(object[]));
            MethodCallExpression methodCallExpression = Expression.Call(Expression.Constant(instance), methodName, methodGenericArgumentTypes, methodAruments.Select(x => Expression.Constant(x)).ToArray());
            return Expression.Lambda<TMethodReturnType>(methodCallExpression, parameterExpression).Compile();
        }
    }
}
