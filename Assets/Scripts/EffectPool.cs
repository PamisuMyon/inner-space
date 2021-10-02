using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : MonoBehaviour
{

    public static EffectPool Instance { get; private set; }

    public GameObject healPrefab;
    public GameObject fleshExplodePrefab;
    public GameObject gunfire1Prefab;
    public GameObject explode1Prefab;
    public GameObject explode2Prefab;
    public GameObject shockwavePrefab;

    List<GameObject> heals;
    List<GameObject> fleshExplodes;
    List<GameObject> gunfire1s;
    List<GameObject> explode1s;
    List<GameObject> explode2s;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        heals = new List<GameObject>();
        gunfire1s = new List<GameObject>();
        fleshExplodes = new List<GameObject>();
        explode1s = new List<GameObject>();
        explode2s = new List<GameObject>();
    }

    public GameObject GetObject(EffectType type)
    {
        if (type == EffectType.Heal)
            return Instantiate(healPrefab, transform);
        if (type == EffectType.FleshExplode)
            return GetObject(fleshExplodes, fleshExplodePrefab);
        if (type == EffectType.Gunfire1)
            return GetObject(gunfire1s, gunfire1Prefab);
        if (type == EffectType.Explode1)
            return GetObject(explode1s, explode1Prefab);
        if (type == EffectType.Explode2)
            return GetObject(explode2s, explode2Prefab);
        return null;
    }

    GameObject GetObject(List<GameObject> list, GameObject prefab)
    {
        foreach (var item in list)
        {
            if (!item.activeInHierarchy)
            {   
                item.SetActive(true);
                return item;
            }
        }
        var go = Instantiate(prefab, transform);
        list.Add(go);
        return go;
    }
    
}
