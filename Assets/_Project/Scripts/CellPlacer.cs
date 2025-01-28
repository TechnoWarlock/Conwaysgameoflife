using UnityEngine;
using UnityEngine.Tilemaps;

public class CellPlacer : MonoBehaviour
{
    [SerializeField] private Tilemap rectTileMap;
    [SerializeField] private Tilemap previewRectTileMap;
    [SerializeField] private Tilemap hexTildeMap;
    [SerializeField] private Tilemap previewHexTildeMap;

    private Tilemap _currentTileMap;
    private Tilemap _currentPreviewTileMap;
    private Tile[] _currentTiles;
    private Tile _currentPreviewTile;
    private int _index;
   
    public void ChooseMode(bool isHexagonal, CellConfigurationSo currentCellConfiguration)
    {
        (_currentTileMap, _currentPreviewTileMap, _currentTiles, _currentPreviewTile) = isHexagonal switch
        {
            true => (hexTildeMap, previewHexTildeMap, currentCellConfiguration.Tiles, currentCellConfiguration.PreviewTile),
            false => (rectTileMap, previewRectTileMap, currentCellConfiguration.Tiles, currentCellConfiguration.PreviewTile)
        };
    }

    
    
    public void PutCell(Vector3Int position)
    {
        _currentTileMap.SetTile(position,_currentTiles[_index]);
         _index = (_index + 1) % _currentTiles.Length;
    }
    
    public void ClearCells()
    {
        _currentTileMap.ClearAllTiles();
    }

    public void RemoveCell(Vector3Int position)
    {
        _currentTileMap.SetTile(position, null);
    }
    public void PutPreviewCell(Vector3Int position)
    {
        _currentPreviewTileMap.SetTile(position,_currentPreviewTile);
    }

    public void RemovePreviewCell(Vector3Int position)
    {
        _currentPreviewTileMap.SetTile(position,null);
    }

    public Tilemap GetTileMap()
    {
        return _currentTileMap;
    }
}