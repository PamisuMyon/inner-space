using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public bool fire;
    public bool move1;
    public bool move2;
    public bool move3;
    public bool move4;

    void Update()
    {
        move1 = Input.GetKey(KeyCode.A);
        move2 = Input.GetKey(KeyCode.X);
        move3 = Input.GetKey(KeyCode.P);
        move4 = Input.GetKey(KeyCode.B);
        fire = Input.GetKey(KeyCode.Space);
    }
}
