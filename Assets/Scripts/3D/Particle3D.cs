using UnityEngine;

public class Particle3D : IParticle
{
    // TODO: Implement 3D particle behavior
    public float Mass { get; set; }
    public float Radius { get; set; }
    public bool IsActive { get; set; }
    public float Density { get; set; }
    public float Pressure { get; set; }

    public void Initialize(Vector3 position, Vector3 velocity, float mass, float radius)
    {
        throw new System.NotImplementedException();
    }

    public void ResetForces()
    {
        throw new System.NotImplementedException();
    }

    public void AddForce(Vector3 force)
    {
        throw new System.NotImplementedException();
    }

    public void Integrate(float deltaTime)
    {
        throw new System.NotImplementedException();
    }

    public void Deactivate()
    {
        throw new System.NotImplementedException();
    }
} 