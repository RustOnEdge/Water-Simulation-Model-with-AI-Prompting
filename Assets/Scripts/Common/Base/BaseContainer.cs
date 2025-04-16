using UnityEngine;

public abstract class BaseContainer : MonoBehaviour
{
    [Header("Container Settings")]
    protected Vector3 containerOffset = Vector3.zero;
    
    [Header("Container Size")]
    [Range(1.0f, 20.0f)]
    [SerializeField] protected float containerLength = 10f;
    [Range(1.0f, 20.0f)]
    [SerializeField] protected float containerWidth = 10f;
    [Range(1.0f, 20.0f)]
    [SerializeField] protected float containerHeight = 10f; // Only used in 3D
    
    [SerializeField] protected Color containerColor = Color.white;    

    protected Bounds containerBounds;

    protected virtual void Start()
    {
        UpdateBounds();
    }

    protected virtual void OnValidate()
    {
        if (Application.isPlaying)
        {
            UpdateBounds();
        }
    }

    protected abstract void UpdateBounds();
    public abstract bool Contains(Vector3 point);
    public abstract Vector3 GetClosestPoint(Vector3 point);
    public abstract Vector3 GetNormal(Vector3 point);

    // Common properties that work for both 2D and 3D
    public Vector3 Offset => containerOffset;
    public Vector3 Size => new Vector3(containerLength, containerWidth, containerHeight);
    public Color Color => containerColor;
} 