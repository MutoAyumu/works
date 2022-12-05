using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool<T> where T : UnityEngine.Object, IObjectPool
{
    T BaseObj = null;
    Transform Parent = null;
    List<T> Pool = new List<T>();
    int Index = 0;

    public List<T> PoolList { get => Pool; }

    public void SetBaseObj(T obj, Transform parent)
    {
        BaseObj = obj;
        Parent = parent;
    }

    public void Pooling(T obj)
    {
        obj.DisactiveForInstantiate();
        Pool.Add(obj);
    }

    public void SetCapacity(int size)
    {
        Pool.Clear();

        if (size < Pool.Count) return;

        for (int i = 0; i < size; i++)
        {
            T Obj = default(T);
            if (Parent)
            {
                Obj = GameObject.Instantiate(BaseObj, Parent);
            }
            else
            {
                Obj = GameObject.Instantiate(BaseObj);
            }
            Pooling(Obj);
        }

        Debug.Log($"<color=cyan>{this.BaseObj.name}</color> : {size}個生成終了");
    }

    public T Instantiate()
    {
        for (int i = 0; i < Pool.Count; i++)
        {
            int index = i;
            if (Pool[index].IsActive) continue;

            Pool[index].Create();
            return Pool[index];
        }

        T Obj = default(T);
        if (Parent)
        {
            Obj = GameObject.Instantiate(BaseObj, Parent);
        }
        else
        {
            Obj = GameObject.Instantiate(BaseObj);
        }
        Pooling(Obj);
        Obj.Create();
        return Obj;
    }
}