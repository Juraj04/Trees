using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BinarySearchTree;

namespace TreeTester
{
    class Program
    {
        public static AvlTree<int> AvlTree { get; set; }
        public static List<int> HelpList { get; set; }

        public static int MaxItems;

        static void Main(string[] args)
        {
            var tester = new TestAvl();
            tester.DoAvlTesting();
        }
        
    }
}
