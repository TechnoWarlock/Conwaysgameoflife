using UnityEngine;

public class CellLiveHandler
{
    // private const int MaxSurvivalLife = 4;
    // private const int MinSurvivalLife = 3;
    // private const int BirthLimit = -2;

    public CellConfigurationSo CellConfiguration { private get; set; }

    private const int AliveCellValue = 1;
    private const int NoAliveCellValue = 0;

    private int[] _currentLive;

    private readonly LifePool _lifePool;

    public CellLiveHandler(bool poolCollectionCheck, int poolDefaultCapacity, int poolMaxSize)
    {
        _lifePool = new LifePool(CreateCell, OnGetCell, poolCollectionCheck, poolDefaultCapacity, poolMaxSize);
    }

    public bool IsCellAlive(int[] life)
    {
        return (life[0] >= CellConfiguration.MinSurvivalLife && life[0] <= CellConfiguration.MaxSurvivalLife) ||
               life[0] == CellConfiguration.BirthLimit;
    }

    // public void ResetToAliveCell(int[] life)
    // {
    //     life[0] = AliveCellValue;
    // }

    public void AliveCell(int[] life)
    {
        life[0] = AliveCellValue;
    }

    public void IndeterminateCell(int[] life)
    {
        life[0] = NoAliveCellValue;
    }

    public bool WasCellAlive(int[] life)
    {
        return life[0] >= AliveCellValue;
    }

    public void CallCell()
    {
        _lifePool.Get();
    }

    public void ReleaseCell(int[] life)
    {
        _lifePool.Release(life);
    }

    public int[] GetCell()
    {
        return _currentLive;
    }
    private void OnGetCell(int[] obj)
    {
        _currentLive = obj;
    }

    private int[] CreateCell()
    {
        
        return new[] { NoAliveCellValue };
    }

    public void AddLife(int[] life)
    {
        if (WasCellAlive(life))
        {
            life[0]++;
            return;
        }

        life[0]--;
    }
}