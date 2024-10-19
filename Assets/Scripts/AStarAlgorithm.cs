using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStarAlgorithm
{
    /// <summary>
    /// Find shortest path from given source to target node in nodes graph using A* algorithm
    /// </summary>
    /// <param name="source">Start node</param>
    /// <param name="target">Target node</param>
    /// <returns>Collection of nodes in order - from source to target</returns>
    public IEnumerable<VisitableNode> FindShortestPath(VisitableNode source, VisitableNode target)
    {
        target.distanceToTarget = 0;
        var sortedList = new SortedList<float, VisitableNode>(new VisitableNodeComparer())
        {
            { target.distanceToTarget, target }
        };

        do
        {
            var visitingNode = sortedList.First().Value;
            if (visitingNode == source)
            {
                break;
            }
            sortedList.RemoveAt(0);

            foreach (var neighbour in visitingNode.neighboursToVisit)
            {
                if (neighbour.neighbour.visited) continue;

                //Heuristic function - distance in straight line from given node to target
                var straightDistance = Vector2.Distance(neighbour.neighbour.transform.position, target.transform.position);
                
                if (visitingNode.distanceToTarget + neighbour.distance + straightDistance < neighbour.neighbour.distanceToTarget)
                {
                    neighbour.neighbour.distanceToTarget = visitingNode.distanceToTarget + neighbour.distance + straightDistance;
                    neighbour.neighbour.predcessor = visitingNode;
                    sortedList.Add(neighbour.neighbour.distanceToTarget, neighbour.neighbour);
                }
            }

            visitingNode.visited = true;
        } while (sortedList.Count > 0);

        var node = source;

        var path = new Queue<VisitableNode>();
        while (node.predcessor != null)
        {
            path.Enqueue(node);
            node = node.predcessor;
        }
        path.Enqueue(target);

        return path;
    }
}