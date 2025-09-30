using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI killCounterText;

    private int enemiesKilled = 0;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        UpdateKillCounterUI();
    }

    public void AddKill()
    {
        enemiesKilled++;
        UpdateKillCounterUI();
        Debug.Log($"Enemigos eliminados: {enemiesKilled}");
    }

    private void UpdateKillCounterUI()
    {
        if (killCounterText != null)
        {
            killCounterText.text = $"Kills: {enemiesKilled}";
        }
    }

    public int GetKillCount()
    {
        return enemiesKilled;
    }
}