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
        private static string[] _splittedTransientFailureExceptions;
        private bool _shouldThrowOnException;
        private readonly ILogger _logger;
        private PolicyWrap _policyWrapForSyncOperations;
        private PolicyWrap _policyWrapForAsyncOperations;
        private readonly IEnumerable<IPolicy> _fallbackPolicies;

        public BasicPollyExceptionHandler(IPolicy[] policies, ILogger logger, bool shouldThrowOnException)
        {
            _logger = logger ?? LoggerFactory.GetLogger(LoggerType.Default);
            _shouldThrowOnException = shouldThrowOnException;
            if (policies.IsNotNullOrEmpty())
            {
                _fallbackPolicies = policies.Where(y => y is IFallbackActionPolicy);
                IEnumerable<IPolicy> notFallbackPolicies = policies.Where(y => !(y is IFallbackActionPolicy));
                if (notFallbackPolicies.IsNotNullOrEmpty())
                {
                    _policyWrapForSyncOperations = PolicyWrap.Wrap(notFallbackPolicies.Select(y => y.GetPolicy(_policyBuilder)).ToArray());
                    _policyWrapForAsyncOperations = PolicyWrap.WrapAsync(notFallbackPolicies.Select(y => y.GetPolicyAsync(_policyBuilder)).ToArray());
                }
            }
        }

        public override void HandleException(Action action, Action onExceptionCompensatingHandler = null)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (onExceptionCompensatingHandler.IsNotNull())
                {
                    SetPolicyWrapWithFallbackPolicies(_policyWrapForSyncOperations, onExceptionCompensatingHandler);
                }
                if (_policyWrapForSyncOperations.IsNotNull() && _splittedTransientFailureExceptions.IsNotNullOrEmpty() && _splittedTransientFailureExceptions.Contains(ex.GetType().Name))
                {
                    _policyWrapForSyncOperations.Execute(action);
                }
                HandleExceptionWithThrowCondition(ex,onExceptionCompensatingHandler);
            }
        }

        public override TReturn HandleException<TReturn>(Func<TReturn> action, Action onExceptionCompensatingHandler = null)
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                if (onExceptionCompensatingHandler.IsNotNull())
                {
                    SetPolicyWrapWithFallbackPolicies(_policyWrapForSyncOperations, onExceptionCompensatingHandler);
                }
                if (_policyWrapForSyncOperations.IsNotNull() && _splittedTransientFailureExceptions.IsNotNullOrEmpty() && _splittedTransientFailureExceptions.Contains(ex.GetType().Name))
                {
                    _policyWrapForSyncOperations.Execute(action);
                }
                HandleExceptionWithThrowCondition(ex, onExceptionCompensatingHandler);
            }
            return default(TReturn);
        }

        public override async Task HandleExceptionAsync(Func<Task> action, Func<CancellationToken, Task> onExceptionCompensatingHandler = null, CancellationToken onExceptionCompensatingHandlerCancellationToken = default(CancellationToken))
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                if (onExceptionCompensatingHandler.IsNotNull())
                {
                    SetPolicyWrapWithFallbackPoliciesForAsync(_policyWrapForAsyncOperations, onExceptionCompensatingHandler);
                }
                if (_policyWrapForAsyncOperations.IsNotNull() && _splittedTransientFailureExceptions.IsNotNullOrEmpty() && _splittedTransientFailureExceptions.Contains(ex.GetType().Name))
                {
                    await _policyWrapForAsyncOperations.ExecuteAsync(action);
                }
                HandleExceptionWithThrowCondition(ex, onExceptionCompensatingHandler, onExceptionCompensatingHandlerCancellationToken);
            }
        }

        public override async Task<TReturn> HandleExceptionAsync<TReturn>(Func<Task<TReturn>> action, Func<CancellationToken, Task> onExceptionCompensatingHandler = null, CancellationToken onExceptionCompensatingHandlerCancellationToken = default(CancellationToken))
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                if (onExceptionCompensatingHandler.IsNotNull())
                {
                    SetPolicyWrapWithFallbackPoliciesForAsync(_policyWrapForAsyncOperations, onExceptionCompensatingHandler);
                }
                if (_policyWrapForAsyncOperations.IsNotNull() && _splittedTransientFailureExceptions.IsNotNullOrEmpty() && _splittedTransientFailureExceptions.Contains(ex.GetType().Name))
                {
                    await _policyWrapForAsyncOperations.ExecuteAsync(action);
                }
                HandleExceptionWithThrowCondition(ex, onExceptionCompensatingHandler, onExceptionCompensatingHandlerCancellationToken);
            }
            return default(TReturn);
        }

        private static PolicyBuilder BuildPolicyBuilderFromPollyTransientFailureExceptionsXMLFile()
        {
            PolicyBuilder policyBuilder = null;
            XDocument xDoc = XDocument.Load(Path.Combine(typeof(BasicPollyExceptionHandler).Assembly.Location, "ExceptionHandling", "PollyBasedExceptionHandling", "PollyTransientFailureExceptions.xml"));
            PollyTransientFailureExceptions transientFailureExceptions = XMLUtility.DeSerialize<PollyTransientFailureExceptions>(xDoc.ToString());
            _splittedTransientFailureExceptions = transientFailureExceptions.CommaSeperatedTransientFailureExceptions.Split(",", StringSplitOptions.RemoveEmptyEntries);
            string[] splittedAssemblyNames = transientFailureExceptions.CommaSeperatedAssemblyNames.Split(",", StringSplitOptions.RemoveEmptyEntries);

            if (_splittedTransientFailureExceptions.IsNotNullOrEmpty())
            {
                string firstTransientFailureException = _splittedTransientFailureExceptions.First();
                Type firstTransientFailureExceptionType = MetaDataUtility.GetTypeFromClassName(splittedAssemblyNames, firstTransientFailureException);
                Type[] transientFailureExceptionTypesArray = new Type[1];
                transientFailureExceptionTypesArray[0] = firstTransientFailureExceptionType;
                policyBuilder = MetaDataUtility.InvokeStaticMethod<Policy, PolicyBuilder>("Handle", transientFailureExceptionTypesArray);

                IEnumerable<string> transientFailureExceptionsOtherThanTheFirst = _splittedTransientFailureExceptions.Skip(1);
                if (transientFailureExceptionsOtherThanTheFirst.IsNotNullOrEmpty())
                {
                    transientFailureExceptionsOtherThanTheFirst.ForEach(x =>
                     {
                         Type transientFailureExceptionTypeForOtherThanTheFirst = MetaDataUtility.GetTypeFromClassName(splittedAssemblyNames, x);
                         Type[] transientFailureExceptionTypesArrayForOtherThanTheFirst = new Type[1];
                         transientFailureExceptionTypesArrayForOtherThanTheFirst[0] = transientFailureExceptionTypeForOtherThanTheFirst;
                         policyBuilder = MetaDataUtility.InvokeInstanceMethod<PolicyBuilder, PolicyBuilder>(policyBuilder, "Or", transientFailureExceptionTypesArrayForOtherThanTheFirst);
                     }
                    );
                }
            }
            return policyBuilder;
        }

        private void SetPolicyWrapWithFallbackPolicies(PolicyWrap policyWrap,Action fallbackAction)
        {
            _fallbackPolicies.ForEach(x =>
                {
                    (x as IFallbackActionPolicy).SetFallbackAction(fallbackAction);
                    policyWrap = policyWrap.Wrap(x.GetPolicy(_policyBuilder));
                }
            );
        }

        private void SetPolicyWrapWithFallbackPoliciesForAsync(PolicyWrap policyWrap, Func<CancellationToken, Task> fallbackAction)
        {
            _fallbackPolicies.ForEach(x =>
                {
                    (x as IFallbackActionPolicy).SetFallbackAction(fallbackAction);
                    policyWrap = policyWrap.WrapAsync(x.GetPolicy(_policyBuilder));
                }
            );
        }

        private void HandleExceptionWithThrowCondition(Exception ex, Action onExceptionCompensatingHandler)
        {
            if (ex.IsNotNull())
            {
                _logger.LogException(ex);
                if (onExceptionCompensatingHandler.IsNotNull() && _fallbackPolicies.IsNullOrEmpty())
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
                if (onExceptionCompensatingHandler.IsNotNull() && _fallbackPolicies.IsNullOrEmpty())
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
