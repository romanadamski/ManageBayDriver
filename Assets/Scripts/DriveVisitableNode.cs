using UnityEngine;

public class DriveVisitableNode : VisitableNode
{
    [SerializeField]
    private bool _isClickable;

    private void OnMouseUpAsButton()
    {
        if (!_isClickable) return;

        DriverController.Instance.DriveToTarget(this);
    }
}