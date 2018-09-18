using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SchemaValidator
{
    class Program
    {
        const int SUCCESS = 0;
        const int FAILED = 1;

        [STAThread]
        static int Main(string[] args)
        {
            var file = "";
            var interactive = false;
            if (args.Length < 1)
            {
                var dlg = new OpenFileDialog
                {
                    Title = "Select IFC file for validation:",
                    Multiselect = false,
                    CheckFileExists = true,
                    Filter = "IFC STEP21|*.ifc|IFC XML|*.ifcxml",
                    FileName = Properties.Settings.Default.LastPath
                };
                if (dlg.ShowDialog() != DialogResult.OK)
                {
                    Console.WriteLine("No file selected");
                    return FAILED;
                }
                file = dlg.FileName;
                Properties.Settings.Default.LastPath = file;
                Properties.Settings.Default.Save();
                interactive = true;
            }
            else
                file = args[0];

            if (!File.Exists(file))
            {
                Console.WriteLine($"File '{file}' doesn't exist.");
                return FAILED;
            }

            var validator = new Validator();
            var ok = validator.Check(file);

            if (interactive)
            {
                if (!ok)
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Check finished. There are {validator.Errors.Count()} errors.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Press any key to quit...");
                Console.ReadKey();
            }

            return ok ? SUCCESS : FAILED;
        }
    }
}
