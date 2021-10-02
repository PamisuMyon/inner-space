using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextRandom : MonoBehaviour
{
    public string normal;
    public string[] random;
    public int randomTimes;
    public float randomDuration;
    public float randomInterval;

    Text text;
    [SerializeField] bool isShowing;

    void Awake()
    {
        text = GetComponent<Text>();
    }
    
    private void OnEnable() 
    {
        ShowRandom();    
    }

    public void ShowRandom() 
    {
        if (!isShowing)
            StartCoroutine(Show());
    }

    IEnumerator Show()
    {
        isShowing = true;
        while (true)
        {
            yield return new WaitForSecondsRealtime(Utils.RandomShake(randomInterval, .5f));
            for (int i = 0; i < Utils.RandomShake(randomTimes, .5f); i++)
            {
                var index = Random.Range(0, random.Length);
                text.text = random[index];
                yield return new WaitForSecondsRealtime(Utils.RandomShake(randomDuration, .1f));
            }
            text.text = normal;
            if (!isShowing)
                yield break;
        }
    }

    private void OnDisable() 
    {
        isShowing = false;    
    }
}
