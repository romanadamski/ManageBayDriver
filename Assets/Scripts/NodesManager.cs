using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodesManager : MonoBehaviour
{
    private List<VisitableNode> _nodes;
    private readonly AStarAlgorithm _aStarAlgorithm = new();

    public static NodesManager Instance { get; private set; }

    private void Awake()
    {
        _nodes = GetComponentsInChildren<VisitableNode>().ToList();
        Instance = this;
    }

    private void ResetNodes()
    {
        foreach(var node in _nodes)
        {
            node.ResetNode();
        }
    }

    /// <summary>
    /// Find visitable node nearest given position
    /// </summary>
    /// <param name="position">Position we search nearest node for</param>
    /// <returns>Visitable node nearest given position</returns>
    public VisitableNode FindNearestNode(Vector2 position)
    {
        if (_nodes.Count == 0) return null;

        var distance = float.PositiveInfinity;
        var nearestNode = _nodes.First();

        foreach (var node in _nodes)
        {
            var newPosition = Vector2.Distance(node.position, position);
            if (distance > newPosition)
            {
                distance = newPosition;
                nearestNode = node;
            }
        }

        return nearestNode;
    }

    /// <summary>
    /// Find shortest path from given source to target node in nodes graph
    /// </summary>
    /// <param name="source">Start node</param>
    /// <param name="target">Target node</param>
    /// <returns>Collection of nodes in order - from source to target</returns>
    public IEnumerable<VisitableNode> FindShortestPath(VisitableNode source, VisitableNode target)
    {
        ResetNodes();
        return _aStarAlgorithm.FindShortestPath(source, target);
    }
}