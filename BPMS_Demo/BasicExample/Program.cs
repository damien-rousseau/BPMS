using System.Activities;
using System.Collections.Generic;

namespace BasicExample
{

    class Program
    {
        static void Main(string[] args)
        {
            Activity workflow1 = new Workflow1();

            var dic1 = new Dictionary<string, object>
            {
                { "PriceIn", 50}
            };

            var dic2 = new Dictionary<string, object>
            {
                { "PriceIn", 100}
            };

            var result1 = WorkflowInvoker.Invoke(workflow1, dic1);
            var result2 = WorkflowInvoker.Invoke(workflow1, dic2);
        }
    }
}
