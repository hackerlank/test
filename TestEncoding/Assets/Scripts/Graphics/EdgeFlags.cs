namespace Graphics
{
    using System;

    public enum EdgeFlags
    {
        crawl = 2,
        creep = 8,
        fly = 0x10,
        goes_through_door = 0x40,
        grapple = 0x20,
        jump = 8,
        normal = 0,
        swim = 1
    }
}

