using UnityEngine;

public class CameraFollowAdvanced : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    [Tooltip("La etiqueta (Tag) del personaje que la cámara debe seguir. Asegúrate de que tu prefab tenga esta etiqueta.")]
    public string targetTag = "Player";
    
    [Header("Configuración de Seguimiento")]
    public float smoothSpeed = 2f;
    public Vector3 offset = new Vector3(0, 0, -10);
    
    [Header("Deadzone (Zona Muerta)")]
    public bool useDeadzone = true;
    public float deadzoneWidth = 2f;
    public float deadzoneHeight = 1f;
    
    [Header("Límites de Cámara")]
    public bool useBounds = false;
    public float minX, maxX, minY, maxY;
    
    [Header("Look Ahead")]
    public bool useLookAhead = true;
    public float lookAheadDistance = 2f;
    public float lookAheadSpeed = 2f;
    
    private Vector3 targetPosition;
    private Vector3 velocity = Vector3.zero;

    // Se ejecuta una vez al inicio, antes del primer frame
    void Start()
    {
        // Si el 'target' no se asignó en el Inspector, lo buscamos por su Tag.
        if (target == null)
        {
            FindPlayerWithTag();
        }
    }

    // Se ejecuta en cada frame para buscar al personaje si aún no se ha encontrado.
    // Esto es útil si el personaje se instancia después de que la cámara se cargue.
    void Update()
    {
        if (target == null)
        {
            FindPlayerWithTag();
        }
    }
    
    void LateUpdate()
    {
        // Si no hay un objetivo, salimos del método para evitar errores.
        if (target == null)
            return;
            
        UpdateTargetPosition();
        MoveCamera();
    }
    
    private void FindPlayerWithTag()
    {
        // Busca en la escena el primer objeto con el Tag especificado.
        GameObject playerObject = GameObject.FindWithTag(targetTag);

        if (playerObject != null)
        {
            target = playerObject.transform;
            Debug.Log("¡Cámara: Personaje encontrado con el Tag '" + targetTag + "' y asignado exitosamente!");
        }
        else
        {
            // Muestra un mensaje de advertencia útil en la consola.
            Debug.LogWarning("¡Advertencia de la cámara! No se encontró un objeto con el Tag '" + targetTag + "' en la escena. Asegúrate de que el personaje tenga esta etiqueta.");
        }
    }

    void UpdateTargetPosition()
    {
        targetPosition = target.position + offset;
        
        // Look ahead basado en la velocidad del personaje
        if (useLookAhead)
        {
            Rigidbody2D targetRb = target.GetComponent<Rigidbody2D>();
            if (targetRb != null)
            {
                // Normalizamos la velocidad para obtener la dirección y aplicamos la distancia.
                Vector3 lookAhead = targetRb.linearVelocity.normalized * lookAheadDistance;
                // Usamos Lerp para un movimiento más suave del look ahead.
                targetPosition += Vector3.Lerp(Vector3.zero, lookAhead, lookAheadSpeed * Time.deltaTime);
            }
        }
        
        // Aplicar deadzone
        if (useDeadzone)
        {
            Vector3 difference = targetPosition - transform.position;
            difference.z = 0; // Ignorar Z para cámaras 2D.
            
            // Si la diferencia está dentro de la zona muerta, la cámara no se mueve.
            if (Mathf.Abs(difference.x) < deadzoneWidth / 2f && 
                Mathf.Abs(difference.y) < deadzoneHeight / 2f)
            {
                targetPosition = transform.position;
                targetPosition.z = offset.z; // Mantenemos la posición Z original.
            }
        }
        
        // Aplicar límites de cámara
        if (useBounds)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
        }
    }
    
    void MoveCamera()
    {
        // Usa SmoothDamp para un movimiento suave con un control preciso de la velocidad.
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, 
                                                ref velocity, 1f / smoothSpeed);
    }
    
    // Método para visualizar la deadzone y los límites en el editor.
    void OnDrawGizmosSelected()
    {
        if (useDeadzone)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, new Vector3(deadzoneWidth, deadzoneHeight, 0));
        }
        
        if (useBounds)
        {
            Gizmos.color = Color.red;
            Vector3 center = new Vector3((minX + maxX) / 2f, (minY + maxY) / 2f, 0);
            Vector3 size = new Vector3(maxX - minX, maxY - minY, 0);
            Gizmos.DrawWireCube(center, size);
        }
    }
}