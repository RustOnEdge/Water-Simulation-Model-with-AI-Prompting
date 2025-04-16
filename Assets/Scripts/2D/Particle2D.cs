using UnityEngine;

public class Particle2D : IParticle
{
    // Properties required by IParticle
    public float Mass { get; set; }
    public float Radius { get; set; }
    public bool IsActive { get; set; }
    public float Density { get; set; }
    public float Pressure { get; set; }

    // 2D specific properties
    public Vector2 position;
    public Vector2 velocity;
    public Vector2 acceleration;
    public Vector2 force;
    public Color particleColor = new Color(0.2f, 0.5f, 1.0f, 0.8f);

    public void Initialize(Vector3 position, Vector3 velocity, float mass, float radius)
    {
        this.position = new Vector2(position.x, position.y);
        this.velocity = new Vector2(velocity.x, velocity.y);
        this.acceleration = Vector2.zero;
        this.force = Vector2.zero;

        Mass = mass;
        Radius = radius;
        Density = 0f;
        Pressure = 0f;
        IsActive = true;
    }

    public void ResetForces()
    {
        force = Vector2.zero;
    }

    public void AddForce(Vector3 force)
    {
        // Convert Vector3 to Vector2 for 2D simulation
        if (!float.IsNaN(force.x) && !float.IsNaN(force.y))
        {
            this.force += new Vector2(force.x, force.y);
        }
    }

    public void Integrate(float deltaTime)
    {
        if (!IsActive) return;

        acceleration = force / Mass;

        if (float.IsNaN(acceleration.x) || float.IsNaN(acceleration.y))
        {
            acceleration = Vector2.zero;
        }

        velocity += acceleration * deltaTime;
        position += velocity * deltaTime;
    }

    public void Deactivate()
    {
        IsActive = false;
        position = Vector2.zero;
        velocity = Vector2.zero;
        acceleration = Vector2.zero;
        force = Vector2.zero;
        Density = 0f;
        Pressure = 0f;
    }

    // Helper method for debug visualization
    public void Draw()
    {
        if (!IsActive) return;
        Gizmos.color = particleColor;
        Gizmos.DrawSphere(new Vector3(position.x, position.y, 0), Radius);
    }
} 