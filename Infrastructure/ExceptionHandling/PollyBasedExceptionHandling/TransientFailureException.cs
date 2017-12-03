using System;
using System.Xml.Serialization;

namespace Infrastructure.ExceptionHandling.PollyBasedExceptionHandling
{
    [Serializable]
    internal class TransientFailureException
    {
        [XmlAttribute(AttributeName = "assemblyName")]
        internal string AssemblyName { get; set; }

        [XmlAttribute(AttributeName = "policies")]
        internal string CommaSeperatedPollyPoliciesNames { get; set; }

        [XmlText]
        internal string CommaSeperatedTransientFailureExceptions { get; set; }
    }
}
