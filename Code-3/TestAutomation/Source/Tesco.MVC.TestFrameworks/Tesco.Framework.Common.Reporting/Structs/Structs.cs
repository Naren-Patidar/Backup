using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.Framework.Common.Reporting.Structs
{
    public static class Structs
    {
        public struct TestProjectDetails
        {
            public string testDll;
            public Test[] tests;
            public int count;
            public int fail;
            public int inconclusive;
        }

        public struct Test
        {
            public string Name;
            public string Status;
            public string ErrorMessage;
            public double Time;
            public string Description;
            public string ExecutionID;
            public string TestGroup;

            //special handling for manual test case
            public string Comments;
            public string TestListID;

            //For handling parallel execution time
            public DateTime StartTime;
            public DateTime EndTime;
        }

        public struct Summary
        {
            public int total;
            public int executed;
            public int passed;
            public double time;
            public DateTime execDate;
        }

        public struct Module
        {
            public string moduleName;
            public string linesCovered;
            public string linesPartiallyCovered;
            public string linesNotCovered;
            public string blocksCovered;
            public string blocksNotCovered;
            public NamespaceTable[] namespaces;
        }

        public struct NamespaceTable
        {
            public string name;
            public string key;
            public string moduleName;
            public string linesCovered;
            public string linesPartiallyCovered;
            public string linesNotCovered;
            public string blocksCovered;
            public string blocksNotCovered;
            public ClassNode[] classes;
        }

        public struct ClassNode
        {
            public string className;
            public string classKeyName;
            public string namespaceKeyName;
            public string linesCovered;
            public string linesPartiallyCovered;
            public string linesNotCovered;
            public string blocksCovered;
            public string blocksNotCovered;
        }
    }
}
