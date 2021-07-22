using System;
using System.Collections.Generic;
using System.Text;

namespace KuenstlicheIntelligenz
{
    class Benchmark
    {
        public string name;
        public string obj_function;
        public double[] min;
        public double[,] matrix;
        public double[,] result;

        public Benchmark()
        {
            obj_function = "";
        }

        public string[] Get_Result()
        {
            string[] tmp = new string[result.GetLength(0) - min.Length];

            int counter = 0;
            for (int x = min.Length - 1; x < result.GetLength(0) - 1; x++)
            {
                tmp[counter] = "Variable " + counter + ": " + result[x, result.GetLength(1) - 1];
                counter++;
            }
            tmp[tmp.Length - 1] = "Result: " + result[result.GetLength(0) - 1, result.GetLength(1) - 1];

            return tmp;
        }
        public void Write_Result()
        {
            Console.WriteLine(string.Empty.PadLeft(Console.WindowWidth - Console.CursorLeft, '─') + "\n");
            for (int x = min.Length - 1; x < result.GetLength(0); x++)
            {
                Console.WriteLine("Var" + x + ": " + result[x, result.GetLength(1) - 1]);
            }
            Console.WriteLine("\n" + string.Empty.PadLeft(Console.WindowWidth - Console.CursorLeft, '─') + "\n");
        }
        public void Write_Matrix()
        {
            if(matrix != null)
            {
                Console.WriteLine(string.Empty.PadLeft(Console.WindowWidth - Console.CursorLeft, '─') + "\n");
                Console.WriteLine("Matrix constructed as followed: \n");
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    for (int x = 0; x < matrix.GetLength(0); x++)
                    {
                        Console.Write("| ");
                        if (matrix[x, y] < 10)
                        {
                            Console.Write(matrix[x, y] + "  ");
                        }
                        if (matrix[x, y] >= 10 && matrix[x, y] < 100)
                        {
                            Console.Write(matrix[x, y] + " ");
                        }
                        if (matrix[x, y] >= 100)
                        {
                            Console.Write(matrix[x, y] + "");
                        }
                    }
                    Console.WriteLine(" |");
                }
                Console.WriteLine("\n" + string.Empty.PadLeft(Console.WindowWidth - Console.CursorLeft, '─') + "\n");
            }
            else
            {
                throw new ArgumentNullException("No Matrix found");    
            }
        }
    }
}
