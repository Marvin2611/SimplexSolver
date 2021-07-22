using System;
using System.Collections.Generic;
using System.Text;

namespace KuenstlicheIntelligenz
{
    class SplitVars
    {
        string toParse;

        string[] min_con;   // Constraints for the mins
        List<string[]> con; // Constraints
        string[] con_y;     // Results of the constraints
        int matrix_x;
        int matrix_y;

        Benchmark parsed;
        public SplitVars(string parse)
        {
            parsed = new Benchmark();
            toParse = parse;

            if(toParse != null)
            {
                Parsing();
            }
        }

        private void Parsing()
        {
            if(toParse != null)
            {
                parsed.obj_function = toParse; // Control

                // 1. Split the txt string into bits using ";"
                string[] obj_tmp = toParse.Split(";");

                // 2. Split the bits of the obj. into smaller bits using " + "
                string[] min_var_tmp = obj_tmp[0].Split(" + ");

                // 3. Get all the values for the matrix
                min_con = Cut_Beginning(min_var_tmp); // Constraints for the mins
                con = new List<string[]>();       // Rest of the constraints
                con_y = Cut_Result(obj_tmp);      // Results of the constraints

                for (int i = 1; i < obj_tmp.Length; i++)
                {
                    string[] tmp = obj_tmp[i].Split(" + ");
                    con.Add(Cut_Constraints(tmp));
                }

                // 4. Define the matrix x and y
                matrix_x = min_var_tmp.Length;
                matrix_y = obj_tmp.Length - 1;

                parsed = Build_Matrix();
                Test_Matrix(parsed);
            }
        }
        private Benchmark Build_Matrix()
        {
            Benchmark tmp = new Benchmark();
            tmp.matrix = new double[matrix_x, matrix_y];
            tmp.min = new double[matrix_x];

            // Insert the constraints
            for (int i = 0; i < tmp.matrix.GetLength(0); i++)
            {
                for (int j = 0; j < tmp.matrix.GetLength(1) - 1; j++)
                {
                    tmp.matrix[i,j] = Convert.ToDouble(con[j][i]);
                }
            }

            // Insert the y values in the last rows
            for (int y = 0; y < tmp.matrix.GetLength(1); y++)
            {
                tmp.matrix[tmp.matrix.GetLength(0) - 1, y] = Convert.ToDouble(con_y[y]);
            }

            // Insert the mins of the matrix
            for (int i = 0; i < tmp.matrix.GetLength(0) - 1; i++)
            {
                tmp.matrix[i, tmp.matrix.GetLength(1) - 1] = Convert.ToDouble(min_con[i]);
                tmp.min[i] = tmp.matrix[i, tmp.matrix.GetLength(1) - 1];
            }

            // Put a 1 in the bottom right of the matrix
            tmp.matrix[tmp.matrix.GetLength(0) - 1, tmp.matrix.GetLength(1) - 1] = 1.0;

            return tmp;
        }

        private void Test_Matrix(Benchmark test)
        {
            test.Write_Matrix();
        }

        // Use, if first array space is filler (example[0] filler)
        private string[] Cut_Beginning(string[] toCut)
        {
            string[] tmp = null;
            if (toCut != null)
            {
                tmp = new string[toCut.Length - 1];
                for (int i = 1; i < toCut.Length; i++)
                {
                    string[] split = toCut[i].Split("*");
                    tmp[i - 1] = split[0];
                }
            }
            return tmp;
        }
        // Cut one stripe to only the variables
        private string[] Cut_Constraints(string[] toCut)
        {
            string[] tmp = null;
            if (toCut != null)
            {
                tmp = new string[toCut.Length];
                for (int i = 1; i < toCut.Length; i++)
                {
                    string[] split = toCut[i].Split("*");
                    tmp[i - 1] = split[0];
                }
            }
            return tmp;
        }
        // Cut the result for each matrix after >=
        private string[] Cut_Result(string[] toCut)
        {
            string[] tmp = null;
            if(toCut != null)
            {
                tmp = new string[toCut.Length - 1];
                for (int i = 1; i < toCut.Length - 1; i++)
                {
                    string[] split = toCut[i].Split(" >= ");
                    tmp[i - 1] = split[1];
                }
            }
            return tmp;
        }

        public Benchmark ReturnParsed()
        {
            return parsed;
        }
    }
}
