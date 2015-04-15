namespace Graphics
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct Color
    {
        public const float alpha = 0.6f;
        public float a;
        public float b;
        public float g;
        public float r;
        public Color(float r, float g, float b) : this(r, g, b, 1f)
        {
        }

        public Color(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public static Color green
        {
            get
            {
                return new Color(0f, 1f, 0f, 0.6f);
            }
        }
        public static Color blue
        {
            get
            {
                return new Color(0f, 0f, 1f, 0.6f);
            }
        }
        public static Color red
        {
            get
            {
                return new Color(1f, 0f, 0f, 0.6f);
            }
        }
        public static Color gray
        {
            get
            {
                return new Color(0.5f, 0.5f, 0.5f, 0.6f);
            }
        }
        public static Color black
        {
            get
            {
                return new Color(0f, 0f, 0f, 0.6f);
            }
        }
        public static Color clear
        {
            get
            {
                return new Color(0f, 0f, 0f, 0.6f);
            }
        }
        public static Color cyan
        {
            get
            {
                return new Color(0f, 1f, 1f, 0.6f);
            }
        }
        public static Color white
        {
            get
            {
                return new Color(1f, 1f, 1f, 0.6f);
            }
        }
        public static Color yellow
        {
            get
            {
                return new Color(1f, 0.92f, 0.016f, 0.6f);
            }
        }
        public static Color operator *(Color c0, Color c1)
        {
            return new Color(c0.r * c1.r, c0.g * c1.g, c0.b * c1.b, c0.a * c1.a);
        }
    }
}

