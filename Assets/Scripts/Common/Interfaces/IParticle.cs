using UnityEngine;

public interface IParticle
{
    float Mass { get; set; }
    float Radius { get; set; }
    bool IsActive { get; set; }
    float Density { get; set; }
    float Pressure { get; set; }

    void Initialize(Vector3 position, Vector3 velocity, float mass, float radius);
    void ResetForces();
    void AddForce(Vector3 force);
    void Integrate(float deltaTime);
    void Deactivate();
} 