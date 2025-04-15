using UnityEngine;

public class CollisionDetection
{
    private Container container;
    private float particleRadius;
    private float bounceCoefficient;
    private float positionSmoothing;
    private Bounds defaultBounds;

    public CollisionDetection(Container container, float particleRadius, float bounceCoefficient, float positionSmoothing)
    {
        this.container = container;
        this.particleRadius = particleRadius;
        this.bounceCoefficient = bounceCoefficient;
        this.positionSmoothing = positionSmoothing;
        this.defaultBounds = new Bounds(Vector2.zero, new Vector2(10f, 10f));
    }

    public void HandleBoundaryCollision(Particle particle)
    {
        if (container == null) return;

        // convert particle position to container's local space
        Vector2 localPos = particle.position - container.Offset;
        Vector2 rotatedPos = RotatePoint(localPos, -container.Rotation);

        // get container half sizes
        float halfWidth = container.Size.x * 0.5f;
        float halfHeight = container.Size.y * 0.5f;

        // check for collision with each wall
        bool collision = false;
        Vector2 normal = Vector2.zero;

        // left wall
        if (rotatedPos.x < -halfWidth + particleRadius)
        {
            rotatedPos.x = -halfWidth + particleRadius;
            normal = Vector2.right;
            collision = true;
        }
        // right wall
        else if (rotatedPos.x > halfWidth - particleRadius)
        {
            rotatedPos.x = halfWidth - particleRadius;
            normal = Vector2.left;
            collision = true;
        }

        // bottom wall
        if (rotatedPos.y < -halfHeight + particleRadius)
        {
            rotatedPos.y = -halfHeight + particleRadius;
            normal = Vector2.up;
            collision = true;
        }
        // top wall
        else if (rotatedPos.y > halfHeight - particleRadius)
        {
            rotatedPos.y = halfHeight - particleRadius;
            normal = Vector2.down;
            collision = true;
        }

        if (collision)
        {
            // rotate normal back to world space
            normal = RotatePoint(normal, container.Rotation).normalized;

            // update particle position
            Vector2 worldPos = RotatePoint(rotatedPos, container.Rotation) + container.Offset;
            particle.position = worldPos;

            // reflect velocity
            float velocityAlongNormal = Vector2.Dot(particle.velocity, normal);
            if (velocityAlongNormal < 0)
            {
                Vector2 velocityChange = (1 + bounceCoefficient) * velocityAlongNormal * normal;
                particle.velocity -= velocityChange * positionSmoothing;
            }

            // apply small position correction to prevent sticking
            particle.position += normal * 0.001f;
        }
    }

    public void HandleDefaultBounds(Particle particle, Bounds bounds)
    {
        Vector2 newPos = particle.position;
        Vector2 boundsMin = bounds.min + Vector3.one * particleRadius;
        Vector2 boundsMax = bounds.max - Vector3.one * particleRadius;

        bool collision = false;
        Vector2 normal = Vector2.zero;

        // check x-axis collision
        if (newPos.x < boundsMin.x)
        {
            newPos.x = boundsMin.x;
            normal = Vector2.right;
            collision = true;
        }
        else if (newPos.x > boundsMax.x)
        {
            newPos.x = boundsMax.x;
            normal = Vector2.left;
            collision = true;
        }

        // check y-axis collision
        if (newPos.y < boundsMin.y)
        {
            newPos.y = boundsMin.y;
            normal = Vector2.up;
            collision = true;
        }
        else if (newPos.y > boundsMax.y)
        {
            newPos.y = boundsMax.y;
            normal = Vector2.down;
            collision = true;
        }

        if (collision)
        {
            particle.position = newPos;

            // reflect velocity
            float velocityAlongNormal = Vector2.Dot(particle.velocity, normal);
            if (velocityAlongNormal < 0)
            {
                Vector2 velocityChange = (1 + bounceCoefficient) * velocityAlongNormal * normal;
                particle.velocity -= velocityChange * positionSmoothing;
            }

            // apply small position correction to prevent sticking
            particle.position += normal * 0.001f;
        }
    }

    public bool ValidatePosition(Particle particle)
    {
        return !float.IsNaN(particle.position.x) && 
               !float.IsNaN(particle.position.y) &&
               Mathf.Abs(particle.position.x) <= 1000f && 
               Mathf.Abs(particle.position.y) <= 1000f;
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