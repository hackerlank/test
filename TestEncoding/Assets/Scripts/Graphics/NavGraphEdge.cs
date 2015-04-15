namespace Graphics
{
    using System;
    using System.Runtime.InteropServices;

    public class NavGraphEdge : GraphEdge
    {
        protected EdgeFlags m_iFlags;
        protected int m_iIDofIntersectingEntity;

        public NavGraphEdge(int from, int to, float cost, EdgeFlags flags = 0, int id = -1) : base(from, to, cost)
        {
            this.m_iFlags = flags;
            this.m_iIDofIntersectingEntity = id;
        }

        public static bool operator ==(NavGraphEdge edge0, NavGraphEdge edge1)
        {
            return ((((edge0.m_iFrom == edge1.m_iFrom) && (edge0.m_iTo == edge1.m_iTo)) && (edge0.m_fCost == edge1.m_fCost)) && (edge0.m_iFlags == edge1.Flags));
        }

        public static bool operator !=(NavGraphEdge edge0, NavGraphEdge edge1)
        {
            return ((((edge0.m_iFrom != edge1.m_iFrom) || (edge0.m_iTo != edge1.m_iTo)) || (edge0.m_fCost != edge1.m_fCost)) || (edge0.m_iFlags == edge1.Flags));
        }

        public EdgeFlags Flags
        {
            get
            {
                return this.m_iFlags;
            }
            set
            {
                this.m_iFlags = value;
            }
        }
    }
}

