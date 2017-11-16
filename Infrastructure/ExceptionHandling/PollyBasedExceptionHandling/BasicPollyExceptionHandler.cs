using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Polly.Wrap;
using Infrastructure.ExceptionHandling.PollyBasedExceptionHandling.Policies;
using Infrastructure.Logging;
using Infrastructure.Logging.Loggers;
using Infrastructure.Utilities;

namespace Infrastructure.ExceptionHandling.PollyBasedExceptionHandling
{
    public class BasicPollyExceptionHandler : BaseExceptionHandler
    {
        private static readonly PolicyBuilder _policyBuilder = BuildPolicyBuilderFromPollyTransientFailureExceptionsXMLFile();
        private static IEnumerable<string> _splittedTransientFailureExceptions;
        private readonly bool _shouldThrowOnException;
        private readonly ILogger _logger;
        private readonly Func<IEnumerable<IPolicy>,PolicyWrap> _policyWrapForSyncOperationsFunc = x => PolicyWrap.Wrap(x.Select(y => y.GetPolicy(_policyBuilder)).ToArray());
        private readonly Func<IEnumerable<IPolicy>, PolicyWrap> _policyWrapForAsyncOperationsFunc = x => PolicyWrap.WrapAsync(x.Select(y => y.GetPolicyAsync(_policyBuilder)).ToArray());
        private readonly IEnumerable<IPolicy> _policies;
        private bool _areFallbackPoliciesAlreadyHandled;

        /// <summary>
        /// Polly based basic exception handler
        /// </summary>
        /// <param name="policies">while setting up unity config for policies, ideally the policies should be set in the order viz. 
        /// fallback, timeout, retry and then circuit breaker</param>
        /// <param name="logger"></param>
        /// <param name="shouldThrowOnException"></param>
        public BasicPollyExceptionHandler(IPolicy[] policies, ILogger logger, bool shouldThrowOnException)
        {
            _logger = logger ?? LoggerFactory.GetLogger(LoggerType.Default);
            _shouldThrowOnException = shouldThrowOnException;
            _policies = policies;
        }

        public override void HandleException(Action action, Action onExceptionCompensatingHandler = null)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (_splittedTransientFailureExceptions.IsNotNullOrEmpty() && _splittedTransientFailureExceptions.Contains(ex.GetType().Name) && _policies.IsNotNullOrEmpty())
                {
                    PolicyWrap policyWrap = GetPolicyWrapWithProperFallbackActionSetForFallbackPolicies(onExceptionCompensatingHandler);
                    policyWrap.Execute(action);
                }
                HandleExceptionWithThrowCondition(ex,onExceptionCompensatingHandler);
            }
        }

        public override TReturn HandleException<TReturn>(Func<TReturn> action, Action onExceptionCompensatingHandler = null)
        {
            TReturn returnValue = default(TReturn);
            try
            {
                returnValue = action();
            }
            catch (Exception ex)
            {
                if (_splittedTransientFailureExceptions.IsNotNullOrEmpty() && _splittedTransientFailureExceptions.Contains(ex.GetType().Name) && _policies.IsNotNullOrEmpty())
                {
                    PolicyWrap policyWrap = GetPolicyWrapWithProperFallbackActionSetForFallbackPolicies(onExceptionCompensatingHandler);
                    returnValue = policyWrap.Execute(action);
                }
                HandleExceptionWithThrowCondition(ex, onExceptionCompensatingHandler);
            }
            return returnValue;
        }

        public override async Task HandleExceptionAsync(Func<CancellationToken,Task> action, CancellationToken actionCancellationToken = default(CancellationToken), Func<CancellationToken, Task> onExceptionCompensatingHandler = null, CancellationToken onExceptionCompensatingHandlerCancellationToken = default(CancellationToken))
        {
            try
            {
                await action(actionCancellationToken);
            }
            catch (Exception ex)
            {
                if (_splittedTransientFailureExceptions.IsNotNullOrEmpty() && _splittedTransientFailureExceptions.Contains(ex.GetType().Name) && _policies.IsNotNullOrEmpty())
                {
                    PolicyWrap policyWrap = GetPolicyWrapWithProperFallbackActionSetForFallbackPoliciesAsync(onExceptionCompensatingHandler);
                    await policyWrap.ExecuteAsync(action,actionCancellationToken);
                }
                HandleExceptionWithThrowCondition(ex, onExceptionCompensatingHandler, onExceptionCompensatingHandlerCancellationToken);
            }
        }

        public override async Task<TReturn> HandleExceptionAsync<TReturn>(Func<CancellationToken,Task<TReturn>> func, CancellationToken funcCancellationToken = default(CancellationToken), Func<CancellationToken, Task> onExceptionCompensatingHandler = null, CancellationToken onExceptionCompensatingHandlerCancellationToken = default(CancellationToken))
        {
            Task<TReturn> returnValue = default(Task<TReturn>);
            try
            {
                returnValue = await func(funcCancellationToken) as Task<TReturn>;
            }
            catch (Exception ex)
            {
                if (_splittedTransientFailureExceptions.IsNotNullOrEmpty() && _splittedTransientFailureExceptions.Contains(ex.GetType().Name) && _policies.IsNotNullOrEmpty())
                {
                    PolicyWrap policyWrap = GetPolicyWrapWithProperFallbackActionSetForFallbackPoliciesAsync(onExceptionCompensatingHandler);
                    returnValue = await policyWrap.ExecuteAsync(func, funcCancellationToken) as Task<TReturn>;
                }
                HandleExceptionWithThrowCondition(ex, onExceptionCompensatingHandler, onExceptionCompensatingHandlerCancellationToken);
            }
            return await returnValue;
        }

        private static PolicyBuilder BuildPolicyBuilderFromPollyTransientFailureExceptionsXMLFile()
        {
            PolicyBuilder policyBuilder = null;
            XDocument xDoc = XDocument.Load(Path.Combine(typeof(BasicPollyExceptionHandler).Assembly.Location, "ExceptionHandling", "PollyBasedExceptionHandling", "PollyTransientFailureExceptions.xml"));
            PollyTransientFailureExceptions pollyTransientFailureExceptions = XMLUtility.DeSerialize<PollyTransientFailureExceptions>(xDoc.ToString());
            _splittedTransientFailureExceptions = pollyTransientFailureExceptions.TransientFailureExceptions.SelectMany(x => x.CommaSeperatedTransientFailureExceptions.Split(",",StringSplitOptions.RemoveEmptyEntries)).Distinct();

            if (_splittedTransientFailureExceptions.IsNotNullOrEmpty())
            {
                string firstTransientFailureException = _splittedTransientFailureExceptions.First();
                string assemblyName = pollyTransientFailureExceptions.TransientFailureExceptions.SingleOrDefault(x => x.CommaSeperatedTransientFailureExceptions.Contains(firstTransientFailureException)).AssemblyName;
                Type firstTransientFailureExceptionType = MetaDataUtility.GetTypeFromClassName(assemblyName, firstTransientFailureException);
                Type[] transientFailureExceptionTypesArray = new Type[1];
                transientFailureExceptionTypesArray[0] = firstTransientFailureExceptionType;
                policyBuilder = MetaDataUtility.InvokeStaticMethod<Policy, PolicyBuilder>("Handle", transientFailureExceptionTypesArray);

                IEnumerable<string> transientFailureExceptionsOtherThanTheFirst = _splittedTransientFailureExceptions.Skip(1);
                if (transientFailureExceptionsOtherThanTheFirst.IsNotNullOrEmpty())
                {
                    transientFailureExceptionsOtherThanTheFirst.ForEach(x =>
                     {
                         assemblyName = pollyTransientFailureExceptions.TransientFailureExceptions.SingleOrDefault(y => y.CommaSeperatedTransientFailureExceptions.Contains(x)).AssemblyName;
                         Type transientFailureExceptionTypeForOtherThanTheFirst = MetaDataUtility.GetTypeFromClassName(assemblyName, x);
                         Type[] transientFailureExceptionTypesArrayForOtherThanTheFirst = new Type[1];
                         transientFailureExceptionTypesArrayForOtherThanTheFirst[0] = transientFailureExceptionTypeForOtherThanTheFirst;
                         policyBuilder = MetaDataUtility.InvokeInstanceMethod<PolicyBuilder, PolicyBuilder>(policyBuilder, "Or", transientFailureExceptionTypesArrayForOtherThanTheFirst);
                     }
                    );
                }
            }
            return policyBuilder;
        }

        private PolicyWrap GetPolicyWrapWithProperFallbackActionSetForFallbackPolicies(Action fallbackAction)
        {
            IEnumerable<IPolicy> clonedPolicies = CloningUtility.Clone(_policies);
            if (fallbackAction.IsNotNull())
            {
                _areFallbackPoliciesAlreadyHandled = true;
                clonedPolicies.Where(x => x is IFallbackActionPolicy).Select(x => x as IFallbackActionPolicy).ForEach(x => x.SetFallbackAction(fallbackAction));
            }
            else
            {
                _areFallbackPoliciesAlreadyHandled = false;
                clonedPolicies = clonedPolicies.Where(x => !(x is IFallbackActionPolicy));
            }
            return _policyWrapForSyncOperationsFunc(clonedPolicies);
        }

        private PolicyWrap GetPolicyWrapWithProperFallbackActionSetForFallbackPoliciesAsync(Func<CancellationToken,Task> fallbackAction)
        {
            IEnumerable<IPolicy> clonedPolicies = CloningUtility.Clone(_policies);
            if (fallbackAction.IsNotNull())
            {
                _areFallbackPoliciesAlreadyHandled = true;
                clonedPolicies.Where(x => x is IFallbackActionPolicy).Select(x => x as IFallbackActionPolicy).ForEach(x => x.SetFallbackAction(fallbackAction));
            }
            else
            {
                _areFallbackPoliciesAlreadyHandled = false;
                clonedPolicies = clonedPolicies.Where(x => !(x is IFallbackActionPolicy));
            }
            return _policyWrapForAsyncOperationsFunc(clonedPolicies);
        }

        private void HandleExceptionWithThrowCondition(Exception ex, Action onExceptionCompensatingHandler)
        {
            if (ex.IsNotNull())
            {
                _logger.LogException(ex);
                if (onExceptionCompensatingHandler.IsNotNull() && !_areFallbackPoliciesAlreadyHandled)
                {
                    onExceptionCompensatingHandler();
                }
                if (_shouldThrowOnException)
                {
                    throw new Exception("Check Inner Exception", ex);
                }
            }
        }

        private void HandleExceptionWithThrowCondition(Exception ex, Func<CancellationToken, Task> onExceptionCompensatingHandler,CancellationToken onExceptionCompensatingHandlerCancellationToken)
        {
            if (ex.IsNotNull())
            {
                _logger.LogException(ex);
                if (onExceptionCompensatingHandler.IsNotNull() && !_areFallbackPoliciesAlreadyHandled)
                {
                    onExceptionCompensatingHandler(onExceptionCompensatingHandlerCancellationToken);
                }
                if (_shouldThrowOnException)
                {
                    throw new Exception("Check Inner Exception", ex);
                }
            }
        }
    }
}
