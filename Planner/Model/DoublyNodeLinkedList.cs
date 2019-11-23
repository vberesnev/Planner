using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Model
{
    public class DoublyNodeLinkedList<T> : IEnumerable<T>
    {
        private DoublyNode<T> head;
        private DoublyNode<T> current;
        private int count;

        public void Add(DoublyNode<T> node)
        {
            if (head == null)
            {
                head = node;
                head.Previous = node;
                head.Next = node;
            }
            else
            {
                node.Previous = head.Previous;
                node.Next = head;
                head.Previous.Next = node;
                head.Previous = node;
            }
            count++;
        }

        public DoublyNode<T> Current(T data)
        {
            if (head == null) return null;
            current = head;
            do
            {
                if (current.Data.Equals(data))
                    return current;
                current = current.Next;
            }
            while (current != head);
            return null;
        }

        public DoublyNode<T> MoveNext()
        {
            current = current.Next;
            return current;
        }

        public DoublyNode<T> MovePrevious()
        {
            current = current.Previous;
            return current;
        }

        public void Clear()
        {
            head = null;
            count = 0;
        }

        public int Count => count;

        public IEnumerator<T> GetEnumerator()
        {
            current = head;
            do
            {
                if (current != null)
                {
                    yield return current.Data;
                    current = current.Next;
                }
            }
            while (current != head);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
