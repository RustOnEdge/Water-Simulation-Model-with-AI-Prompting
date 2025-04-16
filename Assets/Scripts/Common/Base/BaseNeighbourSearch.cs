using UnityEngine;
using Unity.Collections;

public abstract class BaseNeighbourSearch
{
    // Core parameters
    protected readonly float kernelRadius;
    protected readonly float kernelRadiusSqr;
    protected readonly float cellSize;
    protected readonly float particleRadius;
    protected readonly int maxParticles;

    // Constants
    protected const float COLLISION_DAMPING = 0.5f;
    protected const float MIN_DISTANCE = 0.001f;
    protected const float POSITION_SMOOTHING = 0.2f;
    protected const int MAX_PARTICLES_PER_CELL = 16;
    protected const int MAX_NEIGHBORS = 64;

    // Spatial partitioning
    protected NativeArray<int> gridCells;
    protected NativeArray<int> gridCounters;
    protected NativeArray<int> cellStartIndices;
    protected NativeArray<int> particleIndices;

    protected BaseNeighbourSearch(float kernelRadius, int maxParticles, float particleRadius)
    {
        this.kernelRadius = kernelRadius;
        this.kernelRadiusSqr = kernelRadius * kernelRadius;
        this.maxParticles = maxParticles;
        this.particleRadius = particleRadius;
        this.cellSize = kernelRadius;
    }

    public abstract void UpdateGrid(IParticle[] particles, int activeParticles);
    public abstract int GetNeighbours(int particleIndex, int[] neighbors);
    public abstract void HandleCollisions(IParticle[] particles, int activeParticles);

    public virtual void Dispose()
    {
        if (gridCells.IsCreated) gridCells.Dispose();
        if (gridCounters.IsCreated) gridCounters.Dispose();
        if (cellStartIndices.IsCreated) cellStartIndices.Dispose();
        if (particleIndices.IsCreated) particleIndices.Dispose();
    }

    // Helper method to validate positions (shared between 2D/3D)
    protected bool ValidatePosition(Vector3 position)
    {
        return !float.IsNaN(position.x) && 
               !float.IsNaN(position.y) && 
               !float.IsNaN(position.z) &&
               Mathf.Abs(position.x) <= 1000f && 
               Mathf.Abs(position.y) <= 1000f &&
               Mathf.Abs(position.z) <= 1000f;
    }
} 