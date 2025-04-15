using UnityEngine;

public static class SPHKernels
{
    // Poly6 kernel for density calculation
    public static float Poly6(float r2, float h)
    {
        float h2 = h * h;
        if (r2 >= h2) return 0f;
        
        float h4 = h2 * h2;
        float h9 = h4 * h4 * h;
        float q = 1f - r2 / h2;
        return 4f / (Mathf.PI * h9) * q * q * q;
    }

    // Spiky kernel gradient for pressure forces
    public static Vector2 SpikyGradient(Vector2 r, float h)
    {
        float rLength = r.magnitude;
        if (rLength >= h) return Vector2.zero;
        
        float h5 = h * h * h * h * h;
        float q = 1f - rLength / h;
        return -30f / (Mathf.PI * h5) * q * q * r.normalized;
    }

    // Viscosity kernel laplacian
    public static float ViscosityLaplacian(float r2, float h)
    {
        float h2 = h * h;
        if (r2 >= h2) return 0f;
        
        float h5 = h2 * h2 * h;
        float q = Mathf.Sqrt(r2) / h;
        return 40f / (Mathf.PI * h5) * (1f - q);
    }

    // Surface tension kernel
    public static float SurfaceTension(float r2, float h)
    {
        float h2 = h * h;
        if (r2 >= h2) return 0f;
        
        float h5 = h2 * h2 * h;
        float q = Mathf.Sqrt(r2) / h;
        return 32f / (Mathf.PI * h5) * (1f - q) * (1f - q);
    }
} 