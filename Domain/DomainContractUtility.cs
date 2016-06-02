using System;

namespace Domain
{
    /// <summary>
    /// Although this class is having the same functionality as that of ContractUtility 
    /// within the Infrastructure/Utility folder, but replicating the class in this project 
    /// also just to avoid circular dependency between this library and Infrastructure 
    /// library.
    /// </summary>
    internal static class DomainContractUtility
    {
        public static void Requires<TException>(bool conditionToBeSatisfied, string exceptionMessage)
            where TException : Exception
        {
            if (!conditionToBeSatisfied)
            {
                throw Activator.CreateInstance(typeof(TException), exceptionMessage) as TException;
            }
        }
    }
}
