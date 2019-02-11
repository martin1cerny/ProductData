using System;
using System.Diagnostics;
using System.Linq;

namespace Examples
{
    /// <summary>
    /// This is very basic framework that will find all implementations of the 'AbstractExample' in this assembly and will run them.
    /// </summary>
    class Program
    {
        public static void Main()
        {
            var examples = typeof(Program).Assembly.GetTypes()
                .Where(t => typeof(AbstractExample).IsAssignableFrom(t) && !t.IsAbstract)
                .Select(t => Activator.CreateInstance(t))
                .Cast<AbstractExample>()
                .ToList();

            Console.WriteLine($"{examples.Count} examples found.");
            Console.WriteLine();

            foreach (var example in examples)
            {
                Console.WriteLine($"Running example '{example.Annotation}'");
                try
                {
                    var w = Stopwatch.StartNew();
                    example.Run();
                    w.Stop();
                    var c = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Example '{example.Annotation}' finished in {w.ElapsedMilliseconds}ms \r\n");
                    Console.ForegroundColor = c;
                }
                catch (Exception e)
                {
                    var c = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Example '{example.Annotation}' failed: {e.Message} \r\n");
                    Console.ForegroundColor = c;
                }

            }
        }
    }
}
