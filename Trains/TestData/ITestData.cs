using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trains.TestData
{
    ///<summary>
    ///Implement this interface on your own test data!
    ///see SampleData.cs for an example
    ///</summary>
    interface ITestData<RouteType, AnswerType, TestType>
    {
        string GraphData { get; }
        Tuple<RouteType, AnswerType, TestType>[] RouteData { get; }
    }
}
