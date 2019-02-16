using System;
using System.Collections.Generic;

namespace BinarySearchTree
{
    public class AvlNode<T> : BstNode<T> where T : IComparable<T> 
    {
        public int Balance { get; set; }
        public AvlNode(T data): base(data)
        {
            Balance = 0;
        }
    }

    public class AvlTree<T> : BsTree<T> where T : IComparable<T>
    {
        public override T Delete(T data)
        {
            var node = (AvlNode<T>)DeleteNode(Root, data);
            if (node == null)
                return default(T); 
            Count--;

            if (Root == null)
            {
                return node.Data;
            }
            RepairTreeAfterDelete(node);
            return node.Data;
        }

        public override T Insert(T data)
        {
            var node = (AvlNode <T>)InsertNode(new AvlNode<T>(data), data);
            if (node == null)
                return default(T);
            Count++;
            RepairTreeAfterInsert(node);
            return node.Data;
            
        }

        private bool FromLeft(BstNode<T> node)
        {
            if (node.HasOneChild && node.Parent.LeftChild == node.OnlyChild) //ak som mal jedno decko a parent ho ma teraz nalavo
            {
                return true;
            }
            if(node.HasOneChild && node.Parent.RightChild == node.OnlyChild) //ak som mal jedno decko a parent ho ma teraz napravo
            {
                return false;
            }
            //teraz uz mozem byt len list
            if(node.Parent.RightChild == null && node.Parent.LeftChild == null) //ak parent nema deti (novy list)
            {
                var n = (AvlNode < T >) node.Parent;
                if (n.Balance < 0) //balance sa mu nezmenili
                {
                    return true; //ak som ho tahal do lava
                }
                if(n.Balance > 0)
                {
                    return false; //ak som ho tahal doprava
                }
            }
            //bol som list ale mal som brata
            if (node.Parent.RightChild != null) //ak pravy brat stale existuje tak som bol lavy
            {
                return true;
            }

            return false;
        }

        private void RepairTreeAfterDelete(AvlNode<T> n) 
        {
            var workingNode = n;
            if (workingNode.HasOneChild && workingNode.OnlyChild == Root && Count == 1) //ak vymazavam root ktory ma len jedneho syna
            {
                ((AvlNode<T>) Root).Balance = 0; // specialny pripad
                return;
            }
            var fromLeft = FromLeft(workingNode); 
            workingNode = (AvlNode<T>)workingNode.Parent;
            while (workingNode != null)
            {
                if (fromLeft)
                {
                    workingNode.Balance++;
                    if (workingNode.Balance == 2)
                    {
                        var z = (AvlNode<T>)workingNode.RightChild;
                        if (z.Balance < 0)
                        {
                            workingNode = (AvlNode<T>)RightLeftRotation(workingNode);
                        }
                        else
                        {
                            workingNode = (AvlNode<T>) LeftRotation(workingNode);
                            if (workingNode.Balance == -1)
                                break;
                        }
                    }
                    else
                    {
                        
                        if (workingNode.Balance == 1)
                        {
                            break;
                        }
                    }
                }
                else 
                {
                    workingNode.Balance--;
                    if (workingNode.Balance == -2)
                    {
                        var z = (AvlNode<T>)workingNode.LeftChild;
                        if (z.Balance > 0)
                        {
                            workingNode = (AvlNode<T>)LeftRightRotation(workingNode);
                        }
                        else
                        {
                            workingNode = (AvlNode<T>)RightRotation(workingNode);
                            if (workingNode.Balance == 1)
                                break;
                        }
                    }
                    else
                    {
                        
                        if (workingNode.Balance == -1)
                        {
                            break;
                        }

                        
                    }
                }

                if (workingNode.Parent == null)
                    break;
                fromLeft = !workingNode.IsParentsRightChild;
                workingNode = (AvlNode<T>)workingNode.Parent;
            }
            

        }

        private void RepairTreeAfterInsert(AvlNode<T> z) 
        {
            for (var x = z.Parent; x != null; x = z.Parent)
            {
                if (z.IsParentsRightChild)
                {
                    ((AvlNode<T>) x).Balance++;
                    if (((AvlNode<T>) x).Balance == 2)
                    {
                        if (z.Balance < 0)
                        {
                            RightLeftRotation((AvlNode<T>)x);
                        }
                        else
                        {
                            LeftRotation((AvlNode<T>) x);
                        }

                        break;
                    }
                    else
                    {
                        if (((AvlNode<T>) x).Balance == 0)
                            break;

                        z = (AvlNode<T>)x;
                    }
                }
                else
                {
                    ((AvlNode<T>)x).Balance--;
                    if (((AvlNode<T>)x).Balance == -2)
                    {
                        if (z.Balance > 0)
                        {
                            LeftRightRotation((AvlNode<T>)x);
                        }
                        else
                        {
                            RightRotation((AvlNode<T>)x);
                        }

                        break;
                    }
                    else
                    {
                        if (((AvlNode<T>)x).Balance == 0)
                            break;

                        z = (AvlNode<T>)x;
                    }
                }
                
            }
        }

        public override void InOrder(Processor processor) 
        {
            InOrderr((AvlNode<T>)Root);
        }

        private void InOrderr(AvlNode<T> node) 
        {
            if (node != null)
            {
                InOrderr((AvlNode<T>)node.LeftChild);
                Console.WriteLine($"I am: {node.Data} and my parent is {(node.Parent != null ? node.Parent.Data : default(T))} and my balance is: {node.Balance}");
                //processor(node.Data);
                InOrderr((AvlNode<T>)node.RightChild);
            }
        }

        public bool CheckBalance(out string message) //debug method
        {
            var queue = new Queue<BstNode<T>>();
            queue.Enqueue(Root);
            while (queue.Count > 0)
            {
                var workingNode = queue.Dequeue();
                if (workingNode.LeftChild != null)
                    queue.Enqueue(workingNode.LeftChild);

                if (workingNode.RightChild != null)
                    queue.Enqueue(workingNode.RightChild);

                if (((AvlNode<T>) workingNode).Balance > 1 || ((AvlNode<T>) workingNode).Balance < -1)
                {
                    message = "balance 2 somewhere";
                    return false;
                }
                    
                if (workingNode.IsLeaf && ((AvlNode<T>) workingNode).Balance != 0)
                {
                    message = "leaf does not have 0 balance";
                    return false;
                }
                    
            }

            message = "looks good";
            return true;
        }

        protected override BstNode<T> LeftRotation(BstNode<T> node)
        {
            var newHead = base.LeftRotation(node);
            
            
            if (((AvlNode<T>) newHead).Balance == 0)
            {
                ((AvlNode<T>) node).Balance = 1;
                ((AvlNode<T>) newHead).Balance = -1;
            }
            else
            {
                ((AvlNode<T>)node).Balance = 0;
                ((AvlNode<T>)newHead).Balance = 0;
            }

            return newHead;
        }

        protected override BstNode<T> RightRotation(BstNode<T> node)
        {

            var newHead = base.RightRotation(node);
            

            if (((AvlNode<T>)newHead).Balance == 0)
            {
                ((AvlNode<T>)node).Balance = -1;
                ((AvlNode<T>)newHead).Balance = 1;
            }
            else
            {
                ((AvlNode<T>)node).Balance = 0;
                ((AvlNode<T>)newHead).Balance = 0;
            }

            return newHead;
        }

        protected override BstNode<T> RightLeftRotation(BstNode<T> node)
        {
            var newHead = base.RightLeftRotation(node);

            if (((AvlNode<T>)newHead).Balance > 0)
            {
                ((AvlNode<T>)node).Balance = -1;
                ((AvlNode<T>) newHead.RightChild).Balance = 0;
            }
            else
            {
                if (((AvlNode<T>)newHead).Balance == 0)
                {
                    ((AvlNode<T>)node).Balance = 0;
                    ((AvlNode<T>)newHead.RightChild).Balance = 0;
                }
                else
                {
                    ((AvlNode<T>)node).Balance = 0;
                    ((AvlNode<T>)newHead.RightChild).Balance = 1;
                }
            }
            ((AvlNode<T>)newHead).Balance = 0;
            return newHead;
        }

        protected override BstNode<T> LeftRightRotation(BstNode<T> node)
        {
            var newHead = base.LeftRightRotation(node);

            
            if (((AvlNode<T>)newHead).Balance < 0)
            {
                ((AvlNode<T>)node).Balance = 1;
                ((AvlNode<T>)newHead.LeftChild).Balance = 0;
            }
            else
            {
                if (((AvlNode<T>)newHead).Balance == 0)
                {
                    ((AvlNode<T>)node).Balance = 0;
                    ((AvlNode<T>)newHead.LeftChild).Balance = 0;
                }
                else
                {
                    ((AvlNode<T>)node).Balance = 0;
                    ((AvlNode<T>)newHead.LeftChild).Balance = -1;
                }
            }
            ((AvlNode<T>)newHead).Balance = 0;
            return newHead;
        }

        public IEnumerable<BstNode<T>> InOrderTraversalNode() 
        {
            var stack = new Stack<BstNode<T>>();
            var workingNode = Root;
            while (stack.Count > 0 || workingNode != null)
            {
                if (workingNode != null)
                {
                    stack.Push(workingNode);
                    workingNode = workingNode.LeftChild;
                }
                else
                {
                    workingNode = stack.Pop();
                    yield return workingNode;
                    workingNode = workingNode.RightChild;
                }
            }
        }
    }
}
