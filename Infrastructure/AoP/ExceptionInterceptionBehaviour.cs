using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity.InterceptionExtension;
using Infrastructure.Logging.Loggers;
using Infrastructure.Utilities;

namespace Infrastructure.AoP
{
    /// <summary>
    /// Acts as the base class for all other types of Interception Behaviours.
    /// This AoP approach can be used for normal exception handling as well as Exception Handling for async-await code.
    /// Exception Handling for async-await is discussed quite well at https://blogs.msdn.microsoft.com/ptorr/2014/12/10/async-exceptions-in-c/,
    /// http://www.informit.com/articles/article.aspx?p=2425865 , https://www.simple-talk.com/dotnet/net-framework/the-net-4-5-asyncawait-feature-in-promise-and-practice/ ,
    /// https://jeremybytes.blogspot.in/2015/01/task-and-await-basic-exception-handling.html and http://stackoverflow.com/questions/15503782/architecture-for-async-await.
    /// These async-await Exception Handling concepts can be used in an AoP way.
    /// </summary>
    public class ExceptionInterceptionBehaviour : IInterceptionBehavior
    {
        protected readonly ILogger logger;

        public ExceptionInterceptionBehaviour(ILogger logger)
        {
            ContractUtility.Requires<ArgumentNullException>(logger.IsNotNull(), "logger instance cannot be null");
            this.logger = logger;
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        /// <summary>
        /// For Exception handling scenario, refer - 
        /// http://stackoverflow.com/questions/9780908/using-unity-interception-to-solve-exception-handling-as-a-crosscutting-concern
        /// </summary>
        /// <param name="input"></param>
        /// <param name="getNext"></param>
        /// <returns></returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            try
            {
                ExecuteBeforeMethodInvocation(input);
                var methodReturn = getNext().Invoke(input, getNext);
                var methodReturnException = methodReturn.Exception;
                if (methodReturnException.IsNotNull())
                {
                    logger.LogException(methodReturnException);
                    throw new Exception("Method Return Exception.Please check inner exception", methodReturnException);
                }
                else
                {
                    ExecuteAfterMethodInvocation(input, methodReturn);
                }
                return methodReturn;
            }
            catch(Exception ex)
            {
                logger.LogException(ex);
                throw;
            }
        }

        public bool WillExecute { get; } = true;

        #region Overrideable Methods

        protected virtual void ExecuteBeforeMethodInvocation(IMethodInvocation input) { }
        protected virtual void ExecuteAfterMethodInvocation(IMethodInvocation input, IMethodReturn methodReturn) { }
        
        #endregion
    }
}
