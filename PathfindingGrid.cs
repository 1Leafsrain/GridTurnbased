// PathfindingGrid.cs (utility)
using System.Collections.Generic;
using UnityEngine;

public static class PathfindingGrid
{
    public class Node
    {
        public Vector2Int pos;
        public int g, h;
        public Node parent;
        public int F => g + h;
        public Node(Vector2Int p) { pos = p; }
    }

    public static List<Vector2Int> FindPath(Vector2Int start, Vector2Int target, System.Func<Vector2Int, bool> isWalkable)
    {
        var open = new List<Node>();
        var closed = new HashSet<Vector2Int>();
        open.Add(new Node(start) { g = 0, h = Heuristic(start, target) });

        while (open.Count > 0)
        {
            // ambil node dengan F paling kecil
            open.Sort((a, b) => a.F.CompareTo(b.F));
            var current = open[0];
            open.RemoveAt(0);
            closed.Add(current.pos);

            if (current.pos == target)
            {
                // reconstruct path
                var path = new List<Vector2Int>();
                var n = current;
                while (n != null)
                {
                    path.Add(n.pos);
                    n = n.parent;
                }
                path.Reverse();
                return path;
            }

            foreach (var nb in GetNeighbors(current.pos))
            {
                if (closed.Contains(nb) || !isWalkable(nb)) continue;

                int tentativeG = current.g + 1;
                var existing = open.Find(x => x.pos == nb);
                if (existing == null)
                {
                    var node = new Node(nb) { g = tentativeG, h = Heuristic(nb, target), parent = current };
                    open.Add(node);
                }
                else if (tentativeG < existing.g)
                {
                    existing.g = tentativeG;
                    existing.parent = current;
                }
            }
        }

        return null; // no path
    }

    static int Heuristic(Vector2Int a, Vector2Int b) => Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);

    static List<Vector2Int> GetNeighbors(Vector2Int p)
    {
        return new List<Vector2Int> {
            new Vector2Int(p.x+1,p.y),
            new Vector2Int(p.x-1,p.y),
            new Vector2Int(p.x,p.y+1),
            new Vector2Int(p.x,p.y-1)
        };
    }
}
