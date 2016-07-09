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
    /// P.S. -> Apart from above metioned stuffs, this class wil be also helpful for scenarios 
    /// wherein you don't have Visual Studio 2015 Enterprise Edition(which is required if you want
    /// to run the application manually or using unit test cases, in order to make it work for 
    /// Contract.Requires which comes under System.Diagnostics.Contracts namespace available within
    /// the Contracts.DLL).But ideally, you should use Contract.Requires wherever possible, if you 
    /// build some Enterprise app for production environment using Visual Studio 2015 
    /// Enterprise Edition
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
    }
}
