namespace Net
{
    using System;

    public enum BlockingState
    {
        Sendable,
        Waiting,
        TimeOut,
        Exception
    }
}

