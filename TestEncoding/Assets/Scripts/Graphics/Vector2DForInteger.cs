
namespace Graphics
{
    public struct Vector2DForInteger
    {
        #region instance fields

        private int m_x;

        public int x 
        {
            set { this.m_x = value; }
            get { return this.m_x; }
        }

        private int m_y;

        public int y 
        {
            set { this.m_y = value; }
            get { return this.m_y; }
        }

        #endregion

        #region static property

        public static Vector2DForInteger zero 
        {
            get { return new Vector2DForInteger(0,0); }
        }

        #endregion


        #region instance Constructors

        public Vector2DForInteger(int x , int y) 
        {
            this.m_x = x;
            this.m_y = y;
        }

        #endregion


        

    }
}


