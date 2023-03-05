using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dungeon
{
    public class DungeonTask
    {
        public static MoveDirection[] FindShortestPath(Map map)
        {
            var toStart = BfsTask.FindPaths(map, map.InitialPosition, map.Chests);
            var toEnd = BfsTask.FindPaths(map, map.Exit, map.Chests);

            var shortestPath = toStart
                .Join(toEnd, start => start.Value, end => end.Value,
                (start, end) => (start.Length + end.Length, JoinPaths(start, end)))
                .OrderBy(tuple => tuple.Item1)
                .Select(tuple => tuple.Item2)
                .FirstOrDefault();

            if (shortestPath == null)
            {
                var fromStartToEnd = BfsTask.FindPaths(map, map.Exit, new Point[] { map.InitialPosition }).FirstOrDefault();
                if (fromStartToEnd == null)
                    return new MoveDirection[0];
                return PointsToMoveDirections(fromStartToEnd);
            }

            return PointsToMoveDirections(shortestPath);
        }

        private static IEnumerable<Point> JoinPaths(SinglyLinkedList<Point> toStart, SinglyLinkedList<Point> toEnd)
        {
            return toStart.Skip(1).Reverse().Concat(toEnd);
        }

        private static MoveDirection[] PointsToMoveDirections(IEnumerable<Point> points)
        {
            return points
                .Zip(points.Skip(1), (first, second) => (first, second))
                .Select(tuple =>
                {
                    int dx = tuple.second.X - tuple.first.X;
                    int dy = tuple.first.Y - tuple.second.Y;
                    if (dx == 1)
                        return MoveDirection.Right;
                    else if (dx == -1)
                        return MoveDirection.Left;
                    else if (dy == 1)
                        return MoveDirection.Up;
                    else
                        return MoveDirection.Down;
                })
                .ToArray();
        }
    }
}
