using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace KuenstlicheIntelligenz
{
    class IOFunctions
    {
        public List<string> benchmarks = new List<string>();
        public List<string> benchmarks_name = new List<string>();

        public IOFunctions()
        {
            InputBenchmark();
        }

        public void InputBenchmark()
        {
            System.IO.DirectoryInfo ParentDirectory = new System.IO.DirectoryInfo(@"Benchmarks");
            System.IO.FileInfo[] files = ParentDirectory.GetFiles();
            Console.WriteLine(string.Empty.PadLeft(Console.WindowWidth - Console.CursorLeft, '─') + "\n");
            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine("Reading file number: " + (i + 1) + " | " + "File name: " + files[i].Name);
                benchmarks.Add(ReadFile(files[i].FullName));
                benchmarks_name.Add(files[i].Name);
            }
            Console.WriteLine(string.Empty.PadLeft(Console.WindowWidth - Console.CursorLeft, '─') + "\n");
        }

        public void OutputBenchmark(string[] result, string name)
        {
            WriteFile(result, name);
        }

        private static void WriteFile(string[] result, string name)
        {
            string docPath = @"Results";
            try
            {
                using (StreamWriter writer = new StreamWriter(Path.Combine(docPath, name)))
                {
                    foreach (string line in result)
                    {
                        writer.WriteLine(line);
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }

        private string ReadFile(string FileDirectory)
        {
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(FileDirectory);
            line = file.ReadToEnd();
            file.Close();
            return line;
        }
    }
}
