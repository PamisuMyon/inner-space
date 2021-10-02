using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Scroller : MonoBehaviour
{

    public LevelConfig config;
    public LevelConfig endConfig;
    public float beginSpeed;
    public float beginDuation;
    public Transform frontPoint;
    public GameObject start;
    public Volume volume;
    public float bloomStart = 4f;
    public float bloomEnd = 6.62f;
    public float chrStart = 0;
    public float chrEnd = 1;

    StarBackground[] bgs;
    float screenHalfHeight;
    float screenHalfWidth;
    public bool end;

    [Header("View Only")]
    [SerializeField] int stageNum;
    [SerializeField] int waveNum;

    Player player;

    void Start()
    {
        bgs = GameObject.FindObjectsOfType<StarBackground>();

        screenHalfHeight = Camera.main.orthographicSize * .9f;
        screenHalfWidth = screenHalfHeight * Camera.main.orthographicSize;

        player = GameObject.FindObjectOfType<Player>();

        Init();

        StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        while (!IsStageClear() && !end)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1.2f);
        Begin();
        yield return new WaitForSeconds(beginDuation);
        Destroy(start);
        
        if (!end)
            GameManager.Instance.LevelBegin();
        else
            GameManager.Instance.TrueEnd();

        if (config == null) yield break;
        foreach (var stage in config.stages)
        {
            if (stage.startCondition == Condition.WaitForClear)
            {
                while(!IsStageClear())
                    yield return null;
            }
            else if (stage.startCondition == Condition.WaitForInterval)
            {
                yield return new WaitForSeconds(stage.interval);
            }

            if (stage.scrollSpeed > 0)
            {
                SetBgSpeed(stage.scrollSpeed);
            }

            if (stage.bossFight)
            {
                UIManager.Instance.ShowBossHp();
            }
            
            foreach (var wave in stage.waves)
            {
                if (wave.startCondition == Condition.WaitForClear)
                {
                    while (!IsStageClear())
                        yield return null;
                }
                else if (wave.startCondition == Condition.WaitForInterval)
                {
                    yield return new WaitForSeconds(wave.interval);
                }
                
                ApplyDebuff(wave.debuffs);

                foreach (var obstacle in wave.obstacles)
                {
                    var go = Instantiate(obstacle, transform);
                    float y = frontPoint.position.y + Random.Range(-screenHalfHeight, screenHalfHeight);
                    var pos = new Vector3(frontPoint.position.x, y, frontPoint.position.z);
                    go.transform.position = pos;
                    yield return new WaitForSeconds(Utils.RandomShake(wave.spwanInterval, .5f));
                }
                waveNum++;
            }
            stageNum++;
            waveNum = 0;
        }
    }

    bool IsStageClear()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        return gameObjects.Length == 0;
    }

    void Init()
    {
        ChromaticAberration chromatic;
        var b = volume.sharedProfile.TryGet<ChromaticAberration>(out chromatic);
        if (b)
        {
            chromatic.intensity.value = chrStart;
        }
        Bloom bloom;
        b = volume.sharedProfile.TryGet<Bloom>(out bloom);
        if (b)
        {
            bloom.intensity.value = chrEnd;
        }
    }

    void Begin()
    {
        Scrollable[] scrollables = GameObject.FindObjectsOfType<Scrollable>();
        foreach (var item in scrollables)
        {
            DOTween.To(() => item.speed, v => item.speed = v, beginSpeed, beginDuation);
        }
        SetBgSpeed(beginSpeed);

        ChromaticAberration chromatic;
        var b = volume.sharedProfile.TryGet<ChromaticAberration>(out chromatic);
        if (b)
        {
            DOTween.To(() => chromatic.intensity.value, v => chromatic.intensity.value = v, chrEnd, beginDuation * .6f);
        }
        Bloom bloom;
        b = volume.sharedProfile.TryGet<Bloom>(out bloom);
        if (b)
        {
            DOTween.To(() => bloom.intensity.value, v => bloom.intensity.value = v, bloomEnd, beginDuation * .6f);
        }
    }

    void SetBgSpeed(float speed)
    {
        foreach (var item in bgs)
        {
            DOTween.To(() => item.speed, v => item.speed = v, speed, beginDuation);
        }
    }

    void ApplyDebuff(Debuff[] debuffs)
    {
        if (end) return;
        var weapon = player.weapon;
        if (debuffs.Length == 0)
        {
            weapon.fireBackwards = false;
            weapon.state = PlayerWeapon.State.Normal;
            player.turnAlien = false;
            return;
        }

        foreach (var item in debuffs)
        {
            if (item == Debuff.RandomDebuff)
            {
                Apply(GetRandom(), weapon);
            }
            else
            {
                Apply(item, weapon);
            }
        }
    }

    void Apply(Debuff item, PlayerWeapon weapon)
    {
        if (item == Debuff.FireBackwards)
        {
            weapon.fireBackwards = true;
        }
        else if (item == Debuff.RandomAll)
        {
            weapon.state = PlayerWeapon.State.Random;
        }
        else if (item == Debuff.RandomList)
        {
            weapon.state = PlayerWeapon.State.RandomList;
        }
        else if (item == Debuff.Medic)
        {
            weapon.state = PlayerWeapon.State.Medic;
        }
        else if (item == Debuff.TurnAlien)
        {
            player.turnAlien = true;
        }
    }

    Debuff GetRandom()
    {
        List<Debuff> debuffs = new List<Debuff>();
        // debuffs.Add(Debuff.FireBackwards);
        debuffs.Add(Debuff.Medic);
        debuffs.Add(Debuff.Medic);
        debuffs.Add(Debuff.RandomAll);
        debuffs.Add(Debuff.RandomAll);
        debuffs.Add(Debuff.RandomAll);
        debuffs.Add(Debuff.RandomAll);
        debuffs.Add(Debuff.RandomList);
        debuffs.Add(Debuff.RandomList);
        debuffs.Add(Debuff.RandomList);
        debuffs.Add(Debuff.RandomList);
        debuffs.Add(Debuff.TurnAlien);
        return Utils.GetRandom<Debuff>(debuffs);
    }

    public void End()
    {
        end = true;
        config = endConfig;
    }
    
}
