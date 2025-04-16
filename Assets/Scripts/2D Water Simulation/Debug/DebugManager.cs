using UnityEngine;
using Unity.Collections;
using System.Runtime.CompilerServices;

public class DebugManager : MonoBehaviour
{
    [Header("Visualization Options")]
    public bool showParticleVelocities = false;
    public bool showDensityColors = true;
    public bool showPressureForces = false;
    public bool showContainerBounds = true;

    [Header("Performance")]
    public bool showFPS = true;
    public bool showParticleCount = true;
    [Range(0.016f, 1f)]
    public float updateInterval = 0.06f;

    [Header("Debug Colors")]
    [SerializeField] private Color32 velocityColor = new Color32(255, 255, 0, 255);
    [SerializeField] private Color32 densityMinColor = new Color32(102, 178, 255, 255);
    [SerializeField] private Color32 densityMaxColor = new Color32(0, 51, 204, 255);
    [SerializeField] private Color32 pressureColor = new Color32(255, 255, 255, 255);
    [SerializeField] private Color32 containerColor = new Color32(255, 255, 255, 255);

    [Header("Debug Parameters")]
    [Range(0.1f, 2f)]
    public float velocityArrowScale = 0.5f;
    [Range(0.1f, 1f)]
    public float densityOpacity = 0.4f;
    [Range(0.1f, 1f)]
    public float pressureOpacity = 0.15f;

    [Header("Simulation Control")]
    public float minSimulationSpeed = 0.1f;
    public float maxSimulationSpeed = 4.0f;
    [Range(0.1f, 4.0f)]
    public float simulationSpeed = 2.0f;

    // Cached components and values
    private Simulation simulation;
    private Container container;
    private Transform cachedTransform;
    private float fps;
    private float timeSinceLastUpdate;
    private bool isFrozen;
    private bool singleStep;

    // Cached arrays for visualization
    private NativeArray<Vector3> lineVertices;
    private NativeArray<Color32> lineColors;
    private int currentLineCount;
    private const int MAX_LINES = 10000;

    private void Awake()
    {
        cachedTransform = transform;
        simulation = GetComponent<Simulation>();
        container = GetComponent<Container>();

        // Initialize line arrays
        lineVertices = new NativeArray<Vector3>(MAX_LINES * 2, Allocator.Persistent);
        lineColors = new NativeArray<Color32>(MAX_LINES * 2, Allocator.Persistent);
    }

    private void OnDestroy()
    {
        if (lineVertices.IsCreated) lineVertices.Dispose();
        if (lineColors.IsCreated) lineColors.Dispose();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Update()
    {
        if (showFPS && Time.unscaledTime > timeSinceLastUpdate + updateInterval)
        {
            fps = 1.0f / Time.unscaledDeltaTime;
            timeSinceLastUpdate = Time.unscaledTime;
        }

        // Handle simulation control
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isFrozen = !isFrozen;
            singleStep = false;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && isFrozen)
        {
            singleStep = true;
        }

        Time.timeScale = simulationSpeed;
    }

    private void OnGUI()
    {
        if (!showFPS && !showParticleCount) return;

        var style = new GUIStyle(GUI.skin.label)
        {
            fontSize = 14,
            fontStyle = FontStyle.Bold,
            normal = { textColor = Color.white }
        };

        float y = 10f;
        if (showFPS)
        {
            GUI.Label(new Rect(10, y, 100, 20), $"FPS: {fps:F1}", style);
            y += 20;
        }

        if (showParticleCount && simulation != null)
        {
            GUI.Label(new Rect(10, y, 150, 20), $"Particles: {simulation.GetActiveParticleCount()}", style);
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || simulation == null) return;

        currentLineCount = 0;

        if (showParticleVelocities || showDensityColors || showPressureForces)
        {
            DrawParticleDebug();
        }

        if (showContainerBounds && container != null)
        {
            DrawContainerDebug();
        }

        // Draw all accumulated lines
        if (currentLineCount > 0)
        {
            for (int i = 0; i < currentLineCount; i++)
            {
                Gizmos.color = lineColors[i * 2];
                Gizmos.DrawLine(lineVertices[i * 2], lineVertices[i * 2 + 1]);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void DrawParticleDebug()
    {
        if (simulation == null) return;
        
        var particles = simulation.GetParticles();
        if (particles == null) return;

        int activeCount = simulation.GetActiveParticleCount();
        float restDensity = simulation.GetRestDensity();
        float gasConstant = simulation.GetGasConstant();

        for (int i = 0; i < activeCount; i++)
        {
            if (!particles[i].isActive) continue;

            Vector3 position = particles[i].position;
            
            if (showDensityColors && particles[i].density > 0)
            {
                float densityRatio = Mathf.Clamp01((particles[i].density / restDensity - 0.7f) * 3f);
                Color32 color = Color32.Lerp(densityMinColor, densityMaxColor, densityRatio);
                color.a = (byte)(densityOpacity * 255);
                Gizmos.color = color;
                Gizmos.DrawSphere(position, particles[i].radius * 1.2f);
            }

            if (showParticleVelocities && currentLineCount < MAX_LINES)
            {
                Vector3 velocityEnd = position + (Vector3)(particles[i].velocity.normalized * velocityArrowScale);
                AddLine(position, velocityEnd, velocityColor);
            }

            if (showPressureForces && particles[i].pressure > 0 && currentLineCount < MAX_LINES)
            {
                float pressureRatio = particles[i].pressure / (gasConstant * restDensity);
                Color32 color = pressureColor;
                color.a = (byte)(pressureOpacity * Mathf.Clamp01(pressureRatio) * 255);
                float radius = particles[i].radius * 1.5f;
                DrawWireSphere(position, radius, color);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void DrawContainerDebug()
    {
        Vector3 center = cachedTransform.position + (Vector3)(Vector2)container.Offset;
        Vector2 size = container.Size;

        // Draw container bounds
        Vector3 halfSize = new Vector3(size.x * 0.5f, size.y * 0.5f, 0);
        Vector3[] corners = new Vector3[]
        {
            center + new Vector3(-halfSize.x, -halfSize.y),
            center + new Vector3(halfSize.x, -halfSize.y),
            center + new Vector3(halfSize.x, halfSize.y),
            center + new Vector3(-halfSize.x, halfSize.y)
        };

        for (int i = 0; i < 4 && currentLineCount < MAX_LINES; i++)
        {
            AddLine(corners[i], corners[(i + 1) % 4], containerColor);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void DrawWireSphere(Vector3 center, float radius, Color32 color)
    {
        const int segments = 12;
        float angleStep = 360f / segments;
        
        for (int i = 0; i < segments && currentLineCount < MAX_LINES; i++)
        {
            float angle1 = i * angleStep * Mathf.Deg2Rad;
            float angle2 = ((i + 1) % segments) * angleStep * Mathf.Deg2Rad;

            Vector3 point1 = center + new Vector3(Mathf.Cos(angle1) * radius, Mathf.Sin(angle1) * radius);
            Vector3 point2 = center + new Vector3(Mathf.Cos(angle2) * radius, Mathf.Sin(angle2) * radius);

            AddLine(point1, point2, color);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void AddLine(Vector3 start, Vector3 end, Color32 color)
    {
        if (currentLineCount >= MAX_LINES) return;

        int index = currentLineCount * 2;
        lineVertices[index] = start;
        lineVertices[index + 1] = end;
        lineColors[index] = color;
        lineColors[index + 1] = color;
        currentLineCount++;
    }

    public bool ShouldSimulate()
    {
        if (!isFrozen) return true;
        if (singleStep)
        {
            singleStep = false;
            return true;
        }
        return false;
    }
} 