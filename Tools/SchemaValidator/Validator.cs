using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbim.Common;
using Xbim.Common.Enumerations;
using Xbim.Common.Exceptions;
using Xbim.Common.ExpressValidation;
using Xbim.Ifc;
using Xbim.Ifc.Validation;

namespace SchemaValidator
{
    public class Validator
    {
        public IEnumerable<Error> Errors => appender.Errors;

        private ILog log;
        private ErrorAppender appender;

        private bool CheckInternal(IModel model)
        {
            // check for parser exceptions
            var v = new IfcValidator
            {
                ValidateLevel = ValidationFlags.All,
                CreateEntityHierarchy = true
            };
            var errors = v.Validate(model.Instances);
            foreach (var err in errors)
            {
                var identity = err.Item.GetType().Name;
                if (err.Item is IPersistEntity entity)
                {
                    identity = $"#{entity.EntityLabel}={entity.ExpressType.ExpressName}";
                }
                var msg = new StringBuilder();
                msg.AppendLine($"{identity} is invalid.");
                var details = new Stack<ValidationResult>(err.Details);
                while (details.Any())
                {
                    var detail = details.Pop();
                    foreach (var d in detail.Details)
                        details.Push(d);

                    var report = detail.Message;
                    if (string.IsNullOrWhiteSpace(report))
                        report = detail.Report();
                    msg.AppendLine("    " + report);
                }
                log.Error(msg.ToString());
            }

            return !errors.Any();
        }

        public bool Check(IModel model, string logFile = null)
        {
            // set up error logger handler to get errors from parser
            appender = Logger.Setup(logFile);
            log = LogManager.GetLogger("Validator");

            return CheckInternal(model);
        }

        public bool Check(string file)
        {
            // set up error logger handler to get errors from parser
            var logFile = file + ".log";
            appender = Logger.Setup(logFile);
            log = LogManager.GetLogger("Validator");

            try
            {
                // open as an in-memory model (all syntactic errors fill be picked up)
                using (var model = IfcStore.Open(file, null, -1))
                {
                    // STEP21 syntactic errors will be reported in the log
                    if (appender.Errors.Any())
                        // do not proceed because the data is incomplete
                        return false;

                    CheckInternal(model);
                }
            }
            // XML syntactic errors will be fired as an exception
            catch (XbimParserException pe)
            {
                log.Error($"Parser failure: {pe.Message}.");
                return false;
            }
            catch (Exception ge)
            {
                log.Error($"General failure: {ge.Message}.", ge);
                return false;
            }

            return !Errors.Any();
        }
    }
}
