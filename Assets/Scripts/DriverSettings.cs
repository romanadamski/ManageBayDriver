using UnityEngine;

[CreateAssetMenu(fileName = "DriverSettings", menuName = "ScriptableObjects/Driver Settings")]
public class DriverSettings : ScriptableObject
{
    [SerializeField]
    [Range(1,20)]
    private float _moveSpeed = 5f;
    public float MoveSpeed => _moveSpeed;

    [SerializeField]
    [Range(0.1f, 1)]
    private float _acceleration = 0.1f;
    public float Acceleration => _acceleration;
}
