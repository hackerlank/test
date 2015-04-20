using System;

namespace Graphics
{
    public struct Vector2D
    {
        private float m_x;
        public float x
        {
            set
            {
                if (!float.IsNaN(value))
                {
                    this.m_x = value;
                }
            }
            get
            {
                return this.m_x;
            }
        }


        private float m_y;
        public float y
        {
            set
            {
                if (!float.IsNaN(value))
                {
                    this.m_y = value;
                }
            }
            get
            {
                return this.m_y;
            }
        }

        /// <summary>
        /// 此向量长度是否为0
        /// </summary>
        public bool IsZero
        {
            get
            {
                return LengthSq < float.MinValue;
            }
        }

        /// <summary>
        /// 该2D向量的平方长度
        /// </summary>
        public float LengthSq
        {
            get
            {
                return this.m_x * this.m_x + this.m_y * this.m_y;
            }
        }

        /// <summary>
        /// 该2D向量的长度
        /// </summary>
        public float Length
        {
            get
            {
                return (float)Math.Sqrt(LengthSq);
            }
        }

        public static Vector2D Zero
        {
            get { return new Vector2D(); }
        }


        public Vector2D(float x, float y)
        {
            this.m_x = x;
            this.m_y = y;
        }

        /// <summary>
        /// 规范化向量
        /// </summary>
        public void Normalize()
        {
            if (!IsZero)
            {

                float length = Length;

                this /= length;

            }
        }


        public override string ToString()
        {
            return "[" + this.m_x.ToString("f3") + "," + this.m_y.ToString("f3") + "]";
        }

        /// <summary>
        /// 判断2D向量v相对于当前向量的方位
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public int Sign(Vector2D v)
        {
            //未实现
            if (m_y * v.x > m_x * v.y)//顺时针旋转
            {
                return -1;
            }
            else //逆时针旋转
            {
                return 1;
            }

        }

        /// <summary>
        /// 返回值可用来与该向量凑成线性相关向量，可构造行向量
        /// 旋转矩阵
        /// </summary>
        /// <returns></returns>
        public Vector2D Perp()
        {
            return new Vector2D(-y, x);
        }

        public void Truncate(float max)
        {
            //截断

            if (!this.IsZero && this.LengthSq > max * max)
            {
                this.Normalize();
                this *= max;
            }

        }

        /// <summary>
        /// 求两点之间的距离
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static float Distance(Vector2D v0, Vector2D v1)
        {
            return (float)Math.Sqrt(DistanceSq(v0, v1));
        }

        /// <summary>
        /// 求两点之间的平方距离
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static float DistanceSq(Vector2D v0, Vector2D v1)
        {
            Vector2D v;
            v = v0 - v1;
            return v.LengthSq;
        }

        /// <summary>
        /// 求当前向量相对于向量norm的反射向量，注意方向
        /// </summary>
        /// <param name="norm"></param>
        public void Reflect(Vector2D norm)
        {
            norm.Normalize();

            //this
            this += this * norm * 2 * (-norm);
        }


        public static Vector2D Vec2DNormalize(Vector2D v)
        {
            float length = v.LengthSq;

            if (length > float.Epsilon)
            {
                v = v / length;
                return v;
            }
            else
            {
                return new Vector2D();
            }
        }


        //public void WrapAround(ref Vector2D pos , int MaxX , int MaxY) 
        //{
        //    if(pos.x > MaxX)
        //    {
        //        pos.x = 0;
        //    }

        //    if(pos.x < 0)
        //    {
        //        pos.x = MaxX;
        //    }

        //    if(pos.y > MaxY)
        //    {
        //        pos.y = 0;
        //    }

        //    if(pos.y < 0)
        //    {
        //        pos.y = MaxY;
        //    }
        //}

        public bool isSecondInFOVOfFirst(Vector2D posFirst, Vector2D facingFirst, Vector2D posSecond, float fov)
        {
            Vector2D toTarget = Vec2DNormalize(posSecond - posFirst);

            return facingFirst * toTarget >= (float)Math.Cos(fov / 2);
        }

        public void SetZero() { this.m_x = 0; this.m_y = 0; }


        #region 操作符重载

        public static Vector2D operator -(Vector2D v)
        {
            return new Vector2D(-v.m_x, -v.m_y);
        }

        public static Vector2D operator +(Vector2D v0, Vector2D v1)
        {
            Vector2D v = new Vector2D();
            v.m_x = v0.m_x + v1.m_x;
            v.m_y = v0.m_y + v1.m_y;
            return v;
        }

        public static Vector2D operator -(Vector2D v0, Vector2D v1)
        {
            Vector2D v = new Vector2D();
            v.m_x = v0.m_x - v1.m_x;
            v.m_y = v0.m_y - v1.m_y;
            return v;
        }

        public static bool operator ==(Vector2D v0, Vector2D v1)
        {
            return (v0.m_x == v1.m_x && v0.m_y == v1.m_y);
        }

        public static bool operator !=(Vector2D v0, Vector2D v1)
        {
            return (v0.m_x != v1.m_x || v0.m_y != v1.m_y);
        }


        public static float operator *(Vector2D v0, Vector2D v1)
        {
            return v0.m_x * v1.m_x + v0.m_y * v1.m_y;
        }

        public static Vector2D operator *(Vector2D v0, float mul)
        {
            Vector2D v;
            v.m_x = v0.m_x * mul;
            v.m_y = v0.m_y * mul;

            return v;
        }

        public static Vector2D operator *(float mul, Vector2D v0)
        {
            Vector2D v;
            v.m_x = v0.m_x * mul;
            v.m_y = v0.m_y * mul;

            return v;
        }


        public static Vector2D operator /(Vector2D v0, float div)
        {
            if (div == 0 || float.IsNaN(div))
            {
                return new Vector2D();
            }
            else
            {
                Vector2D v = new Vector2D();
                v.m_x = v0.m_x / div;
                v.m_y = v0.m_y / div;

                return v;
            }
        }

        #endregion

    }
}


