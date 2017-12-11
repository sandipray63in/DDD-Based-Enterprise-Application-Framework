using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Infrastructure.Utilities
{
    public static class XSDUtility
    {
        public static string Validate(string assemblyName, string xsdPath, IEnumerable<string> xmlStringsToValidate)
        {
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(x => x.GetName().Name.Equals(assemblyName, StringComparison.InvariantCultureIgnoreCase));
            ContractUtility.Requires<ArgumentNullException>(assembly.IsNotNull(), assemblyName + " not found");
            return Validate(assembly, xsdPath, xmlStringsToValidate);
        }

        public static string Validate(Assembly assembly,string xsdPath, IEnumerable<string> xmlStringsToValidate)
        {
            XmlSchemaSet schemas = new XmlSchemaSet();
            using (Stream stream = assembly.GetManifestResourceStream(xsdPath))
            {
                schemas.Add("", XmlReader.Create(stream));
            }

            StringBuilder errorsStringBuilder = new StringBuilder();
            xmlStringsToValidate.ForEach(xmlString =>
                {
                    XDocument xDoc = XDocument.Parse(xmlString);
                    xDoc.Validate(schemas, (o, e) =>
                    {
                        errorsStringBuilder.Append(e.Message);
                        errorsStringBuilder.Append(Environment.NewLine);
                    });
                }
            );
            return errorsStringBuilder.ToString();
        }

        public static string Validate<TType>(string assemblyName, string xsdPath, IEnumerable<TType> typesToValidate)
        {
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(x => x.GetName().Name.Equals(assemblyName, StringComparison.InvariantCultureIgnoreCase));
            ContractUtility.Requires<ArgumentNullException>(assembly.IsNotNull(), assemblyName + " not found");
            return Validate(assembly, xsdPath, typesToValidate);
        }

        public static string Validate<TType>(Assembly assembly, string xsdPath, IEnumerable<TType> typesToValidate)
        {
            return Validate(assembly,xsdPath,typesToValidate.Select(x => XMLUtility.Serialize<TType>(x)));
        }
    }
}
