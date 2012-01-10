using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Trains.FileReader
{
    /// <summary>
    /// 
    /// </summary>
    class TestFileReader : ITestFile
    {
        /// <summary>
        /// Build the TestFileReader from the given file
        /// </summary>
        /// <param name="fileName">file to open</param>
        public TestFileReader(string fileName)
        {
           string[] lines = null; 
            // Try to read our file
            try
            {
                lines = File.ReadAllLines(fileName);
            }
            catch (FileNotFoundException e)
            {
                // Explain to the user what is up.
                Console.WriteLine(String.Format("Couldnt find File {0} . Exception thrown {1}", fileName, e.Message));
                Console.WriteLine("For your reference, the cwd is " + Environment.CurrentDirectory);
                throw e;
            }

            // Simple check on the quality of the data
            if (lines == null || lines.Count() != 11)
            {
                Console.WriteLine("See Readme for expected file format");
                throw new InvalidDataException();
            }

            answers = new int?[lines.Count()-1];

            // Parse each line into Either Graph Data or answer
            for( int i = 0; i < lines.Count(); i++)
            {
                if(i == 0)
                {
                    graph = Regex.Replace(lines[0],"Graph:[\\ ]{1}","");
                } 
                else if ( lines[i].Contains("NO SUCH ROUTE"))
                {
                    answers[i-1] = null;
                } 
                else
                {
                    var line = Regex.Replace(lines[i], "Output [#][0-9]+[\\ ]?:[\\ ]?", "");
                    answers[i-1] = Int32.Parse(line);
                }
            }
        }

        /// <summary>
        /// String representing the graph
        /// </summary>
        public string Graph
        {
            get { return graph; }
        }

        /// <summary>
        /// Array of expected answers, null == "NO SUCH ROUTE"
        /// </summary>
        public int?[] ExpectedAnswers
        {
            get { return answers; }
        }

        protected string graph;
        protected int?[] answers; 
    }
}
