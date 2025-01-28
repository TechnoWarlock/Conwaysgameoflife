using UnityEngine;

public interface ISurroundingPositions
{
    Vector3Int[] GetSurroundingPositions(Vector3Int cellPosition);
}