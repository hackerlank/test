namespace Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class PathFinderFast : IPathFinder
    {
        private PathFinderNodeFast[] mCalcGrid;
        private List<PathFinderNode> mClose = new List<PathFinderNode>();
        private int mCloseNodeCounter;
        private byte mCloseNodeValue = 2;
        private double mCompletedTime;
        private bool mDebugFoundPath;
        private bool mDebugProgress;
        private bool mDiagonals = true;
        private sbyte[,] mDirection = new sbyte[,] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 }, { 1, -1 }, { 1, 1 }, { -1, 1 }, { -1, -1 } };
        private int mEndLocation;
        private HeuristicFormula mFormula = HeuristicFormula.Manhattan;
        private bool mFound;
        private byte[,] mGrid;
        private ushort mGridX;
        private ushort mGridXMinus1;
        private ushort mGridY;
        private ushort mGridYLog2;
        private int mH;
        private bool mHeavyDiagonals;
        private int mHEstimate = 2;
        private int mHoriz;
        private int mLocation;
        private ushort mLocationX;
        private ushort mLocationY;
        private int mNewG;
        private int mNewLocation;
        private ushort mNewLocationX;
        private ushort mNewLocationY;
        private PriorityQueueB<int> mOpen;
        private byte mOpenNodeValue = 1;
        private bool mPunishChangeDirection;
        private int mSearchLimit = 0x7d0;
        private bool mStop;
        private bool mStopped = true;
        private bool mTieBreaker;

        public event PathFinderDebugHandler PathFinderDebug;

        public PathFinderFast(byte[,] grid)
        {
            if (grid == null)
            {
                throw new Exception("Grid cannot be null");
            }
            this.mGrid = grid;
            this.mGridX = (ushort) (this.mGrid.GetUpperBound(0) + 1);
            this.mGridY = (ushort) (this.mGrid.GetUpperBound(1) + 1);
            this.mGridXMinus1 = (ushort) (this.mGridX - 1);
            this.mGridYLog2 = (ushort) Math.Log((double) this.mGridY, 2.0);
            if ((Math.Log((double) this.mGridX, 2.0) != ((int) Math.Log((double) this.mGridX, 2.0))) || (Math.Log((double) this.mGridY, 2.0) != ((int) Math.Log((double) this.mGridY, 2.0))))
            {
                throw new Exception("Invalid Grid, size in X and Y must be power of 2");
            }
            if ((this.mCalcGrid == null) || (this.mCalcGrid.Length != (this.mGridX * this.mGridY)))
            {
                this.mCalcGrid = new PathFinderNodeFast[this.mGridX * this.mGridY];
            }
            this.mOpen = new PriorityQueueB<int>(new ComparePFNodeMatrix(this.mCalcGrid));
        }

        public List<PathFinderNode> FindPath(Point start, Point end)
        {
            PathFinderFast fast = this;
            lock (fast)
            {
                this.mFound = false;
                this.mStop = false;
                this.mStopped = false;
                this.mCloseNodeCounter = 0;
                this.mOpenNodeValue = (byte) (this.mOpenNodeValue + 2);
                this.mCloseNodeValue = (byte) (this.mCloseNodeValue + 2);
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
                this.mLocation = (start.Y << this.mGridYLog2) + start.X;
                this.mEndLocation = (end.Y << this.mGridYLog2) + end.X;
                this.mCalcGrid[this.mLocation].G = 0;
                this.mCalcGrid[this.mLocation].F = this.mHEstimate;
                this.mCalcGrid[this.mLocation].PX = (ushort) start.X;
                this.mCalcGrid[this.mLocation].PY = (ushort) start.Y;
                this.mCalcGrid[this.mLocation].Status = this.mOpenNodeValue;
                this.mOpen.Push(this.mLocation);
                while ((this.mOpen.Count > 0) && !this.mStop)
                {
                    this.mLocation = this.mOpen.Pop();
                    if (this.mCalcGrid[this.mLocation].Status != this.mCloseNodeValue)
                    {
                        this.mLocationX = (ushort) (this.mLocation & this.mGridXMinus1);
                        this.mLocationY = (ushort) (this.mLocation >> this.mGridYLog2);
                        if (this.mDebugProgress && (this.PathFinderDebug != null))
                        {
                            this.PathFinderDebug(0, 0, this.mLocation & this.mGridXMinus1, this.mLocation >> this.mGridYLog2, PathFinderNodeType.Current, -1, -1);
                        }
                        if (this.mLocation == this.mEndLocation)
                        {
                            this.mCalcGrid[this.mLocation].Status = this.mCloseNodeValue;
                            this.mFound = true;
                            break;
                        }
                        if (this.mCloseNodeCounter > this.mSearchLimit)
                        {
                            this.mStopped = true;
                            return null;
                        }
                        if (this.mPunishChangeDirection)
                        {
                            this.mHoriz = this.mLocationX - this.mCalcGrid[this.mLocation].PX;
                        }
                        for (int i = 0; i < (!this.mDiagonals ? 4 : 8); i++)
                        {
                            this.mNewLocationX = (ushort) (this.mLocationX + this.mDirection[i, 0]);
                            this.mNewLocationY = (ushort) (this.mLocationY + this.mDirection[i, 1]);
                            this.mNewLocation = (this.mNewLocationY << this.mGridYLog2) + this.mNewLocationX;
                            if (((this.mNewLocationX < this.mGridX) && (this.mNewLocationY < this.mGridY)) && (this.mGrid[this.mNewLocationX, this.mNewLocationY] != 0))
                            {
                                if (this.mHeavyDiagonals && (i > 3))
                                {
                                    this.mNewG = this.mCalcGrid[this.mLocation].G + ((int) (this.mGrid[this.mNewLocationX, this.mNewLocationY] * 2.41));
                                }
                                else
                                {
                                    this.mNewG = this.mCalcGrid[this.mLocation].G + this.mGrid[this.mNewLocationX, this.mNewLocationY];
                                }
                                if (this.mPunishChangeDirection)
                                {
                                    if (((this.mNewLocationX - this.mLocationX) != 0) && (this.mHoriz == 0))
                                    {
                                        this.mNewG += Math.Abs((int) (this.mNewLocationX - end.X)) + Math.Abs((int) (this.mNewLocationY - end.Y));
                                    }
                                    if (((this.mNewLocationY - this.mLocationY) != 0) && (this.mHoriz != 0))
                                    {
                                        this.mNewG += Math.Abs((int) (this.mNewLocationX - end.X)) + Math.Abs((int) (this.mNewLocationY - end.Y));
                                    }
                                }
                                if (((this.mCalcGrid[this.mNewLocation].Status != this.mOpenNodeValue) && (this.mCalcGrid[this.mNewLocation].Status != this.mCloseNodeValue)) || (this.mCalcGrid[this.mNewLocation].G > this.mNewG))
                                {
                                    this.mCalcGrid[this.mNewLocation].PX = this.mLocationX;
                                    this.mCalcGrid[this.mNewLocation].PY = this.mLocationY;
                                    this.mCalcGrid[this.mNewLocation].G = this.mNewG;
                                    switch (this.mFormula)
                                    {
                                        case HeuristicFormula.MaxDXDY:
                                            this.mH = this.mHEstimate * Math.Max(Math.Abs((int) (this.mNewLocationX - end.X)), Math.Abs((int) (this.mNewLocationY - end.Y)));
                                            break;

                                        case HeuristicFormula.DiagonalShortCut:
                                        {
                                            int num2 = Math.Min(Math.Abs((int) (this.mNewLocationX - end.X)), Math.Abs((int) (this.mNewLocationY - end.Y)));
                                            int num3 = Math.Abs((int) (this.mNewLocationX - end.X)) + Math.Abs((int) (this.mNewLocationY - end.Y));
                                            this.mH = ((this.mHEstimate * 2) * num2) + (this.mHEstimate * (num3 - (2 * num2)));
                                            break;
                                        }
                                        case HeuristicFormula.Euclidean:
                                            this.mH = (int) (this.mHEstimate * Math.Sqrt(Math.Pow((double) (this.mNewLocationY - end.X), 2.0) + Math.Pow((double) (this.mNewLocationY - end.Y), 2.0)));
                                            break;

                                        case HeuristicFormula.EuclideanNoSQR:
                                            this.mH = (int) (this.mHEstimate * (Math.Pow((double) (this.mNewLocationX - end.X), 2.0) + Math.Pow((double) (this.mNewLocationY - end.Y), 2.0)));
                                            break;

                                        case HeuristicFormula.Custom1:
                                        {
                                            Point point = new Point(Math.Abs((int) (end.X - this.mNewLocationX)), Math.Abs((int) (end.Y - this.mNewLocationY)));
                                            int num4 = Math.Abs((int) (point.X - point.Y));
                                            int num5 = Math.Abs((int) (((point.X + point.Y) - num4) / 2));
                                            this.mH = this.mHEstimate * (((num5 + num4) + point.X) + point.Y);
                                            break;
                                        }
                                        default:
                                            this.mH = this.mHEstimate * (Math.Abs((int) (this.mNewLocationX - end.X)) + Math.Abs((int) (this.mNewLocationY - end.Y)));
                                            break;
                                    }
                                    if (this.mTieBreaker)
                                    {
                                        int num6 = this.mLocationX - end.X;
                                        int num7 = this.mLocationY - end.Y;
                                        int num8 = start.X - end.X;
                                        int num9 = start.Y - end.Y;
                                        int num10 = Math.Abs((int) ((num6 * num9) - (num8 * num7)));
                                        this.mH += (int) (num10 * 0.001);
                                    }
                                    this.mCalcGrid[this.mNewLocation].F = this.mNewG + this.mH;
                                    if (this.mDebugProgress && (this.PathFinderDebug != null))
                                    {
                                        this.PathFinderDebug(this.mLocationX, this.mLocationY, this.mNewLocationX, this.mNewLocationY, PathFinderNodeType.Open, this.mCalcGrid[this.mNewLocation].F, this.mCalcGrid[this.mNewLocation].G);
                                    }
                                    this.mOpen.Push(this.mNewLocation);
                                    this.mCalcGrid[this.mNewLocation].Status = this.mOpenNodeValue;
                                }
                            }
                        }
                        this.mCloseNodeCounter++;
                        this.mCalcGrid[this.mLocation].Status = this.mCloseNodeValue;
                        if (this.mDebugProgress && (this.PathFinderDebug != null))
                        {
                            this.PathFinderDebug(0, 0, this.mLocationX, this.mLocationY, PathFinderNodeType.Close, this.mCalcGrid[this.mLocation].F, this.mCalcGrid[this.mLocation].G);
                        }
                    }
                }
                if (this.mFound)
                {
                    PathFinderNode node;
                    this.mClose.Clear();
                    int x = end.X;
                    int y = end.Y;
                    PathFinderNodeFast fast2 = this.mCalcGrid[(end.Y << this.mGridYLog2) + end.X];
                    node.F = fast2.F;
                    node.G = fast2.G;
                    node.H = 0;
                    node.PX = fast2.PX;
                    node.PY = fast2.PY;
                    node.X = end.X;
                    node.Y = end.Y;
                    while ((node.X != node.PX) || (node.Y != node.PY))
                    {
                        this.mClose.Add(node);
                        if (this.mDebugFoundPath && (this.PathFinderDebug != null))
                        {
                            this.PathFinderDebug(node.PX, node.PY, node.X, node.Y, PathFinderNodeType.Path, node.F, node.G);
                        }
                        x = node.PX;
                        y = node.PY;
                        fast2 = this.mCalcGrid[(y << this.mGridYLog2) + x];
                        node.F = fast2.F;
                        node.G = fast2.G;
                        node.H = 0;
                        node.PX = fast2.PX;
                        node.PY = fast2.PY;
                        node.X = x;
                        node.Y = y;
                    }
                    this.mClose.Add(node);
                    if (this.mDebugFoundPath && (this.PathFinderDebug != null))
                    {
                        this.PathFinderDebug(node.PX, node.PY, node.X, node.Y, PathFinderNodeType.Path, node.F, node.G);
                    }
                    this.mStopped = true;
                    return this.mClose;
                }
                this.mStopped = true;
                return null;
            }
        }

        public void FindPathStop()
        {
            this.mStop = true;
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
                if (this.mDiagonals)
                {
                    this.mDirection = new sbyte[,] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 }, { 1, -1 }, { 1, 1 }, { -1, 1 }, { -1, -1 } };
                }
                else
                {
                    this.mDirection = new sbyte[,] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 } };
                }
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

        internal class ComparePFNodeMatrix : IComparer<int>
        {
            private PathFinderFast.PathFinderNodeFast[] mMatrix;

            public ComparePFNodeMatrix(PathFinderFast.PathFinderNodeFast[] matrix)
            {
                this.mMatrix = matrix;
            }

            public int Compare(int a, int b)
            {
                if (this.mMatrix[a].F > this.mMatrix[b].F)
                {
                    return 1;
                }
                if (this.mMatrix[a].F < this.mMatrix[b].F)
                {
                    return -1;
                }
                return 0;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        internal struct PathFinderNodeFast
        {
            public int F;
            public int G;
            public ushort PX;
            public ushort PY;
            public byte Status;
        }
    }
}

