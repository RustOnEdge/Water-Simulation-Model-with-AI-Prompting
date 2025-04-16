using UnityEngine;
using System.Collections.Generic;

public class Simulation : MonoBehaviour
{
    [SerializeField] private bool clearParticles = false;
    
    [Header("simulation settings")]
    [SerializeField] private int maxParticles = 450;
    [SerializeField] private float spawnRate = 450f;
    [SerializeField] private float spawnRadius = 0.3f;
    private Vector2 initialVelocityRange = new Vector2(-1f, 1f);

    [Header("particle properties")]
    [SerializeField] private float particleRadius = 0.15f;
    [SerializeField] private float particleMass = 1f;
    [SerializeField] private float damping = 0.99f;
    [SerializeField] private float bounceCoefficient = 0.3f;
    [SerializeField] private float positionSmoothing = 0.1f;

    [Header("SPH parameters")]
    [SerializeField] private float kernelRadius = 0.3f;
    [SerializeField] private float restDensity = 1000f;
    [SerializeField] private float gasConstant = 2000f;
    [SerializeField] private float viscosity = 0.01f;

    private Vector2 gravity = new Vector2(0, -9.81f);

    [Header("References")]
    [SerializeField] private Container container;
    private DebugManager debugManager;

    // particle system
    private Particle[] particles;
    private int activeParticles = 0;
    private float spawnAccumulator = 0f;

    private Bounds defaultBounds;
    private NeighbourSearch neighbourSearch;
    private CollisionDetection collisionDetection;
    private SPHDensity sphDensity;
    private SPHPressure sphPressure;
    private SPHViscosity sphViscosity;

    void Start()
    {
        if (container == null)
        {
            Debug.LogWarning("Container reference is missing! Using default bounds.");
            defaultBounds = new Bounds(Vector2.zero, new Vector2(10f, 10f));
            
            container = FindObjectOfType<Container>();
            if (container == null)
            {
                Debug.LogWarning("No Container found in scene. Please add a Container component to your scene.");
            }
        }

        // create particle array
        particles = new Particle[maxParticles];
        for (int i = 0; i < maxParticles; i++)
        {
            particles[i] = new Particle();
            particles[i].isActive = false;
        }

        // initialize neighbour search
        neighbourSearch = new NeighbourSearch(
            kernelRadius,
            maxParticles,
            container != null ? container.Size : defaultBounds.size,
            container != null ? container.Offset : defaultBounds.center,
            particleRadius
        );

        // initialize collision detection
        collisionDetection = new CollisionDetection(
            container,
            particleRadius,
            bounceCoefficient,
            positionSmoothing
        );

        // initialize SPH components
        sphDensity = new SPHDensity(kernelRadius, restDensity);
        sphPressure = new SPHPressure(kernelRadius, gasConstant, restDensity);
        sphViscosity = new SPHViscosity(kernelRadius, viscosity);

        debugManager = GetComponent<DebugManager>();
    }

    void FixedUpdate()
    {
        // spawn new particles
        SpawnParticles();

        // update spatial partitioning
        neighbourSearch.UpdateGrid(particles, activeParticles);
        
        // calculate SPH forces
        sphDensity.CalculateDensities(particles, activeParticles, neighbourSearch);
        sphPressure.CalculatePressureForces(particles, activeParticles, neighbourSearch);
        sphViscosity.CalculateViscosityForces(particles, activeParticles, neighbourSearch);

        // handle particle collisions
        neighbourSearch.HandleCollisions(particles, activeParticles);

        // apply gravity and integrate
        UpdateParticles();

        if (clearParticles)
        {
            ClearParticles();
        }
    }

    private void SpawnParticles()
    {
        if (activeParticles >= maxParticles) return;

        spawnAccumulator += spawnRate * Time.fixedDeltaTime;
        int particlesToSpawn = Mathf.FloorToInt(spawnAccumulator);
        spawnAccumulator -= particlesToSpawn;

        for (int i = 0; i < particlesToSpawn; i++)
        {
            if (activeParticles >= maxParticles) break;

            // find an inactive particle slot
            int particleIndex = -1;
            for (int j = 0; j < maxParticles; j++)
            {
                if (!particles[j].isActive)
                {
                    particleIndex = j;
                    break;
                }
            }

            if (particleIndex < 0) break;

            Vector2 spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
            Vector2 initialVelocity = new Vector2(
                Random.Range(initialVelocityRange.x, initialVelocityRange.y),
                Random.Range(initialVelocityRange.x, initialVelocityRange.y)
            );

            particles[particleIndex].Initialise(
                spawnPosition,
                initialVelocity,
                particleMass,
                particleRadius
            );

            if (particleIndex >= activeParticles)
            {
                activeParticles = particleIndex + 1;
            }
        }
    }

    private void UpdateParticles()
    {
        for (int i = 0; i < activeParticles; i++)
        {
            if (particles[i].isActive)
            {
                particles[i].ResetForces();
                particles[i].AddForce(gravity * particles[i].mass);
                particles[i].Integrate(Time.fixedDeltaTime);

                if (container != null)
                {
                    collisionDetection.HandleBoundaryCollision(particles[i]);
                }
                else
                {
                    collisionDetection.HandleDefaultBounds(particles[i], defaultBounds);
                }

                if (!collisionDetection.ValidatePosition(particles[i]))
                {
                    particles[i].Deactivate();
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (particles != null && debugManager != null)
        {
            // Draw particles
            if (debugManager.showParticleVelocities || debugManager.showDensityColors || debugManager.showPressureForces)
            {
                debugManager.DrawParticleDebug();
            }

            // Draw container
            if (debugManager.showContainerBounds)
            {
                debugManager.DrawContainerDebug();
            }
        }
    }

    public void ClearParticles()
    {
        for (int i = 0; i < activeParticles; i++)
        {
            particles[i].Deactivate();
        }
        activeParticles = 0;
    }

    // Add getter methods for debug manager
    public float GetRestDensity() { return restDensity; }
    public float GetGasConstant() { return gasConstant; }
    public int GetActiveParticleCount() { return activeParticles; }
    public Particle[] GetParticles() { return particles; }
} 