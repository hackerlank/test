namespace Graphics
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2DForInteger
    {
        private int m_x;
        private int m_y;
        public Vector2DForInteger(int x, int y)
        {
            this.m_x = x;
            this.m_y = y;
        }

        public int x
        {
            get
            {
                return this.m_x;
            }
            set
            {
                this.m_x = value;
            }
        }
        public int y
        {
            get
            {
                return this.m_y;
            }
            set
            {
                this.m_y = value;
            }
        }
        public static Vector2DForInteger zero
        {
            get
            {
                return new Vector2DForInteger(0, 0);
            }
        }
    }
}

