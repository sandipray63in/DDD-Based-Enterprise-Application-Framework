using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Infrastructure.Extensions
{
    public static  class AoPExtensions
    {
        public static MethodInvocationData GetMethodInvocationData(this IMethodInvocation input)
        {
            return new MethodInvocationData
            {
                ClassName = input.MethodBase.DeclaringType.Name,
                MethodName = input.MethodBase.Name,
                //Todo - Need to test the StringJoin part
                Generic = input.MethodBase.DeclaringType.IsGenericType ? string.Format("<{0}>", input.MethodBase.DeclaringType.GetGenericArguments().StringJoin(",")) : string.Empty,
                //Todo - Need to test the StringJoin part
                Arguments = input.Arguments.Cast<ParameterInfo>().StringJoin(",")
            };
        }
    }
}
