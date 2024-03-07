using System.Collections.Generic;
using System.Drawing.Design;
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

    private static bool DFS(Space start, Space goal) // Search in Profundity
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

    private static bool BFS(Space start, Space goal) // Search in width
    {
 var queue = new Queue<Space>();
        var prev = new Dictionary<Space, Space>();

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
                break;
            }

            if (currNode.Bottom is not null && !prev.ContainsKey(currNode.Bottom))
            {
                prev[currNode.Bottom] = currNode;
                queue.Enqueue(currNode.Bottom);
            }

            if (currNode.Top is not null && !prev.ContainsKey(currNode.Top))
            {
                prev[currNode.Top] = currNode;
                queue.Enqueue(currNode.Top);
            }

            if (currNode.Left is not null && !prev.ContainsKey(currNode.Left))
            {
                prev[currNode.Left] = currNode;
                queue.Enqueue(currNode.Left);
            }

            if (currNode.Right is not null && !prev.ContainsKey(currNode.Right))
            {
                prev[currNode.Right] = currNode;
                queue.Enqueue(currNode.Right);
            }
        }

        var attempt = goal;
        while (attempt != start)
        {
            if (!prev.ContainsKey(attempt))
                return false;

            attempt.IsSolution = true;
            attempt = prev[attempt];
        }

        return false;
    }

    private static bool Dijkstra(Space start, Space goal) 
    {
        var queue = new PriorityQueue<Space, float>(); // First arg is type of queue, second is type priority
        var dist = new Dictionary<Space, float>();
        var prev = new Dictionary<Space, Space>();
        
        queue.Enqueue(start, 0.0f); // add item in queue
        dist[start] = 0.0f;

        while (queue.Count > 0)
        {
            var currNode = queue.Dequeue(); // Remove item in queue
            if(currNode == goal)
                break;
            List<Space> edges = new();

            if (currNode.Bottom is not null) 
                edges.Add(currNode.Bottom);
            if (currNode.Top is not null)
                edges.Add(currNode.Top);
            if (currNode.Left is not null)
                edges.Add(currNode.Left);
            if (currNode.Right is not null)
                edges.Add(currNode.Right);

            foreach (var edge in edges)
            {
                edge.Visited = true;
                var newWeight = dist[currNode] + 1;

                if (!dist.ContainsKey(edge))
                {
                    dist[edge] = float.PositiveInfinity;
                    prev[edge] = null!;
                }

                if (newWeight < dist[edge])
                {
                    dist[edge] = newWeight;
                    prev[edge] = currNode;
                    queue.Enqueue(edge, newWeight);
                }
            }
        }

        var attempt = goal;
        while (attempt != start)
        {
            if (!prev.ContainsKey(attempt))
                return false;
            
            attempt.IsSolution = true;
            attempt = prev[attempt];
        }

        return true;
    }

    private static bool AStar(Space start, Space goal)
    {
        var queue = new PriorityQueue<Space, float>();
        var dist = new Dictionary<Space, float>();
        var prev = new Dictionary<Space, Space>();
        
        queue.Enqueue(start, 0.0f);
        dist[start] = 0.0f;

        while (queue.Count > 0)
        {
            var currNode = queue.Dequeue();
            if(currNode == goal)
                break;
            List<Space> edges = new();

            if (currNode.Bottom is not null)
                edges.Add(currNode.Bottom);
            if (currNode.Top is not null)
                edges.Add(currNode.Top);
            if (currNode.Left is not null)
                edges.Add(currNode.Left);
            if (currNode.Right is not null)
                edges.Add(currNode.Right);

            foreach (var edge in edges)
            {
                edge.Visited = true;
                var hipotenusa = (currNode.X - goal.X) * (currNode.X - goal.X) + (currNode.Y - goal.Y) * (currNode.Y - goal.Y);
                var newWeight = dist[currNode] + hipotenusa + 1;

                if (!dist.ContainsKey(edge))
                {
                    dist[edge] = float.PositiveInfinity;
                    prev[edge] = null!;
                }

                if (newWeight < dist[edge])
                {
                    dist[edge] = newWeight;
                    prev[edge] = currNode;
                    queue.Enqueue(edge, newWeight);
                }
            }
        }

        var attempt = goal;
        while (attempt != start)
        {
            if (!prev.ContainsKey(attempt))
                return false;
            
            attempt.IsSolution = true;
            attempt = prev[attempt];
        }

        return true;
    }
}