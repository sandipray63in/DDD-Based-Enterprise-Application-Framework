using System;
using System.Xml.Serialization;

namespace Infrastructure.ExceptionHandling.PollyBasedExceptionHandling
{
    [Serializable]
    [XmlRoot("PollyTransientFailures")]
    internal class PollyTransientFailureExceptions
    {
        [XmlElement("AssemblyNames")]
        internal string CommaSeperatedAssemblyNames { get; set; }

        [XmlElement("TransientFailureExceptions")]
        internal string CommaSeperatedTransientFailureExceptions { get; set; }
    }
}
