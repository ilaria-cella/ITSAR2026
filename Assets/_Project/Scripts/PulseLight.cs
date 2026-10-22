
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class PulseLight : MonoBehaviour
{
    private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

    [Header("Emissione")]
    [Tooltip("Colore base dell'emissione (la luminosità viene moltiplicata per l'intensità).")]
    [SerializeField, ColorUsage(false, true)] private Color emissionColor = Color.cyan;

    [Header("Pulsazione")]
    [SerializeField, Min(0f)] private float minIntensity = 0.5f;
    [SerializeField, Min(0f)] private float maxIntensity = 3f;
    [Tooltip("Pulsazioni complete al secondo (Hz).")]
    [SerializeField, Min(0f)] private float pulseSpeed = 0.5f;
    [Tooltip("Sfasa la pulsazione, utile se hai più cristalli.")]
    [SerializeField] private float phaseOffset = 0f;
    [SerializeField] private bool useUnscaledTime = false;

    private Renderer rend;
    private MaterialPropertyBlock block;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        block = new MaterialPropertyBlock();
        rend.sharedMaterial.EnableKeyword("_EMISSION");
    }

    private void Update()
    {
        float t = useUnscaledTime ? Time.unscaledTime : Time.time;

        // Sine normalizzato 0..1
        float wave = (Mathf.Sin((t * pulseSpeed + phaseOffset) * Mathf.PI * 2f) + 1f) * 0.5f;
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, wave);

        rend.GetPropertyBlock(block);
        block.SetColor(EmissionColorId, emissionColor * intensity);
        rend.SetPropertyBlock(block);
    }
}