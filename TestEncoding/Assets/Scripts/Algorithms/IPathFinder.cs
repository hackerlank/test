namespace Algorithms
{
    using System;
    using System.Collections.Generic;

    internal interface IPathFinder
    {
        event PathFinderDebugHandler PathFinderDebug;

        List<PathFinderNode> FindPath(Point start, Point end);
        void FindPathStop();

        double CompletedTime { get; set; }

        bool DebugFoundPath { get; set; }

        bool DebugProgress { get; set; }

        bool Diagonals { get; set; }

        HeuristicFormula Formula { get; set; }

        bool HeavyDiagonals { get; set; }

        int HeuristicEstimate { get; set; }

        bool PunishChangeDirection { get; set; }

        int SearchLimit { get; set; }

        bool Stopped { get; }

        bool TieBreaker { get; set; }
    }
}

