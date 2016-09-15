using System;

namespace Infrastructure.Utilities
{
    /// <summary>
    /// Although .NET Framework's Contract.Requires should suffice in most of the cases 
    /// but it has the limitations that it cannot be applied on Class variables/properties and
    /// method local variables(can only be applied on method parameters).Also all Contract.Requires 
    /// code should be coded first and then any other code should be there else exception will 
    /// be thrown.To overcome these limitations, having this utility class
    /// 
    /// N.B. -> Whatever mentioned above doesn't mean that .NET Code Contracts is having poor 
    /// capabilities.Infact it's awesome.It has lot of optimized wirings inside it which is not 
    /// that simple as this class.
    /// 
    /// </summary>
    public static class ContractUtility
    {
        public static void Requires<TException>(bool conditionToBeSatisfied, string exceptionMessage)
            where TException : Exception
        {
            if (!conditionToBeSatisfied)
            {
                throw Activator.CreateInstance(typeof(TException), exceptionMessage) as TException;
            }
        }

        /// <summary>
        /// This is useful when exceptionMessage itself needs to be build after checking the value of conditionToBeSatisfied.
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <param name="conditionToBeSatisfied"></param>
        /// <param name="exceptionMessageFunc"></param>
        public static void Requires<TException>(bool conditionToBeSatisfied, Func<string> exceptionMessageFunc)
            where TException : Exception
        {
            if (!conditionToBeSatisfied)
            {
                var exceptionMessage = exceptionMessageFunc();
                throw Activator.CreateInstance(typeof(TException), exceptionMessage) as TException;
            }
        }
    }
}
