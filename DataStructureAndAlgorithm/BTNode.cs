using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Windows.Markup;

namespace DataStructureAndAlgorithm
{
    public class BTNode<T>:IEnumerable<T>
        where T : IComparable<T>
    {
        private T Data;

        private BTNode<T> _left;
        private BTNode<T> _right;
        private BTNode<T> _head;
        public int Count { get; private set; }

        public BTNode(T data)
        {
            Data = data;
            _left = null;
            _right = null;
        }


        public BTNode(BTNode<T> left, BTNode<T> right, T data)
        {
            Data = data;
            _left = left;
            _right = right;
        }
        public T GetData()
        {
            return Data;
        }

        public BTNode<T> GetRight()
        {
            return _right;
        }

        public BTNode<T> GetLeft()
        {
            return _left;
        }

        public void SetRight(BTNode<T> right)
        {
           _right = right;
        }

        public void SetLeft(BTNode<T> left)
        {
            _left = left;
        }
        public static BTNode<T> NewNode(T data)
        {
            return new BTNode<T>(data);
        }
        public IEnumerable<T> PreOrderRecursive(BTNode<T> head)
        {

            if (head == null)
            {
                yield break;
            }

            BTNode<T> current = head;

            yield return current.Data;
            PreOrderRecursive(current._left);
            PreOrderRecursive(current._right);
        }

        public IEnumerable<T> PreOrderNonRecursive(BTNode<T> head)
        {

            if (head == null)
            {
                yield break;
            }

            BTNode<T> current = head;
            var stack = new Stack<BTNode<T>>();
            stack.Push(current);

            while (stack.Count > 0)
            {
                //return value and pus to stack
                yield return stack.Pop().Data;

                if (current._left != null)
                    stack.Push(current._left);
                if (current._right != null)
                    stack.Push(current._right);
            }
        }

        public IEnumerable<T> InOrderRecursive(BTNode<T> head)
        {
            if (head == null)
            {
                yield break;
            }

            BTNode<T> current = head;
            InOrderRecursive(current._left);
            yield return current.Data;
            InOrderRecursive(current._right);
        }

        public IEnumerable<T> InOrderNonRecursive(BTNode<T> head)
        {
            if (head == null)
            {
                yield break;
            }

            var stack = new Stack<BTNode<T>>();
            BTNode<T> current = head;

            while (stack.Count > 0 || current != null)
            {
                if (current != null)
                {
                    stack.Push(current);
                    current = current._left;
                }
                else
                {
                    current = stack.Pop();
                    yield return current.Data;
                    current = current._right;
                }
            }
        }

        //BFS traversal
        public IEnumerable<T> LevelOrder(BTNode<T> head)
        {
            if (head == null)
            {
                yield break;
            }

            var queue = new Queue<BTNode<T>>();
            queue.Enqueue(head);
            BTNode<T> node = null;

            while (queue.Count > 0)
            {
                node = queue.Dequeue();
                yield return node.Data;
                if (node._left != null)
                {
                    queue.Enqueue(node._left);
                }
                if (node._right != null)
                {
                    queue.Enqueue(node._right);
                }
            }
        }

        public IEnumerable<T> PostOrderRecursive(BTNode<T> head)
        {
            BTNode<T> current = head;
            if (current == null)
            {
                yield break;
            }

            PostOrderRecursive(current._left);
            PostOrderRecursive(current._right);
            yield return current.Data;
        }

        public IEnumerable<T> PostOrderNonRecursive(BTNode<T> head)
        {
            if (head == null)
            {
                yield break;
            }

            var visited = new HashSet<BTNode<T>>();
            var stack = new Stack<BTNode<T>>();
            BTNode<T> current = head;

            while (stack.Count > 0 || current != null)
            {
                if (current != null)
                {
                    stack.Push(current);
                    current = current._left;
                }
                else
                {
                    current = stack.Pop();
                    if (current._right != null && !visited.Contains(current))
                    {
                        stack.Push(current);
                        current = current._right;
                    }
                    else
                    {
                        visited.Add(current);
                        //return value and pus to stack
                        yield return current.Data;
                        current = null;
                    }
                }
            }
        }

        public bool Remove(T value)
        {
            if (Contains(value))
            {
                _head = Remove(_head, value);
                Count--;
                return true;
            }
            else
            {
                return false;
            }
        }
        private BTNode<T> Remove(BTNode<T> node, T value)
        {
            if (node == null)
                return null;
            int cmp = value.CompareTo(node.Data);

            //dig into left subtree, if the value we're looking for is smaller than the current value
            if (cmp < 0)
            {
                node._left = Remove(node._left, value);
            }
            //dig into right subtree, if the value we're looking for is smaller than the current value
            else if (cmp > 0)
            {
                node._left = Remove(node._right, value);
            }
            //find node you want to emove
            else
            {
                //happens with only a right subtree or no subtree at all. 
                if (node._left == null)
                {
                    var rightChild = node._right;
                    node.Data = default(T);
                    node = null;
                    return rightChild;
                }
                else if (node._right == null)
                {
                    var leftChild = node._left;
                    node.Data = default(T);
                    node = null;
                    return leftChild;
                }
                else
                {
                    //find the leftmost (smallest) value in right subtree by traversing as far left as possible in the right subtree
                    BTNode<T> tmp = DigLeft(node._right);

                    //swap the data
                    node.Data = tmp.Data;

                    //remove smallest value just swiped
                    node._right = Remove(node._right, tmp.Data);

                    //according to William Fisset
                    //if we wanted to find the largest node in left sub tree. Here is what to do
                    //var tmp = DigRight(node.Left);
                    // node.Data = tmp.Data;
                    //node.Left = Remove(node.Left, tmp.Data);
                }
            }

            return node;
        }

        private BTNode<T> DigLeft(BTNode<T> nodeRight)
        {
            var current = nodeRight;
            while (current._left != null)
            {
                current = current._left;
            }

            return current;
        }

        private BTNode<T> DigRight(BTNode<T> nodeLeft)
        {
            var current = nodeLeft;
            while (current._right != null)
            {
                current = current._right;
            }

            return current;
        }
        public bool AddNode(T value)
        {
            if (Contains(value))
            {
                return false;
            }

            _head = Add(_head, value);
            Count++;
            return true;
        }

        private BTNode<T> Add(BTNode<T> node, T value)
        {
            //base case: found leaf node
            if (node == null)
                node = new BTNode<T>(null, null, value);
            else
            {
                //place elems values in left subtree
                if (value.CompareTo(node.Data) < 0)
                {
                    node._left = Add(node._left, value);
                }
                else
                {
                    node._right = Add(node._right, value);
                }
            }
            return node;
        }
        private void AddTo(BTNode<T> node, T value)
        {
            //value is less than current node value
            if (value.CompareTo(node.Data) < 0)
            {
                if (node._left == null)
                {
                    //no left, make it new left node
                    node._left = new BTNode<T>(null, null, value);
                }
                else
                {
                    //add to left node
                    AddTo(node._left, value);
                }
            }
            else
            {
                //if theres no right add to the right
                if (node._right == null)
                {
                    node._right = new BTNode<T>(null, null, value);
                }
                else
                {
                    //add to Right node
                    AddTo(node._right, value);
                }
            }
        }
        public bool Contains(T value)
        {
            return Contains(_head, value);
        }

        public bool Contains(BTNode<T> node, T value)
        {
            if (node == null) return false;
            int cmp = value.CompareTo(node.Data);

            //dig into left subtree
            if (cmp < 0)
                return Contains(node._left, value);

            //dig into right subtree
            if (cmp > 0)
                return Contains(node._right, value);

            //found the value
            else return true;
        }
        public IEnumerator<T> GetEnumerator()
        {
            if (_head == null)
            {
                yield break;
            }

            var stack = new Stack<BTNode<T>>();
            BTNode<T> current = _head;

            while (stack.Count > 0 || current != null)
            {
                if (current != null)
                {
                    stack.Push(current);
                    current = current._left;
                }
                else
                {
                    current = stack.Pop();
                    yield return current.Data;
                    current = current._right;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region Problem solutions

        //first approach
        //find largest value in left node
        //find largest value in last node
        //return value.
        public int FindMaxElementRecursiveFirstApproach(BTNode<int> root)
        {
            int max = 0;
            if(root == null)
               return max;
            if (root._left == null && root._right == null)
                return root.Data;
            BTNode<int> current = root; 
            if (current._left.Data > max)
                max = current._left.Data;

            FindMaxElementRecursiveFirstApproach(current._left);
            FindMaxElementRecursiveFirstApproach(current._right);

            return max;
        }

        // traverse over in level order (left to right).. inserting into a queue in the process, then dequeue to get largest
        public int FindMaxElementLevelOrderSecondApproach(BTNode<int> root)
        {
            int max = 0;
            if (root == null)
                return max;
            Queue<BTNode<int>> q = new Queue<BTNode<int>>();
            BTNode<int> current = root;
            
            //Add node to queue
            q.Enqueue(current);
            BTNode<int> node = null;
            while (q.Count > 0)
            {
                node = q.Dequeue();
                if (max < node.Data)
                    max = node.Data;
                if (node._left != null)
                    q.Enqueue(node._left);
                if (node._right != null)
                    q.Enqueue(node._right);
            }
            return max;
        }

        // traverse over in level order (left to right).. inserting into a queue in the process, then dequeue to get largest
        public int FindMaxElementInOrderThirdApproach(BTNode<int> root)
        {
            List<int> elements = new List<int>();
            if (root == null)
                return 0;
            var stack = new Stack<BTNode<int>>();
            BTNode<int> current = root;
            
            while (current != null && stack.Count > 0)
            {
                if (current != null)
                {
                    stack.Push(current);
                    current = current._left;
                }
                else
                {
                    current = stack.Pop();
                    elements.Add(current.Data);
                    current = current._right;
                }
            }
            //convert to array and get last element
            int listLenght = elements.Count;
            return elements[listLenght - 1];
        }


        public void PrintLevelOrder(BTNode<T> head)
        {
            var iterator = LevelOrder(head);
            PrintBST(iterator);
        }

        public void PrintInOrder(BTNode<T> head)
        {
            var iterator = InOrderNonRecursive(head);
            PrintBST(iterator);
        }
        public static void PrintBST(IEnumerable<T> argument)
        {
            foreach (var i in argument)
            {
                Console.WriteLine(i);
            }
        }


        //geeksforgeeks.org
        public static BTNode<T> BSTToSortedLinkedList(BTNode<T> head, BTNode<T> head_ref)
        {
            if (head == null)
                return head_ref;

            //recursively convert right subtree
            head_ref = BSTToSortedLinkedList(head._right, head_ref);

            //insert root into linkedlist
            head._right = head_ref;

            //change left pointer of previous of head to point to null
            if (head_ref != null)
                (head_ref)._left = null;

            //change head of linkedlist
            head_ref = head;


            //recursively convert left subtree
            head_ref = BSTToSortedLinkedList(head._left, head_ref);

            return head_ref;
        }

        //geeksforgeeks.org
        /// <summary>
        /// 
        /// </summary>
        /// <param name="head">Root of minheap</param>
        /// <param name="headOfSortedList">pointer to head node of sorted linkedlist</param>
        /// <returns></returns>
        public static BTNode<T> SortedLinkedListToMinHeap(BTNode<T> head, BTNode<T> headOfSortedList)
        {
            if (head == null)
                return null;

            //store parent nodes in queue
            var queue = new Queue<BTNode<T>>();

            //first node is always the root node
            head = headOfSortedList;

            //advance pointer to the next node
            headOfSortedList = headOfSortedList._right;

            //set right child to null
            head._right = null;

            //add first node to the queue
            queue.Enqueue(head);

            //loop until end of linkedlist
            while (head != null)
            {
                //take parent node from queue
                var parent = queue.Dequeue();

               //take next two nodes from list add them as children of current parent node
               var leftChild = head;
               head = head._right;
               leftChild._right = null;
               queue.Enqueue(leftChild);

               //assign left child of parents
               parent._left = leftChild;
               if (head != null)
               {
                   var rightChild = head;
                   head = head._right;
                   rightChild._right = null;
                   queue.Enqueue(rightChild);

                   //assign the right child of parents
                   parent._right = rightChild;
               }
            }

            return head;
        }

        //geeksforgeeks.org
        public static BTNode<T> BSTToMinHeap(BTNode<T> head)
        {
            //head of linkedlist
            BTNode<T> headOfSortedList = null;

            //convert bst to linkedlist
            headOfSortedList = BSTToSortedLinkedList(head, headOfSortedList);

            //insert root into linkedlist
            head = null;

            head = BSTToSortedLinkedList(head, headOfSortedList);
            return head;
        }

        #endregion
    }
}
