using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "MiniJam#74/LevelConfig", order = 0)]
public class LevelConfig : ScriptableObject 
{
    public Stage[] stages;
}

[System.Serializable]
public struct Stage
{
    public Condition startCondition;
    public float scrollSpeed;
    public Wave[] waves;
    public float interval;
    public bool bossFight;
}

[System.Serializable]
public struct Wave
{
    public Condition startCondition;
    public Formation formation;
    public GameObject[] obstacles;
    public float interval;
    public float spwanInterval;
    public Debuff[] debuffs;
    public float debuffDuration;
}

public enum Condition
{
    WaitForInterval, WaitForClear
}

public enum Formation
{
    Random, TwoSide
}

public enum Debuff
{
    FireBackwards, RandomList, RandomAll, Medic, TurnAlien, RandomDebuff
}