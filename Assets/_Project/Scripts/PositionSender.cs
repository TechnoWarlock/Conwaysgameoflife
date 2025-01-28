using UnityEngine;

public class PositionSender : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    private Camera _cam;
    private LifeSimulator _lifeSimulator;
    private CellPlacer _cellPlacer;
    private SoundTrigger _soundTrigger;

    private Vector3Int _lastPreviewPosition = Vector3Int.zero;
    private Vector3Int _previewPosition;
    private Vector2 _mousePosition;
    private Vector3 _mouseWorldPosition;
    private Vector3 _previewCellPosition;

    public void Initialize(LifeSimulator lifeSimulator, CellPlacer cellPlacer, SoundTrigger soundTrigger)
    {
        _lifeSimulator = lifeSimulator;
        _cellPlacer = cellPlacer;
        _soundTrigger = soundTrigger;
        _cam = Camera.main;
    }

    private void OnEnable()
    {
        inputReader.OnPointerPress += PlaceObject;
    }
    
    private void OnDisable()
    {
        inputReader.OnPointerPress -= PlaceObject;
    }

    void Update()
    {
        if (inputReader.IsPointerOnUI)
        {
            ClearPreviewCell();
            return;
        }

        UpdatePreviewPosition();
        UpdatePreviewCell();
    }

    private void PlaceObject(Vector2 position)
    {
        GetTileMapPosition(position);
        _lifeSimulator.AddCellFromInput(_previewPosition);
    }

    private void UpdatePreviewPosition()
    {
        _mousePosition = inputReader.PointerPosition;
       GetTileMapPosition(_mousePosition);
    }

    private void GetTileMapPosition(Vector3 position)
    {
        _mouseWorldPosition = _cam.ScreenToWorldPoint(position);
        _mouseWorldPosition.z = _cellPlacer.GetTileMap().transform.position.z;
        _previewPosition = _cellPlacer.GetTileMap().WorldToCell(_mouseWorldPosition);
    }
    private void UpdatePreviewCell()
    {
        if (_lastPreviewPosition != _previewPosition)
        {
            ClearPreviewCell();
            _cellPlacer.PutPreviewCell(_previewPosition);
            SoundTrigger.PreviewCellClip();
            _lastPreviewPosition = _previewPosition;
        }
    }

    private void ClearPreviewCell()
    {
        _cellPlacer.RemovePreviewCell(_lastPreviewPosition);
    }
}