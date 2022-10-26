using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

[CreateAssetMenu(fileName = "AttackHitBoxData", menuName = "HitBox/AttackHitBoxData", order = 0)]
public class AttackHitBoxData : ScriptableObject
{
    [SerializeField]
    private Vector3 createPosition;
    public Vector3 CreatePosition { get { return createPosition; } }

    [SerializeField]
    private GameObject hitBoxPrefab;
    public GameObject HitBoxPrefab { get { return hitBoxPrefab; } }

    [SerializeField]
    private float timeScale = 1f;
    public float TimeScale { get { return timeScale; } }

    [SerializeField]
    private float timeScaleLifeTime = 1f;
    public float TimeScaleLifeTime { get { return timeScaleLifeTime; } }

    [SerializeField]
    private List<ObjectTweenAnimationData> hitTweenDataList;
    public List<ObjectTweenAnimationData> HitTweeDataList { get { return hitTweenDataList; } }

}
