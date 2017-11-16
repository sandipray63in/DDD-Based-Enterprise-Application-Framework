using System;
using System.Xml.Serialization;

namespace Infrastructure.ExceptionHandling.PollyBasedExceptionHandling
{
    [Serializable]
    [XmlRoot("PollyTransientFailures")]
    internal class PollyTransientFailureExceptions
    {
        [XmlArray("PollyTransientFailures")]
        [XmlArrayItem("TransientFailureException", typeof(TransientFailureException))]
        internal TransientFailureException[] TransientFailureExceptions { get; set; }
    }
}
