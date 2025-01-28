using System;
using UnityEngine.Pool;

public class LifePool
{
    private readonly IObjectPool<int[]> _lives;

    public LifePool(Func<int[]> creatFunc,Action<int[]> onGet, bool collectionCheck, int defaultCapacity, int maxSize)
    {
        _lives = new ObjectPool<int[]>(creatFunc,onGet,null,null,collectionCheck,defaultCapacity,maxSize);
    }

    public void Get()
    {
        _lives.Get();
    }

    public void Release(int[] life)
    {
        _lives.Release(life);
    }
    
    

    
}