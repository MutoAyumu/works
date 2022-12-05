using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerAttachment : MonoBehaviour
{
    #region 変数
    [SerializeField, Tooltip("倒さないといけない敵のノルマ(個)")] int _quota = 10;
    [Space(10)]
    [SerializeField] Image _gameOverPanel;
    [SerializeField] Image _gameClearPanel;
    #endregion

    #region プロパティ
    /// <summary>
    /// 敵のノルマ
    /// </summary>
    public int Quota => _quota;

    public Image GameOverPanel => _gameOverPanel;
    public Image GameClearPanel => _gameClearPanel;
    #endregion

    #region デリゲート
    public delegate void MonoEvent();
    MonoEvent _updateEvent;
    #endregion

    private void Awake()
    {
        if (_gameOverPanel)
            _gameOverPanel.gameObject.SetActive(false);

        if (_gameClearPanel)
            _gameClearPanel.gameObject.SetActive(false);

        GameManager.Instance.SetupUpdateCallback(this);
        GameManager.Instance.OnSetup(this);
    }
    private void Update()
    {
        _updateEvent?.Invoke();
    }

    /// <summary>
    /// Updateで呼びたい処理を登録しておく
    /// </summary>
    public void SetupCallBack(MonoEvent e)
    {
        _updateEvent = e;
    }
}
