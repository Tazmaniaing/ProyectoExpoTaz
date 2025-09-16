using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    // La única instancia de esta clase. Acceso global.
    public static GameManager instance;
    // Un evento que se invoca cuando cualquier dato del juego cambia.
    public static UnityEvent OnGameDataChanged = new UnityEvent();

    [Header("Datos del Jugador")]
    // Lista de ítems recolectados por su nombre.
    public List<string> collectedItems = new List<string>();
    public int score = 0;
    
    [Header("Salud y Objetos")]
    // Variables para el sistema de salud.
    public int maxHealth = 3;
    public int currentHealth = 3;
    // Variable para los diamantes.
    public int totalDiamonds = 0;

    void Awake()
    {
        // Implementación del patrón Singleton para garantizar una única instancia.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        totalDiamonds = PlayerPrefs.GetInt("Diamonds");
    }

    /// <summary>
    /// Agrega un ítem recolectable a la lista.
    /// </summary>
    public void AddCollectedItem(string itemName, int scoreValue = 100)
    {
        if (!collectedItems.Contains(itemName))
        {
            collectedItems.Add(itemName);
            score += scoreValue;
            OnGameDataChanged.Invoke();
        }
    }

    /// <summary>
    /// Cambia la salud del jugador por una cantidad específica.
    /// </summary>
    /// <param name="amount">La cantidad a sumar o restar de la salud.</param>
    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        // La salud no puede ser menor a 0 ni mayor a la salud máxima.
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnGameDataChanged.Invoke();
    }
    
    /// <summary>
    /// Agrega un diamante a la cuenta total.
    /// </summary>
    /// <param name="amount">La cantidad de diamantes a sumar.</param>
    public void AddDiamond(int amount = 1)
    {
        totalDiamonds += amount;
        PlayerPrefs.SetInt("Diamonds", totalDiamonds);
        OnGameDataChanged.Invoke();
    }

    /// <summary>
    /// Verifica si un ítem ya ha sido recolectado.
    /// </summary>
    public bool HasCollectedItem(string itemName)
    {
        return collectedItems.Contains(itemName);
    }
}