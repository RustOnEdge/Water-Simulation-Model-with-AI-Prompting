using UnityEngine;

public class Container : MonoBehaviour
{
    [Header("Container Settings")]
    private Vector2 containerOffset = Vector2.zero;
    
    [Header("Container Size")]
    [Range(1.0f, 20.0f)]
    [SerializeField] private float containerLength = 10f;
    [Range(1.0f, 20.0f)]
    [SerializeField] private float containerWidth = 10f;
    
    [Header("Container Rotation")]
    [Range(0f, 360f)]
    [SerializeField] private float rotation = 0f;
    
    [SerializeField] private Color containerColor = Color.white;    

    private Bounds containerBounds;
    private Matrix4x4 rotationMatrix;

    // Public properties
    public float Rotation => rotation;
    public Vector2 Size => new Vector2(containerLength, containerWidth);
    public Vector2 Offset => containerOffset;

    private void Start()
    {
        UpdateBounds();
        UpdateRotationMatrix();
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            UpdateBounds();
            UpdateRotationMatrix();
        }
    }

    private void UpdateRotationMatrix()
    {
        float radians = rotation * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);
        rotationMatrix = new Matrix4x4(
            new Vector4(cos, -sin, 0, 0),
            new Vector4(sin, cos, 0, 0),
            new Vector4(0, 0, 1, 0),
            new Vector4(0, 0, 0, 1)
        );
    }

    private void UpdateBounds()
    {
        containerBounds = new Bounds(containerOffset, new Vector2(containerLength, containerWidth));
    }

    public bool Contains(Vector2 point)
    {
        Vector2 localPos = point - containerOffset;
        Vector2 rotatedPos = RotatePoint(localPos, -rotation);
        
        return Mathf.Abs(rotatedPos.x) <= containerLength * 0.5f &&
               Mathf.Abs(rotatedPos.y) <= containerWidth * 0.5f;
    }

    public Vector2 GetClosestPoint(Vector2 point)
    {
        Vector2 localPos = point - containerOffset;
        Vector2 rotatedPos = RotatePoint(localPos, -rotation);
        
        Vector2 clampedPos = new Vector2(
            Mathf.Clamp(rotatedPos.x, -containerLength * 0.5f, containerLength * 0.5f),
            Mathf.Clamp(rotatedPos.y, -containerWidth * 0.5f, containerWidth * 0.5f)
        );
        
        return RotatePoint(clampedPos, rotation) + containerOffset;
    }

    public Vector2 GetNormal(Vector2 point)
    {
        Vector2 localPos = point - containerOffset;
        Vector2 rotatedPos = RotatePoint(localPos, -rotation);
        
        float dx = Mathf.Abs(rotatedPos.x) - containerLength * 0.5f;
        float dy = Mathf.Abs(rotatedPos.y) - containerWidth * 0.5f;
        
        Vector2 normal;
        if (dx > dy)
        {
            normal = new Vector2(Mathf.Sign(rotatedPos.x), 0);
        }
        else
        {
            normal = new Vector2(0, Mathf.Sign(rotatedPos.y));
        }
        
        return RotatePoint(normal, rotation).normalized;
    }

    private Vector2 RotatePoint(Vector2 point, float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(
            point.x * cos - point.y * sin,
            point.x * sin + point.y * cos
        );
    }
} 