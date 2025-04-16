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
    
    [SerializeField] private Color containerColor = Color.white;    

    private Bounds containerBounds;

    // Public properties
    public Vector2 Size => new Vector2(containerLength, containerWidth);
    public Vector2 Offset => containerOffset;

    private void Start()
    {
        UpdateBounds();
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            UpdateBounds();
        }
    }

    private void UpdateBounds()
    {
        containerBounds = new Bounds(containerOffset, new Vector2(containerLength, containerWidth));
    }

    public bool Contains(Vector2 point)
    {
        Vector2 localPos = point - containerOffset;
        return Mathf.Abs(localPos.x) <= containerLength * 0.5f &&
               Mathf.Abs(localPos.y) <= containerWidth * 0.5f;
    }

    public Vector2 GetClosestPoint(Vector2 point)
    {
        Vector2 localPos = point - containerOffset;
        Vector2 clampedPos = new Vector2(
            Mathf.Clamp(localPos.x, -containerLength * 0.5f, containerLength * 0.5f),
            Mathf.Clamp(localPos.y, -containerWidth * 0.5f, containerWidth * 0.5f)
        );
        
        return clampedPos + containerOffset;
    }

    public Vector2 GetNormal(Vector2 point)
    {
        Vector2 localPos = point - containerOffset;
        float dx = Mathf.Abs(localPos.x) - containerLength * 0.5f;
        float dy = Mathf.Abs(localPos.y) - containerWidth * 0.5f;
        
        if (dx > dy)
        {
            return new Vector2(Mathf.Sign(localPos.x), 0);
        }
        else
        {
            return new Vector2(0, Mathf.Sign(localPos.y));
        }
    }
} 