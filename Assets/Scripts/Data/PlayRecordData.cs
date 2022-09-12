using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

[System.Serializable]
public class PlayRecordData
{
    public int bestScore = 0;
    public float bestDistance = 0;
    public int bestPassCornerTrackCount = 0;
    public int bestPassJumpTrackCount = 0;
    public int bestHitEnemyCount = 0;

    public int useCharacter = 100;
    public List<int> haveCharacterIDs = new List<int>();
    public int gold = 0;

}
