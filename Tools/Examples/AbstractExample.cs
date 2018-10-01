using ColorCode;
using ColorCode.Formatting;
using ColorCode.Styling.StyleSheets;
using SchemaValidator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using Xbim.Common;
using Xbim.Ifc;

namespace Examples
{
    /// <summary>
    /// Examples inherited from this abstract class have acces to a range of functions which help
    /// to develop valid IFC4 files with comments. Both STEP21 and XML files are produced
    /// both containing comments in the right place
    /// </summary>
    public abstract class AbstractExample
    {
        private Dictionary<int, string> _comments = new Dictionary<int, string>();
        protected IfcStore model;

        protected IList<string> ModelViews => model.Header.FileDescription.Description;

        /// <summary>
        /// Helper function to simplify the code to create IFC model content. It is just a shortcut.
        /// </summary>
        /// <typeparam name="T">Type to be created. This has to come from IFC4 namespace</typeparam>
        /// <param name="init">Function used to initialize values in the newly created object.</param>
        /// <returns></returns>
        protected T New<T>(Action<T> init = null) where T : IInstantiableEntity
        {
            return init == null ?
                model.Instances.New<T>() :
                model.Instances.New<T>(init);
        }

        /// <summary>
        /// Creates new entity and keeps the comment for it. When model is saved these comments will
        /// be injected in the file created to make it easier to create example files.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="comment"></param>
        /// <returns></returns>
        protected T New<T>(string comment) where T : IInstantiableEntity
        {
            var entity = model.Instances.New<T>();
            _comments.Add(entity.EntityLabel, comment);
            return entity;
        }

        /// <summary>
        /// Sets a comment for the entity. This will be injected into the resulting file.
        /// </summary>
        /// <param name="entity">Entity to be commented</param>
        /// <param name="comment">Commenting text</param>
        protected void Comment(IPersistEntity entity, string comment)
        {
            // append to existing comment
            if (_comments.ContainsKey(entity.EntityLabel))
                _comments[entity.EntityLabel] = $"{_comments[entity.EntityLabel]}\r\n{comment}";
            // create new comment
            else
                _comments.Add(entity.EntityLabel, comment);
        }

        /// <summary>
        /// Constructor creates model for example model creation automatically.
        /// This is always IFC4 model. Credentials provided to constructor
        /// will be used in 'Model Owner History' object in IFC
        /// </summary>
        /// <param name="name">Data author name</param>
        /// <param name="surname">Data author surname</param>
        /// <param name="organization">Data author's organization</param>
        public AbstractExample(string name, string surname, string organization)
        {
            model = Helper.CreateModel(name, surname, organization);
        }

        /// <summary>
        /// Function to be used to run the example
        /// </summary>
        public abstract void Run();

        /// <summary>
        /// Annotation to be inserted as a comment in the very beginning of the result
        /// </summary>
        public abstract string Annotation { get; }

        /// <summary>
        /// Checks schema errors like properties cardinality and IFC schema 'where rules'.
        /// This automatically prints errors to the console. Every example should run thi in the end 
        /// so that only valid models are produced as a reference data.
        /// </summary>
        /// <param name="throwOnErrors">Throws an exception when there are errors in the model</param>
        /// <returns>List of errors. Model is valid if this is empty.</returns>
        protected IEnumerable<Error> Validate(bool throwOnErrors, string logFile = null)
        {
            var validator = new Validator();
            var ok = validator.Check(model, logFile);
            if (throwOnErrors && !ok)
                throw new Exception($"Model contains errors: \r\n{string.Concat("\r\n", validator.Errors.Select(e => e.Message))}");
            return validator.Errors;
        }

        /// <summary>
        /// This is a helper function which will save model as IFC and IFCXML and will inject all comments defined for 
        /// any entity in the model. This will also run a validation and will create a log file with any issues existing
        /// in the model.
        /// </summary>
        /// <param name="pathWithoutExtention">Path withoout extension. Extensions .ifc, .ifcxml and .log will be used to create files</param>
        /// <param name="throwOnSchemaErrors">Makes the validator to throw an exception when there are errors. Files will not be
        /// generated in that case</param>
        /// <param name="rootTypes">Root (main) types for XML serialization</param>
        protected void SaveAs(string pathWithoutExtention, bool throwOnSchemaErrors,  params Type[] rootTypes)
        {
            var ifc = pathWithoutExtention + ".ifc";
            var xml = pathWithoutExtention + ".ifcxml";
            var log = pathWithoutExtention + ".log";

            // make sure all errors are logged alongside the files created
            Validate(throwOnSchemaErrors, log);

            model.SaveAs(ifc);
            AddCommentsToStep21(ifc);

            Helper.SaveXmlWithRoots(model, xml, rootTypes);
            AddCommentsToXml(xml);

            SaveColorizedStep(ifc);
        }

        private string GetMetadata()
        {
            using (var w = new StringWriter())
            {
                w.WriteLine();
                w.WriteLine();
                w.WriteLine($"Created: {DateTime.UtcNow.ToString("s")} (UTC)");

                return w.ToString();
            }
        }

        private void SaveColorizedStep(string file)
        {
            var content = File.ReadAllText(file);
            var html = file + ".htm";
            var css = (StyleSheets.Default as DefaultStyleSheet).GetCssFile(); ;
            using (var w = File.CreateText(html))
            {
                w.WriteLine(@"<!DOCTYPE html>
                    <html lang=""en"">
                      <head>
                        <meta charset = ""utf-8"">
                        <title>" + Path.GetFileNameWithoutExtension(file) + @"</title>
                        <style>" + css + @"

                        .step .number {
                            color: red;
                        }
                        .type {
                            color: blue;
                        }
                        </style>
                      </head>
                      <body>");

                var cc = new CodeColorizer();
                cc.Colorize(content, Languages.Step, new HtmlClassFormatter(), StyleSheets.Default, w);

                w.WriteLine(@"</body></html>");
                w.Close();
            }
        }

        private static readonly Regex idExp = new Regex("^#(?<id>[0-9]+)");

        private void AddCommentsToStep21(string path)
        {
            if (!_comments.Any() && string.IsNullOrWhiteSpace(Annotation))
                return;

            var lines = File.ReadAllLines(path);
            // override
            using (var w = File.CreateText(path))
            {
                // annotation
                if (!string.IsNullOrWhiteSpace(Annotation))
                {
                    w.WriteLine("/****************************************************************");
                    var r = new StringReader(Annotation + GetMetadata());
                    var commentLine = "";
                    while ((commentLine = r.ReadLine()) != null)
                        w.WriteLine($" * {commentLine}");
                    w.WriteLine(" ****************************************************************/");
                }

                // comments
                foreach (var line in lines)
                {
                    var idStr = idExp.Match(line).Groups["id"].Value;
                    if (string.IsNullOrWhiteSpace(idStr))
                    {
                        w.WriteLine(line);
                        continue;
                    }
                    var id = int.Parse(idStr);
                    if (!_comments.TryGetValue(id, out string comment))
                    {
                        w.WriteLine(line);
                        continue;
                    }

                    // empty line to create visual distinction
                    var entity = model.Instances[id];
                    w.WriteLine();
                    w.WriteLine("/* * *");
                    var r = new StringReader(comment);
                    var commentLine = "";
                    while (!string.IsNullOrWhiteSpace(commentLine = r.ReadLine()))
                        w.WriteLine($" * {commentLine}");
                    // write attribute names to make the content more clear
                    w.WriteLine($" * {entity.ExpressType.ExpressNameUpper}({string.Join(", ", GetStructureDescription(entity))}) */");
                    w.WriteLine(line);
                }
            }
        }

        private IEnumerable<string> GetStructureDescription(IPersistEntity entity)
        {
            var type = entity.ExpressType;
            return type.Properties.Select(p => p.Value.Name);
        }

        private void AddCommentsToXml(string xml)
        {
            if (!_comments.Any() && string.IsNullOrWhiteSpace(Annotation))
                return;

            var doc = XDocument.Parse(File.ReadAllText(xml));

            // annotation
            if (!string.IsNullOrWhiteSpace(Annotation))
            {
                var r = new StringReader(Annotation + GetMetadata());
                var commentLine = "";
                while ((commentLine = r.ReadLine()) != null)
                {
                    var xCom = new XComment($" {commentLine} ");
                    doc.Root.AddBeforeSelf(xCom);
                }
            }

            // comments
            var elements = new Stack<XElement>(doc.Elements());
            while (elements.Any())
            {
                var element = elements.Pop();

                var idAttr = element.Attribute("id");
                if (idAttr != null && int.TryParse(idAttr.Value.Substring(1), out int id))
                {
                    if (_comments.TryGetValue(id, out string comment))
                    {
                        var r = new StringReader(comment);
                        var commentLine = "";
                        while (!string.IsNullOrWhiteSpace(commentLine = r.ReadLine()))
                        {
                            var xmlComment = new XComment($" {commentLine} ");
                            element.AddBeforeSelf(xmlComment);
                        }

                    }
                }

                foreach (var e in element.Elements())
                    elements.Push(e);
            }

            // override the original
            using (var w = XmlWriter.Create(xml, new XmlWriterSettings { Indent = true, IndentChars = "  " }))
            {
                doc.WriteTo(w);
            }
        }
    }
}
