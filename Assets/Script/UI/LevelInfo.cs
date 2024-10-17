using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelInfo", menuName = "Game/LevelInfo", order = 1)]
public class LevelInfo : ScriptableObject
{
    public string levelName;
    public LevelType levelDifficulty;
    public GooType requiredGooType;
}