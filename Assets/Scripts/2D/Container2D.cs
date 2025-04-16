using UnityEngine;

public class Container2D : BaseContainer
{
    protected override void UpdateBounds()
    {
        containerBounds = new Bounds(containerOffset, new Vector2(containerLength, containerWidth));
    }

    public override bool Contains(Vector3 point)
    {
        Vector2 localPos = new Vector2(point.x, point.y) - new Vector2(containerOffset.x, containerOffset.y);
        return Mathf.Abs(localPos.x) <= containerLength * 0.5f &&
               Mathf.Abs(localPos.y) <= containerWidth * 0.5f;
    }

    public override Vector3 GetClosestPoint(Vector3 point)
    {
        Vector2 localPos = new Vector2(point.x, point.y) - new Vector2(containerOffset.x, containerOffset.y);
        Vector2 clampedPos = new Vector2(
            Mathf.Clamp(localPos.x, -containerLength * 0.5f, containerLength * 0.5f),
            Mathf.Clamp(localPos.y, -containerWidth * 0.5f, containerWidth * 0.5f)
        );
        
        Vector2 result = clampedPos + new Vector2(containerOffset.x, containerOffset.y);
        return new Vector3(result.x, result.y, 0);
    }

    public override Vector3 GetNormal(Vector3 point)
    {
        Vector2 localPos = new Vector2(point.x, point.y) - new Vector2(containerOffset.x, containerOffset.y);
        float dx = Mathf.Abs(localPos.x) - containerLength * 0.5f;
        float dy = Mathf.Abs(localPos.y) - containerWidth * 0.5f;
        
        Vector2 normal;
        if (dx > dy)
        {
            normal = new Vector2(Mathf.Sign(localPos.x), 0);
        }
        else
        {
            normal = new Vector2(0, Mathf.Sign(localPos.y));
        }
        
        return new Vector3(normal.x, normal.y, 0);
    }
} 