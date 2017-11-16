using System;
using System.Xml.Serialization;

namespace Infrastructure.ExceptionHandling.PollyBasedExceptionHandling
{
    [Serializable]
    internal class TransientFailureException
    {
        [XmlAttribute]
        internal string AssemblyName { get; set; }

        [XmlText]
        internal string CommaSeperatedTransientFailureExceptions { get; set; }
    }
}
