using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AreaEffect : MonoBehaviour
{
    public enum EffectType
    {
        Damage,
        Heal
    }

    [Header("Tipo di Area Principale")]
    [SerializeField] private EffectType effectType = EffectType.Damage;
    [SerializeField] private float amountPerTick = 10f;
    [SerializeField] private float tickInterval = 1f;

    [Header("Effetti Movimento Opzionali")]
    [SerializeField] private bool enableSlide = false;
    [SerializeField, Range(0.1f, 0.98f)] private float slideInertia = 0.9f;

    [Space(5)]
    [SerializeField] private bool enableSlow = false;
    [SerializeField, Range(0.1f, 0.9f)] private float slowPercentage = 0.5f;

    [Space(5)]
    [SerializeField] private bool enableSpeedBoost = false;
    [SerializeField, Min(0.1f)] private float speedBoostPercentage = 0.5f;

    [SerializeField] private float speedEffectDuration = 5f;

    private int playerLayer;
    private float tickTimer;
    private float speedEffectTimer;
    private float activeSpeedMultiplier;

    private bool isPlayerInside;
    private Health playerHealth;
    private CharacterController playerController;

    private Vector3 lastPlayerPosition;
    private Vector3 slideVelocity;

    private void Awake()
    {
        playerLayer = LayerMask.NameToLayer("Player");

        Collider col = GetComponent<Collider>();
        if (!col.isTrigger)
        {
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != playerLayer) return;

        isPlayerInside = true;
        playerHealth = other.GetComponentInParent<Health>();
        playerController = other.GetComponentInParent<CharacterController>();

        if (playerController != null)
        {
            lastPlayerPosition = playerController.transform.position;
            slideVelocity = Vector3.zero;
        }

        tickTimer = 0f;
        ApplyPeriodicEffect();
        ActivateSpeedEffect();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != playerLayer) return;

        tickTimer += Time.deltaTime;
        if (tickTimer >= tickInterval)
        {
            tickTimer -= tickInterval;
            ApplyPeriodicEffect();
        }

        ActivateSpeedEffect();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != playerLayer) return;

        isPlayerInside = false;
        playerHealth = null;
        tickTimer = 0f;
        slideVelocity = Vector3.zero;
    }

    private void LateUpdate()
    {
        if (playerController == null) return;

        Vector3 currentPosition = playerController.transform.position;
        Vector3 frameDisplacement = currentPosition - lastPlayerPosition;
        frameDisplacement.y = 0f;

        Vector3 extraMovement = Vector3.zero;

        if (speedEffectTimer > 0f)
        {
            speedEffectTimer -= Time.deltaTime;
            if (frameDisplacement.sqrMagnitude > 0.000001f)
            {
                extraMovement += frameDisplacement * activeSpeedMultiplier;
            }
        }

        if (isPlayerInside && enableSlide)
        {
            Vector3 currentHorizontalVel = Time.deltaTime > 0f ? frameDisplacement / Time.deltaTime : Vector3.zero;
            slideVelocity = Vector3.Lerp(currentHorizontalVel, slideVelocity, slideInertia);
            extraMovement += slideVelocity * Time.deltaTime;
        }

        if (extraMovement.sqrMagnitude > 0.000001f)
        {
            playerController.Move(extraMovement);
        }

        lastPlayerPosition = playerController.transform.position;

        if (!isPlayerInside && speedEffectTimer <= 0f)
        {
            playerController = null;
        }
    }

    private void ApplyPeriodicEffect()
    {
        if (playerHealth == null || playerHealth.IsDead) return;

        if (effectType == EffectType.Damage)
        {
            playerHealth.TakeDamage(amountPerTick);
        }
        else if (effectType == EffectType.Heal)
        {
            playerHealth.Heal(amountPerTick);
        }
    }

    private void ActivateSpeedEffect()
    {
        float modifier = 0f;

        if (enableSlow)
        {
            modifier -= slowPercentage;
        }

        if (enableSpeedBoost)
        {
            modifier += speedBoostPercentage;
        }

        if (!Mathf.Approximately(modifier, 0f))
        {
            activeSpeedMultiplier = modifier;
            speedEffectTimer = speedEffectDuration;
        }
    }
}