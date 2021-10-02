using System.Collections.Generic;
using UnityEngine;

public class StarBackground : MonoBehaviour
{
    public float speedMulti;
    public float speed;
    public int number;
    public int numberRandomness;
    public int bleed;
    public SpriteRenderer[] starPrefabs;
    
    Camera cam;
    float halfWidth;
    List<SpriteRenderer> stars;

    void Start()
    {
        cam = Camera.main;
        halfWidth = cam.aspect * cam.orthographicSize;
        halfWidth += bleed;

        stars = new List<SpriteRenderer>();
        Generate();
    }

    void Generate()
    {
        Vector2 origin = transform.position + new Vector3(-halfWidth, -cam.orthographicSize - bleed);
        int row = (int)(2 * (cam.orthographicSize + bleed));
        int column = (int)(2 * halfWidth);
        List<int> grid = new List<int>();
        for (int i = 0; i < row * column; i++)
        {
            grid.Add(i);
        }

        var num = number + Random.Range(-numberRandomness, numberRandomness);
        while (num > 0 && grid.Count > 0)
        {
            int index = Random.Range(0, grid.Count);
            int i = grid[index];
            int r = i / column;
            int c = i % column;
            grid.RemoveAt(index);
            num--;

            index = Random.Range(0, starPrefabs.Length);
            var go = Instantiate(starPrefabs[index], transform);
            go.transform.position = origin + new Vector2(c, r);
            // Debug.Log("r: " + r + " c: " + c);
            stars.Add(go);
        }
    }

    void Update() 
    {
        float leftEdge = cam.transform.position.x - halfWidth;
        foreach (var item in stars)
        {
            if (speed > 0)
                item.transform.position += -Vector3.right * speed * speedMulti * Time.deltaTime;
            if (item.transform.position.x - .5f < leftEdge)
            {
                item.transform.position += new Vector3(2 * halfWidth, 0, 0);
            }
        }
    }

}
