using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;

namespace DataStructureAndAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var head =  new BTNode<int>(8);

            head.SetLeft(new BTNode<int>(4));
            head.SetRight(new BTNode<int>(12));

            head.GetRight().SetLeft(new BTNode<int>(10));
            head.GetRight().SetRight(new BTNode<int>(14));

            head.GetLeft().SetLeft(new BTNode<int>(2));
            head.GetLeft().SetRight(new BTNode<int>(6));

            //head = BTNode<int>.BSTToMinHeap(head);
            //head.PrintLevelOrder(head);

            head.PrintInOrder(head);


        }
    }
}
