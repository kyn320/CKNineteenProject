using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBuffSlot : MonoBehaviour
{
    [SerializeField]
    private UIColorData colorData;

    [SerializeField]
    private UIBaseImage buffIcon;

    [SerializeField]
    private UIBaseImage labelImage;

    [SerializeField]
    private UIAmountText lifeTimeText;

    public void SetBuff(BuffBehaviour buffBehaviour)
    {
        var buffData = buffBehaviour.GetBuffData();

        switch (buffData.Type)
        {
            case BuffType.Buff:
                labelImage.SetColor(colorData.colorDic["Buff"]);
                break;
            case BuffType.Debuff:
                labelImage.SetColor(colorData.colorDic["DeBuff"]);
                break;
        }

        buffIcon.SetImage(buffData.Icon);
        lifeTimeText.UpdateAmount(buffBehaviour.GetLifeTime());

        buffBehaviour.updateBuffEvent.AddListener(UpdateLifeTime);
        buffBehaviour.endBuffEvent.AddListener(OnEndBuff);
    }

    public void UpdateLifeTime(BuffBehaviour buffBehaviour, float lifeTime)
    {
        lifeTimeText.UpdateAmount(lifeTime);
    }

    public void OnEndBuff(BuffBehaviour buffBehaviour)
    {
        Destroy(gameObject);
    }
}
