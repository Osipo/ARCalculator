using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ARCalc.Structures
{
    public class LinkedList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
    {
        private ElementType<T> _head;//pointer to the first element. (->) Ignored:: Element field.
        private ElementType<T> _tail;//last element of the list.

        private Int32 _count;

        public LinkedList()
        {
            this._count = 0;
            this._head = new ElementType<T>();
            this._tail = new ElementType<T>();
            this._tail.Next = null;
        }

        public LinkedList(IEnumerable<T> seq) : this()
        {
            foreach (var item in seq)
                Add(item);
        }

        public Int32 Count
        {
            get
            {
                return _count;
            }
        }

        //EMPTY(L): BOOLEAN
        public Boolean IsEmpty()
        {
            return _count == 0;
        }

        public Boolean IsReadOnly
        {
            get
            {
                return false;
            }
        }

        //COPY TO ARRAY(array)
        public void CopyTo(T[] array, Int32 arrayIndex)
        {
            ElementType<T> first = _head.Next;
            if (arrayIndex < 0 || arrayIndex >= array.Length)
            {
                throw new ArgumentOutOfRangeException($"Argument {nameof(arrayIndex)} is out of range of {nameof(array)} index.");
            }
            Int32 i = arrayIndex;
            while (first != null && i < array.Length)
            {//THROW ArgumentException IF(array.Length - arrayIndex < Count)
                array[i] = first.Element;
                first = first.Next;
                i += 1;
            }
        }

        //RETRIEVE.
        public T this[Int32 index]
        {
            get
            {
                ElementType<T> b = _head;
                Int32 q = 0;
                while (b.Next != null && q < index)
                {
                    q += 1;
                    b = b.Next;
                }
                return b.Element;
            }
            set
            {
                ElementType<T> b = _head;
                Int32 q = 0;
                while (b.Next != null && q < index)
                {
                    q += 1;
                    b = b.Next;
                }
                b.Element = value;
            }
        }

        //0 pointer to the _head. So pos must be [1..count]
        public void Insert(Int32 p, T item)
        {
            if (p <= 0 || p > Count + 1)
            {
                throw new ArgumentOutOfRangeException($"Argument {nameof(p)} is out of range of list index. Correct values are in [1...{Count}]");
            }

            #region _tail
            //if(IsEmpty)
            else if (Count == 0)
            {
                this._tail.Element = item;
                _count += 1;
                this._head.Next = _tail;
                return;
            }
            //Append.
            else if (p == Count + 1)
            {
                _tail.Next = new ElementType<T>();
                _tail.Next.Element = item;
                _tail = _tail.Next;
                _count += 1;
                return;
            }
            #endregion
            Int32 q = 1;
            ElementType<T> elem = new ElementType<T>();
            elem.Element = item;
            ElementType<T> temp = _head;//_head pos = 0.
            while (q < p)
            {//move to p.
                temp = temp.Next;
                q += 1;
            }
            ElementType<T> pp = temp;
            temp = temp.Next;//temp = p.next 
            pp.Next = elem;//p.next.element = x
            elem.Next = temp;//p.next.next = temp
            this._count += 1;
        }

        //INSERT.
        public void Insert(T item, ElementType<T> position)
        {
            if (_count == Int32.MaxValue)
                throw new InvalidOperationException("Cannot add new item. Maximum capacity (max value of Int32) is exceeded.");
            ElementType<T> temp = position.Next;
            ElementType<T> n = new ElementType<T>();
            n.Element = item;
            n.Next = temp;
            position.Next = n;
            this._count += 1;
        }

        public void Add(T item)
        {
            Insert(Count + 1, item);//to the end.
        }


        public void RemoveAt(Int32 p)
        {
            Int32 q = 1;
            if (Count == 0)
                throw new InvalidOperationException("Cannot remove from empty list.");
            else if (p > Count || p <= 0)
                throw new ArgumentOutOfRangeException($"Argument: {nameof(p)} is out of range. Valid values are in [1...{Count}]");
            ElementType<T> pp = _head;
            while (q < p && pp.Next != null)
            {
                pp = pp.Next;
                q += 1;
            }
            pp.Next = pp.Next.Next;
            this._count -= 1;
        }

        //DELETE
        public Boolean Remove(T item)
        {
            ElementType<T> p = _head;
            while (p.Next != null)
            {
                if (p.Next.Element.Equals(item))
                {
                    p.Next = p.Next.Next;
                    _count -= 1;
                    return true;
                }
                p = p.Next;
            }
            return false;
        }

        //MAKENULL
        public void Clear()
        {
            this._count = 0;
            this._head.Next = null;
            this._tail = null;
            this._tail = new ElementType<T>();
            this._tail.Next = null;
        }

        public Int32 IndexOf(T item)
        {
            ElementType<T> p = _head.Next;
            Int32 idx = 1;
            while (p != null)
            {
                if (p.Element.Equals(item))
                {
                    return idx;
                }
                idx += 1;
                p = p.Next;
            }
            return -1;
        }

        public Boolean Contains(T item)
        {
            if (IndexOf(item) != -1)
                return true;
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            ElementType<T> p = _head.Next;
            while (p != null)
            {
                yield return p.Element;
                p = p.Next;
            }
        }


        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[ ");
            ElementType<T> p = _head.Next;
            while (p != null)
            {
                sb.Append(p.Element.ToString() + " ");
                p = p.Next;
            }
            sb.Append("]");
            return sb.ToString();
        }
    }
}
