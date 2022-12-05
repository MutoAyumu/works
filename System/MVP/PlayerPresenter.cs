using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerPresenter : MonoBehaviour
{
    [Header("設定")]
    [SerializeField, Tooltip("線香のテキストを設定")] MVPText _senkouText;
    [SerializeField, Tooltip("HPのテキストを設定")] MVPText _hpText;

    private void Start()
    {
        if(_senkouText)
        {
            var senkou = GameObject.FindGameObjectWithTag("Player").GetComponent<SenkouHealth>();

            if(!senkou)
            {
                Debug.LogError("SenkouHealthが見つかりませんでした");
                return;
            }

            senkou.Health.Subscribe(x =>
            {
                var m = (int)(x / 60);
                var s = (int)(x - (60 * m));

                _senkouText.SetText($"線香の残り時間：[{m:00}:{s:00}]");
            }).AddTo(this);
        }
        else
        {
            Debug.LogError("SenkouTextが設定されていません");
        }

        if(_hpText)
        {
            var hp = GameObject.FindGameObjectWithTag("Player").GetComponent<Durability>();

            if(!hp)
            {
                Debug.LogError("Durabilityが見つかりませんでした");
                return;
            }

            hp.HP.Subscribe(x =>
            {
                _hpText.SetText($"蚊取り豚の耐久力:{x:00}");
            }).AddTo(this);
        }
        else
        {
            Debug.LogError("HpTextが設定されていません");
        }
    }
}
