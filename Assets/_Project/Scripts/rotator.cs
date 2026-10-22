using UnityEngine;

public class Rotator : MonoBehaviour
{
    [Header("Rotazione")]
    [Tooltip("Gradi al secondo sull'asse Y. Valori negativi invertono il senso.")]
    [SerializeField] private float rotationSpeed = 45f;

    [Tooltip("Ruota in spazio locale (consigliato se Core è figlio di altri oggetti).")]
    [SerializeField] private Space rotationSpace = Space.Self;

    private void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, rotationSpace);
    }
}