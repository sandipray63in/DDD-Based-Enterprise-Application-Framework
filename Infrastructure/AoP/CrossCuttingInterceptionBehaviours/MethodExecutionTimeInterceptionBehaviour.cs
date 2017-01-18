using System.Diagnostics;
using Microsoft.Practices.Unity.InterceptionExtension;
using Infrastructure.Extensions;
using Infrastructure.Logging.Loggers;

namespace Infrastructure.AoP.CrossCuttingInterceptionBehaviours
{
    public class MethodExecutionTimeInterceptionBehaviour : ExceptionInterceptionBehaviour
    {
        private Stopwatch stopWatch = new Stopwatch();

        public MethodExecutionTimeInterceptionBehaviour(ILogger logger) : base(logger)
        {

        }

        protected override void ExecuteBeforeMethodInvocation(IMethodInvocation input)
        {
            stopWatch.Start();
        }

        protected override void ExecuteAfterMethodInvocation(IMethodInvocation input, IMethodReturn methodReturn)
        {
            stopWatch.Stop();
            var elapsedTime = stopWatch.Elapsed.ToString();
            var methodInvocationData = input.GetMethodInvocationData();
            var executionTimeSpanMessage = string.Format(" The method {0} within class {1} finsished execution in time : {2}", methodInvocationData.MethodName, methodInvocationData.ClassName, elapsedTime);
            logger.LogMessage(executionTimeSpanMessage);
        }
    }
}
