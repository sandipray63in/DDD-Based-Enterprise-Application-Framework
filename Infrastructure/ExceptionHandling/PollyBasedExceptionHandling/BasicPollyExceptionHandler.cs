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
        private static readonly ILogger _staticLoggerInstance = LoggerFactory.GetLogger(LoggerType.Default);
        private static readonly PolicyBuilder _policyBuilder = BuildPolicyBuilderFromPollyTransientFailureExceptionsXMLFile();
        private static PollyTransientFailureExceptions _pollyTransientFailureExceptions;
        private static IEnumerable<string> _splittedTransientFailureExceptions;
        private readonly bool _shouldThrowOnException;
        private readonly ILogger _logger;
        private readonly Func<IEnumerable<IPolicy>, PolicyWrap> _policyWrapForSyncOperationsFunc = x => PolicyWrap.Wrap(x.Select(y => y.GetPolicy(_policyBuilder)).ToArray());
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
                ExceptionHandlingUtility.WrapActionWithExceptionHandling(() =>
                {
                    if (_splittedTransientFailureExceptions.IsNotNullOrEmpty() && _splittedTransientFailureExceptions.Contains(ex.GetType().Name) && _policies.IsNotNullOrEmpty())
                    {
                        PolicyWrap policyWrap = GetPolicyWrapWithProperFallbackActionSetForFallbackPolicies(ex, onExceptionCompensatingHandler);
                        policyWrap.Execute(action);
                    }
                }, _logger);
                HandleExceptionWithThrowCondition(ex, onExceptionCompensatingHandler);
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
                returnValue = ExceptionHandlingUtility.WrapFuncWithExceptionHandling(() =>
                {
                    if (_splittedTransientFailureExceptions.IsNotNullOrEmpty() && _splittedTransientFailureExceptions.Contains(ex.GetType().Name) && _policies.IsNotNullOrEmpty())
                    {
                        PolicyWrap policyWrap = GetPolicyWrapWithProperFallbackActionSetForFallbackPolicies(ex, onExceptionCompensatingHandler);
                        return policyWrap.Execute(action);
                    }
                    return default(TReturn);
                }, _logger);
                HandleExceptionWithThrowCondition(ex, onExceptionCompensatingHandler);
            }
            return returnValue;
        }

        public override async Task HandleExceptionAsync(Func<CancellationToken, Task> action, CancellationToken actionCancellationToken = default(CancellationToken), Func<CancellationToken, Task> onExceptionCompensatingHandler = null, CancellationToken onExceptionCompensatingHandlerCancellationToken = default(CancellationToken))
        {
            try
            {
                await action(actionCancellationToken);
            }
            catch (Exception ex)
            {
                ExceptionHandlingUtility.WrapActionWithExceptionHandling(async () =>
                {
                    if (_splittedTransientFailureExceptions.IsNotNullOrEmpty() && _splittedTransientFailureExceptions.Contains(ex.GetType().Name) && _policies.IsNotNullOrEmpty())
                    {
                        PolicyWrap policyWrap = GetPolicyWrapWithProperFallbackActionSetForFallbackPoliciesAsync(ex, onExceptionCompensatingHandler);
                        await policyWrap.ExecuteAsync(action, actionCancellationToken);
                    }
                }, _logger);
                HandleExceptionWithThrowCondition(ex, onExceptionCompensatingHandler, onExceptionCompensatingHandlerCancellationToken);
            }
        }

        public override async Task<TReturn> HandleExceptionAsync<TReturn>(Func<CancellationToken, Task<TReturn>> func, CancellationToken funcCancellationToken = default(CancellationToken), Func<CancellationToken, Task> onExceptionCompensatingHandler = null, CancellationToken onExceptionCompensatingHandlerCancellationToken = default(CancellationToken))
        {
            Task<TReturn> returnValue = default(Task<TReturn>);
            try
            {
                returnValue = await func(funcCancellationToken) as Task<TReturn>;
            }
            catch (Exception ex)
            {
                returnValue = await ExceptionHandlingUtility.WrapFuncWithExceptionHandling(async () =>
                 {
                     if (_splittedTransientFailureExceptions.IsNotNullOrEmpty() && _splittedTransientFailureExceptions.Contains(ex.GetType().Name) && _policies.IsNotNullOrEmpty())
                     {
                         PolicyWrap policyWrap = GetPolicyWrapWithProperFallbackActionSetForFallbackPoliciesAsync(ex, onExceptionCompensatingHandler);
                         return await policyWrap.ExecuteAsync(func, funcCancellationToken) as Task<TReturn>;
                     }
                     return default(Task<TReturn>);
                 }, _logger);

                HandleExceptionWithThrowCondition(ex, onExceptionCompensatingHandler, onExceptionCompensatingHandlerCancellationToken);
            }
            return await returnValue;
        }

        private static PolicyBuilder BuildPolicyBuilderFromPollyTransientFailureExceptionsXMLFile()
        {
            return ExceptionHandlingUtility.WrapFuncWithExceptionHandling(() =>
            {
                PolicyBuilder policyBuilder = null;
                XDocument xDoc = XDocument.Load(Path.Combine(typeof(BasicPollyExceptionHandler).Assembly.Location, "ExceptionHandling", "PollyBasedExceptionHandling", "PollyTransientFailureExceptions.xml"));
                _pollyTransientFailureExceptions = XMLUtility.DeSerialize<PollyTransientFailureExceptions>(xDoc.ToString());
                _splittedTransientFailureExceptions = _pollyTransientFailureExceptions.TransientFailureExceptions.SelectMany(x => x.CommaSeperatedTransientFailureExceptions.Split(",", StringSplitOptions.RemoveEmptyEntries)).Distinct();

                if (_splittedTransientFailureExceptions.IsNotNullOrEmpty())
                {
                    string firstTransientFailureException = _splittedTransientFailureExceptions.First();
                    string assemblyName = _pollyTransientFailureExceptions.TransientFailureExceptions.SingleOrDefault(x => x.CommaSeperatedTransientFailureExceptions.Contains(firstTransientFailureException)).AssemblyName;
                    Type firstTransientFailureExceptionType = MetaDataUtility.GetType(assemblyName, firstTransientFailureException);
                    Type[] transientFailureExceptionTypesArray = new Type[1];
                    transientFailureExceptionTypesArray[0] = firstTransientFailureExceptionType;
                    policyBuilder = MetaDataUtility.InvokeStaticMethod<Policy, PolicyBuilder>("Handle", transientFailureExceptionTypesArray);

                    IEnumerable<string> transientFailureExceptionsOtherThanTheFirst = _splittedTransientFailureExceptions.Skip(1);
                    if (transientFailureExceptionsOtherThanTheFirst.IsNotNullOrEmpty())
                    {
                        transientFailureExceptionsOtherThanTheFirst.ForEach(x =>
                         {
                             assemblyName = _pollyTransientFailureExceptions.TransientFailureExceptions.SingleOrDefault(y => y.CommaSeperatedTransientFailureExceptions.Contains(x)).AssemblyName;
                             Type transientFailureExceptionTypeForOtherThanTheFirst = MetaDataUtility.GetType(assemblyName, x);
                             Type[] transientFailureExceptionTypesArrayForOtherThanTheFirst = new Type[1];
                             transientFailureExceptionTypesArrayForOtherThanTheFirst[0] = transientFailureExceptionTypeForOtherThanTheFirst;
                             policyBuilder = MetaDataUtility.InvokeInstanceMethod<PolicyBuilder, PolicyBuilder>(policyBuilder, "Or", transientFailureExceptionTypesArrayForOtherThanTheFirst);
                         }
                        );
                    }
                }
                return policyBuilder;
            }, _staticLoggerInstance);
        }

        private PolicyWrap GetPolicyWrapWithProperFallbackActionSetForFallbackPolicies(Exception ex, Action fallbackAction)
        {
            IEnumerable<IPolicy> policiesForCurrentException = GetPoliciesForException(ex);
            if (fallbackAction.IsNotNull())
            {
                _areFallbackPoliciesAlreadyHandled = true;
                policiesForCurrentException.Where(x => x is IFallbackActionPolicy).Select(x => x as IFallbackActionPolicy).ForEach(x => x.SetFallbackAction(fallbackAction));
            }
            else
            {
                _areFallbackPoliciesAlreadyHandled = false;
                policiesForCurrentException = policiesForCurrentException.Where(x => !(x is IFallbackActionPolicy));
            }
            return _policyWrapForSyncOperationsFunc(policiesForCurrentException);
        }

        private PolicyWrap GetPolicyWrapWithProperFallbackActionSetForFallbackPoliciesAsync(Exception ex, Func<CancellationToken, Task> fallbackAction)
        {
            IEnumerable<IPolicy> policiesForCurrentException = GetPoliciesForException(ex);
            if (fallbackAction.IsNotNull())
            {
                _areFallbackPoliciesAlreadyHandled = true;
                policiesForCurrentException.Where(x => x is IFallbackActionPolicy).Select(x => x as IFallbackActionPolicy).ForEach(x => x.SetFallbackAction(fallbackAction));
            }
            else
            {
                _areFallbackPoliciesAlreadyHandled = false;
                policiesForCurrentException = policiesForCurrentException.Where(x => !(x is IFallbackActionPolicy));
            }
            return _policyWrapForAsyncOperationsFunc(policiesForCurrentException);
        }

        private IEnumerable<IPolicy> GetPoliciesForException(Exception ex)
        {
            string exceptionTypeName = ex.GetType().Name;
            IEnumerable<string> pollyExceptionPoliciesFromXMLFileToBeAppliedForCurrentException = _pollyTransientFailureExceptions.TransientFailureExceptions
                                                                                                 .SingleOrDefault(x => x.CommaSeperatedTransientFailureExceptions.Contains(exceptionTypeName))
                                                                                                 .CommaSeperatedPollyPoliciesNames.Split(",", StringSplitOptions.RemoveEmptyEntries)
                                                                                                 .Distinct();
            IEnumerable<IPolicy> clonedPolicies = CloningUtility.Clone(_policies);
            clonedPolicies = clonedPolicies.Where(x => pollyExceptionPoliciesFromXMLFileToBeAppliedForCurrentException.Contains(x.GetType().Name));
            IEnumerable<string> pollyExceptionPoliciesPresentInXMLFileForCurrentExceptionButNotInjectedInDependencies =
                                pollyExceptionPoliciesFromXMLFileToBeAppliedForCurrentException.Except(clonedPolicies.Select(x => x.GetType().Name));
            if (pollyExceptionPoliciesPresentInXMLFileForCurrentExceptionButNotInjectedInDependencies.IsNotNullOrEmpty())
            {
                _logger.LogWarning("The following transient failures are part of the polly exception xml file for the current exception viz. "
                                    + exceptionTypeName + " but not injected as part of the dependencies : " + Environment.NewLine
                                    + pollyExceptionPoliciesPresentInXMLFileForCurrentExceptionButNotInjectedInDependencies.Aggregate((a, b) => a + Environment.NewLine + b));
            }
            return clonedPolicies;
        }

        private void HandleExceptionWithThrowCondition(Exception ex, Action onExceptionCompensatingHandler)
        {
            _logger.LogException(ex);
            if (onExceptionCompensatingHandler.IsNotNull() && !_areFallbackPoliciesAlreadyHandled)
            {
                _areFallbackPoliciesAlreadyHandled = true;
                onExceptionCompensatingHandler();
            }
            if (_shouldThrowOnException)
            {
                throw new Exception("Check Inner Exception", ex);
            }

        }

        private void HandleExceptionWithThrowCondition(Exception ex, Func<CancellationToken, Task> onExceptionCompensatingHandler, CancellationToken onExceptionCompensatingHandlerCancellationToken)
        {
            _logger.LogException(ex);
            if (onExceptionCompensatingHandler.IsNotNull() && !_areFallbackPoliciesAlreadyHandled)
            {
                _areFallbackPoliciesAlreadyHandled = true;
                onExceptionCompensatingHandler(onExceptionCompensatingHandlerCancellationToken);
            }
            if (_shouldThrowOnException)
            {
                throw new Exception("Check Inner Exception", ex);
            }
        }
    }
}
