using System;
using System.Collections.Generic;

namespace KuenstlicheIntelligenz
{
    class Program
    {
        static IOFunctions input = new IOFunctions();
        static List<Benchmark> benchmarks = new List<Benchmark>();
        static List<string> benchmark_name = new List<string>();

        static void Main(string[] args)
        {
            Console.SetWindowSize(Console.LargestWindowWidth / 2,Console.LargestWindowHeight / 2);

            // 1. Take the unsorted string from the input and parse it into the individual benchmark object
            for(int i = 0; i < input.benchmarks.Count; i++)
            {
                SplitVars splitting = new SplitVars(input.benchmarks[i]);
                benchmarks.Add(splitting.ReturnParsed());
                benchmark_name.Add(input.benchmarks_name[i]);
            }

            // 2. Calculate with the simplex algorithm for the individual benchmark object
            for(int i = 0; i < benchmarks.Count; i++)
            {
                SimplexMethod method = new SimplexMethod(benchmarks[i]);
                method.Calculate();
                benchmarks[i].result = method.matrix;
            }

            // 3. Return the results in the console window and results folder
            for (int i = 0; i < benchmarks.Count; i++)
            {
                //benchmarks[i].Write_Result();

                string[] output = benchmarks[i].Get_Result();
                for (int x = 0; x < output.Length; x++)
                {
                    Console.WriteLine(output[x]);
                }
                Console.WriteLine("");

                input.OutputBenchmark(output, benchmark_name[i]);
            }
            Console.ReadLine();

            // 1. Textdatei in Array einlesen                   xxxxxX
            // 2. Auseinanderziehen in einzelne Komponenten     xxxxxX
            // 3. Simplex algorithmus anwenden = Calculate      xxxxx
            // 4. Ergebnis ausgeben / LPSolve Überprüfung       xxxxx
        }
    }
}
