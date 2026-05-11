using System.Collections.Generic;
using System.Linq;
using UnityEngine;

struct PathNode
{
    public Cell current;
    public Cell previous;
    public int distanceFromOrigin;
}

public class PriorityQueue<T>
{
    private List<(T item, int priority)> heap = new();

    public int Count => heap.Count;

    public void enqueue(T item, int priority)
    {
        heap.Add((item, priority));
        heapifyUp(heap.Count - 1);
    }

    public T dequeue()
    {
        var root = heap[0].item;

        heap[0] = heap[^1];
        heap.RemoveAt(heap.Count - 1);

        heapifyDown(0);

        return root;
    }

    private void heapifyUp(int index)
    {
        while (index > 0)
        {
            int parent = (index - 1) / 2;

            if (heap[index].priority >= heap[parent].priority)
                break;

            (heap[index], heap[parent]) =
                (heap[parent], heap[index]);

            index = parent;
        }
    }

    private void heapifyDown(int index)
    {
        while (true)
        {
            int left = index * 2 + 1;
            int right = index * 2 + 2;
            int smallest = index;

            if (left < heap.Count &&
                heap[left].priority < heap[smallest].priority)
            {
                smallest = left;
            }

            if (right < heap.Count &&
                heap[right].priority < heap[smallest].priority)
            {
                smallest = right;
            }

            if (smallest == index)
                break;

            (heap[index], heap[smallest]) =
                (heap[smallest], heap[index]);

            index = smallest;
        }
    }
}

public class Pathfinder : MonoBehaviour
{
    public delegate bool PathTarget(Cell c);

    public List<Cell> findPath(Cell _origin, PathTarget _target, Cell _excludeCell = null)
    {
        HashSet<Cell> closedSet = new();
        Dictionary<Cell, Cell> cameFrom = new();
        Dictionary<Cell, int> weight = new();
        PriorityQueue<Cell> openQueue = new();

        weight[_origin] = 0;

        openQueue.enqueue(_origin, 0);

        while (openQueue.Count > 0)
        {
            Cell current = openQueue.dequeue();

            if (closedSet.Contains(current))
                continue;

            closedSet.Add(current);

            if (_target(current))
            {
                return reconstructPath(current, cameFrom);
            }

            var neighbours = current.getNeighbours()
                .Where(n => n.accessible)
                .OrderBy(_ => Random.value);

            foreach (var neighbour in neighbours)
            {
                if (closedSet.Contains(neighbour) || _excludeCell == neighbour)
                    continue;

                int moveCost = 1;

                int newDistance = weight[current] + moveCost;

                if (!weight.ContainsKey(neighbour) ||
                    newDistance < weight[neighbour])
                {
                    weight[neighbour] = newDistance;

                    cameFrom[neighbour] = current;

                    openQueue.enqueue(neighbour, newDistance);
                }
            }
        }

        return new List<Cell>();
    }

    private List<Cell> reconstructPath(
        Cell end,
        Dictionary<Cell, Cell> cameFrom)
    {
        List<Cell> path = new List<Cell>();

        Cell current = end;

        while (current != null)
        {
            path.Add(current);

            if (!cameFrom.ContainsKey(current))
                break;

            current = cameFrom[current];
        }

        path.Reverse();

        return path;
    }
}