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
        
        var movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (movement.x < 0)
            move3 = move4 = true;
        else if (movement.x > 0)
            move1 = move2 = true;
        if (movement.y < 0)
            move1 = move3 = true;
        else if (movement.y > 0)
            move2 = move4 = true;
        
        fire = Input.GetKey(KeyCode.Space);
    }
}
