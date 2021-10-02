using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance { get; private set;}

    public GameObject gamePanel;
    public ValueBar hpBar;
    public Text life;
    public Text coin;
    public GameObject endPanel;
    public Image key8;
    public ValueBar bossHpBar;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {

    }

    public void ShowGame()
    {
        gamePanel.SetActive(true);
        endPanel.SetActive(false);
    }

    public void HideAll()
    {
        gamePanel.SetActive(false);
        endPanel.SetActive(false);
    }

    public void ShowEnd()
    {
        gamePanel.SetActive(false);
        endPanel.SetActive(true);
        TextRandom[] textRandoms = endPanel.GetComponentsInChildren<TextRandom>();
        foreach (var item in textRandoms)
        {
            item.ShowRandom();
        }
        if (GameManager.Instance.life <= 0)
        {
            key8.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }

    public void ShowBossHp()
    {
        bossHpBar.gameObject.SetActive(true);
    }

    public void UpdateBossHp(float maxhp, float hp)
    {
        bossHpBar.UpdateValue(maxhp, hp);
    }

    public void UpdateHp(float maxhp, float hp)
    {
        hpBar.UpdateValue(maxhp, hp);
    }

    public void UpdateLife(int value)
    {
        life.text = "x" + value;
    }

    public void UpdateCoin(int value)
    {
        coin.text = "x" + value;
    }

}
