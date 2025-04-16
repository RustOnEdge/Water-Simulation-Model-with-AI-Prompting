using UnityEngine;

public abstract class BaseSimulation : MonoBehaviour
{
    [Header("Base Simulation Settings")]
    [SerializeField] protected bool clearParticles = false;
    [SerializeField] protected int maxParticles = 450;
    [SerializeField] protected float spawnRate = 450f;
    [SerializeField] protected float spawnRadius = 0.3f;

    [Header("Particle Properties")]
    [SerializeField] protected float particleRadius = 0.15f;
    [SerializeField] protected float particleMass = 1f;
    [SerializeField] protected float damping = 0.99f;
    [SerializeField] protected float bounceCoefficient = 0.3f;
    [SerializeField] protected float positionSmoothing = 0.1f;

    [Header("SPH Parameters")]
    [SerializeField] protected float kernelRadius = 0.3f;
    [SerializeField] protected float restDensity = 1000f;
    [SerializeField] protected float gasConstant = 2000f;
    [SerializeField] protected float viscosity = 0.01f;

    // Common properties
    protected int activeParticles = 0;
    protected float spawnAccumulator = 0f;

    // Abstract methods that must be implemented by 2D/3D versions
    protected abstract void InitializeParticles();
    protected abstract void UpdateParticles();
    protected abstract void SpawnParticles();
    protected abstract void HandleCollisions();
    protected abstract void CalculateForces();

    protected virtual void Start()
    {
        InitializeParticles();
    }

    protected virtual void FixedUpdate()
    {
        if (!enabled) return;

        SpawnParticles();
        CalculateForces();
        HandleCollisions();
        UpdateParticles();

        if (clearParticles)
        {
            ClearParticles();
        }
    }

    public virtual void ClearParticles()
    {
        activeParticles = 0;
        clearParticles = false;
    }

    // Common getters for debug/visualization
    public int GetActiveParticleCount() => activeParticles;
    public float GetRestDensity() => restDensity;
    public float GetGasConstant() => gasConstant;
} 