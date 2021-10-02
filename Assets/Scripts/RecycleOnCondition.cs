using System.Collections;
using UnityEngine;

public class RecycleOnCondition : MonoBehaviour
{
    
    public float delay = -1;
    public bool destroy;
    
    private void Start() 
    {
        if (delay != -1)
        {
            StartCoroutine(DelayDeactivate());
        }
    }

    IEnumerator DelayDeactivate()
    {
        yield return new WaitForSeconds(delay);
        OnAnimFinished();
    }

    public void OnAnimFinished()
    {
        if (!destroy)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
