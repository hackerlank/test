namespace Graphics
{
    using System;

    public class InvertedAABBox2D
    {
        private Vector2D m_vCenter;
        private Vector2D m_vLeftTop;
        private Vector2D m_vRightBottom;

        public InvertedAABBox2D(Vector2D lt, Vector2D rb)
        {
            this.m_vLeftTop = lt;
            this.m_vRightBottom = rb;
            this.m_vCenter = (Vector2D) ((lt + rb) * 0.5f);
        }

        public float Bottom
        {
            get
            {
                return this.m_vRightBottom.y;
            }
        }

        public Vector2D BottomRight
        {
            get
            {
                return this.m_vRightBottom;
            }
        }

        public Vector2D Center
        {
            get
            {
                return this.m_vCenter;
            }
        }

        public float Left
        {
            get
            {
                return this.m_vLeftTop.x;
            }
        }

        public float Right
        {
            get
            {
                return this.m_vRightBottom.x;
            }
        }

        public float Top
        {
            get
            {
                return this.m_vLeftTop.y;
            }
        }

        public Vector2D TopLeft
        {
            get
            {
                return this.m_vLeftTop;
            }
        }
    }
}

