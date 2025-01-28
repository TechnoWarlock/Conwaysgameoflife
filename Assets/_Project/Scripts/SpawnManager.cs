using UnityEditor;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private CellConfigurationSo hexCellConfiguration;
    [SerializeField] private CellConfigurationSo rectCellConfiguration;

    private PositionSender _positionSender;
    private SpawnTickHandler _spawnTickHandler;
    private CellPlacer _cellPlacer;
    private LifeSimulator _lifeSimulator;
    private CellLiveHandler _cellLiveHandler;
    private CellConfigurationSo _currentCellConfiguration;
    private CursorChanger _cursorChanger;
    private SoundTrigger _soundTrigger;
    private CameraHandler _cameraHandler;

    private const bool PoolCollectionCheck = false;
    private const int PoolDefaultCapacity = 320000;
    private const int PoolMaxSize = 520000;

    private bool _isReset;

    private void Awake()
    {
        Initialize();
        ChangeMode(false);
    }

    private void Initialize()
    {
        _positionSender = GetComponent<PositionSender>();
        _spawnTickHandler = GetComponent<SpawnTickHandler>();
        _soundTrigger = GetComponent<SoundTrigger>();
        _cellPlacer = GetComponent<CellPlacer>();
        _cameraHandler = GetComponent<CameraHandler>();
        _cursorChanger = new CursorChanger();

        _cellLiveHandler = new CellLiveHandler(PoolCollectionCheck, PoolDefaultCapacity, PoolMaxSize);
        _lifeSimulator = new LifeSimulator(_cellPlacer, _cellLiveHandler);

        _positionSender.Initialize(_lifeSimulator, _cellPlacer, _soundTrigger);
        _spawnTickHandler.SetLifeSimulator(_lifeSimulator);
    }

    public void PlayAndPause(bool isPlay)
    {
        if (!isPlay)
        {
            _spawnTickHandler.StartTimer(false);
            return;
        }
        if (!VerifyBoardCells()) return;
        SaveState();
        _spawnTickHandler.StartTimer(isPlay);
    }

    private bool VerifyBoardCells()
    {
        if (!_lifeSimulator.HasCells())
        {
            GlobalData.SetPlay(this,false);
            return false;
        }
        GlobalData.SetPlay(this,true);
        return true;
    }
    private void SaveState()
    {
        if (_isReset)
        {
            _isReset = false;
            _lifeSimulator.SaveInitialState();
            _cameraHandler.SavePosition();
        }
    }

    public void ChangeSpeed(float speed)
    {
        _spawnTickHandler.SetSpeed(speed);
    }

    public void ResetBoard()
    {
        _isReset = true;
        _spawnTickHandler.Reset();
        if (!_lifeSimulator.HasInitialState()) return;
        _cameraHandler.ResetPosition();
        _lifeSimulator.PutInitialState();
    }

    public void CleanBoard()
    {
        _isReset = true;
        _spawnTickHandler.Reset();
        if (!VerifyBoardCells()) return;
        _lifeSimulator.Clear();
        _cameraHandler.ReturnInitialPosition();
    }

    public void StepByStep()
    {
        _spawnTickHandler.Reset();
        if (!VerifyBoardCells()) return;
        SaveState();
        _lifeSimulator.SimulateStep();
    }

    public void ChangeMode(bool isHexagon)
    {
        CleanBoard();
        switch (isHexagon)
        {
            case true:
                _lifeSimulator.SetSurroundPositions(new SurroundingHexPositions());
                _currentCellConfiguration = hexCellConfiguration;
                break;
            case false:
                _lifeSimulator.SetSurroundPositions(new SurroundingRectPositions());
                _currentCellConfiguration = rectCellConfiguration;
                break;
        }

        _cellLiveHandler.CellConfiguration = _currentCellConfiguration;
        _cursorChanger.ChangeCursorIcon(_currentCellConfiguration.Icon);
        _cellPlacer.ChooseMode(isHexagon, _currentCellConfiguration);
    }

}