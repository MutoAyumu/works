using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SystemTextPresenter : MonoBehaviour
{
    [Header("設定")]
    [SerializeField, Tooltip("制限時間のテキストを設定")] MVPText _timerText;
    [SerializeField, Tooltip("倒した数のテキストを設定")] MVPText _countText;

    private void Start()
    {
        if(_timerText)
        {
            GameManager.Instance.GameTime.Subscribe(x =>
            {
                var m = (int)(x / 60);
                var s = (int)(x - (60 * m));
                _timerText.SetText($"経過時間 [{m:00} : {s:00}]");
            }).AddTo(this);
        }
        if(_countText)
        {
            GameManager.Instance.Score.Subscribe(x =>
            {
                _countText.SetText($"倒した数 [{x:000} 匹]");
            }).AddTo(this);
        }
    }
}
