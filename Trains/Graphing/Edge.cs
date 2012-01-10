using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trains.Graphing
{
    /// <summary>
    /// Represents an edge from start to end with a distance.
    /// </summary>
    class Edge
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="start">starting point of this directed edge</param>
        /// <param name="end">end point</param>
        /// <param name="distance">Distance or cost of this edge</param>
        public Edge(Node start, Node end, int distance)
        {
            Start = start;
            End = end;
            Distance = distance;
        }

        public Node Start { get; set; }
        public Node End { get; set; }
        public int Distance { get; set; }

        /// <summary>
        /// Convenience method allowing us to easily debug.
        /// </summary>
        public override string ToString()
        {
            return "From " + Start.NodeName + " To " + End.NodeName + " with a Distance of " + Distance;
        }
    }
}
