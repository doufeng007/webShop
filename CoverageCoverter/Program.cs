using System;

namespace CoverageCoverter
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");


            if (args.Length != 2)
            {
                Console.WriteLine("Coverage Convert - reads VStest binary code coverage data, and outputs it in XML format.");
                Console.WriteLine("Usage:  ConverageConvert <sourcefile> <destinationfile>");
                return 1;
            }

            CoverageInfo info;
            string path;
            try
            {
                path = System.IO.Path.GetDirectoryName(args[0]);
                info = CoverageInfo.CreateFromFile(args[0], new string[] { path }, new string[] { });
            }
            catch (Exception e)
            {
                Console.WriteLine("Error opening coverage data: {0}", e.Message);
                return 1;
            }

            CoverageDS data = info.BuildDataSet();

            try
            {
                data.WriteXml(args[1]);
            }
            catch (Exception e)
            {

                Console.WriteLine("Error writing to output file: {0}", e.Message);
                return 1;
            }

            return 0;
        }
    }
}
