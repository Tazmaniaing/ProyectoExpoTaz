using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Text scoreText;
    public GameObject iconKey1;
    public GameObject iconKey2;
    public GameObject iconGem;

    // Con Awake() podemos verificar si el script se está ejecutando.
    void Awake()
    {
        Debug.Log("UIManager: Awake() ejecutado. El Canvas está activo.");
        
        // Opcional: Si el Canvas es hijo de otro objeto, aseguramos que esté activo
        if (transform.parent != null)
        {
            transform.parent.gameObject.SetActive(true);
        }
    }

    void OnEnable()
    {
        if (GameManager.instance != null)
        {
            GameManager.OnGameDataChanged.AddListener(UpdateUI);
        }
        UpdateUI();
    }

    void OnDisable()
    {
        if (GameManager.instance != null)
        {
            GameManager.OnGameDataChanged.RemoveListener(UpdateUI);
        }
    }

    public void UpdateUI()
    {
        if (GameManager.instance == null) return;
        
        if (scoreText != null)
        {
            scoreText.text = "Puntuación: " + GameManager.instance.score;
        }

        if (iconKey1 != null) iconKey1.SetActive(GameManager.instance.HasCollectedItem("Llave1"));
        if (iconKey2 != null) iconKey2.SetActive(GameManager.instance.HasCollectedItem("Llave2"));
        if (iconGem != null) iconGem.SetActive(GameManager.instance.HasCollectedItem("Gema"));
    }
}