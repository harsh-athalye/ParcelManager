using System;

namespace ParcelManager
{
    class Program
    {
        static void Main(string[] args)
        {
            double _weight, _height, _width, _depth;
            Tuple<string,double> output=null;

            Console.Write("Enter Weight in kg: ");
            string weight = Console.ReadLine();
            if (!double.TryParse(weight, out _weight))
            {
                Console.WriteLine("Invalid Weight value entered.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter Height in cm: ");
            string height = Console.ReadLine();
            if (!double.TryParse(height, out _height))
            {
                Console.WriteLine("Invalid Height value entered.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter Width in cm: ");
            string width = Console.ReadLine();
            if (!double.TryParse(width, out _width))
            {
                Console.WriteLine("Invalid Width value entered.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter Depth in cm: ");
            string depth = Console.ReadLine();
            if (!double.TryParse(depth, out _depth))
            {
                Console.WriteLine("Invalid Depth value entered.");
                Console.ReadKey();
                return;
            }

            try
            {
                output = RuleEngine.ApplyRule(new Parcel(_weight, _height, _width, _depth));
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error occurred: {0}", ex.Message);
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nCategory: {0}", output.Item1);
            Console.WriteLine("Cost: {0}", (double.IsNaN(output.Item2) ? "N/A" : output.Item2.ToString("C0")));
            Console.ReadKey();
        }

        
    }
}

