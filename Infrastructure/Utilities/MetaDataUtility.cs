using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructure.Utilities
{
    public static class MetaDataUtility
    {
        /// <summary>
        /// Gets the type of the class from the supplied class name and assembly names
        /// </summary>
        /// <param name="assemblyNames"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static Type GetTypeFromClassName(IEnumerable<string> assemblyNames, string className)
        {
            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => assemblyNames.Select(y => y.ToUpper().Trim()).Contains(x.GetName().Name.ToUpper().Trim()));
            ContractUtility.Requires<ArgumentNullException>(assemblies.IsNotNullOrEmpty(), assemblyNames + " not found");
            Type type = null;
            foreach(Assembly assembly in assemblies)
            {
                type = assembly.GetTypes().SingleOrDefault(x => x.Name.Contains(className, StringComparison.InvariantCultureIgnoreCase));
                if(type.IsNotNull())
                {
                    break;
                }
            }
            ContractUtility.Requires<ArgumentNullException>(type.IsNotNull(), className + " not found within " + assemblyNames);
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
