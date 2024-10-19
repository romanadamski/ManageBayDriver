using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DriverController : MonoBehaviour
{
    [SerializeField]
    private DriverSettings driverSettings;

    private readonly Queue<VisitableNode> _targetQueue = new();
    private Rigidbody2D _rigidbody2D;
    private Vector2 _currentTarget;
    private bool _isDriving;
    private float _targetAngle;
    private float _speedFactor;
    private float _startingRotationSpeedFactor;
    private int _queueCount;
    private float _distanceToNextNode;

    private float RotationSpeed => _speedFactor * 3;

    private const float PARK_PRECISION = 0.05f;
    private const float PARK_DISTANCE = 3f;
    private const float TARGET_NODE_PRECISION = 0.5f;

    public static DriverController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!_isDriving) return;

        _distanceToNextNode = Vector2.Distance(transform.position, FixTargetPositionToRightSide(_currentTarget));

        SetTargetAngle();
        
        if (_queueCount == 0 && _distanceToNextNode <= PARK_DISTANCE)
        {
            //Slow down when parking
            _speedFactor = Mathf.Clamp(
                driverSettings.MoveSpeed * (_distanceToNextNode / PARK_DISTANCE),
                0.0f,
                _speedFactor);
        }
        else
        {
            _speedFactor = Mathf.Lerp(
               _speedFactor,
               driverSettings.MoveSpeed,
               Time.deltaTime * driverSettings.Acceleration);
        }

        if (_distanceToNextNode < TARGET_NODE_PRECISION)
        {
            if (_queueCount > 0) //There is next node
            {
                _currentTarget = _targetQueue.Dequeue().position;
                _queueCount = _targetQueue.Count;
            }
            else //Last node
            {
                if (_distanceToNextNode <= PARK_PRECISION)
                {
                    _isDriving = false;

                    _rigidbody2D.velocity = Vector2.zero;
                    _rigidbody2D.angularVelocity = 0;
                    _speedFactor = 0;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (!_isDriving) return;

        _rigidbody2D.AddForce((Vector2)transform.right * _speedFactor - _rigidbody2D.velocity, ForceMode2D.Impulse);
        _rigidbody2D.MoveRotation(Quaternion.Lerp(
            transform.rotation,
            Quaternion.AngleAxis(_targetAngle, Vector3.forward),
            Time.fixedDeltaTime * RotationSpeed * _startingRotationSpeedFactor));
    }

    private void SetTargetAngle()
    {
        Vector2 targetDir = FixTargetPositionToRightSide(_currentTarget) - (Vector2)transform.position;
        _targetAngle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
    }

    private void SetNodeQueue(IEnumerable<VisitableNode> queue)
    {
        foreach (var node in queue)
        {
            _targetQueue.Enqueue(node);
        }
    }

    private Vector2 FixTargetPositionToRightSide(Vector2 targetPosition) => targetPosition - ((Vector2)transform.up * 0.2f);

    private void StartDiriving()
    {
        _currentTarget = _targetQueue.Dequeue().position;
        _queueCount = _targetQueue.Count;

        StartCoroutine(TurnToTargetOnStart());
        _isDriving = true;
    }

    private IEnumerator TurnToTargetOnStart()
    {
        //Increase turning speed when starting
        _startingRotationSpeedFactor = 10;

        yield return new WaitForSeconds(0.2f);

        _startingRotationSpeedFactor = 1;
    }

    public void DriveToTarget(VisitableNode target)
    {
        if (_isDriving) return;

        var startNode = NodesManager.Instance.FindNearestNode(transform.position);
        var queue = NodesManager.Instance.FindShortestPath(startNode, target);

        SetNodeQueue(queue);
        StartDiriving();
    }
}
