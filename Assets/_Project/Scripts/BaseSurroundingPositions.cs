using UnityEngine;

public abstract class BaseSurroundingPositions : ISurroundingPositions
{
    protected readonly Vector3Int[] _offsets;
    protected Vector3Int[] _result;

    protected BaseSurroundingPositions(int offsetsSize, int resultSize)
    {
        _offsets = new Vector3Int[offsetsSize];
        _result = new Vector3Int[resultSize];
        SetOffsets();
    }
    
    protected abstract void SetOffsets();

    public abstract Vector3Int[] GetSurroundingPositions(Vector3Int cellPosition);


}