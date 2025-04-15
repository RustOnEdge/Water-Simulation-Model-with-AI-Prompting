using UnityEngine;
using System.Collections.Generic;

public class SPHViscosity
{
    private float kernelRadius;
    private float viscosity;
    private int[] neighborCache;
    private const int MAX_NEIGHBORS = 64;

    public SPHViscosity(float kernelRadius, float viscosity)
    {
        this.kernelRadius = kernelRadius;
        this.viscosity = viscosity;
        this.neighborCache = new int[MAX_NEIGHBORS];
    }

    public void CalculateViscosityForces(Particle[] particles, int activeParticles, NeighbourSearch neighbourSearch)
    {
        for (int i = 0; i < activeParticles; i++)
        {
            if (!particles[i].isActive) continue;

            int neighborCount = neighbourSearch.GetNeighbours(i, neighborCache);
            Vector2 viscosityForce = CalculateViscosityForce(particles[i], particles, neighborCache, neighborCount);
            particles[i].AddForce(viscosityForce);
        }
    }

    private Vector2 CalculateViscosityForce(Particle particle, Particle[] particles, int[] neighbors, int neighborCount)
    {
        Vector2 force = Vector2.zero;
        for (int i = 0; i < neighborCount; i++)
        {
            int neighborIndex = neighbors[i];
            Vector2 r = particle.position - particles[neighborIndex].position;
            float r2 = r.sqrMagnitude;
            if (r2 < kernelRadius * kernelRadius)
            {
                Vector2 velocityDiff = particles[neighborIndex].velocity - particle.velocity;
                force += viscosity * particles[neighborIndex].mass * 
                         velocityDiff * SPHKernels.ViscosityLaplacian(r2, kernelRadius);
            }
        }
        return force;
    }
} 