using System;
using System.Xml.Serialization;

namespace Infrastructure.ExceptionHandling.PollyBasedExceptionHandling
{
    [Serializable]
    internal class TransientFailureException
    {
        [XmlAttribute(AttributeName = "assemblyName")]
        internal string AssemblyName { get; set; }

        [XmlAttribute(AttributeName = "partialOrFullExceptionNames")]
        internal string CommaSeperatedTransientFailureExceptions { get; set; }

        [XmlText]
        internal string CommaSeperatedPollyPoliciesNames { get; set; }
    }
}
