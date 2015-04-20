

namespace Graphics
{
    public struct Color
    {
        public float a;

        public float b;

        public float g;

        public float r;

        public Color(float r, float g, float b) :this(r,g,b,1)
        {
        
        }

        public Color(float r, float g, float b, float a) 
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public const float alpha = 0.6f;

        public static Color green 
        {
            get { return new Color(0, 1, 0, alpha); }
        }

        public static Color blue 
        {
            get { return new Color(0, 0, 1, alpha); } 
        }

        public static Color red 
        {
            get { return new Color(1, 0, 0, alpha); }
        }

        public static Color gray
        {
            get { return new Color(0.5f, 0.5f, 0.5f, alpha); }
        }

        public static Color black
        {
            get { return new Color(0, 0, 0, alpha); }
        }

        public static Color clear
        {
            get { return new Color(0, 0, 0, alpha); }
        }

        public static Color cyan
        {
            get { return new Color(0, 1, 1, alpha); }
        }

        public static Color white
        {
            get { return new Color(1, 1, 1, alpha); }
        }

        public static Color yellow
        {
            get { return new Color(1f, 0.92f, 0.016f, alpha); }
        }

        public static Color operator *(Color c0 , Color c1)
        {
            return new Color(c0.r * c1.r,c0.g * c1.g,c0.b * c1.b,c0.a * c1.a);
        }
    }
}
