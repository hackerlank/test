using System;
using System.Collections.Generic;
using UnityEngine;

namespace Graphics
{
    public class InvertedAABBox2D
    {

        private Vector2D m_vLeftTop;

        public Vector2D TopLeft 
        {
            get { return this.m_vLeftTop; }
        }

        private Vector2D m_vRightBottom;

        public Vector2D BottomRight
        {
            get { return this.m_vRightBottom; }
        }

        private Vector2D m_vCenter;

        public Vector2D Center 
        {
            get { return this.m_vCenter; }
        }

        public float Top 
        {
            get { return this.m_vLeftTop.y; }
        }

        public float Left 
        {
            get { return this.m_vLeftTop.x; }
        }

        public float Bottom 
        {
            get { return this.m_vRightBottom.y; }
        }

        public float Right
        {
            get { return this.m_vRightBottom.x; }
        }

        /// <summary>
        /// 原点在左上方
        /// </summary>
        /// <param name="tl"></param>
        /// <param name="br"></param>
        public InvertedAABBox2D(Vector2D lt, Vector2D rb) 
        {
            this.m_vLeftTop = lt;
            this.m_vRightBottom = rb;
            this.m_vCenter = (lt + rb) * 0.5f;
        }

        /// <summary>
        /// 判断两个矩形盒子是否相交
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        //public bool isOverlappedWith(InvertedAABBox2D other)
        //{
        //    return (other.Top > this.Bottom) &&
        //              (other.Bottom < this.Top) &&
        //              (other.Left < this.Right) ||
        //              (other.Right > this.Left);
        //}



    }
}
