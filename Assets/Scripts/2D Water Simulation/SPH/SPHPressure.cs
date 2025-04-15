using UnityEngine;
using System.Collections.Generic;

public class SPHPressure
{
    private float kernelRadius;
    private float gasConstant;
    private float restDensity;
    private int[] neighborCache;
    private const int MAX_NEIGHBORS = 64;

    public SPHPressure(float kernelRadius, float gasConstant, float restDensity)
    {
        this.kernelRadius = kernelRadius;
        this.gasConstant = gasConstant;
        this.restDensity = restDensity;
        this.neighborCache = new int[MAX_NEIGHBORS];
    }

    public void CalculatePressureForces(Particle[] particles, int activeParticles, NeighbourSearch neighbourSearch)
    {
        for (int i = 0; i < activeParticles; i++)
        {
            if (!particles[i].isActive) continue;

            int neighborCount = neighbourSearch.GetNeighbours(i, neighborCache);
            Vector2 pressureForce = CalculatePressureForce(particles[i], particles, neighborCache, neighborCount);
            particles[i].AddForce(pressureForce);
        }
    }

    private Vector2 CalculatePressureForce(Particle particle, Particle[] particles, int[] neighbors, int neighborCount)
    {
        Vector2 force = Vector2.zero;
        float pressure = CalculatePressure(particle.density);

        for (int i = 0; i < neighborCount; i++)
        {
            int neighborIndex = neighbors[i];
            Vector2 r = particle.position - particles[neighborIndex].position;
            float r2 = r.sqrMagnitude;
            if (r2 < kernelRadius * kernelRadius)
            {
                float neighborPressure = CalculatePressure(particles[neighborIndex].density);
                force += -r.normalized * (pressure + neighborPressure) * 
                         particles[neighborIndex].mass * SPHKernels.SpikyGradient(r, kernelRadius).magnitude;
            }
        }
        return force;
    }

    private float CalculatePressure(float density)
    {
        return gasConstant * (density - restDensity);
    }
} 