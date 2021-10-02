using UnityEngine;

public class Scrollable : MonoBehaviour
{
    public float initialSpeed;
    public float speed;

    void Start()
    {
        speed = initialSpeed;
    }

    void Update() 
    {
        if (speed > 0)
            transform.position += -Vector3.right * speed * Time.deltaTime;
    }
}
