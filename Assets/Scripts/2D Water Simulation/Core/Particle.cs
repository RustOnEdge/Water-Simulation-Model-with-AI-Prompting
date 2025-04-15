using UnityEngine;

public class Particle
{
    // basic properties
    public float mass = 1.0f;
    public float radius = 0.5f;
    public Color particleColour = new Color(0.2f, 0.5f, 1.0f, 0.8f);

    // position and movement
    public Vector2 position;
    public Vector2 velocity;
    public Vector2 acceleration;
    public Vector2 force;

    // SPH properties
    public float density;
    public float pressure;
    public float restDensity = 1000f;
    public float viscosity = 0.1f;
    public float gasConstant = 2000f;

    public float damping = 0.98f;

    // status
    public bool isActive = true;

    // initialise particle
    public void Initialise(Vector2 startPosition, Vector2 startVelocity, float particleMass,
                          float particleRadius)
    {
        position = startPosition;
        velocity = startVelocity;
        acceleration = Vector2.zero;
        force = Vector2.zero;

        mass = particleMass;
        radius = particleRadius;

        // Initialize SPH properties
        density = 0f;
        pressure = 0f;

        isActive = true;
    }

    // reset forces at the beginning of each simulation step
    public void ResetForces()
    {
        force = Vector2.zero;
    }

    // add a force to this particle
    public void AddForce(Vector2 additionalForce)
    {
        // avoid nan forces
        if (float.IsNaN(additionalForce.x) || float.IsNaN(additionalForce.y))
        {
            return;
        }

        force += additionalForce;
    }

    // update particle position and velocity
    public void Integrate(float deltaTime)
    {
        if (!isActive) return;

        // calculate acceleration from force and mass (f = ma, so a = f/m)
        acceleration = force / mass;

        // avoid nan acceleration
        if (float.IsNaN(acceleration.x) || float.IsNaN(acceleration.y))
        {
            acceleration = Vector2.zero;
        }

        // update velocity using semi-implicit euler integration (more stable)
        velocity += acceleration * deltaTime;
        velocity *= damping;

        // update position
        position += velocity * deltaTime;
    }

    // deactivate particle
    public void Deactivate()
    {
        isActive = false;
        position = Vector2.zero;
        velocity = Vector2.zero;
        acceleration = Vector2.zero;
        force = Vector2.zero;
        density = 0f;
        pressure = 0f;
    }

    // draw particle using gizmos
    public void Draw()
    {
        if (!isActive) return;
        Gizmos.color = particleColour;
        Gizmos.DrawSphere(position, radius);
    }
} 