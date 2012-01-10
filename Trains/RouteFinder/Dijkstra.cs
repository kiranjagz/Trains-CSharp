using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trains.Graphing;

namespace Trains.RouteFinder
{
    // Thanks to wikipedia for psuedcode for this algorithm :)
    // http://en.wikipedia.org/wiki/Dijkstra%27s_algorithm

    /// <summary>
    /// Dijkstra's minimum distance route algorithm
    /// </summary>
    class Dijkstra
    {
        /// <summary>
        /// Contructor for Dijkstra'a algorithm handler
        /// </summary>
        /// <param name="graph">The Graph to be utilized</param>
        public Dijkstra(Graph graph)
        {
            this.graph = graph;
        }

        /// <summary>
        /// Implements a modified Djikstra's Algorithm to find Min routes from Node start
        /// Also has the ability to search for a Cycle if bool findCycle is true
        /// </summary>
        /// <param name="start">Node to start search from, also the end node if finding cycles</param>
        /// <param name="findCycle"> Should we find cycles or just find all other nodes?</param>
        /// <returns> Returns a dictionary giving the distance and route from Start to the key in the dictionary</returns>
        public Dictionary<char, Tuple<int, string>> GetMinRoutes(Node start, bool findCycle)
        {
            // So that we dont count it a million times
            var count = graph.Nodes.Count;

            // We want to keep track of distances, which we've visited, routes. And eventually return a formated value
            Dictionary<char, int> distances = new Dictionary<char, int>(count);
            Dictionary<char, bool> visited = new Dictionary<char, bool>(count);
            Dictionary<char, string> routes = new Dictionary<char, string>(count);
            Dictionary<char, Tuple<int, string>> retVal = new Dictionary<char, Tuple<int, string>>(count);

            // Initialize to default values
            foreach (Node n in graph.Nodes)
            {
                distances.Add(n.NodeName, Int32.MaxValue);
                routes.Add(n.NodeName, start.NodeName.ToString());
            }

            // We're not coming back here, so assume distance of 0
            if (!findCycle)
            {
                distances[start.NodeName] = 0;
            }

            // Copy the Edges list so this function doesnt have side-effects
            var edges = graph.Edges.ToList();

            // Start from our start position.
            var current = start;

            // We want to visit every node if possible (see "break;" below)
            while (visited.Keys.Count < count)
            {
                // Skip adding the start node once, so that we'll return to it again.
                if (findCycle)
                {
                    // But from now on, track which we've visited
                    findCycle = false;
                }
                else
                {
                    visited.Add(current.NodeName, true);
                }

                // Check each edge for a better route
                foreach (Edge edge in graph.GetAdjacentNodes(current))
                {
                    // Never check this edge again.
                    edges.Remove(edge);

                    // If we dont have a route, or if we've found a better route
                    if (distances[edge.End.NodeName] == Int32.MaxValue
                        || distances[current.NodeName] + edge.Distance < distances[edge.End.NodeName])
                    {
                        if (current.NodeName == start.NodeName)
                        {
                            distances[edge.End.NodeName] = edge.Distance;
                        }
                        else
                        {
                            distances[edge.End.NodeName] = distances[current.NodeName] + edge.Distance;
                        }
                        routes[edge.End.NodeName] = routes[current.NodeName] + "-" + edge.End;
                    }
                }

                // Figure out all nodes which are adjacent to the currently visited nodes
                List<char> adjacencies = new List<char>();

                adjacencies = adjacencies.Union(
                        graph.GetAdjacentNodes(current)
                        .Select(edge => edge.End.NodeName)).ToList();

                foreach (char n in visited.Keys)
                {
                    adjacencies = adjacencies.Union(
                        graph.GetAdjacentNodes(new Node(n))
                        .Select(edge => edge.End.NodeName)).ToList();
                }

                // Select the node with the lowest distance, and hasnt been visited
                current =
                    (from KeyValuePair<char, int> pair in distances
                     where pair.Key != current.NodeName
                     where !visited.Keys.Contains(pair.Key)
                     where adjacencies.Contains(pair.Key)
                     orderby pair.Value
                     select new Node(pair.Key)).DefaultIfEmpty(null).First();

                // There are maybe nodes left, but they're not accessible
                if (current == null)
                {
                    break;
                }
            }

            // Build our return value.
            foreach (KeyValuePair<char, string> pair in routes)
            {
                retVal.Add(pair.Key, new Tuple<int, string>(distances[pair.Key], pair.Value));
            }

            return retVal;
        }

        /// <summary>
        /// Backing store for public property
        /// </summary>
        protected Graph graph;
    }
}
