using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WayPoint))]
public class EnemyGenerator : MonoBehaviour
{
    [SerializeField, Tooltip("蚊のプレハブ")] MosquitoBase _enemy;//蚊のオブジェクト
    [Tooltip("巡回する座標")] Vector3[] _wayPoints;
    [SerializeField, Tooltip("巡回する座標の数")] int _wayPointsLimit = 4;
    [SerializeField, Tooltip("マップ内に何体出現していいか")] int _limit = 10;
    
    int _createCount;
    ObjectPool<MosquitoBase> _pool = new ObjectPool<MosquitoBase>();

    const int _poolingLimit = 50;

    void Start()
    {
        var root = new GameObject("EnemyRoot").transform;
        _pool.SetBaseObj(_enemy, root);
        _pool.SetCapacity(_poolingLimit);

        _wayPoints = GetComponent<WayPoint>().LocalNodes;

        _wayPointsLimit = Mathf.Clamp(_wayPointsLimit, 0, _wayPoints.Length);

        for(int i = 0; i < _limit; i++)
        {
            Spawn();
        }
    }

    /// <summary>
    /// 敵を生成する
    /// </summary>
    void Spawn()
    {
        var quotaCount = GameManager.Instance.Quota.Value;

        if (quotaCount <= _createCount) return;

        _createCount++;
        var points = new Vector3[_wayPointsLimit];

        for(int i = 0; i < _wayPointsLimit; i++)
        {
            var r = Random.Range(0, _wayPoints.Length);
            points[i] = _wayPoints[r];
        }

        var enemy = _pool.Instantiate();
        enemy.transform.position = points[0];
        enemy.Move.Init(points);
        enemy.Health.Init();

        //敵が死んだときに呼ばれるデリデートに生成の関数を登録
        enemy.Health.OnDestroy = Spawn;
    }
}
