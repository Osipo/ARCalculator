using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ARCalc.Structures
{
    public class LinkedStack<T> : ICollection<T>, IEnumerable<T>, IEnumerable
    {
        private ElementType<T> _head;
        private Int32 _count;//count

        public LinkedStack()
        {
            this._count = 0;
        }

        //EMPTY(S): BOOLEAN
        public Boolean IsEmpty()
        {
            return this._count == 0;
        }

        //PUSH
        public void Push(T item)
        {
            if (_count == Int32.MaxValue)
                throw new InvalidOperationException("Cannot add new item. Maximum capacity (max value of Int32) is exceeded.");
            _count += 1;
            ElementType<T> node = new ElementType<T>();
            node.Element = item;
            node.Next = _head;
            _head = node;
        }

        //POP
        public void Pop() 
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Cannot Pop from empty stack");
            }
            else
            {
                ElementType<T> temp = _head;
                _head = _head.Next;
                _count -= 1;
            }
        }

        //TOP
        public T Top()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Error. Stack is empty.");
            }
            else return _head.Element;
        }

        public void Add(T item)
        {
            Push(item);
        }

        public bool Remove(T item)
        {
            if (IsEmpty())
            {
                Console.WriteLine("Error. Stack is empty");
                return false;
            }
            else if (Top().Equals(item))
            {
                Pop();
                return true;
            }
            else
                return false;
        }

        public bool Contains(T item)
        {
            ElementType<T> q = _head;
            while (q != null)
            {
                if (q.Element.Equals(item))
                {
                    return true;
                }
                q = q.Next;
            }
            return false;
        }

        public Boolean IsReadOnly
        {
            get { return false; }
        }

        public Int32 Count
        {
            get
            {
                return this._count;
            }
        }

        //MAKENULL
        public void Clear()
        {
            this._head = null;
            this._count = 0;
        }

        public void CopyTo(T[] array, Int32 arrayIndex)
        {
            if (arrayIndex < 0 || arrayIndex >= array.Length)
            {
                throw new ArgumentOutOfRangeException($"Argument {nameof(arrayIndex)} is out of range of {nameof(array)} index.");
            }
            ElementType<T> c = _head;
            for (Int32 i = arrayIndex; i < array.Length && c != null; i++)
            {
                array[i] = c.Element;
                c = c.Next;
            }
        }

        public T[] ToArray()
        {
            ElementType<T> c = _head;
            T[] r = new T[_count];
            for (Int32 i = 0; i < r.Length && c != null; i++)
            {
                r[i] = c.Element;
                c = c.Next;
            }
            return r;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            ElementType<T> current = _head;
            while (current != null)
            {
                yield return current.Element;
                current = current.Next;
            }
        }

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("$[ ");
            ElementType<T> c = _head;
            while (c != null)
            {
                sb.Append(c.Element.ToString() + " ");
                c = c.Next;
            }
            sb.Append("]");
            return sb.ToString();
        }
    }
}
