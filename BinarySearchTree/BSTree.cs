using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BinarySearchTree
{
    public class BstNode<T> where T : IComparable<T>
    {
        public T Data { get; set; }
        public BstNode<T> LeftChild { get; set; }
        public BstNode<T> RightChild { get; set; }
        public BstNode<T> Parent { get; set; }
        public bool IsLeaf => LeftChild == null && RightChild == null;
        public bool HasOneChild => (LeftChild == null || RightChild == null) && !IsLeaf;
        public BstNode<T> OnlyChild => LeftChild ?? RightChild;
        public bool IsParentsRightChild => /*Parent != null && */Parent.RightChild == this;

        public void RemoveMeFromParent()
        {
            if (Parent != null)
            {
                if (Parent.LeftChild == this)
                {
                    Parent.LeftChild = null;
                }
                else
                {
                    Parent.RightChild = null;
                }
            }
        }

        public BstNode(T data)
        {
            Data = data;
            Parent = null;
        }

        public BstNode(T data, BstNode<T> parent)
        {
            Data = data;
            Parent = parent;
        }
    }

    public class BsTree<T> where T : IComparable<T>
    {
        protected BstNode<T> Root { get; set; }
        public int Count { get; protected set; }
        public bool IsEmpty => Count == 0;

        public delegate void Processor(T data);

        public BsTree()
        {
            Count = 0;
        }

        public void Clear()
        {
            Count = 0;
            if (Root == null)
                return;
            var queue = new Queue<BstNode<T>>();
            queue.Enqueue(Root);
            while (queue.Count > 0)
            {
                var workingNode = queue.Dequeue();
                if (workingNode.LeftChild != null)
                    queue.Enqueue(workingNode.LeftChild);

                if (workingNode.RightChild != null)
                    queue.Enqueue(workingNode.RightChild);

                workingNode.Parent = null;
            }

            Root = null;
        }

        //******************INSERT****************************************************************************
        public virtual T Insert(T data)
        {
            var node = InsertNode(new BstNode<T>(data),  data);
            if (node != null)
            {
                Count++;
                return node.Data;
            }

            return default(T);
        }
        
        protected BstNode<T> InsertNode(BstNode<T> newNode, T data)
        {
            if (Root == null)
            {
                Root = newNode;
                return Root;
            }
            var currentNode = Root;

            while (currentNode != null)
            {
                if (data.CompareTo(currentNode.Data) == 0)
                {
                    throw new Exception("Tree already contains such data"); 
                }

                if (data.CompareTo(currentNode.Data) < 0)
                {
                    if (currentNode.LeftChild == null)
                    {
                        currentNode.LeftChild = newNode;
                        newNode.Parent = currentNode;
                        return currentNode.LeftChild;
                    }
                    currentNode = currentNode.LeftChild;
                }
                else
                {
                    if (currentNode.RightChild == null)
                    {
                        currentNode.RightChild = newNode;
                        newNode.Parent = currentNode;
                        return currentNode.RightChild;
                    }
                    currentNode = currentNode.RightChild;
                }
                
            }
            return null;

        }
        //*******************FIND*************************************************************
        public T Find(T data)
        {
            var node = FindNode(Root, data);
            return node != null ? node.Data : default(T);
        }

        public T this[T data]
        {
            get
            {
                var node = FindNode(Root, data);
                return node != null ? node.Data : default(T);
            }
        }

        public bool Contains(T data)
        {
            return FindNode(Root, data) != null;
        }

        public bool TryFind(T findData, out T outData)
        {
            var result = false;
            outData = default(T);
            var node = FindNode(Root, findData);
            if (node != null)
            {
                result = true;
                outData = node.Data;
            }

            return result;
        }

        private BstNode<T> FindNode(BstNode<T> node, T data)
        {
            var currentNode = node;

            while (currentNode != null)
            {
                if (data.CompareTo(currentNode.Data) == 0)
                {
                    return currentNode;
                }

                if (data.CompareTo(currentNode.Data) < 0)
                {
                    currentNode = currentNode.LeftChild;
                    continue;
                }

                if (data.CompareTo(currentNode.Data) > 0)
                {
                    currentNode = currentNode.RightChild;
                    continue;
                }
            }

            return null;
        }
        //**************************DELETE********************************************************
        public virtual T Delete(T data)
        {
            var node = DeleteNode(Root, data);
            if (node != null)
            {
                Count--;
                return node.Data;
            }

            return default(T);
        }

        protected BstNode<T> DeleteNode(BstNode<T> node, T data)
        {
            var nodeToDelete = FindNode(node, data);

            if (nodeToDelete != null)
            {
                if (nodeToDelete.LeftChild != null && nodeToDelete.RightChild != null)
                {
                    var rightMostNode = RightMostValueFromLeftSubTree(nodeToDelete.LeftChild);
                    var savedData = nodeToDelete.Data;
                    nodeToDelete.Data = rightMostNode.Data; //switch data
                    rightMostNode.Data = savedData;
                    nodeToDelete = rightMostNode;
                }

                if (nodeToDelete.IsLeaf)
                {
                    nodeToDelete.RemoveMeFromParent();
                    if (nodeToDelete == Root)
                    {
                        Root = null;
                    }
                    return nodeToDelete;
                }
                else if (nodeToDelete.HasOneChild)
                {
                    if (nodeToDelete == Root)
                    {
                        Root = Root.OnlyChild;
                        Root.Parent = null;
                        return nodeToDelete;
                    }
                    if (nodeToDelete.Parent.LeftChild == nodeToDelete)
                    {
                        nodeToDelete.Parent.LeftChild = nodeToDelete.OnlyChild;
                    }
                    else
                    {
                        nodeToDelete.Parent.RightChild = nodeToDelete.OnlyChild;
                    }

                    nodeToDelete.OnlyChild.Parent = nodeToDelete.Parent;
                    return nodeToDelete;
                }
            }
            return null;
        }

        private BstNode<T> LeftMostValueFromRightSubTree(BstNode<T> childNode)
        {
            while (true)
            {
                if (childNode.LeftChild == null) return childNode;
                childNode = childNode.LeftChild;
            }
        }

        private BstNode<T> RightMostValueFromLeftSubTree(BstNode<T> childNode)
        {
            while (true)
            {
                if (childNode.RightChild == null) return childNode;
                childNode = childNode.RightChild;
            }
        }

        public virtual void InOrder(Processor processor)
        {
            DoInOrder(Root,processor);
        }

        private void DoInOrder(BstNode<T> node, Processor processor) //act
        {
            if (node != null)
            {
                DoInOrder(node.LeftChild, processor);
                Console.WriteLine($"I am: {node.Data} and my parent is {(node.Parent != null ? node.Parent.Data: default(T))}");
                //processor(node.Data);
                DoInOrder(node.RightChild, processor);
            }
        }

        protected virtual BstNode<T> RightRotation(BstNode<T> node)
        {
            var newHead = node.LeftChild;
            var headHeadRightLeftChild = newHead.RightChild;
            var newHeadParent = node.Parent;
            newHead.Parent = newHeadParent;
            node.Parent = newHead;
            newHead.RightChild = node;
            node.LeftChild = headHeadRightLeftChild;
            if (headHeadRightLeftChild != null)
                headHeadRightLeftChild.Parent = node;
            
            if (newHeadParent == null)
            {
                Root = newHead;
                return newHead;
            }
            if (newHeadParent.RightChild == node)
            {
                newHeadParent.RightChild = newHead;
            }
            else
            {
                newHeadParent.LeftChild = newHead;
            }

            return newHead;

        }

        protected virtual BstNode<T> LeftRotation(BstNode<T> node)
        {
            var newHead = node.RightChild;
            var newHeadLeftRightChild = newHead.LeftChild;
            var newHeadParent = node.Parent;
            newHead.Parent = newHeadParent;
            node.Parent = newHead;
            newHead.LeftChild = node;
            node.RightChild = newHeadLeftRightChild;
            if (newHeadLeftRightChild != null)
                newHeadLeftRightChild.Parent = node;

            if (newHeadParent == null)
            {
                Root = newHead;
                return newHead;
            }
            if (newHeadParent.RightChild == node)
            {
                newHeadParent.RightChild = newHead;
            }
            else
            {
                newHeadParent.LeftChild = newHead;
            }

            return newHead;
        }

        protected virtual BstNode<T> RightLeftRotation(BstNode<T> node)
        {
            var newHeadRightChild = node.RightChild;
            var newHead = newHeadRightChild.LeftChild;
            var newHeadRightLeftChild = newHead.RightChild;
            var newHeadParent = node.Parent;
            newHeadRightChild.LeftChild = newHeadRightLeftChild;
            if (newHeadRightLeftChild != null)
                newHeadRightLeftChild.Parent = newHeadRightChild;
            newHead.RightChild = newHeadRightChild;
            newHeadRightChild.Parent = newHead;
            var newHeadLeftRightChild = newHead.LeftChild;
            node.RightChild = newHeadLeftRightChild;
            if (newHeadLeftRightChild != null)
                newHeadLeftRightChild.Parent = node;
            newHead.LeftChild = node;
            node.Parent = newHead;
            newHead.Parent = newHeadParent;
            if (newHeadParent == null)
            {
                Root = newHead;
                return newHead;
            }

            if (newHeadParent.RightChild == node)
            {
                newHeadParent.RightChild = newHead;
            }
            else
            {
                newHeadParent.LeftChild = newHead;
            }

            return newHead;
        }

        protected virtual BstNode<T> LeftRightRotation(BstNode<T> node)
        {
            var newHeadLeftChild = node.LeftChild;
            var newHead = newHeadLeftChild.RightChild;
            var newHeadLeftRightChild = newHead.LeftChild;
            var newHeadParent = node.Parent;
            newHeadLeftChild.RightChild = newHeadLeftRightChild;
            if (newHeadLeftRightChild != null)
                newHeadLeftRightChild.Parent = newHeadLeftChild;
            newHead.LeftChild = newHeadLeftChild;
            newHeadLeftChild.Parent = newHead;
            var newHeadRightLeftChild = newHead.RightChild;
            node.LeftChild = newHeadRightLeftChild;
            if (newHeadRightLeftChild != null)
                newHeadRightLeftChild.Parent = node;
            newHead.RightChild = node;
            node.Parent = newHead;
            newHead.Parent = newHeadParent;
            if (newHeadParent == null)
            {
                Root = newHead;
                return newHead;
            }

            if (newHeadParent.RightChild == node)
            {
                newHeadParent.RightChild = newHead;
            }
            else
            {
                newHeadParent.LeftChild = newHead;
            }

            return newHead;
        }

        public IEnumerable<T> InOrderTraversal()
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
                    yield return workingNode.Data;
                    workingNode = workingNode.RightChild;
                }
            }
        }

        public IEnumerable<T> LevelOrderTraversal()
        {
            if (Root == null) yield break;
            var queue = new Queue<BstNode<T>>();
            queue.Enqueue(Root);
            while (queue.Count > 0)
            {
                var workingNode = queue.Dequeue();
                if (workingNode.LeftChild != null)
                    queue.Enqueue(workingNode.LeftChild);

                if (workingNode.RightChild != null)
                    queue.Enqueue(workingNode.RightChild);

                yield return workingNode.Data;
            }
        }




    }

    
}
