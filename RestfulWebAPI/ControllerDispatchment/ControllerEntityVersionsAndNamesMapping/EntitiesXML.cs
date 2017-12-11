using System;
using System.Xml.Serialization;

namespace RestfulWebAPI.ControllerDispatchment.ControllerEntityVersionsAndNamesMapping
{
    [Serializable]
    [XmlRoot("Entities")]    
    internal class EntitiesXML
    {
        [XmlAttribute(AttributeName = "defaultIDType")]
        internal string DefaultIDType { get; set; }

        [XmlArray("Entities")]
        [XmlArrayItem("entity", typeof(EntityXML))]
        internal EntityXML[] EntityXMLs { get; set; }
    }
}