using System;
using System.IO;

namespace SchemaValidator
{
    class Program
    {
        const int SUCCESS = 0;
        const int FAILED = 1;

        static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("No file specified for the check");
                return FAILED;
            }
            var file = args[0];
            if (!File.Exists(file))
            {
                Console.WriteLine($"File '{file}' doesn't exist.");
                return FAILED;
            }

            var validator = new Validator();
            var ok = validator.Check(file);

            return ok ? SUCCESS : FAILED;
        }
    }
}
