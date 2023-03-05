using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dungeon
{
	public class BfsTask
	{
        //   public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
        //      {
        //	foreach(var chest in chests)
        //	{
        //              var trackPath = new Dictionary<Point, Point>();
        //		trackPath[start] = new Point(int.MinValue, int.MaxValue);
        //              var queue = new Queue<Point>();
        //		queue.Enqueue(start);
        //              bool chestIsFound = false;

        //              while (queue.Count != 0)
        //		{
        //			var point = queue.Dequeue();
        //			var validNeighbours = point.Neighbours()
        //                      .Where(p => !trackPath.ContainsKey(p))
        //                      .Where(p => map.InBounds(p))
        //				.Where(p => map.Dungeon[p.X, p.Y] == MapCell.Empty);
        //			foreach(var neighbour in validNeighbours)
        //			{
        //                      trackPath[neighbour] = point;
        //				if (chestIsFound = neighbour == chest)
        //					break;
        //                      queue.Enqueue(neighbour);
        //			}
        //			if (chestIsFound)
        //				break;
        //		}

        //		if (!chestIsFound)
        //			break;

        //		var list = new List<Point>();
        //		for(var point = chest; point != new Point(int.MinValue, int.MaxValue); point = trackPath[point])
        //		{
        //			list.Add(point);
        //		}
        //		list.Reverse();
        //		SinglyLinkedList<Point> path = null;
        //		foreach (var point in list)
        //			path = new SinglyLinkedList<Point>(point, path);
        //		yield return path;
        //	}
        //}

        //public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
        //{
        //    foreach (var chest in chests)
        //    {
        //        var queue = new Queue<SinglyLinkedList<Point>>();
        //        queue.Enqueue(new SinglyLinkedList<Point>(start));
        //        var visited = new HashSet<Point>();
        //        SinglyLinkedList<Point> foundChest = null;

        //        while (queue.Count != 0)
        //        {
        //            var pointInList = queue.Dequeue();
        //            var validNeighbours = pointInList.Value.Neighbours()
        //                .Where(p => !visited.Contains(p) && map.InBounds(p) && map.Dungeon[p.X, p.Y] == MapCell.Empty);
        //            foreach (var neighbour in validNeighbours)
        //            {
        //                var newItem = new SinglyLinkedList<Point>(neighbour, pointInList);
        //                visited.Add(neighbour);
        //                if (neighbour == chest)
        //                {
        //                    foundChest = newItem;
        //                    break;
        //                }
        //                queue.Enqueue(newItem);
        //            }
        //            if (foundChest != null)
        //                break;
        //        }

        //        if (foundChest == null)
        //            break;

        //        yield return foundChest;
        //    }
        //}

        //public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
        //{
        //    foreach (var chest in chests)
        //    {
        //        var queue = new Queue<SinglyLinkedList<Point>>();
        //        queue.Enqueue(new SinglyLinkedList<Point>(start));
        //        var visited = new HashSet<Point>();
        //        SinglyLinkedList<Point> foundChest = null;

        //        while (queue.Count != 0)
        //        {
        //            var pointInList = queue.Dequeue();
        //            var validNeighbours = pointInList.Value.Neighbours()
        //                .Where(p => !visited.Contains(p) && map.InBounds(p) && map.Dungeon[p.X, p.Y] == MapCell.Empty);
        //            foreach (var neighbour in validNeighbours)
        //            {
        //                var newItem = new SinglyLinkedList<Point>(neighbour, pointInList);
        //                visited.Add(neighbour);
        //                if (neighbour == chest)
        //                {
        //                    foundChest = newItem;
        //                    break;
        //                }
        //                queue.Enqueue(newItem);
        //            }
        //            if (foundChest != null)
        //                break;
        //        }

        //        if (foundChest == null)
        //            break;

        //        yield return foundChest;
        //    }
        //}

        public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
        {
            var queue = new Queue<SinglyLinkedList<Point>>();
            queue.Enqueue(new SinglyLinkedList<Point>(start));
            var visited = new HashSet<Point>();
            var paths = new Dictionary<Point, SinglyLinkedList<Point>>();

            foreach (var chest in chests)
                paths[chest] = new SinglyLinkedList<Point>(start);

            while (queue.Count != 0)
            {
                var pointInList = queue.Dequeue();
                var validNeighbours = pointInList.Value.Neighbours()
                    .Where(p => !visited.Contains(p) && map.InBounds(p) && map.Dungeon[p.X, p.Y] == MapCell.Empty);
                foreach (var neighbour in validNeighbours)
                {
                    var newItem = new SinglyLinkedList<Point>(neighbour, pointInList);
                    visited.Add(neighbour);
                    if (paths.ContainsKey(neighbour))
                    {
                        paths[neighbour] = newItem;
                    }
                    queue.Enqueue(newItem);
                }
            }

            foreach (var chest in chests)
                if (paths[chest].Value != start)
                    yield return paths[chest];
        }
    }

	public static class PointExtensions
	{
        public static IEnumerable<Point> Neighbours(this Point point)
        {
            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 ^ y == 0)
                        yield return new Point(point.X + x, point.Y + y);
                }
        }
    }
}