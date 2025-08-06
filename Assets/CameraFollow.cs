using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Configuración de Seguimiento")]
    public Transform target; // El personaje a seguir
    public float smoothSpeed = 0.125f; // Velocidad de suavizado
    public Vector3 offset = new Vector3(0, 0, -10); // Offset de la cámara
    
    void LateUpdate()
    {
        if (target == null)
            return;
            
        // Posición deseada
        Vector3 desiredPosition = target.position + offset;
        
        // Interpolación suave hacia la posición deseada
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
        // Aplicar la nueva posición
        transform.position = smoothedPosition;
    }
}