using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int life = 3;
    public int coin;
    public Player player;

    public bool isEnd = true;
    public int current;

    [Space]
    public AudioSource healAudio;
    public AudioSource hitAudio;
    public AudioSource gameOverAudio;
    public AudioSource shockwaveAudio;

    [Space]
    public AudioClip titleClip;
    public AudioClip gameClip;
    public AudioClip endClip;

    AudioSource audioSource;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start() 
    {
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindObjectOfType<Player>();

        audioSource.clip = titleClip;
        audioSource.Play();
    }

    private void Update() 
    {
        if (current == 0)
        {
            if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8))
            {
                GameStart();
            }
        }
        else if (isEnd)
        {
            if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8))
            {
                Continue();
            }
        }    
    }

    public void RegisterPlayer(Player player)
    {
        this.player = player;
    }

    public void GameStart()
    {
        SceneManager.LoadScene(current = 1);
        isEnd = false;
    }

    public void LevelBegin()
    {
        audioSource.clip = gameClip;
        audioSource.Play();
    }

    public void GameOver()
    {
        isEnd = true;
        Time.timeScale = 0;
        life--;
        coin = 0;
        UIManager.Instance.UpdateLife(life);
        UIManager.Instance.ShowEnd();
        gameOverAudio.pitch = Random.Range(.8f, 1.2f);
        gameOverAudio.Play();
    }

    public void GameEnd()
    {
        Invoke("DelayEnd", 3f);
    }
    
    void DelayEnd()
    {
        SceneManager.LoadScene(current = 2);
        audioSource.clip = titleClip;
        audioSource.Play();
    }

    public void Continue()
    {
        isEnd = false;
        Time.timeScale = 1;
        player.gameObject.SetActive(true);
        player.Revive();
        UIManager.Instance.ShowGame();
    }

    public void UpdateCoin(int value)
    {
        coin += value;
        UIManager.Instance.UpdateCoin(coin);
    }

    public void Title()
    {
        SceneManager.LoadScene(current = 0);
    }

    public void TrueEnd()
    {
        audioSource.clip = endClip;
        audioSource.Play();
    }

    public void PlayHeal()
    {
        healAudio.pitch = Random.Range(0.8f, 1.2f);
        healAudio.Play();
    }

    public void PlayHit()
    {
        hitAudio.pitch = Random.Range(0.8f, 1.2f);
        hitAudio.Play();
    }

    public void PlayShockwave()
    {
        shockwaveAudio.pitch = Random.Range(0.8f, 1.2f);
        shockwaveAudio.Play();
    }
}
