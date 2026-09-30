using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public TextMeshProUGUI scoreText;
    void Awake() { Instance = this; }
    public void UpdateScore()
    {
        scoreText.text = $"{GameManager.Instance.teamA.score} - {GameManager.Instance.teamB.score}";
    }
}