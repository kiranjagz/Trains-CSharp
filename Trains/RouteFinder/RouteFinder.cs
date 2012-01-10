using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trains.Graphing;
using System.Text.RegularExpressions;
using Trains.TestData;

namespace Trains.RouteFinder
{
    /// <summary>
    /// Finder class represents an automated route finding and calculating system
    /// </summary>
    class Finder
    {
        /// <summary>
        /// Constructor for Finder class
        /// </summary>
        /// <param name="graph"> The graph upon which to do work </param>
        public Finder(Graph graph)
        {
            RouteMap = graph;
        }

        /// <summary>
        /// The route map in use by this finder.
        /// </summary>
        public Graph RouteMap { get; set; }

        /// <summary> 
        /// Returns a path starting at start, a maximum depth (number of stops) of maxDepth
        /// will only return strings of exactly maxDepth if exactLentgh is true
        ///</summary>
        /// 
        /// <param name="start"> The starting node for the route </param>
        /// <param name="maxDepth"> The maximum length (number of stops) of the route </param>
        /// <param name="exactLength"> return only exaclt maxDepth sized routes? </param>
        /// <returns> A list of routes that match the invoked criteria </returns>
        public List<string> getSteps(Node start, int maxDepth, bool exactLength)
        {
            // So far returning nothing.
            List<string> retVal = new List<string>();
            if (maxDepth == 0)
            {
                // Stop here and Return.
                retVal.Add(start.ToString());
                return retVal;
            }

            // Need to go deeper! get Adjacencies
            Edge[] adjacent = RouteMap.GetAdjacentNodes(start);
            foreach (Edge edge in adjacent)
            {
                // Step deeper to build the path
                List<string> subGraphs = getSteps(edge.End, maxDepth - 1, exactLength);

                // For each Subgraph, mark that we were "here"
                foreach (string graph in subGraphs)
                {
                    retVal.Add(start.NodeName + "-" + graph);
                }

                // Add this < maxDepth length path
                if (!exactLength)
                {
                    retVal.Add(edge.Start.NodeName.ToString());
                }
            }

            return retVal;
        }

        /// <summary>
        /// Follows a given route and returns the distance
        /// </summary>
        /// <param name="route"> a dash ("-") separated route of node hops. eg A-B-D-C</param>
        /// <returns> null if "NO SUCH ROUTE" </returns>
        public int? RouteDistance(string route)
        {
            int retVal = 0;
            char? currentLocation = null;

            // For each step in the route
            foreach (char letter in route)
            {
                // Sanity check this step
                if (!(letter >= 'A' && letter <= 'E'))
                {
                    // skip this malformed stop or "-"
                    continue;
                }

                if (currentLocation == null)
                {
                    currentLocation = letter;
                    continue;
                }

                // Get the edge
                Edge match =
                    (from edge in RouteMap.Edges
                     where edge.Start.NodeName == currentLocation
                         && edge.End.NodeName == letter
                     orderby edge.Distance
                     select edge).DefaultIfEmpty(null).First();

                if (match == null)
                {
                    // NO SUCH ROUTE
                    return null;
                }
                // Update our Odometer, Mark current position
                retVal += match.Distance;
                currentLocation = letter;
            }

            return retVal;
        }

        /// <summary>
        /// Finds all cycles starting (and ending) at Node start, with a maxLength over the cycle
        /// Because this does a very brute force approach, be smart about how large of a graph and maxLength given
        /// Runtime will be something like (big "O") O(NumEdges ^ (maxLength/MinLength))
        /// </summary>
        /// <param name="start"> Node start (and end) of the cycle. </param>
        /// <param name="maxLength"> int maximum length of the cycles to find </param>
        /// <returns> The number of cycles found </returns>
        public int CyclesShorterThan(Node start, int maxLength)
        {
            // This should use the bellman ford algorithm
            // But, given all edges are atleast 1 unit long, we can just get all 30/Min(Edge Lengths) step routes,
            // filtering out the ones which are longer thand 30 cost and those which dont end on the start node (non-cycles)
            // inefficient computationally, but saves coding time :)

            // Find the smallest edge
            var minEdge =
                (from edge in RouteMap.Edges
                 orderby edge.Distance
                 select edge.Distance).First();

            // Find all routes with a number of edges of maxLength/minEdge
            // ie, MaxLength = 25 and MinEdge = 5, we can find upto 5 stop routes.
            var routes = getSteps(start, maxLength / minEdge, false);

            // filter out the ones which dont end on our start (non-cycles)
            var cycles =
                (from route in routes
                 where route[route.Length - 1] == start.NodeName
                 where route.Length > 2 // Must have more than just the starting location
                 where RouteDistance(route) <= maxLength
                 select route).Distinct().ToList(); ;

            return cycles.Count;
        }

        /// <summary>
        /// Do Job dispatchs all the work given to it via ITestData objects.
        /// </summary>
        /// <param name="route">A tuple representing the work to be done</param>
        /// <returns>the answer of how many routes exist (Null == NO SUCH ROUTE)</returns>
        public int? DoJob(Tuple<string, int?, SampleData.TestType> route)
        {
            int? answer = 0;
            try
            {
                switch (route.Item3)
                {
                    // Cases for normal Route Distance
                    case SampleData.TestType.Normal:
                        answer = this.RouteDistance(route.Item1);
                        break;
                    // Cases for Maximum Three Stops
                    case SampleData.TestType.MaxThreeStops:
                        var routes = this.getSteps(new Node(route.Item1[0]), 3, false)
                            .Distinct<string>()
                            .ToList<string>();

                        answer =
                            (from s in routes
                             where s.Length > 1
                             where s[s.Length - 1] == route.Item1[2] // Where the ending is what we expect
                             select s).Count();
                        break;
                    // Cases for Exactly Four Stops
                    case SampleData.TestType.ExactlyFourStops:
                        routes = this.getSteps(new Node(route.Item1[0]), 4, true)
                            .Distinct<string>()
                            .ToList<string>();
                        answer =
                            (from s in routes
                             where s.Length > 1
                             where s[s.Length - 1] == route.Item1[2]
                             select s).Count();
                        break;
                    // Find the shortest route
                    case SampleData.TestType.ShortestRoute:
                        // Dr. Dijkstra has the answer!
                        Dijkstra drDijkstra = new Dijkstra(RouteMap);

                        var minRoutes =
                            drDijkstra.GetMinRoutes(
                            new Node(route.Item1[0]),
                            route.Item1[0] == route.Item1[2]);
                        answer = minRoutes[route.Item1[2]].Item1;
                        // This means no route was found!
                        if (answer == Int32.MaxValue)
                        {
                            answer = null;
                        }
                        break;

                    // This should use the bellman ford algorithm
                    // But, given all edges are atleast 1 unit long, we can just get all 30/Min(Edge Lengths) step routes,
                    // filtering out the ones which are longer thand 30 cost and those which dont end on the start node (non-cycles)
                    // inefficient computationally, but saves coding time :)
                    case SampleData.TestType.RoutesLessThanThirty:
                        answer = this.CyclesShorterThan(new Node(route.Item1[2]), 29);
                        break;
                }
            }
            catch (NotImplementedException e)
            {
                Console.WriteLine("Not Implemented, yet!");
                // Do nothing for now
            }
            catch (Exception e)
            {
                Console.WriteLine("Caught Another Exception... ewww...");
            }
            return answer;
        }
    }
}
