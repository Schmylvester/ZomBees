using System.Collections.Generic;
using System.Linq;
using UnityEngine;

struct PathNode
{
    public Cell current;
    public Cell previous;
    public int distanceFromOrigin;
}

public class Pathfinder : MonoBehaviour
{
    public delegate bool PathTarget(Cell _c);
    public List<Cell> findPath(Cell origin, PathTarget target)
    {
        var closedList = new List<PathNode>();
        var openList = new List<PathNode>();
        var workingCandidate = new PathNode{ current = origin, previous = null, distanceFromOrigin = 0 };
        while (true)
        {
            closedList.Add(workingCandidate);
            openList.Remove(workingCandidate);

            if (target.Invoke(workingCandidate.current))
            {
                if (workingCandidate.current.getBase()) {
                    var path = new List<Cell>();
                    var cell = workingCandidate.current;
                    do {
                        path.Add(cell);
                        cell = closedList.Find(c => c.current == cell).previous;
                    } while (cell != null);
                    path.Reverse();
                    return path;
                }
            }

            var neighbours = workingCandidate.current.getNeighbours()
                // remove inaccessible nodes
                .Where(n => n.accessible)
                // remove anything we are already going to check
                .Where(n => openList.FindIndex(i => i.current == n) == -1)
                // remove anything we already checked
                .Where(n => closedList.FindIndex(i => i.current == n) == -1)
                // add neighbours in a random order so that equidistant paths are not prioritised by order in the array
                .OrderBy(x => Random.value);


            foreach (var neighbour in neighbours) {
                openList.Add(new PathNode { current = neighbour, previous = workingCandidate.current, distanceFromOrigin = workingCandidate.distanceFromOrigin + 1 });
            }

            if (openList.Count == 0)
            {
                return new();
            }

            openList.Sort((a, b) => a.distanceFromOrigin.CompareTo(b.distanceFromOrigin));
            workingCandidate = openList[0];
        }
    }
}
