using UnityEngine;

public class GOD_Rotator : MonoBehaviour
{
    public enum RotationDirection
    {
        Clockwise,
        CounterClockwise
    }

    [Header("âÒì]ê›íË")]
    [SerializeField] private RotationDirection direction = RotationDirection.Clockwise;
    [SerializeField] private float speed = 90f; // ìx/ïb

    void Update()
    {
        float rotationAmount = speed * Time.deltaTime;

        if (direction == RotationDirection.Clockwise)
        {
            rotationAmount *= -1f;
        }

        transform.Rotate(0f, 0f, rotationAmount);
    }
}