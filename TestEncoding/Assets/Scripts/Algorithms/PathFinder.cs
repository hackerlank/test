namespace Algorithms
{
    using map;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class PathFinder : IPathFinder
    {
        private List<PathFinderNode> mClose = new List<PathFinderNode>();
        private double mCompletedTime;
        private bool mDebugFoundPath;
        private bool mDebugProgress;
        private bool mDiagonals = true;
        private HeuristicFormula mFormula = HeuristicFormula.Manhattan;
        private TileFlag[,] mGrid;
        private bool mHeavyDiagonals;
        private int mHEstimate = 10;
        private int mHoriz;
        private PriorityQueueB<PathFinderNode> mOpen = new PriorityQueueB<PathFinderNode>(new ComparePFNode());
        private bool mPunishChangeDirection;
        private int mSearchLimit = 0x7d0;
        private bool mStop;
        private bool mStopped = true;
        private bool mTieBreaker;

        public event PathFinderDebugHandler PathFinderDebug;

        public PathFinder(TileFlag[,] grid)
        {
            if (grid == null)
            {
                throw new Exception("Grid cannot be null");
            }
            this.mGrid = grid;
        }

        public List<PathFinderNode> FindPath(Point start, Point end)
        {
            PathFinderNode node;
            sbyte[,] numArray;
            bool flag = false;
            int upperBound = this.mGrid.GetUpperBound(0);
            int num2 = this.mGrid.GetUpperBound(1);
            this.mStop = false;
            this.mStopped = false;
            this.mOpen.Clear();
            this.mClose.Clear();
            if (this.mDebugProgress && (this.PathFinderDebug != null))
            {
                this.PathFinderDebug(0, 0, start.X, start.Y, PathFinderNodeType.Start, -1, -1);
            }
            if (this.mDebugProgress && (this.PathFinderDebug != null))
            {
                this.PathFinderDebug(0, 0, end.X, end.Y, PathFinderNodeType.End, -1, -1);
            }
            if (this.mDiagonals)
            {
                numArray = new sbyte[,] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 }, { 1, -1 }, { 1, 1 }, { -1, 1 }, { -1, -1 } };
            }
            else
            {
                numArray = new sbyte[,] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 } };
            }
            node.G = 0;
            node.H = this.mHEstimate;
            node.F = node.G + node.H;
            node.X = start.X;
            node.Y = start.Y;
            node.PX = node.X;
            node.PY = node.Y;
            this.mOpen.Push(node);
            while ((this.mOpen.Count > 0) && !this.mStop)
            {
                node = this.mOpen.Pop();
                if (this.mDebugProgress && (this.PathFinderDebug != null))
                {
                    this.PathFinderDebug(0, 0, node.X, node.Y, PathFinderNodeType.Current, -1, -1);
                }
                if ((node.X == end.X) && (node.Y == end.Y))
                {
                    this.mClose.Add(node);
                    flag = true;
                    break;
                }
                if (this.mClose.Count > this.mSearchLimit)
                {
                    this.mStopped = true;
                    return null;
                }
                for (int i = 0; i < (!this.mDiagonals ? 4 : 8); i++)
                {
                    PathFinderNode node2;
                    node2.X = node.X + numArray[i, 0];
                    node2.Y = node.Y + numArray[i, 1];
                    if ((((node2.X >= 0) && (node2.Y >= 0)) && (node2.X <= upperBound)) && (node2.Y <= num2))
                    {
                        int num4;
                        if (this.mHeavyDiagonals && (i > 3))
                        {
                            num4 = node.G + 14;
                        }
                        else
                        {
                            num4 = node.G + 10;
                        }
                        if (!GraphUtils.IsContainsFlag(this.mGrid[node2.X, node2.Y], TileFlag.TILE_BLOCK_NORMAL))
                        {
                            int num5 = -1;
                            for (int j = 0; j < this.mOpen.Count; j++)
                            {
                                PathFinderNode node4 = this.mOpen[j];
                                if (node4.X == node2.X)
                                {
                                    PathFinderNode node5 = this.mOpen[j];
                                    if (node5.Y == node2.Y)
                                    {
                                        num5 = j;
                                        break;
                                    }
                                }
                            }
                            if (num5 != -1)
                            {
                                PathFinderNode node6 = this.mOpen[num5];
                                if (node6.G <= num4)
                                {
                                    continue;
                                }
                            }
                            int num7 = -1;
                            for (int k = 0; k < this.mClose.Count; k++)
                            {
                                PathFinderNode node7 = this.mClose[k];
                                if (node7.X == node2.X)
                                {
                                    PathFinderNode node8 = this.mClose[k];
                                    if (node8.Y == node2.Y)
                                    {
                                        num7 = k;
                                        break;
                                    }
                                }
                            }
                            if (num7 != -1)
                            {
                                PathFinderNode node9 = this.mClose[num7];
                                if (node9.G <= num4)
                                {
                                    continue;
                                }
                            }
                            node2.PX = node.X;
                            node2.PY = node.Y;
                            node2.G = num4;
                            switch (this.mFormula)
                            {
                                case HeuristicFormula.MaxDXDY:
                                {
                                    int introduced38 = Math.Abs((int) (node2.X - end.X));
                                    node2.H = this.mHEstimate * Math.Max(introduced38, Math.Abs((int) (node2.Y - end.Y)));
                                    break;
                                }
                                case HeuristicFormula.DiagonalShortCut:
                                {
                                    int introduced39 = Math.Abs((int) (node2.X - end.X));
                                    int num9 = Math.Min(introduced39, Math.Abs((int) (node2.Y - end.Y)));
                                    int introduced40 = Math.Abs((int) (node2.X - end.X));
                                    int num10 = introduced40 + Math.Abs((int) (node2.Y - end.Y));
                                    node2.H = ((this.mHEstimate * 2) * num9) + (this.mHEstimate * (num10 - (2 * num9)));
                                    break;
                                }
                                case HeuristicFormula.Euclidean:
                                {
                                    double introduced41 = Math.Pow((double) (node2.X - end.X), 2.0);
                                    node2.H = (int) (this.mHEstimate * Math.Sqrt(introduced41 + Math.Pow((double) (node2.Y - end.Y), 2.0)));
                                    break;
                                }
                                case HeuristicFormula.EuclideanNoSQR:
                                {
                                    double introduced42 = Math.Pow((double) (node2.X - end.X), 2.0);
                                    node2.H = (int) (this.mHEstimate * (introduced42 + Math.Pow((double) (node2.Y - end.Y), 2.0)));
                                    break;
                                }
                                case HeuristicFormula.Custom1:
                                {
                                    Point point = new Point(Math.Abs((int) (end.X - node2.X)), Math.Abs((int) (end.Y - node2.Y)));
                                    int num11 = Math.Abs((int) (point.X - point.Y));
                                    int num12 = Math.Abs((int) (((point.X + point.Y) - num11) / 2));
                                    node2.H = this.mHEstimate * (((num12 + num11) + point.X) + point.Y);
                                    break;
                                }
                                default:
                                {
                                    int introduced37 = Math.Abs((int) (node2.X - end.X));
                                    node2.H = this.mHEstimate * (introduced37 + Math.Abs((int) (node2.Y - end.Y)));
                                    break;
                                }
                            }
                            if (this.mTieBreaker)
                            {
                                int num13 = node.X - end.X;
                                int num14 = node.Y - end.Y;
                                int num15 = start.X - end.X;
                                int num16 = start.Y - end.Y;
                                int num17 = Math.Abs((int) ((num13 * num16) - (num15 * num14)));
                                node2.H += (int) (num17 * 0.001);
                            }
                            node2.F = node2.G + node2.H;
                            if (this.mDebugProgress && (this.PathFinderDebug != null))
                            {
                                this.PathFinderDebug(node.X, node.Y, node2.X, node2.Y, PathFinderNodeType.Open, node2.F, node2.G);
                            }
                            this.mOpen.Push(node2);
                        }
                    }
                }
                this.mClose.Add(node);
                if (this.mDebugProgress && (this.PathFinderDebug != null))
                {
                    this.PathFinderDebug(0, 0, node.X, node.Y, PathFinderNodeType.Close, node.F, node.G);
                }
            }
            if (flag)
            {
                PathFinderNode node3 = this.mClose[this.mClose.Count - 1];
                for (int m = this.mClose.Count - 1; m >= 0; m--)
                {
                    PathFinderNode node10 = this.mClose[m];
                    if (node3.PX == node10.X)
                    {
                        PathFinderNode node11 = this.mClose[m];
                        if (node3.PY == node11.Y)
                        {
                            goto Label_0856;
                        }
                    }
                    if (m != (this.mClose.Count - 1))
                    {
                        goto Label_08F3;
                    }
                Label_0856:
                    if (this.mDebugFoundPath && (this.PathFinderDebug != null))
                    {
                        PathFinderNode node12 = this.mClose[m];
                        PathFinderNode node13 = this.mClose[m];
                        PathFinderNode node14 = this.mClose[m];
                        PathFinderNode node15 = this.mClose[m];
                        this.PathFinderDebug(node3.X, node3.Y, node12.X, node13.Y, PathFinderNodeType.Path, node14.F, node15.G);
                    }
                    node3 = this.mClose[m];
                    continue;
                Label_08F3:
                    this.mClose.RemoveAt(m);
                }
                this.mStopped = true;
                return this.mClose;
            }
            Debug.LogError("Path Not Found!");
            this.mStopped = true;
            return null;
        }

        public void FindPathStop()
        {
            this.mStop = true;
        }

        public Point GetBestAccessiblePoint(Point start, Point end)
        {
            Point point = null;
            List<ManhattanPoint> list = new List<ManhattanPoint>();
            bool flag = false;
            for (int i = 1; !flag; i++)
            {
                list.Clear();
                int num2 = end.Y + i;
                int num3 = end.Y - i;
                int num4 = end.X - i;
                int num5 = end.X + i;
                for (int j = num4; j <= num5; j++)
                {
                    for (int k = num3; k <= num2; k++)
                    {
                        if (((j == num4) || (j == num5)) || ((k == num3) || (k == num2)))
                        {
                            list.Add(new ManhattanPoint(j, k, this.GetManhattan(new Vector2((float) j, (float) k), new Vector2((float) end.X, (float) end.Y))));
                            if (!GraphUtils.IsContainsFlag(this.mGrid[j, k], TileFlag.TILE_BLOCK_NORMAL))
                            {
                                flag = true;
                            }
                        }
                    }
                }
            }
            List<ManhattanPoint> list2 = new List<ManhattanPoint>();
            if (!flag)
            {
                return point;
            }
            foreach (ManhattanPoint point2 in list)
            {
                if (!GraphUtils.IsContainsFlag(this.mGrid[point2.X, point2.Y], TileFlag.TILE_BLOCK_NORMAL))
                {
                    list2.Add(point2);
                }
            }
            list2.Sort(new CompareWithManhattan());
            return new Point(list2[0].X, list2[0].Y);
        }

        public int GetManhattan(Vector2 p1, Vector2 p2)
        {
            return (((int) Math.Abs((float) (p1.x - p2.x))) + ((int) Math.Abs((float) (p1.y - p2.y))));
        }

        public double CompletedTime
        {
            get
            {
                return this.mCompletedTime;
            }
            set
            {
                this.mCompletedTime = value;
            }
        }

        public bool DebugFoundPath
        {
            get
            {
                return this.mDebugFoundPath;
            }
            set
            {
                this.mDebugFoundPath = value;
            }
        }

        public bool DebugProgress
        {
            get
            {
                return this.mDebugProgress;
            }
            set
            {
                this.mDebugProgress = value;
            }
        }

        public bool Diagonals
        {
            get
            {
                return this.mDiagonals;
            }
            set
            {
                this.mDiagonals = value;
            }
        }

        public HeuristicFormula Formula
        {
            get
            {
                return this.mFormula;
            }
            set
            {
                this.mFormula = value;
            }
        }

        public bool HeavyDiagonals
        {
            get
            {
                return this.mHeavyDiagonals;
            }
            set
            {
                this.mHeavyDiagonals = value;
            }
        }

        public int HeuristicEstimate
        {
            get
            {
                return this.mHEstimate;
            }
            set
            {
                this.mHEstimate = value;
            }
        }

        public bool PunishChangeDirection
        {
            get
            {
                return this.mPunishChangeDirection;
            }
            set
            {
                this.mPunishChangeDirection = value;
            }
        }

        public int SearchLimit
        {
            get
            {
                return this.mSearchLimit;
            }
            set
            {
                this.mSearchLimit = value;
            }
        }

        public bool Stopped
        {
            get
            {
                return this.mStopped;
            }
        }

        public bool TieBreaker
        {
            get
            {
                return this.mTieBreaker;
            }
            set
            {
                this.mTieBreaker = value;
            }
        }

        internal class ComparePFNode : IComparer<PathFinderNode>
        {
            public int Compare(PathFinderNode x, PathFinderNode y)
            {
                if (x.F > y.F)
                {
                    return 1;
                }
                if (x.F < y.F)
                {
                    return -1;
                }
                return 0;
            }
        }

        public class CompareWithManhattan : IComparer<PathFinder.ManhattanPoint>
        {
            public int Compare(PathFinder.ManhattanPoint p1, PathFinder.ManhattanPoint p2)
            {
                return p1.ManhattanDistance.CompareTo(p1.ManhattanDistance);
            }
        }

        public class ManhattanPoint
        {
            public int ManhattanDistance;
            public int X;
            public int Y;

            public ManhattanPoint(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public ManhattanPoint(int x, int y, int mandis)
            {
                this.X = x;
                this.Y = y;
                this.ManhattanDistance = mandis;
            }
        }
    }
}

