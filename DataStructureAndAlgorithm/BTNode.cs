using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
    }
}
