using UnityEngine;

public class NeighbourSearch3D : BaseNeighbourSearch
{
    public NeighbourSearch3D(float kernelRadius, int maxParticles, float particleRadius) 
        : base(kernelRadius, maxParticles, particleRadius)
    {
        // TODO: Initialize 3D grid
    }

    public override void UpdateGrid(IParticle[] particles, int activeParticles)
    {
        throw new System.NotImplementedException();
    }

    public override int GetNeighbours(int particleIndex, int[] neighbors)
    {
        throw new System.NotImplementedException();
    }

    public override void HandleCollisions(IParticle[] particles, int activeParticles)
    {
        throw new System.NotImplementedException();
    }
} 