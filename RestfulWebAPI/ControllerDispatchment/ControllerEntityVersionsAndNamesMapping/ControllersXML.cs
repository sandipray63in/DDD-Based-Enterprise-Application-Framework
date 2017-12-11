using System;
using System.Xml.Serialization;

namespace RestfulWebAPI.ControllerDispatchment.ControllerEntityVersionsAndNamesMapping
{
    [Serializable]
    [XmlRoot("Controllers")]
    internal class ControllersXML
    {
        [XmlArray("Controllers")]
        [XmlArrayItem("controller", typeof(ControllerXML))]
        internal ControllerXML[] ControllerXMLs { get; set; }
    }
}
