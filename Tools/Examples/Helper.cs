using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Xbim.Common;
using Xbim.Common.Step21;
using Xbim.Ifc;
using Xbim.IO.Xml;

namespace Examples
{
    public static class Helper
    {
        /// <summary>
        /// Gets credentials usable to create new models
        /// </summary>
        /// <param name="name"></param>
        /// <param name="surname"></param>
        /// <param name="organization"></param>
        /// <returns></returns>
        public static XbimEditorCredentials GetCredentials(string name, string surname, string organization)
        {
            return new XbimEditorCredentials
            {
                ApplicationDevelopersName = "xBIM Team",
                ApplicationFullName = "xBIM Toolkit",
                ApplicationIdentifier = "xBIM",
                ApplicationVersion = "4.0",
                EditorsFamilyName = name,
                EditorsGivenName = surname,
                EditorsOrganisationName = organization
            };
        }

        /// <summary>
        /// Creates new IFC4 in-memory model with given credentials to be used for all
        /// owner history objects and relations
        /// </summary>
        /// <param name="name">Data author's name</param>
        /// <param name="surname">Data author's surname</param>
        /// <param name="organization">Data author's organization</param>
        /// <returns></returns>
        public static IfcStore CreateModel(string name, string surname, string organization)
        {
            var credentials = GetCredentials(name, surname, organization);
            return IfcStore.Create(credentials, XbimSchemaVersion.Ifc4, XbimStoreType.InMemoryModel);
        }

        //public static  string GetPathInSamples(string directory, string name)
        //{
        //    var dir = Path.Combine(@"..\..\..\..\SampleFiles", directory);
        //    if (!Directory.Exists(dir))
        //        Directory.CreateDirectory(dir);

        //    return Path.Combine(dir, name);
        //}


        /// <summary>
        /// Writes IFC4 XML file with prefered root types. For example IfcProject or IfcProjectLibrary are usually 
        /// used as roots. But if for the purpose of an example there is other main entity that should be used as
        /// a root so that the general structure of the XML file is most clear for this entity.
        /// </summary>
        /// <param name="model">xBIM Model</param>
        /// <param name="path">XML output path</param>
        /// <param name="rootTypes">Prefered types. Orded is used for the preference.</param>
        public static void SaveXmlWithRoots(IModel model, string path, params Type[] rootTypes)
        {
            using (var xml = XmlWriter.Create(path, new XmlWriterSettings { Indent = true, IndentChars = "  " }))
            {
                var w = new XbimXmlWriter4(XbimXmlSettings.IFC4Add2);

                // by defining order we can say what will be used as the root object
                IEnumerable<IPersistEntity> entities = new IPersistEntity[0];
                foreach (var t in rootTypes)
                    entities = entities.Concat(model.Instances.OfType(t.Name, false));
                // add everything. Duplicated entities will be written only once.
                entities = entities.Concat(model.Instances);

                w.Write(model, xml, entities);
                xml.Close();
            }
        }
    }
}
