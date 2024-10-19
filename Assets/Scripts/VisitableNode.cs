using System.Collections.Generic;
using UnityEngine;

public class VisitableNode : MonoBehaviour
{
    [SerializeField]
    private List<VisitableNode> _neighbours;

    public readonly List<NeighbourToVisit> neighboursToVisit = new ();
    public bool visited;
    public float distanceToTarget;
    public VisitableNode predcessor;

    public Vector2 position;

    private void Awake()
    {
        position = transform.position;
        FillNeighboursToVisit();
    }

    private void FillNeighboursToVisit()
    {
        foreach (var neighbour in _neighbours)
        {
            var neighbourToVisit = new NeighbourToVisit
            {
                neighbour = neighbour,
                distance = Vector2.Distance(neighbour.position, position)
            };
            neighboursToVisit.Add(neighbourToVisit);
        }
    }

    /// <summary>
    /// Reset visitable node values to default
    /// </summary>
    public void ResetNode()
    {
        visited = false;
        distanceToTarget = float.PositiveInfinity;
        predcessor = null;
    }
}
