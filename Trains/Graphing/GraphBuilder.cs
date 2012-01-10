using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Trains.Graphing
{
    /// <summary>
    /// Tool Class for building a Graph object
    /// </summary>
    class GraphBuilder
    {
        /// <summary>
        /// Constructor initializes the store for the edges and nodes
        /// </summary>
        public GraphBuilder()
        {
            nodes = new List<Node>();
            edges = new List<Edge>();
        }

        ///<summary>
        /// Takes a string to convert to a graph according to instructions
        /// Utilizes fluent interface
        ///</summary>
        ///<param name="input"> the source string to build the graph from</param>
        public GraphBuilder BuildFromString(string input)
        {
            // General pattern expected.
            string pattern = "([A-E][A-E][0-9]{1,9}(( )?,( )?)?)+";

            // Disregard Malformed input, you wiley recruiters you...
            Match match = Regex.Match(input, pattern);

            // If you're not being tricksy
            if (!match.Success)
            {
                Console.WriteLine("Malformed input detected. Aborting.");
                return null;
            }

            // Split on the comma
            pattern = "[\\s]*[,][\\s]*";
            String[] tokens = Regex.Split(input, pattern);

            Dictionary<char, Node> nodeDictionary = new Dictionary<char, Node>(tokens.Count());
            foreach (String token in tokens)
            {
                Node node1 = new Node(token[0]);
                Node node2 = new Node(token[1]);

                // Ensure we dont add nodes more than once.
                if (!nodeDictionary.ContainsKey(token[0]))
                {
                    nodeDictionary.Add(token[0], node1);
                }

                if (!nodeDictionary.ContainsKey(token[1]))
                {
                    nodeDictionary.Add(token[1], node2);
                }

                nodes = nodeDictionary.Values.ToList();
                // Dont have to check edges so stringently 
                // Email said: "For a given route will never appear more than once, and Node1 will != Node2"
                edges.Add(new Edge(node1, node2, int.Parse(token.Substring(2))));
            }

            //Save the source of this graph.
            source = input;

            return this;
        }

        /// <summary>
        ///  Sets the title of the graph
        /// </summary>
        /// <param name="title"> title to set </param>
        /// <returns> this </returns>
        public GraphBuilder setTitle(string title)
        {
            this.title = title;
            return this;
        }

        /// <summary>
        ///  Returns a graph instance based on the source provided
        /// </summary>
        /// <returns> the graph </returns>
        public Graph getGraph()
        {
            return new Graph(nodes, edges, title, source);
        }

        protected IList<Node> nodes;
        protected IList<Edge> edges;
        protected string title;
        protected string source;
    }
}
