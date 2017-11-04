using Microsoft.Practices.Unity.InterceptionExtension;
using Infrastructure.Extensions;
using Infrastructure.Logging.Loggers;

namespace Infrastructure.AoP.CrossCuttingInterceptionBehaviours
{
    public class MethodParametersValuesInterceptionBehaviour : ExceptionInterceptionBehaviour
    {
        public MethodParametersValuesInterceptionBehaviour(ILogger logger) : base(logger)
        {

        }

        protected override void ExecuteBeforeMethodInvocation(IMethodInvocation input)
        {
            MethodInvocationData methodInvocationData = input.GetMethodInvocationData();
            string preMethodMessage = string.Format("{0}{1}.{2}({3})", methodInvocationData.ClassName, methodInvocationData.Generic, methodInvocationData.MethodName, methodInvocationData.Arguments);
            logger.LogMessage(preMethodMessage);
        }

        protected override void ExecuteAfterMethodInvocation(IMethodInvocation input, IMethodReturn methodReturn)
        {
            MethodInvocationData methodInvocationData = input.GetMethodInvocationData();
            string postMethodMessage = string.Format("{0}{1}.{2}({3})", methodInvocationData.ClassName, methodInvocationData.Generic, methodInvocationData.MethodName, methodReturn.ReturnValue);
            logger.LogMessage(postMethodMessage);
        }
    }
}
