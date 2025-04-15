using UnityEngine;
using Unity.Collections;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

public class NeighbourSearch
{
    // Core parameters
    private readonly float kernelRadius;
    private readonly float kernelRadiusSqr;
    private readonly float cellSize;
    private readonly Vector2Int gridDimensions;
    private readonly Vector2 gridOffset;
    private readonly float particleRadius;
    private readonly int maxParticles;

    // Spatial partitioning
    private NativeArray<int> gridCells;
    private NativeArray<int> gridCounters;
    private NativeArray<Vector2> positions;
    private NativeArray<int> cellStartIndices;
    private NativeArray<int> particleIndices;
    
    // Neighbor cache
    private readonly int[] neighborCache;
    
    // Constants
    private const float COLLISION_DAMPING = 0.5f;
    private const float MIN_DISTANCE = 0.001f;
    private const float POSITION_SMOOTHING = 0.2f;
    private const int MAX_PARTICLES_PER_CELL = 16;
    private const int MAX_NEIGHBORS = 64; // Conservative maximum number of neighbors

    public NeighbourSearch(float kernelRadius, int maxParticles, Vector2 containerSize, Vector2 containerOffset, float particleRadius)
    {
        this.kernelRadius = kernelRadius;
        this.kernelRadiusSqr = kernelRadius * kernelRadius;
        this.maxParticles = maxParticles;
        this.particleRadius = particleRadius;

        // Initialize grid
        cellSize = kernelRadius;
        gridDimensions = new Vector2Int(
            Mathf.Max(1, (int)(containerSize.x / cellSize + 0.5f)),
            Mathf.Max(1, (int)(containerSize.y / cellSize + 0.5f))
        );
        gridOffset = containerOffset - containerSize * 0.5f;

        int totalCells = gridDimensions.x * gridDimensions.y;

        // Initialize native arrays
        gridCells = new NativeArray<int>(totalCells * MAX_PARTICLES_PER_CELL, Allocator.Persistent);
        gridCounters = new NativeArray<int>(totalCells, Allocator.Persistent);
        positions = new NativeArray<Vector2>(maxParticles, Allocator.Persistent);
        cellStartIndices = new NativeArray<int>(totalCells + 1, Allocator.Persistent);
        particleIndices = new NativeArray<int>(maxParticles, Allocator.Persistent);
        
        // Initialize neighbor cache
        neighborCache = new int[MAX_NEIGHBORS];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void UpdateGrid(Particle[] particles, int activeParticles)
    {
        // Reset counters
        for (int i = 0; i < gridCounters.Length; i++) gridCounters[i] = 0;

        // First pass: Count particles per cell
        for (int i = 0; i < activeParticles; i++)
        {
            if (!particles[i].isActive) continue;

            positions[i] = particles[i].position;
            Vector2Int cell = WorldToCell(positions[i]);
            
            if (IsValidCell(cell))
            {
                int cellIndex = CellToIndex(cell);
                int baseIndex = cellIndex * MAX_PARTICLES_PER_CELL;
                int count = gridCounters[cellIndex];
                
                if (count < MAX_PARTICLES_PER_CELL)
                {
                    gridCells[baseIndex + count] = i;
                    gridCounters[cellIndex] = count + 1;
                }
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetNeighbours(int particleIndex, int[] neighbors)
    {
        if (!positions.IsCreated || neighbors == null || neighbors.Length == 0) return 0;

        Vector2Int cell = WorldToCell(positions[particleIndex]);
        int neighborCount = 0;

        // Search neighboring cells
        for (int dx = -1; dx <= 1 && neighborCount < neighbors.Length; dx++)
        {
            for (int dy = -1; dy <= 1 && neighborCount < neighbors.Length; dy++)
            {
                Vector2Int neighborCell = new Vector2Int(cell.x + dx, cell.y + dy);
                if (!IsValidCell(neighborCell)) continue;

                int cellIndex = CellToIndex(neighborCell);
                int baseIndex = cellIndex * MAX_PARTICLES_PER_CELL;
                int cellCount = gridCounters[cellIndex];

                for (int i = 0; i < cellCount && neighborCount < neighbors.Length; i++)
                {
                    int otherIndex = gridCells[baseIndex + i];
                    if (otherIndex == particleIndex) continue;

                    float sqrDist = (positions[particleIndex] - positions[otherIndex]).sqrMagnitude;
                    if (sqrDist <= kernelRadiusSqr)
                    {
                        neighbors[neighborCount++] = otherIndex;
                    }
                }
            }
        }

        return neighborCount;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void HandleCollisions(Particle[] particles, int activeParticles)
    {
        float collisionRadius = particleRadius * 2.1f;
        float collisionRadiusSqr = collisionRadius * collisionRadius;

        for (int i = 0; i < activeParticles; i++)
        {
            if (!particles[i].isActive) continue;

            int neighborCount = GetNeighbours(i, neighborCache);
            for (int j = 0; j < neighborCount; j++)
            {
                int otherIndex = neighborCache[j];
                Vector2 diff = particles[i].position - particles[otherIndex].position;
                float distSqr = diff.sqrMagnitude;

                if (distSqr < collisionRadiusSqr && distSqr > MIN_DISTANCE)
                {
                    ResolveCollision(particles[i], particles[otherIndex], diff, distSqr);
                }
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ResolveCollision(Particle p1, Particle p2, Vector2 diff, float distSqr)
    {
        float dist = Mathf.Sqrt(distSqr);
        Vector2 normal = diff / dist;

        Vector2 relativeVelocity = p1.velocity - p2.velocity;
        float velocityAlongNormal = Vector2.Dot(relativeVelocity, normal);

        if (velocityAlongNormal < 0)
        {
            float restitution = 1f - COLLISION_DAMPING;
            float impulseStrength = -(1f + restitution) * velocityAlongNormal;
            impulseStrength /= 1f / p1.mass + 1f / p2.mass;

            Vector2 impulse = impulseStrength * normal;
            p1.velocity += (impulse / p1.mass) * POSITION_SMOOTHING;
            p2.velocity -= (impulse / p2.mass) * POSITION_SMOOTHING;

            float overlap = particleRadius * 2.1f - dist;
            Vector2 correction = overlap * normal * 0.5f;
            p1.position += correction;
            p2.position -= correction;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Vector2Int WorldToCell(Vector2 worldPosition)
    {
        Vector2 relativePos = worldPosition - gridOffset;
        return new Vector2Int(
            (int)(relativePos.x / cellSize),
            (int)(relativePos.y / cellSize)
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsValidCell(Vector2Int cell)
    {
        return (uint)cell.x < (uint)gridDimensions.x && 
               (uint)cell.y < (uint)gridDimensions.y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int CellToIndex(Vector2Int cell)
    {
        return cell.x + cell.y * gridDimensions.x;
    }

    public void Dispose()
    {
        if (gridCells.IsCreated) gridCells.Dispose();
        if (gridCounters.IsCreated) gridCounters.Dispose();
        if (positions.IsCreated) positions.Dispose();
        if (cellStartIndices.IsCreated) cellStartIndices.Dispose();
        if (particleIndices.IsCreated) particleIndices.Dispose();
    }

    // Public properties for grid visualization
    public Vector2 GridOffset => gridOffset;
    public Vector2Int GridDimensions => gridDimensions;
    public float CellSize => cellSize;
} 