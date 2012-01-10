using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trains.Graphing
{
    /// <summary>
    /// Node class
    /// </summary>
    class Node
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodeName">Single character name of this node</param>
        public Node(char nodeName)
        {
            NodeName = nodeName;
        }

        /// <summary>
        /// Public property NodeName
        /// </summary>
        public char NodeName { set; get; }

        /// <summary>
        /// Convenience method 
        /// </summary>
        /// <returns> string representation of this Node</returns>
        public override string ToString()
        {
            return NodeName.ToString();
        }

    }
}