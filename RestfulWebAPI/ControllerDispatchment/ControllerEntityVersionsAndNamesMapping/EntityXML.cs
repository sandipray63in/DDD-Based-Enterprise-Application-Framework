using System;
using System.Xml.Serialization;

namespace RestfulWebAPI.ControllerDispatchment.ControllerEntityVersionsAndNamesMapping
{
    [Serializable]
    internal class EntityXML
    {
        [XmlAttribute(AttributeName = "name")]
        internal string Name { get; set; }

        [XmlAttribute(AttributeName = "idType")]
        internal string IdType { get; set; }
    }
}