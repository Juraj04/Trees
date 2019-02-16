using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BinarySearchTree
{
    class Program
    {
        public static AvlTree<int> AvlTree { get; set; }
        static void Main(string[] args)
        {
            //TryBst();
            
           
            TryAvl();

        }

        private static void TryAvl()
        {
            AvlTree = new AvlTree<int>();

            /*Insert(200, false);
            Insert(400, true);
            Insert(300, true);
            Insert(350, true);
            Insert(325, true);
            Insert(312, true);
            Insert(320, true);
            Insert(324, true);
            Insert(316, true);
            Insert(13, false);
            Insert(14, false);
            Insert(15, true);*/
            //Delete(5, true);
            //return;
            Insert(100, false);
            Insert(50, false);
            Insert(150, false);
            Insert(25, false);
            Insert(75, false);
            Insert(125, false);
            Insert(175, false);
            Insert(12, false);
            Insert(37, false);
            Insert(62, false);
            Insert(87, false);
            Insert(112, false);
            Insert(137, false);
            Insert(167, false);
            Insert(6, false);
            Insert(18, false);
            Insert(43, false);
            Insert(56, false);
            Insert(106, false);
            Insert(2, false);
            Delete(175, false);
            Delete(87, false);
            Delete(100, true);
            Delete(75, true);
            DeleteFromConsole();
            return;
            //Delete(175, true);
            //Insert(12, false);
            //Insert(14, true);
            //Insert(11, true);
            foreach (var i in AvlTree.LevelOrderTraversal())
            {
                Console.WriteLine(i);
            }

            Console.ReadLine();
        }

        private static void DeleteFromConsole()
        {
            while (true)
            {
                Console.Write("Enter number to delete: ");
                var no = Int32.Parse(Console.ReadLine());
                if (no == 0)
                    return;
                Delete(no,true);
            }
        }

        private static void Insert(int number, bool write)
        {
            Console.WriteLine($"Inserting {number}");
            AvlTree.Insert(number);
            if (write)
            {
                AvlTree.InOrder(null);
                //foreach (var i in AvlTree.IterativeInOrder())
                //{
                   // Console.WriteLine(i);
                //}
                Console.WriteLine($"je nas tu: {AvlTree.Count}");
                Console.ReadLine();
            }
        }

        private static void Delete(int number, bool write)
        {
            Console.WriteLine($"Deleting {number}");
            AvlTree.Delete(number);
            if (write)
            {
                AvlTree.InOrder(null);
                //foreach (var i in AvlTree.IterativeInOrder())
                //{
                // Console.WriteLine(i);
                //}
                Console.WriteLine($"je nas tu: {AvlTree.Count}");
                Console.ReadLine();
            }
        }

        private static void TryBst()
        {
            var bst = new BsTree<int>();
            bst.Insert(5);
            bst.Insert(2);
            bst.Insert(1);
            bst.Insert(8);
            bst.Insert(3);
            bst.Insert(9);
            bst.Insert(4);
            bst.Insert(10);
            bst.InOrder((data => Console.WriteLine($"I am: {data}")));
            Console.WriteLine($"je nas tu: {bst.Count}");

            Console.WriteLine($"je tu 5?: {bst[5]}");
            Console.ReadLine();
            //var l = bst.Find(50);
            Console.WriteLine(bst.Contains(5));
            Console.WriteLine(bst.Contains(2));
            Console.WriteLine(bst.Contains(9));
            Console.ReadLine();
            Console.WriteLine($"deleting {2}");
            bst.Delete(2);
            bst.InOrder((data => Console.WriteLine($"I am: {data}")));
            Console.WriteLine($"je nas tu: {bst.Count}");
            Console.WriteLine($"inserting {2}");
            bst.Insert(2);
            bst.InOrder((data => Console.WriteLine($"I am: {data}")));
            Console.WriteLine($"je nas tu: {bst.Count}");
            Console.ReadLine();
            Console.WriteLine($"deleting {8}");
            bst.Delete(8);
            bst.InOrder((data => Console.WriteLine($"I am: {data}")));
            Console.WriteLine($"je nas tu: {bst.Count}");
            Console.ReadLine();
            Console.WriteLine($"deleting {15}");
            bst.Delete(15);
            bst.InOrder((data => Console.WriteLine($"I am: {data}")));
            Console.WriteLine($"je nas tu: {bst.Count}");
            Console.ReadLine();
        }
    }
}
