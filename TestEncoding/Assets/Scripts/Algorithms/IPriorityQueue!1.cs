namespace Algorithms
{
    using System;

    public interface IPriorityQueue<T>
    {
        T Peek();
        T Pop();
        int Push(T item);
        void Update(int i);
    }
}

