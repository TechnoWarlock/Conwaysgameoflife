using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class LifeSimulator
{
    private readonly CellPlacer _cellPlacer;
    private readonly CellLiveHandler _cellLiveHandler;

    private readonly Dictionary<Vector3Int, int[]> _cells = new();
    private BaseSurroundingPositions _neighborCalculator;
    private List<Vector3Int> _currentCells = new();
    private List<Vector3Int> _cellsToRemove = new();
    private Vector3Int[] _surroundingPositions;

    private List<Vector3Int> _initialState = new();
    private readonly Stopwatch _stopwatch;

    public LifeSimulator(CellPlacer cellPlacer, CellLiveHandler cellLiveHandler)
    {
        _cellPlacer = cellPlacer;
        _cellLiveHandler = cellLiveHandler;
        _stopwatch = new Stopwatch();
    }

    public void SetSurroundPositions(BaseSurroundingPositions surroundingPositions)
    {
        _neighborCalculator = surroundingPositions;
    }

    public void AddCellFromInput(Vector3Int position)
    {
        if (_cells.Remove(position, out int[] life))
        {
            RemoveCell(position);
            SoundTrigger.RemoveCellClip();
            return;
        }
        _cellLiveHandler.CallCell();
        life = _cellLiveHandler.GetCell();
        _cellLiveHandler.AliveCell(life);
        _cells.Add(position, life);
        AddCell(position);
        SoundTrigger.PutCellClip();
    }
    

    public bool HasCells()
    {
        return _cells.Count > 0;
    }

    public void Clear()
    {
        _cells.Clear();
        _initialState.Clear();
        _cellPlacer.ClearCells();
    }


    private void AddCell(Vector3Int position)
    {
        _cellPlacer.PutCell(position);
    }

    private void RemoveCell(Vector3Int position)
    {
        _cellPlacer.RemoveCell(position);
    }

    public void SaveInitialState()
    {
        _initialState = _cells.Keys.ToList();
    }

    public void PutInitialState()
    {
        _cells.Clear();
        _cellPlacer.ClearCells();
        foreach (Vector3Int item in _initialState)
        {
            AddCellFromInput(item);
        }
    }

    public bool HasInitialState()
    {
        return _initialState.Count > 0;
    }

    public void SimulateStep()
    {
        _stopwatch.Restart();
        
        _cellsToRemove = new List<Vector3Int>();
        _currentCells = _cells.Keys.ToList();

        foreach (Vector3Int item in _currentCells)
        {
            _surroundingPositions = _neighborCalculator.GetSurroundingPositions(item);

            foreach (Vector3Int jtem in _surroundingPositions)
            {
                if (_cells.TryGetValue(jtem, out int[] life))
                {
                    _cellLiveHandler.AddLife(life);
                    continue;
                }

                _cellLiveHandler.CallCell();
                life = _cellLiveHandler.GetCell();
                _cells.Add(jtem, life);
            }
        }
        
        foreach (KeyValuePair<Vector3Int, int[]> item in _cells)
        {
            if (!_cellLiveHandler.IsCellAlive(item.Value))
            {
                RemoveCell(item.Key);
                _cellLiveHandler.IndeterminateCell(item.Value);
                _cellLiveHandler.ReleaseCell(item.Value);
                _cellsToRemove.Add(item.Key);
                continue;
            }

            if (!_cellLiveHandler.WasCellAlive(item.Value))
            {
                AddCell(item.Key);
            }

            _cellLiveHandler.AliveCell(item.Value);
        }

        foreach (Vector3Int item in _cellsToRemove)
        {
            _cells.Remove(item);
        }

        _stopwatch.Stop();
       
        if (_stopwatch.Elapsed.TotalSeconds >= 0.5)
        {
            Debug.Log($"Too many cells:");
        }
    }
}