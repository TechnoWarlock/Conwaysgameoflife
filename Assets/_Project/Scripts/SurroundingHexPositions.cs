using UnityEngine;

public class SurroundingHexPositions : BaseSurroundingPositions
{
    public SurroundingHexPositions() : base(8, 6)
    {
    }
    
    protected override void SetOffsets()
    {
        int index = 0;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;
                _offsets[index++] = new Vector3Int(i, j, 0);
            }
        }
    }

    public override Vector3Int[] GetSurroundingPositions(Vector3Int cellPosition)
    {
        int index = 0;
        if (cellPosition.y % 2 == 0)
        {
            for (int i = 0; i < _offsets.Length; i++)
            {
                if (_offsets[i].x == 1 && _offsets[i].y == 1) continue;
                if (_offsets[i].x == 1 && _offsets[i].y == -1) continue;
                _result[index++] = new Vector3Int(cellPosition.x + _offsets[i].x, cellPosition.y + _offsets[i].y,
                    cellPosition.z + _offsets[i].z);
            }
            return _result;
        }
        for (int i = 0; i < _offsets.Length; i++)
        {
            if (_offsets[i].x == -1 && _offsets[i].y == 1) continue;
            if (_offsets[i].x == -1 && _offsets[i].y == -1) continue;
            _result[index++] = new Vector3Int(cellPosition.x + _offsets[i].x, cellPosition.y + _offsets[i].y,
                cellPosition.z + _offsets[i].z);
        }
        return _result;

       
    }
}