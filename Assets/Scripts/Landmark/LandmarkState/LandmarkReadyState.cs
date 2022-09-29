using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LandmarkReadyState : LandmarkStateBase
{
    public override void Action()
    {
        manager.objCallBox = null;

        manager.currentHp = (int)(manager.maxHp * 0.3);
        manager.hpBarVolume = manager.currentHp / manager.maxHp;

        if (manager.objHpBar == null)
        {
            CreateHpBar();
        }
    }

    private void Update()
    {
        if (manager.objHpBar != null)
        {
            manager.SetHpBarPos(manager.CalcHpBarPos());
            CalcHpTimer();

            manager.CalcHp();
        }
    }
    private void CreateHpBar()
    {
        manager.objHpBar = Instantiate(manager.prefabsHpBar);

        manager.objHpBar.gameObject.SetActive(false);
        manager.objHpBar.transform.SetParent(manager.canvasLandmark.transform);

        manager.rectTransformHpBar = manager.objHpBar.GetComponent<RectTransform>();
        manager.sliderHpBar = manager.objHpBar.GetComponent<Slider>();

        manager.sliderHpBar.value = manager.hpBarVolume;

        manager.objHpBar.gameObject.SetActive(true);
    }

    private void CalcHpTimer()
    {
        if (manager.currentHp < manager.maxHp)
        {
            manager.addHpTimerCount += Time.deltaTime;

            if (manager.addHpTimerCount > manager.addHpTimerMaxCount)
            {
                manager.addHpTimerCount = .0f;

                AddHp(manager.addHpVolume);
            }
        }
        else if (manager.hpBarVolume >= (manager.maxHp / manager.maxHp) - 0.01f)
        {
            manager.currentHp = manager.maxHp;

            manager.SetState(Landmark.LandmarkState.LANDMARK_WORK);
        }
    }

    private void AddHp(float volume)
    {
        manager.currentHp += volume;
    }
}
