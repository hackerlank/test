﻿namespace Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class PriorityQueueB<T> : IPriorityQueue<T>
    {
        protected List<T> InnerList;
        protected IComparer<T> mComparer;

        public PriorityQueueB()
        {
            this.InnerList = new List<T>();
            this.mComparer = Comparer<T>.Default;
        }

        public PriorityQueueB(IComparer<T> comparer)
        {
            this.InnerList = new List<T>();
            this.mComparer = comparer;
        }

        public PriorityQueueB(IComparer<T> comparer, int capacity)
        {
            this.InnerList = new List<T>();
            this.mComparer = comparer;
            this.InnerList.Capacity = capacity;
        }

        public void Clear()
        {
            this.InnerList.Clear();
        }

        protected virtual int OnCompare(int i, int j)
        {
            return this.mComparer.Compare(this.InnerList[i], this.InnerList[j]);
        }

        public T Peek()
        {
            if (this.InnerList.Count > 0)
            {
                return this.InnerList[0];
            }
            return default(T);
        }

        public T Pop()
        {
            T local = this.InnerList[0];
            int i = 0;
            this.InnerList[0] = this.InnerList[this.InnerList.Count - 1];
            this.InnerList.RemoveAt(this.InnerList.Count - 1);
            while (true)
            {
                int j = i;
                int num2 = (2 * i) + 1;
                int num3 = (2 * i) + 2;
                if ((this.InnerList.Count > num2) && (this.OnCompare(i, num2) > 0))
                {
                    i = num2;
                }
                if ((this.InnerList.Count > num3) && (this.OnCompare(i, num3) > 0))
                {
                    i = num3;
                }
                if (i == j)
                {
                    return local;
                }
                this.SwitchElements(i, j);
            }
        }

        public int Push(T item)
        {
            int count = this.InnerList.Count;
            this.InnerList.Add(item);
            while (count != 0)
            {
                int j = (count - 1) / 2;
                if (this.OnCompare(count, j) >= 0)
                {
                    return count;
                }
                this.SwitchElements(count, j);
                count = j;
            }
            return count;
        }

        public void RemoveLocation(T item)
        {
            int index = -1;
            for (int i = 0; i < this.InnerList.Count; i++)
            {
                if (this.mComparer.Compare(this.InnerList[i], item) == 0)
                {
                    index = i;
                }
            }
            if (index != -1)
            {
                this.InnerList.RemoveAt(index);
            }
        }

        protected void SwitchElements(int i, int j)
        {
            T local = this.InnerList[i];
            this.InnerList[i] = this.InnerList[j];
            this.InnerList[j] = local;
        }

        public void Update(int i)
        {
            int num4;
            int num = i;
            while (true)
            {
                if (num == 0)
                {
                    break;
                }
                num4 = (num - 1) / 2;
                if (this.OnCompare(num, num4) >= 0)
                {
                    break;
                }
                this.SwitchElements(num, num4);
                num = num4;
            }
            if (num < i)
            {
                return;
            }
            while (true)
            {
                int j = num;
                int num3 = (2 * num) + 1;
                num4 = (2 * num) + 2;
                if ((this.InnerList.Count > num3) && (this.OnCompare(num, num3) > 0))
                {
                    num = num3;
                }
                if ((this.InnerList.Count > num4) && (this.OnCompare(num, num4) > 0))
                {
                    num = num4;
                }
                if (num == j)
                {
                    return;
                }
                this.SwitchElements(num, j);
            }
        }

        public int Count
        {
            get
            {
                return this.InnerList.Count;
            }
        }

        public T this[int index]
        {
            get
            {
                return this.InnerList[index];
            }
            set
            {
                this.InnerList[index] = value;
                this.Update(index);
            }
        }
    }
}
