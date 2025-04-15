using UnityEngine;
using System.Collections.Generic;

public class SPHDensity
{
    private float kernelRadius;
    private float restDensity;
    private int[] neighborCache;
    private const int MAX_NEIGHBORS = 64;

    public SPHDensity(float kernelRadius, float restDensity)
    {
        this.kernelRadius = kernelRadius;
        this.restDensity = restDensity;
        this.neighborCache = new int[MAX_NEIGHBORS];
    }

    public void CalculateDensities(Particle[] particles, int activeParticles, NeighbourSearch neighbourSearch)
    {
        for (int i = 0; i < activeParticles; i++)
        {
            if (!particles[i].isActive) continue;

            int neighborCount = neighbourSearch.GetNeighbours(i, neighborCache);
            particles[i].density = CalculateDensity(particles[i], particles, neighborCache, neighborCount);
        }
    }

    private float CalculateDensity(Particle particle, Particle[] particles, int[] neighbors, int neighborCount)
    {
        float density = 0f;
        for (int i = 0; i < neighborCount; i++)
        {
            int neighborIndex = neighbors[i];
            Vector2 r = particle.position - particles[neighborIndex].position;
            float r2 = r.sqrMagnitude;
            if (r2 < kernelRadius * kernelRadius)
            {
                density += particles[neighborIndex].mass * SPHKernels.Poly6(r2, kernelRadius);
            }
        }
        return density;
    }

    public float GetRestDensity()
    {
        return restDensity;
    }
} 