using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trains.Graphing;
using Trains.RouteFinder;
using Trains.TestData;
using Trains.FileReader;
namespace Trains
{
    class Program
    {
        static void Main(string[] args)
        {
            SampleData data = null;
            try
            {
                TestFileReader inputFile = new TestFileReader("input.txt");
                data = new SampleData(inputFile.Graph, inputFile.ExpectedAnswers);
            }
            catch (Exception e)
            {
                Console.WriteLine("There was an issue reading the input file, defaulting to sample data");
                data = new SampleData();
            }
           
            // Create your own testData class to try other combinations!
            
            var builder = new GraphBuilder();
            Graph graph = builder.BuildFromString(data.GraphData)
                .setTitle("Kiwiland Rail")
                .getGraph();

            // Display the Graph
            Console.Write(graph + "\n");

            var planner = new Finder(graph);
            foreach (Tuple<string, int?, SampleData.TestType> route in data.RouteData)
            {
                var answer = planner.DoJob(route);
                string displayableAnswer = answer == null ? "NO SUCH ROUTE" : answer.ToString();
                // Std out for how we did.
                if (route.Item2 == null)
                {
                    if (answer == null)
                    {
                        Console.WriteLine(displayableAnswer);
                    }
                    else
                    {
                        Console.WriteLine("We figured " + displayableAnswer + ", but the correct answer was " + route.Item2);
                    }

                }
                else
                {
                    if (answer != route.Item2)
                    {

                        Console.WriteLine("We figured " + displayableAnswer + ", but the correct answer was " + route.Item2);
                    }
                    else
                    {
                        Console.WriteLine("Output: " + answer);
                    }
                }
            }
        }
    }
}
