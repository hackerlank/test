using System;
using System.Collections.Generic;

namespace Graphics
{
    public enum EdgeFlags : int
    {
        normal            = 0,
        swim              = 1 << 0,
        crawl             = 1 << 1,
        creep             = 1 << 3,
        jump              = 1 << 3,
        fly               = 1 << 4,
        grapple           = 1 << 5,
        goes_through_door = 1 << 6
    }

    public class NavGraphEdge : GraphEdge
    {
        protected EdgeFlags m_iFlags;

        public EdgeFlags Flags 
        {
            set { this.m_iFlags = value;}
            get { return this.m_iFlags; }
        }

        protected int m_iIDofIntersectingEntity;


        public NavGraphEdge(int from,
                            int to,
                            float cost,
                            EdgeFlags flags = EdgeFlags.normal,
                            int id = -1) : base(from,to,cost) 
        {
            this.m_iFlags = flags;
            this.m_iIDofIntersectingEntity = id;

           
        }

        public static bool operator ==(NavGraphEdge edge0, NavGraphEdge edge1)
        {
            return edge0.m_iFrom == edge1.m_iFrom &&
                   edge0.m_iTo == edge1.m_iTo &&
                   edge0.m_fCost == edge1.m_fCost &&
                   edge0.m_iFlags == edge1.Flags;
        }

        public static bool operator !=(NavGraphEdge edge0, NavGraphEdge edge1)
        {
            return edge0.m_iFrom != edge1.m_iFrom ||
                   edge0.m_iTo != edge1.m_iTo ||
                   edge0.m_fCost != edge1.m_fCost ||
                   edge0.m_iFlags == edge1.Flags;
        }


    }
}
