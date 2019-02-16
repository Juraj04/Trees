using BinarySearchTree;
using System;
using System.Collections.Generic;
using System.Linq;


namespace TreeTester
{
    

    class TestAvl
    {
        public AvlTree<int> AvlTree { get; set; }
        public List<int> HelpList { get; set; }
        public static int MaxItems;

        public TestAvl()
        {
            AvlTree = new AvlTree<int>();
            MaxItems = 100000;
            HelpList = new List<int>(MaxItems);
        }

        public void DoAvlTesting()
        {
            //naplnit
            TestInsertion(MaxItems);
            //skontrolovat count
            var result = CountTest(true) &&
                         //skontrolovat in order
                         InOrderTest(true) &&
                         //skontrolovat balance
                         BalanceTest() &&
                         //skontrolovat vysky
                         HeightTest() &&
                         //test hladania
                         TestFind() &&

                         //vymazat do polovice
                         TestDelete(HelpList.Count / 2) && //ma v sebe aj count test aj inorder
                         //skontrolovat balance
                         BalanceTest() &&
                         //skontrolovat vysky
                         HeightTest() &&
                         //test hladania
                         TestFind() &&


                         //vymazat cely
                         TestDeleteUntilEmpty() &&
                         //pre istotu
                         CountTest(true) &&
                         //nahodne insetovanie/deletovanie
                         RandomRemoveInsertTest(1000) &&
                         //vysky
                         HeightTest() &&
                         //hladanie
                         TestFind();
                           
            Console.WriteLine();
            Console.WriteLine(result ? "All tests were successful" : "Some of tests failed");
            Console.ReadLine();

        }

        private bool RandomRemoveInsertTest(int rounds)
        {
            Console.WriteLine("Starting random test...");
            var randomSeed = new Random();
            var randomCount = new Random(randomSeed.Next());
            var randomInsOrDel = new Random(randomSeed.Next());
            var randomInsertNumber = new Random(randomSeed.Next());
            var randomDeleteNumber = new Random(randomSeed.Next());
            //zistit kolko krat ideme robit mazanie/vkladanie
            var help = 0;
            while (help < rounds)
            {
                var count = randomCount.Next(100, 500);
                var insOrDel = randomInsOrDel.NextDouble(); //0 insert, 1 delete
                for (var i = 0; i < count; i++)
                {
                    if (AvlTree.IsEmpty)
                        insOrDel = 0.2; //ak je prazdny nech radsej insertuje
                    if (insOrDel < 0.5)
                    {
                        var number = randomInsertNumber.Next();
                        if (number == 0 || AvlTree.Contains(number))
                            continue;
                        AvlTree.Insert(number);
                        HelpList.Add(number);
                    }
                    else
                    {
                        var index = randomDeleteNumber.Next(0, HelpList.Count - 1);
                        var value = HelpList[index];
                        HelpList.Remove(value);
                        if (!AvlTree.Contains(value))
                        {
                            Console.WriteLine($"Tree does not contain that value {value}");
                            return false;
                        }
                        var deletedValue = AvlTree.Delete(value);
                        if (value != deletedValue)
                        {
                            Console.WriteLine($"Wrong data deleted... should: {value}, was: {deletedValue}");
                            return false;
                        }

                        if (AvlTree.Contains(value))
                        {
                            Console.WriteLine($"Tree still contains removed value {value}");
                            return false;
                        }
                    }
                    
                }
                if (!CountTest(false))
                {
                    Console.WriteLine("Count test failed during random test");
                    return false;
                }

                if (!InOrderTest(false))
                {
                    Console.WriteLine("In order test failed during random test");
                    return false;
                }

                help++;
            }


            if (!CountTest(true))
            {
                Console.WriteLine("Count test failed right after random test");
                return false;
            }
            if (!InOrderTest(true))
            {
                Console.WriteLine("In order test failed right after random test");
                return false;
            }

            Console.WriteLine("Random test successful");
            return true;
        }


        private bool CountTest(bool message)
        {
            if(message)
                Console.WriteLine("Starting count test...");
            if (HelpList.Count != AvlTree.Count)
            {
                Console.WriteLine($"help list count {HelpList.Count} != avltree count {AvlTree.Count}");
                return false;
            }
            if(message)
                Console.WriteLine("Count test successful");
            return true;
        }

        private bool InOrderTest(bool message)
        {
            if(message)
                Console.WriteLine("Starting in order test...");
            if (!CountTest(false))
            {
                Console.WriteLine("Count test failed before in order test");
                return false;
            }
            var index = 0;
            HelpList.Sort((i1, i2) => i1 - i2);
            foreach (var j in AvlTree.InOrderTraversal())
            {
                if (j != HelpList[index])
                {
                    Console.WriteLine($"Avltree is not sorted correctly {j} != {HelpList[index]}");
                    return false;
                }

                index++;
            }
            if(message)
                Console.WriteLine("In order test successful");
            return true;
        }

        private bool BalanceTest()
        {
            Console.WriteLine("Starting balance test...");
            if (!AvlTree.CheckBalance(out var message))
            {
                Console.WriteLine("Balance test failed " + message);
                return false;
            }

            Console.WriteLine("Balance test successful");
            return true;
        }

        private void TestInsertion(int numberOfItems)
        {
            Console.WriteLine("Starting insertion...");
            var random = new Random();
            for (var i = 0; i < numberOfItems; i++)
            {
                var number = random.Next();
                if (number == 0 || AvlTree.Contains(number))
                    continue;
                AvlTree.Insert(number);
                HelpList.Add(number);
            }

            Console.WriteLine("Insertion finished");
        }

        private bool TestFind()
        {
            Console.WriteLine("Starting find test...");
            foreach (var i in HelpList)
            {
                var result = AvlTree.Find(i);
                if (result == 0)
                {
                    Console.WriteLine($"could not find value {i}");
                    return false;
                }

                if (result != i)
                {
                    Console.WriteLine($"tree should find {i} but found {result}");
                    return false;
                }

                if (!AvlTree.Contains(i))
                {
                    Console.WriteLine($"Contains test failed... result = {result} i = {i}");
                    return false;
                }

            }

            Console.WriteLine("Find test successful");
            return true;
            
        }

        private bool TestDelete(int count)
        {
            Console.WriteLine("Starting delete test...");
            var random = new Random(50);
            HelpList.Sort((i1, i2) => i1 - i2);
            for (int i = 0; i < count; i++)
            {
                var index = random.Next(0, HelpList.Count - 1);
                var value = HelpList[index];
                HelpList.Remove(value);
                if (!AvlTree.Contains(value))
                {
                    Console.WriteLine($"Tree does not contain that value {value}");
                    return false;
                }
                var deletedValue = AvlTree.Delete(value);
                if (value != deletedValue)
                {
                    Console.WriteLine($"Wrong data deleted... should: {value}, was: {deletedValue}");
                    return false;
                }

                if (AvlTree.Contains(value))
                {
                    Console.WriteLine($"Tree still contains removed value {value}");
                    return false;
                }

                if (i % 100 == 0)
                {
                    if (!InOrderTest(false))
                    {
                        Console.WriteLine("In order test failed during deletion");
                        return false;
                    }
                }
            }

            if (!CountTest(true))
            {
                Console.WriteLine("Count test failed right after deletion");
                return false;
            }

            if (!InOrderTest(true))
            {
                Console.WriteLine("In order test failed right after deletion");
                return false;
            }

            Console.WriteLine("delete test successful");
            return true;
        }

        private bool TestDeleteUntilEmpty()
        {
            if (TestDelete(HelpList.Count))
            {
                Console.WriteLine("Tree should be empty");
                return true;
            }
            else
            {
                return false;
            }
            
        }

        private bool HeightTest()
        {
            Console.WriteLine("Starting height test...");
            foreach (var node in AvlTree.InOrderTraversalNode())
            {
                var balance = TreeHeight((AvlNode<int>)node.RightChild) - TreeHeight((AvlNode<int>)node.LeftChild);

                if (balance > 1 || balance < -1 || balance != ((AvlNode<int>)node).Balance)
                {
                    Console.WriteLine("Incorrect balance value!");
                    return false;
                }
            }

            Console.WriteLine("Height test successful");
            return true;
        }

        //Source: https://www.geeksforgeeks.org/iterative-method-to-find-height-of-binary-tree/
        private int TreeHeight(AvlNode<int> node)
        {
            if (node == null)
                return 0;

            var q = new Queue<AvlNode<int>>();

            q.Enqueue(node);
            int height = 0;

            while (true)
            {
                var nodeCount = q.Count();
                if (nodeCount == 0)
                    return height;
                height++;

                // Dequeue all nodes of current level and Enqueue all 
                // nodes of next level 
                while (nodeCount > 0)
                {
                    var newNode = q.Dequeue();
                    if (newNode.LeftChild != null)
                        q.Enqueue((AvlNode<int>)newNode.LeftChild);
                    if (newNode.RightChild != null)
                        q.Enqueue((AvlNode<int>)newNode.RightChild);
                    nodeCount--;
                }
            }
        }
    }
}
