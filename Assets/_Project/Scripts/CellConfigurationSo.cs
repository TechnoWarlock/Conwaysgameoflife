using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "CellConfiguration", menuName = "Cells/CellConfiguration")]
public class CellConfigurationSo : ScriptableObject
{
    [SerializeField][Tooltip("Take the value and add one")] private int maxSurvivalLife;
    [SerializeField][Tooltip("Take the value and add one")] private int minSurvivalLife;
    [SerializeField][Tooltip("Take the value, subtract one, and then multiply by minus one.")] private int birthLimit;

    [SerializeField] private Tile[] tiles;

    [SerializeField] private Tile previewTile;
    
    [SerializeField] private Texture2D icon;
    public int MaxSurvivalLife => maxSurvivalLife;

    public int MinSurvivalLife => minSurvivalLife;

    public int BirthLimit => birthLimit;

    public Tile[] Tiles => tiles;
    
    public Tile PreviewTile => previewTile;
    public Texture2D Icon => icon;
}