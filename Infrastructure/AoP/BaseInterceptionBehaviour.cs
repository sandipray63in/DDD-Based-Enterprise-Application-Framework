using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity.InterceptionExtension;
using Infrastructure.SemanticLogging.CrossCuttingEventSources;

namespace Infrastructure.AoP
{

    public abstract class BaseInterceptionBehaviour : IInterceptionBehavior
    {
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
                    ExceptionLogEvents.Log.LogException(methodReturnException.ToString());
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
                ExceptionLogEvents.Log.LogException(ex.ToString());
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
