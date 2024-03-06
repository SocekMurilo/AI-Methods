using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.ApplicationServices;

namespace MazeAI;

public class Solver
{
    public int  Option { get; set; }
    public Maze Maze   { get; set; } = null!;

    public string Algorithm
    {
        get
        {
            return (Option % 4) switch
            {
                0 => "DFS",
                1 => "BFS",
                2 => "dijkstra",
                _ => "aStar"
            };
        }
    }

    public void Solve()
    {
        var goal = Maze.Spaces.FirstOrDefault(s => s.Exit);

        if (Maze.Root is null || goal is null)
            return;

        switch (Option % 4)
        {
            case 0:
                DFS(Maze.Root, goal);
                break;
            case 1:
                BFS(Maze.Root, goal);
                break;
            case 2:
                Dijkstra(Maze.Root, goal);
                break;
            case 3:
                AStar(Maze.Root, goal);
                break;
        }
    }

    private static bool DFS(Space start, Space goal)
    {
        if (start.Visited)
            return false;
        
        start.Visited = true;

        if (start ==  goal)
        {
            start.IsSolution = true;
            return true;
        }
        
        List<Space> crrStart = new List<Space> {start.Left, start.Top, start.Right, start.Bottom };

        start.IsSolution = crrStart.Any(crr => crr is not null && DFS(crr, goal));
        return start.IsSolution;

    }

    private static bool BFS(Space start, Space goal)
    {
        var queue = new Queue<Space>();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var currNode = queue.Dequeue();

            if (currNode.Visited)
                continue;

            currNode.Visited = true;

            if (currNode == goal)
            {
                currNode.IsSolution = true;
                return true;
            }

            var children = new List<Space> {currNode.Top, currNode.Left, currNode.Bottom, currNode.Right};

            foreach(var child in children)
                if(child is not null && !child.Visited)
                    queue.Enqueue(child);

        }
        return false;
    }

    private static bool Dijkstra(Space start, Space goal)
    {
        throw new NotImplementedException();
    }

    private static bool AStar(Space start, Space goal)
    {
        throw new NotImplementedException();
    }
}