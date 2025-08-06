using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator doorAnimator;

    void Start()
    {
        // Obtiene el componente Animator en el objeto de la puerta
        doorAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el objeto que entró es el jugador (usando su tag)
        if (other.CompareTag("Player"))
        {
            // Llama a la función para abrir la puerta
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        // Si hay un Animator, activa el parámetro 'IsOpen' a true.
        // Esto hará que la transición de IDLE a Abrir se ejecute.
        if (doorAnimator != null)
        {
            doorAnimator.SetBool("IsOpen", true);
        }
        
        // Aquí puedes agregar cualquier otra lógica, como cargar un nuevo nivel, etc.
        Debug.Log("La puerta se ha abierto.");
    }

    // Puedes agregar una función para cerrar la puerta si lo necesitas.
    private void CloseDoor()
    {
        if (doorAnimator != null)
        {
            doorAnimator.SetBool("IsOpen", false);
        }
    }
}