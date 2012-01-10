using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trains.FileReader
{
    interface ITestFile
    {
        string Graph { get; }
        int?[] ExpectedAnswers { get; }
    }
}
