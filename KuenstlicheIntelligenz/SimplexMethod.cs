using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace KuenstlicheIntelligenz
{
    class SimplexMethod
    {
        public Benchmark parsed;

        public double[] max;

        public double[,] matrix;

        private Stopwatch timer = new Stopwatch();

        public SimplexMethod(Benchmark parse)
        {
            parsed = parse;
        }
        public void Calculate()
        {
            timer.Start();

            // Turn into squared matrix
            matrix = parsed.matrix;
            matrix = Turn_into_Squared();

            // 1. Turn into maximize problem
            matrix = Transpose(matrix);
            max = Turn_Min_In_Max();

            // Fill in the negative values
            for (int i = 0; i < matrix.GetLength(0) - 1; i++)
            {
                matrix[i, matrix.GetLength(1) - 1] = max[i];
            }

            // Build the Slack Matrix
            matrix = Build_Slack_Matrix();
            
            // Solve Algorithm
            Solve();

            timer.Stop();
            Show_Time();
        }

        // Converts the positive values into negatives and adds 1
        public double[] Turn_Min_In_Max()
        {
            double[] tmp = new double[matrix.GetLength(0)];

            for (int i = 0; i < matrix.GetLength(0) - 1; i++)
            {
                tmp[i] -= matrix[i, matrix.GetLength(1) - 1];
            }
            tmp[tmp.Length - 1] = 1;

            return tmp;
        }

        public void Solve()
        {

            bool isSolved = false;
            while (!isSolved)
            {
                // Get the pivot element
                int ppos_x = Pivot_X();
                int ppos_y = Pivot_Y(ppos_x);
                double pivot = matrix[ppos_x, ppos_y];

                // Turn Pivot Row 1
                double[] new_row = Divide_Pivot_Row(ppos_y, pivot);
                for (int x = 0; x < matrix.GetLength(0); x++)
                {
                    matrix[x, ppos_y] = new_row[x];
                }

                ColumnTo0(ppos_x, ppos_y);
                isSolved = Check_If_Solved();
            }
        }

        // Create a squared matrix
        public double[,] Turn_into_Squared()
        {
            double[,] tmp = new double[matrix.GetLength(0), matrix.GetLength(0)];
            double[] row = new double[matrix.GetLength(0)];
            double[] mins = new double[matrix.GetLength(0)];

            int missing_rows = matrix.GetLength(0) - matrix.GetLength(1);

            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                row[x] = matrix[x, 0];
                mins[x] = matrix[x, matrix.GetLength(1) - 1];
            }

            for (int x = 0; x < tmp.GetLength(0); x++)
            {
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    tmp[x, y] = matrix[x, y];
                }
            }

            for (int y = matrix.GetLength(1) - 1; y < tmp.GetLength(1); y++)
            {
                for (int x = 0; x < tmp.GetLength(0); x++)
                {
                    tmp[x, y] = row[x];
                }
            }

            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                tmp[x, tmp.GetLength(1) - 1] = mins[x];
            }

            return tmp;
        }

        public int Pivot_X()
        {
            int ppos_x;

            double[] last_row = new double[matrix.GetLength(0)];
            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                last_row[x] = matrix[x, matrix.GetLength(1) - 1];
            }

            ppos_x = Find_Biggest_Negative(last_row);
            return ppos_x;
        }
        public int Pivot_Y(int ppos_x)
        {
            int ppos_y;
            double[] pivot_y = Divide_Column(ppos_x, matrix.GetLength(0) - 1);
            ppos_y = Find_Smaller_Number(pivot_y);
            return ppos_y;
        }

        public bool Check_If_Solved()
        {
            bool isTrue = false;
            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                if(matrix[x, matrix.GetLength(1) - 1] < 0)
                {
                    return isTrue;
                }
            }
            isTrue = true;
            return isTrue;
        }

        public void ColumnTo0(int ppos_x, int ppos_y)
        {
            // Divide the rows with the pivot row to get 0 in every column
            // 1. Find row to divide
            // 2. Find dividing value
            // 3. Divide till last row

            int next_column = 0;
            for (int y = 0; y < matrix.GetLength(1); y++)
            {
                if (y != ppos_y && matrix[ppos_x, y] != 0)
                {
                    next_column = y;

                    double dividing_value = TurnValue(matrix[ppos_x, next_column]);

                    for (int x = 0; x < matrix.GetLength(0); x++)
                    {
                        matrix[x, next_column] = matrix[x, next_column] + matrix[x, ppos_y] * dividing_value;
                    }

                    next_column++;
                }
            }
        }

        // Flip a value to the opposite
        public double TurnValue(double val)
        {
            if(val < 0)
            {
                return Math.Abs(val);
            }
            else if(val > 0)
            {
                return val*-1;
            }
            else
            {
                return 0;
            }
        }

        public double[] Divide_Pivot_Row(int ppos_y, double pivot)
        {
            double[] divided_r = new double[matrix.GetLength(0)];
            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                divided_r[x] = matrix[x, ppos_y] / pivot;
            }
            return divided_r;
        }
 
        public double[] Divide_Column(int pivot, int result)
        {
            //Get both rows
            double[] divided_c = new double[matrix.GetLength(1)];

            for (int y = 0; y < matrix.GetLength(1); y++)
            {
                divided_c[y] = matrix[result, y] / matrix[pivot, y];
            }
            return divided_c;
        }

        public double[,] Build_Slack_Matrix()
        {
            double[,] tmp = new double[matrix.GetLength(0)*2,matrix.GetLength(1)];
            double[] y_val = new double[matrix.GetLength(1)];

            // Get the 
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                y_val[i] = matrix[matrix.GetLength(0) - 1,i];
            }

            // Fill the old matrix into the tmp
            for (int x = 0; x < matrix.GetLength(0) - 1; x++)
            {
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    tmp[x, y] = matrix[x, y];
                }
            }

            // Set 1 in every slack slot
            int pos = matrix.GetLength(0) - 1;

            for (int y = 0; y < tmp.GetLength(1); y++)
            {
                for (int x = matrix.GetLength(0) - 1; x < tmp.GetLength(0); x++)
                {
                    if(pos == x)
                    {
                        tmp[x, y] = 1;
                    }
                }
                pos++;
            }

            // Add the result values back in
            for (int i = 0; i < tmp.GetLength(1); i++)
            {
                tmp[tmp.GetLength(0) - 1, i] = y_val[i];
            }
            tmp[tmp.GetLength(0) - 1, tmp.GetLength(1) - 1] = 0;

            return tmp;
        }
        public int Find_Smaller_Number(double[] column)
        {
            double neg = 1000000;
            int pos = -1;
            for (int i = 0; i < column.Length; i++)
            {
                if(column[i] > 0)
                {
                    if (column[i] < neg)
                    {
                        neg = column[i];
                        pos = i;
                    }
                    else if (pos == -1)
                    {
                        pos = i;
                    }
                }
            }
            return pos;
        }
        public int Find_Biggest_Negative(double[] row)
        {
            double neg = 0;
            int pos = -1;
            for (int i = 0; i < row.Length; i++)
            {
                if (row[i] < neg)
                {
                    neg = row[i];
                    pos = i;
                }
            }
            return pos;
        }
        public double[,] Transpose(double[,] matrix)
        {
            int width = matrix.GetLength(0);
            int height = matrix.GetLength(1);

            double[,] result = new double[height, width];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    result[j, i] = matrix[i, j];
                }
            }

            return result;
        }

        public void Show_Result()
        {
            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                Console.WriteLine("Variable " + x + ": " + matrix[x,matrix.GetLength(1) - 1]);
            }
        }
        public void Show_Time()
        {
            Console.WriteLine(string.Empty.PadLeft(Console.WindowWidth - Console.CursorLeft, '─') + "\n");
            Console.WriteLine("Calculation Time in milliseconds: " + timer.Elapsed);
            Console.WriteLine(string.Empty.PadLeft(Console.WindowWidth - Console.CursorLeft, '─') + "\n");
        }
        public void Write_Max()
        {
            Console.WriteLine(string.Empty.PadLeft(Console.WindowWidth - Console.CursorLeft, '─') + "\n");
            Console.WriteLine("Max values: \n");
            for (int i = 0; i < max.Length; i++)
            {
                Console.Write(" " + max[i] + " ");

            }
            Console.WriteLine("\n");
        }
    }
}
