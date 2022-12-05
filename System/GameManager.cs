using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using UnityEngine.UI;

/// <summary>
/// ゲームの管理クラス
/// </summary>
public class GameManager
{
    #region プロパティ
    /// <summary>
    /// GameManagerのインスタンス
    /// </summary>
    public static GameManager Instance = new GameManager();

    public IReadOnlyReactiveProperty<float> GameTime => _gameTime;
    public IReadOnlyReactiveProperty<int> Score => _score;
    public IReadOnlyReactiveProperty<int> Quota => _quota;

    #endregion

    #region 変数

    FloatReactiveProperty _gameTime = new FloatReactiveProperty();
    IntReactiveProperty _score = new IntReactiveProperty();
    IntReactiveProperty _quota = new IntReactiveProperty();

    Image _gameOverPanel;
    Image _gameClearPanel;

    bool _isPause;
    #endregion

    /*ToDo
    スコアの管理
    時間の管理
    ポーズ処理周り
    */

    /*説明
    PlayerModeChange(PlayerMode mode)
        蚊取り豚のモードを変更させる関数
        引数で与えられたモードに切り替わる

    AddScore(int score)
        スコアの加算をする関数
        引数で与えられた値が負の値であれば、ログだけを出して加算はしない
        0または正の値であった場合ログを出してスコアの加算を行う
     */

    //コンストラクタ
    public GameManager() 
    {
        Debug.Log("New GameManager");
    }

    #region イベント

    /// <summary>
    /// ポーズ時の処理を登録
    /// </summary>
    public event Action OnPause;
    /// <summary>
    /// ポーズ解除時の処理を登録
    /// </summary>
    public event Action OnResume;

    /// <summary>
    /// メラメラモードがPowerUpになった時に呼ばれるイベント
    /// </summary>
    public event Action OnPowerUpEvent;
    /// <summary>
    /// メラメラモードがNormalになった時に呼ばれるイベント
    /// </summary>
    public event Action OnPowerDownEvent;

    /// <summary>
    /// ゲームオーバー時のイベント
    /// </summary>
    public event Action OnGameOverEvent;
    /// <summary>
    /// ゲームクリア時のイベント
    /// </summary>
    public event Action OnGameClearEvent;

    #endregion

    /// <summary>
    /// 蚊取り豚のモードを切り替える関数
    /// </summary>
    /// <param name="mode"></param>
    public void PlayerModeChange(PlayerMode mode)
    {
        switch(mode)
        {
            case PlayerMode.PowerUp:
                OnPowerUpEvent?.Invoke();
                break;
            case PlayerMode.Normal:
                OnPowerDownEvent?.Invoke();
                break;
        }

        Debug.Log($"モードを切り替えた {mode}");
    }

    /// <summary>
    /// スコアを加算する
    /// </summary>
    public void AddScore(int score)
    {
        if(score < 0)
        {
            Debug.Log("与えられた値が不正な値でした");
            return;
        }

        Debug.Log($"与えられた値:{score} 　現在のスコア:{_score.Value}");
        _score.Value += score;

        if(_score.Value >= _quota.Value)
        {
            OnGameClear();
        }
    }

    /// <summary>
    /// 変数の初期設定
    /// </summary>
    /// <param name="attachment"></param>
    public void OnSetup(GameManagerAttachment attachment)
    {
        //初期化
        _score.Value = 0;
        _gameTime.Value = 0;

        //倒すノルマを設定
        _quota.Value = attachment.Quota;
        //パネルの参照
        _gameClearPanel = attachment.GameClearPanel;
        _gameOverPanel = attachment.GameOverPanel;
    }

    public void OnGameOver()
    {
        OnGameOverEvent?.Invoke();
        _gameOverPanel?.gameObject.SetActive(true);
        Debug.Log("OnGameOver");
    }

    void OnGameClear()
    {
        OnGameClearEvent?.Invoke();
        _gameClearPanel?.gameObject.SetActive(true);
        Debug.Log("OnGameClear");
    }

    /// <summary>
    /// フレーム処理
    /// </summary>
    void OnUpdate()
    {
        _gameTime.Value += Time.deltaTime;

        //ポーズの処理部分
        if(InputUtility.GetDownPause)
        {
            //ポーズ中
            if(_isPause)
            {
                OnResume?.Invoke();
                Debug.Log("ポーズ解除");
            }
            else
            {
                OnPause?.Invoke();
                Debug.Log("ポーズ開始");
            }

            //逆にする
            _isPause = !_isPause;
        }
    }

    #region コールバック

    public void SetupUpdateCallback(GameManagerAttachment attachment)
    {
        attachment.SetupCallBack(OnUpdate);
    }

    #endregion
}

/// <summary>
/// 蚊取り豚のモード用のEnum
/// </summary>
public enum PlayerMode
{
    //通常モード
    Normal = 0,
    //メラメラモード
    PowerUp = 1,
}
