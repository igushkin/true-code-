using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.extension
{
    public class MyConsole
    {
        public static string ask(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine();
        }

        public static int askInt(string prompt, int minValue = int.MinValue, int maxValue = int.MaxValue)
        {
            Console.WriteLine(prompt);
            int input;

            while (!int.TryParse(Console.ReadLine(), out input) || input < minValue || input > maxValue) {
                Console.WriteLine("Wrong input. Try again");
            }

            return input;
        }

        public static Guid askGuid(string prompt)
        {
            Console.WriteLine(prompt);
            Guid input;

            while (!Guid.TryParse(Console.ReadLine(), out input))
            {
                Console.WriteLine("Wrong input. Try again");
            }

            return input;
        }

    }
}
