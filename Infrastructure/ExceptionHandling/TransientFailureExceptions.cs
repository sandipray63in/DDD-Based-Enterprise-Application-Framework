using System;
using System.Xml.Serialization;

namespace Infrastructure.ExceptionHandling
{
    [Serializable]
    [XmlRoot("TransientFailure")]
    public class TransientFailureExceptions
    {
        [XmlElement("Normal")]
        public string CommaSeperatedNormalTransientFailures { get; set; }

        [XmlElement("TimeConsuming")]
        public string CommaSeperatedTimeConsumingTransientFailures { get; set; }
    }
}
