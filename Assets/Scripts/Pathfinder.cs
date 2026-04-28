using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TargetType
{
    Base,
    Tower,
}

struct PathNode
{
    public Cell current;
    public Cell previous;
    public int distanceFromOrigin;
}

public class Pathfinder : MonoBehaviour
{
    public List<Cell> findPath(Cell origin, TargetType target)
    {
        var closedList = new List<PathNode>();
        var openList = new List<PathNode>();
        var workingCandidate = new PathNode{ current = origin, previous = null, distanceFromOrigin = 0 };
        while (true)
        {
            closedList.Add(workingCandidate);
            openList.Remove(workingCandidate);

            if (target == TargetType.Base)
            {
                if (workingCandidate.current.getBase()) {
                    var path = new List<Cell>();
                    var cell = workingCandidate.current;
                    do {
                        path.Add(cell);
                        cell = closedList.Find(c => c.current == cell).previous;
                    } while (cell != null);
                    return path;
                }
            }

            var neighbours = workingCandidate.current.getNeighbours();
            var filter_a = neighbours.Where(n => n.accessible);
            var filter_b = filter_a.Where(n => openList.FindIndex(i => i.current == n) == -1);
            var filter_c = filter_b.Where(n => closedList.FindIndex(i => i.current == n) == -1);

            foreach (var neighbour in filter_c) {
                openList.Add(new PathNode { current = neighbour, previous = workingCandidate.current, distanceFromOrigin = workingCandidate.distanceFromOrigin + 1 });
            }

            if (openList.Count == 0)
            {
                throw new System.Exception("Broken loop");
            }

            openList.Sort((PathNode a, PathNode b) => { return a.distanceFromOrigin - b.distanceFromOrigin; });
            workingCandidate = openList[0];
        }
    }
}
