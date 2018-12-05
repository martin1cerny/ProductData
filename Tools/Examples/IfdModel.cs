using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Common;
using Xbim.Common.Metadata;
using Xbim.Common.Model;
using Xbim.Common.Step21;
using Xbim.IO.Step21;
using Xbim.ISO_12006_3_V3;

namespace Examples
{
    public class IfdModel : StepModel
    {
        private Dictionary<int, string> _comments = new Dictionary<int, string>();

        private IfdModel(string version, string date): base(new EntityFactoryIso120063Version3())
        {
            version = version ?? "1.0";
            date = date ?? DateTime.Now.ToString("s");

            // set globally unique ID where necessary
            EntityNew += (IPersistEntity entity) => {
                var idProps = entity.ExpressType
                    .Properties.Values
                    .Where(p => p.PropertyInfo.PropertyType == typeof(xtdGlobalUniqueID));
                foreach (var idProp in idProps)
                {
                    xtdGlobalUniqueID id = Guid.NewGuid().ToBase64();
                    idProp.PropertyInfo.SetValue(entity, id);
                }

                if (entity is xtdRoot root)
                {
                    root.VersionDate = date;
                    root.VersionID = version;
                }
            };
        }

        /// <summary>
        /// Default version and date to be used for new xtdRoot entities.
        /// </summary>
        /// <param name="version">Version which will be assigned to all new xtdRoot entities as VersionID</param>
        /// <param name="date">Date which will be assigned to all new xtdEntities as VersionDate</param>
        public static IfdModel Create(string version, DateTime date)
        {
            return new IfdModel(version, date.ToString("s"));
        }

        /// <summary>
        /// This will create new empty model. All xtdRoot entities will have 
        /// the default values VersionID="1.0", VersionDate = DateTime.Now
        /// </summary>
        /// <returns>Empty model ready for creation of the content</returns>
        public static IfdModel Create()
        {
            return new IfdModel("1.0", DateTime.Now.ToString("s"));
        }

        /// <summary>
        /// This will open existing IFD model and will set version and date to all new entities when created
        /// </summary>
        /// <param name="path">Path to IFD file (with .stp extension)</param>
        /// <param name="versionForNew">Version to be assigned to all new xtdRoot entities</param>
        /// <param name="dateForNew">Date to be assigned to all new xtdRoot entities</param>
        /// <returns></returns>
        public static IfdModel Open(string path, string versionForNew, DateTime dateForNew)
        {
            var model = new IfdModel(versionForNew, dateForNew.ToString("s"));
            model.LoadStep21(path);
            return model;
        }

        public void Comment(IPersistEntity entity, string comment)
        {
            if (_comments.TryGetValue(entity.EntityLabel, out string msg))
                _comments[entity.EntityLabel] = msg + "\r\n" + comment;
            else
                _comments.Add(entity.EntityLabel, comment);
        }

        public void Save(string file)
        {
            if (!file.EndsWith(".stp", StringComparison.InvariantCultureIgnoreCase))
                file = file + ".stp";
            using (var output = File.CreateText(file))
            {
                Write(output);
                output.Close();
            }
        }

        /// <summary>
        /// Writes full model into output writer as STEP21 file
        /// </summary>
        /// <param name="model">Model to be serialized</param>
        /// <param name="output">Output writer</param>
        /// <param name="metadata">Metadata to be used for serialization</param>
        /// <param name="map">Optional map can be used to map occurrences in the file</param>
        private void Write(TextWriter output)
        {
            ExpressMetaData metadata = Metadata;
            var header = Header ?? new StepFileHeader(StepFileHeader.HeaderCreationMode.InitWithXbimDefaults, this);
            string fallBackSchema = EntityFactory.SchemasIds.FirstOrDefault();

            if (!header.FileSchema.Schemas.Any())
            {
                foreach (var id in EntityFactory.SchemasIds)
                    header.FileSchema.Schemas.Add(id);
            }

            Part21Writer.WriteHeader(header, output, fallBackSchema);

            foreach (var entity in Instances)
            {
                if (_comments.TryGetValue(entity.EntityLabel, out string comment))
                {
                    comment = comment.Trim('\r', '\n');
                    comment = comment.Replace("\r\n", "\r\n* ");
                    output.WriteLine();
                    output.WriteLine($"/**");
                    output.WriteLine("* " + comment);
                    output.WriteLine($"* {GetMetaComment(entity)}*/");
                }
                Part21Writer.WriteEntity(entity, output, metadata, null);
                output.WriteLine();
            }
            Part21Writer.WriteFooter(output);
        }

        private string GetMetaComment(IPersistEntity e)
        {
            var tName = e.ExpressType.ExpressNameUpper;
            var props = e.ExpressType.Properties.Select(kv => kv.Value.Name);
            return $"{tName}=({string.Join(", ", props)})";
        }


        public T New<T>(Action<T> init = null) where T : IInstantiableEntity
        {
            var e = Instances.New<T>();
            init?.Invoke(e);
            return e;
        }

        public IEnumerable<T> Get<T>(Func<T, bool> condition =  null) where T : IPersistEntity
        {
            if (condition == null)
                return Instances.OfType<T>();
            return Instances.Where<T>(condition);
        }
    }

    public static class GuidExtensions
    {
        public static string ToBase64(this Guid guid)
        {
            var winBytes = guid.ToByteArray();

            var data1 = BitConverter.ToUInt32(winBytes, 0);
            var data2 = BitConverter.ToUInt16(winBytes, 4);
            var data3 = BitConverter.ToUInt16(winBytes, 6);


            // Creation of six 32 Bit integers from the components of the GUID structure
            var num = new uint[6];
            num[0] = (data1 / 16777216); //    16. byte  (pGuid->Data1 / 16777216) is the same as (pGuid->Data1 >> 24)
            num[1] = (data1 % 16777216);
            // 15-13. bytes (pGuid->Data1 % 16777216) is the same as (pGuid->Data1 & 0xFFFFFF)
            num[2] = (uint)(data2 * 256 + data3 / 256); // 12-10. bytes
            num[3] = (uint)((data3 % 256) * 65536 + winBytes[8] * 256 + winBytes[9]); // 09-07. bytes
            num[4] = (uint)(winBytes[10] * 65536 + winBytes[11] * 256 + winBytes[12]); // 06-04. bytes
            num[5] = (uint)(winBytes[13] * 65536 + winBytes[14] * 256 + winBytes[15]); // 03-01. bytes

            //convert nums to base 64 characters
            var digits = 2;
            var chars = new StringBuilder();
            for (var i = 0; i < 6; i++)
            {
                chars.Append(To64String(num[i], digits));
                digits = 4;
            }
            return chars.ToString();
        }

        /// <summary>
        ///   Helper function to convert from Guid to base 64 string
        /// </summary>
        private static string To64String(uint num, int nDigits)
        {
            //StringBuilder result= new StringBuilder();
            var result = new char[nDigits];
            var act = num;
            for (var iDigit = 0; iDigit < nDigits; iDigit++)
            {
                result[nDigits - iDigit - 1] = CConversionTable[(int)(act % 64)];
                act /= 64;
            }
            return new string(result);
        }

        private const string CConversionTable =
           //          1         2         3         4         5         6   
           //0123456789012345678901234567890123456789012345678901234567890123
           "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_$";
    }
}
