using System;
using System.Xml.Serialization;

namespace RestfulWebAPI.ControllerDispatchment.ControllerEntityVersionsAndNamesMapping
{
    [Serializable]
    internal class ControllerXML
    {
        [XmlAttribute(AttributeName = "entityName")]
        internal string EntityName { get; set; }

        [XmlAttribute(AttributeName = "controllerName")]
        internal string ControllerName { get; set; }

        [XmlAttribute(AttributeName = "version")]
        internal string Version { get; set; }
    }
}