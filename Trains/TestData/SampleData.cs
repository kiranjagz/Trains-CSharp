using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trains.TestData
{
    /// <summary>
    /// Sample data given in the email. Implements a common interface.
    /// </summary>
    class SampleData : ITestData<string, int?, Trains.TestData.SampleData.TestType>
    {
        public SampleData(string graphString, int?[] answers)
        {
            graphData = graphString;

            // Tuple (route, expected answer, Test Type)
            
            routeData = new Tuple<string, int?, TestType>[] {
                       Tuple.Create("A-B-C", answers[0], TestType.Normal),     // Route 1
                       Tuple.Create("A-D", answers[1], TestType.Normal),       // Route 2
                       Tuple.Create("A-D-C", answers[2], TestType.Normal),     // Route 3
                       Tuple.Create("A-E-B-C-D", answers[3], TestType.Normal), // Route 4
                       Tuple.Create("A-E-D", answers[4], TestType.Normal),     // Route 5
                       Tuple.Create("C-C", answers[5], TestType.MaxThreeStops),    // Starting at "C" , ending at "C", max 3 stops
                       Tuple.Create("A-C", answers[6], TestType.ExactlyFourStops),       // Starting at "A" , ending at "C", exactly 4 stops
                       Tuple.Create("A-C", answers[7], TestType.ShortestRoute),      // Min Distance from "A" to "C"
                       Tuple.Create("B-B", answers[8], TestType.ShortestRoute),     // Min DIstance from "B" to "B"
                       Tuple.Create("C-C", answers[9], TestType.RoutesLessThanThirty)     // Number of routes from C to C with a distance of <30
                    };

        }

        // Basic constructor will return default sample data
        public SampleData()
        {

        }
        /// <summary>
        /// A simple way to keep track of what kind of test we're doing 
        /// </summary>
        public enum TestType { Normal, MaxThreeStops, ExactlyFourStops, ShortestRoute, RoutesLessThanThirty };

        /// <summary>
        /// The given graph data from the email
        /// </summary>
        public string GraphData
        {
            get
            {
                if (graphData == null)
                {
                    graphData = "AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7";
                }
                return graphData;
            }

            set
            {
                graphData = value;
            }
        }

        /// <summary>
        /// Returns Tuples representing quetions, see the comments for each tuple
        /// </summary>
        public Tuple<string, int?, TestType>[] RouteData
        {
            get
            {
                // create it only once.
                if (routeData == null)
                {
                    // Tuple (route, expected answer, Test Type)
                    routeData = new Tuple<string, int?, TestType>[] {
                       Tuple.Create("A-B-C",(int?) 9, TestType.Normal),     // Route 1
                       Tuple.Create("A-D",(int?) 5, TestType.Normal),       // Route 2
                       Tuple.Create("A-D-C",(int?) 13, TestType.Normal),     // Route 3
                       Tuple.Create("A-E-B-C-D",(int?) 22, TestType.Normal), // Route 4
                       Tuple.Create("A-E-D",(int?) null, TestType.Normal),     // Route 5
                       Tuple.Create("C-C",(int?) 2, TestType.MaxThreeStops),    // Starting at "C" , ending at "C", max 3 stops
                       Tuple.Create("A-C",(int?) 3, TestType.ExactlyFourStops),       // Starting at "A" , ending at "C", exactly 4 stops
                       Tuple.Create("A-C",(int?) 9, TestType.ShortestRoute),      // Min Distance from "A" to "C"
                       Tuple.Create("B-B",(int?) 9, TestType.ShortestRoute),     // Min DIstance from "B" to "B"
                       Tuple.Create("C-C",(int?) 7, TestType.RoutesLessThanThirty)     // Number of routes from C to C with a distance of <30
                    };
                }

                return routeData;
            }

            set
            {
                routeData = value;
            }
        }

        /// <summary>
        /// Backing store for the public property
        /// </summary>
        protected Tuple<string, int?, TestType>[] routeData;
        protected string graphData;
    }
}