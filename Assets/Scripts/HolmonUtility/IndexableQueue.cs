namespace HolmonUtility
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class IndexableQueue<T> : IEnumerable<T>
    {
        private LinkedList<T> items = new LinkedList<T>();

        public void Enqueue(T item)
        {
            items.AddLast(item);
        }

        public T Dequeue()
        {
            if (items.Count == 0)
                throw new InvalidOperationException("The queue is empty.");

            T value = items.First.Value;
            items.RemoveFirst();
            return value;
        }

        public T DequeueAt(int index)
        {
            if (index < 0 || index >= items.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");

            LinkedListNode<T> current = items.First;
            for (int i = 0; i < index; i++)
            {
                current = current.Next;
            }
            T value = current.Value;
            items.Remove(current);
            return value;
        }

        public T Peek()
        {
            if (items.Count == 0)
                throw new InvalidOperationException("The queue is empty.");

            return items.First.Value;
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= items.Count)
                    throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");

                LinkedListNode<T> current = items.First;
                for (int i = 0; i < index; i++)
                {
                    current = current.Next;
                }
                return current.Value;
            }
        }

        public int Count
        {
            get { return items.Count; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Clear()
        {
            items.Clear();
        }
    }
}