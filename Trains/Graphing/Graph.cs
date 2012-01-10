using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trains.Graphing
{
    /// <summary>
    /// A graph is composed of a list of nodes and a list of edges
    /// </summary>
    class Graph
    {
        /// <summary>
        /// Constructor will store the data about the graph in the object
        /// </summary>
        /// <param name="nodes">List of nodes in the graph</param>
        /// <param name="edges">List of edges in the graph</param>
        /// <param name="title">A name for this graph "ie Kiwiland Train Map"</param>
        /// <param name="source">The original string source for this graph</param>
        public Graph(IList<Node> nodes, IList<Edge> edges, string title, string source)
        {
            Nodes = nodes;
            Edges = edges;
            Title = title;
            Source = source;
        }

        public string Source;
        public IList<Node> Nodes;
        public IList<Edge> Edges;
        public string Title;
        
        /// <summary>
        /// Get all exiting edges from startNode
        /// </summary>
        /// <param name="startNode"> Start node to find all edges from </param>
        /// <returns> an array of edges which are adjacent to the startNode</returns>
        public Edge[] GetAdjacentNodes(Node startNode)
        {
            return
                (from edge in Edges
                 where edge.Start.NodeName == startNode.NodeName
                 select edge).ToArray<Edge>();
        }

        ///<summary>
        /// Convenience method
        /// </summary>
        override public string ToString()
        {
            var s = new StringBuilder();
            s.AppendFormat("===== {0} =====\n", Title);
            foreach (Edge edge in Edges)
            {
                s.AppendFormat("Node({0}) has edge to Node({1}) with Distance of {2}\n",
                    edge.Start.NodeName,
                    edge.End.NodeName,
                    edge.Distance)
                ;
            }
            return s.ToString();
        }
    }
}
