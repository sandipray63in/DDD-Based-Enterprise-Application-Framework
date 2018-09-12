using Polly;

namespace Infrastructure.ExceptionHandling.PollyBasedExceptionHandling.Policies
{
    /// <summary>
    /// Some scenarios might need Action or Funcs to be injected as part of dependencies.
    /// https://stackoverflow.com/questions/9822363/how-to-register-a-class-that-has-func-as-parameter/9955052
    /// can be helpful in such scenarios
    /// </summary>
    public interface IPolicy 
    {
        Policy GetPolicy(PolicyBuilder policyBuilder);

        Policy GetPolicyAsync(PolicyBuilder policyBuilder);
    }
}
