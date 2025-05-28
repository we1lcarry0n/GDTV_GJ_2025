using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using Random = System.Random;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField] private int _levelUpgradeLength;
    [SerializeField] private int _minChunkLenX;
    [SerializeField] private int _maxChunkLenX;
    [SerializeField] private TileBase _groundTile;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileBase _confinerTileUpper;
    [SerializeField] private TileBase _confinerTileLower;
    [SerializeField] private Tilemap _confinerTilemap;

    [SerializeField] private GameObject _colliderObject;

    [SerializeField] private int _levelHeight;
    [SerializeField] private GameObject _upgradePoint;
    [SerializeField] private GameObject _boulderStart;
    [SerializeField] private GameObject _boulderEnd;
    [SerializeField] private GameObject _endGamePoint;

    [SerializeField] private Transform _gridTransform;
    [SerializeField] private GameObject[] _straight3Len;
    [SerializeField] private GameObject[] _straight4Len;
    [SerializeField] private GameObject[] _straight5Len;
    [SerializeField] private GameObject[] _straight6Len;
    [SerializeField] private GameObject[] _straight8Len;
    [SerializeField] private GameObject[] _straight10Len;

    [SerializeField] private GameObject[] _upwardDecorations;
    [SerializeField] private GameObject[] _downwardDecorations;

    [SerializeField] private GameObject _upperEnemy;
    [SerializeField] private GameObject _lowerEnemy;

    private Random _random;
    private int _currentXPosition = 20;
    private int _currentYPosition = 0;

    private void Start()
    {
        FillTestTilemap();
    }

    [ContextMenu("Fill tilemap")]
    public void FillTestTilemap()
    {
        _random = new Random();
        _tilemap.ClearAllTiles();
        _confinerTilemap.ClearAllTiles();
        GenerateStartingChunk();
        for (int i = 0; i < 3; i++)
        {
            int _totalLength = 0;
            while (_totalLength < _levelUpgradeLength)
            {
                if (_totalLength == 0)
                {
                    int chunkLen = _random.Next(_minChunkLenX, _maxChunkLenX);
                    _totalLength += chunkLen;
                    GenerateStreightChunk(chunkLen);
                }
                if (_levelUpgradeLength - _totalLength > _maxChunkLenX)
                {
                    int chunkLen = _random.Next(_minChunkLenX, _maxChunkLenX);
                    _totalLength += chunkLen;
                    GenerateLevelChunk(chunkLen);
                }
                else
                {
                    int endLengthX = _levelUpgradeLength - _totalLength;
                    _totalLength += endLengthX;
                    GenerateStreightChunk(endLengthX);
                }
            }
            if (i == 2)
            {
                GenerateUpgradeRoom(true);
                return;
            }
            GenerateUpgradeRoom(false);
            _totalLength = 0;
        }
    }

    private void GenerateLevelChunk(int chunkLenX)
    {
        int coef = _random.Next(0, 6);
        if (coef == 3)
        {
            GenerateDownwardChunk(chunkLenX);
            return;
        }
        if (coef == 2)
        {
            GenerateUpwardChunk(chunkLenX);
            return;
        }
        GenerateStreightChunk(chunkLenX);
    }

    private void GenerateUpgradeRoom(bool isFinal)
    {
        GenerateColliderBounds(_currentXPosition, _currentXPosition + 6, _currentYPosition, _currentYPosition + _levelHeight);
        for (int x = _currentXPosition; x < _currentXPosition + 6; x++)
        {
            if (x == _currentXPosition + 2)
            {
                if (isFinal)
                {
                    Instantiate(_endGamePoint, new Vector3(_currentXPosition, _currentYPosition, 0), Quaternion.identity);
                }
                else
                {
                    Instantiate(_upgradePoint, new Vector3(_currentXPosition, _currentYPosition, 0), Quaternion.identity);
                }
            }
            Vector3Int lowerPosition = new Vector3Int(x, _currentYPosition - 2, 0);
            _confinerTilemap.SetTile(lowerPosition, _confinerTileLower);
            Vector3Int lowerPosition1 = new Vector3Int(x, _currentYPosition - 3, 0);
            _confinerTilemap.SetTile(lowerPosition1, _confinerTileLower);
            for (int y = _currentYPosition - 1; y < _currentYPosition + _levelHeight + 1; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                _tilemap.SetTile(position, _groundTile);
                _confinerTilemap.SetTile(position, _confinerTileLower);
            }
            Vector3Int upperPosition0 = new Vector3Int(x, _currentYPosition + _levelHeight, 0);
            _confinerTilemap.SetTile(upperPosition0, _confinerTileUpper);
            Vector3Int upperPosition = new Vector3Int(x, _currentYPosition + _levelHeight + 1, 0);
            _confinerTilemap.SetTile(upperPosition, _confinerTileUpper);
            Vector3Int upperPosition1 = new Vector3Int(x, _currentYPosition + _levelHeight + 2, 0);
            _confinerTilemap.SetTile(upperPosition1, _confinerTileUpper);
        }
        _currentXPosition += 6;
    }

    private void GenerateDecoration(GameObject[] decorationToPick, int currentLenX, int currentLenY)
    {
        int index = _random.Next(0, decorationToPick.Length - 1);
        GameObject decoration = Instantiate(decorationToPick[index], Vector3.zero, Quaternion.identity);
        float decorationOffsetX = decoration.GetComponent<Decoration>().XOffset;
        decoration.transform.position = decoration.transform.position + new Vector3(currentLenX - decorationOffsetX, currentLenY, 0);
        decoration.transform.SetParent(_gridTransform, true);
    }

    private void DecorateStraightChunk(int chunkLenX)
    {
        // With some chance generate a boulder prefab trigger on start and end of chunk
        int appliedLen = 0;
        while (appliedLen < chunkLenX - 1)
        {
            if (chunkLenX - appliedLen - 1 >= 10)
            {
                int randomIndex = _random.Next(0, 5);
                if (randomIndex == 4)
                {
                    GenerateDecoration(_straight10Len, _currentXPosition + appliedLen, _currentYPosition);
                    appliedLen += 10;
                }
                if (randomIndex == 3)
                {
                    GenerateDecoration(_straight8Len, _currentXPosition + appliedLen, _currentYPosition);
                    appliedLen += 8;
                }
                if (randomIndex == 0)
                {
                    GenerateDecoration(_straight5Len, _currentXPosition + appliedLen, _currentYPosition);
                    appliedLen += 5;
                }
                if (randomIndex == 1)
                {
                    GenerateDecoration(_straight4Len, _currentXPosition + appliedLen, _currentYPosition);
                    appliedLen += 4;
                }
                if (randomIndex == 2)
                {
                    GenerateDecoration(_straight3Len, _currentXPosition + appliedLen, _currentYPosition);
                    appliedLen += 3;
                }
                return;
            }
            if (chunkLenX - appliedLen - 1 >= 8)
            {
                int randomIndex = _random.Next(0, 4);
                if (randomIndex == 3)
                {
                    GenerateDecoration(_straight8Len, _currentXPosition + appliedLen, _currentYPosition);
                    appliedLen += 8;
                }
                if (randomIndex == 0)
                {
                    GenerateDecoration(_straight5Len, _currentXPosition + appliedLen, _currentYPosition);
                    appliedLen += 5;
                }
                if (randomIndex == 1)
                {
                    GenerateDecoration(_straight4Len, _currentXPosition + appliedLen, _currentYPosition);
                    appliedLen += 4;
                }
                if (randomIndex == 2)
                {
                    GenerateDecoration(_straight3Len, _currentXPosition + appliedLen, _currentYPosition);
                    appliedLen += 3;
                }
                return;
            }
            if (chunkLenX - appliedLen - 1 >= 5)
            {
                int randomIndex = _random.Next(0, 3);
                if (randomIndex == 0)
                {
                    GenerateDecoration(_straight5Len, _currentXPosition + appliedLen, _currentYPosition);
                    appliedLen += 5;
                }
                if (randomIndex == 1)
                {
                    GenerateDecoration(_straight4Len, _currentXPosition + appliedLen, _currentYPosition);
                    appliedLen += 4;
                }
                if (randomIndex == 2)
                {
                    GenerateDecoration(_straight3Len, _currentXPosition + appliedLen, _currentYPosition);
                    appliedLen += 3;
                }
                return;
            }
            if (chunkLenX - appliedLen - 1 >= 4)
            {
                int randomIndex = _random.Next(1, 3);
                if (randomIndex == 1)
                {
                    GenerateDecoration(_straight4Len, _currentXPosition + appliedLen, _currentYPosition);
                    appliedLen += 4;
                }
                if (randomIndex == 2)
                {
                    GenerateDecoration(_straight3Len, _currentXPosition + appliedLen, _currentYPosition);
                    appliedLen += 3;
                }
                return;
            }
            if (chunkLenX - appliedLen - 1 >= 3)
            {
                int randomIndex = _random.Next(1, 3);
                if (randomIndex == 1)
                {
                    return;
                }
                if (randomIndex == 2)
                {
                    GenerateDecoration(_straight3Len, _currentXPosition + appliedLen, _currentYPosition);
                    appliedLen += 3;
                }
                return;
            }
        }
    }

    private void DecorateUpwardChunk(int chunkLenX)
    {
        // SIMPLY ADD TWO RANDOM 3-LENGTH DECORATIONS
        GenerateDecoration(_upwardDecorations, _currentXPosition, _currentYPosition);
    }

    private void DecorateDownwardChunk(int chunkLenX)
    {
        // SIMPLY ADD TWO RANDOM 3-LENGTH DECORATIONS
        GenerateDecoration(_downwardDecorations, _currentXPosition, _currentYPosition);
    }

    private void GenerateStreightChunk(int chunkLenX)
    {
        DecorateStraightChunk(chunkLenX);
        GenerateEnemies(chunkLenX, _currentXPosition, _currentYPosition);
        GenerateColliderBounds(_currentXPosition, _currentXPosition + chunkLenX, _currentYPosition, _currentYPosition + _levelHeight);
        for (int x = _currentXPosition; x < _currentXPosition + chunkLenX; x++)
        {
            Vector3Int lowerPosition = new Vector3Int(x, _currentYPosition - 1, 0);
            _confinerTilemap.SetTile(lowerPosition, _confinerTileLower);
            Vector3Int lowerPosition1 = new Vector3Int(x, _currentYPosition - 2, 0);
            _confinerTilemap.SetTile(lowerPosition1, _confinerTileLower);
            for (int y = _currentYPosition; y < _currentYPosition + _levelHeight; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                _tilemap.SetTile(position, _groundTile);
                _confinerTilemap.SetTile(position, _confinerTileLower);
            }
            Vector3Int upperPosition0 = new Vector3Int(x, _currentYPosition + _levelHeight - 1, 0);
            _confinerTilemap.SetTile(upperPosition0, _confinerTileUpper);
            Vector3Int upperPosition = new Vector3Int(x, _currentYPosition + _levelHeight, 0);
            _confinerTilemap.SetTile(upperPosition, _confinerTileUpper);
            Vector3Int upperPosition1 = new Vector3Int(x, _currentYPosition + _levelHeight + 1, 0);
            _confinerTilemap.SetTile(upperPosition1, _confinerTileUpper);
        }
        _currentXPosition += chunkLenX;
    }

    private void GenerateUpwardChunk(int chunkLenX)
    {
        GenerateCurvedUPEnemy(chunkLenX, _currentXPosition, _currentYPosition);
        DecorateUpwardChunk(chunkLenX);
        GenerateCurvedColliderBounds(_currentXPosition + 1, _currentXPosition + chunkLenX - 1, _currentYPosition + chunkLenX - 2, _currentYPosition + chunkLenX + _levelHeight - 2);
        GenerateStreightChunk(1);
        for (int x = _currentXPosition; x < _currentXPosition + chunkLenX - 2; x++)
        {
            Vector3Int lowerPosition = new Vector3Int(x, _currentYPosition - 1, 0);
            _confinerTilemap.SetTile(lowerPosition, _confinerTileLower);
            Vector3Int lowerPosition1 = new Vector3Int(x, _currentYPosition - 2, 0);
            _confinerTilemap.SetTile(lowerPosition1, _confinerTileLower);
            for (int y = _currentYPosition; y < _currentYPosition + _levelHeight; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                _tilemap.SetTile(position, _groundTile);
                _confinerTilemap.SetTile(position, _confinerTileLower);
            }
            Vector3Int upperPosition0 = new Vector3Int(x, _currentYPosition + _levelHeight - 1, 0);
            _confinerTilemap.SetTile(upperPosition0, _confinerTileUpper);
            Vector3Int upperPosition = new Vector3Int(x, _currentYPosition + _levelHeight, 0);
            _confinerTilemap.SetTile(upperPosition, _confinerTileUpper);
            Vector3Int upperPosition1 = new Vector3Int(x, _currentYPosition + _levelHeight + 1, 0);
            _confinerTilemap.SetTile(upperPosition1, _confinerTileUpper);
            _currentYPosition++;
        }
        _currentXPosition += chunkLenX - 2;
        GenerateStreightChunk(1);
    }

    private void GenerateDownwardChunk(int chunkLenX)
    {
        GenerateCurvedDOWNEnemy(chunkLenX, _currentXPosition, _currentYPosition);
        DecorateDownwardChunk(chunkLenX);
        GenerateCurvedColliderBounds(_currentXPosition + 1, _currentXPosition + chunkLenX - 1, _currentYPosition - chunkLenX + 2, _currentYPosition - chunkLenX + _levelHeight + 2);
        GenerateStreightChunk(1);
        for (int x = _currentXPosition; x < _currentXPosition + chunkLenX - 2; x++)
        {
            Vector3Int lowerPosition = new Vector3Int(x, _currentYPosition - 1, 0);
            _confinerTilemap.SetTile(lowerPosition, _confinerTileLower);
            Vector3Int lowerPosition1 = new Vector3Int(x, _currentYPosition - 2, 0);
            _confinerTilemap.SetTile(lowerPosition1, _confinerTileLower);
            for (int y = _currentYPosition; y < _currentYPosition + _levelHeight; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                _tilemap.SetTile(position, _groundTile);
                _confinerTilemap.SetTile(position, _confinerTileLower);
            }
            Vector3Int upperPosition0 = new Vector3Int(x, _currentYPosition + _levelHeight - 1, 0);
            _confinerTilemap.SetTile(upperPosition0, _confinerTileUpper);
            Vector3Int upperPosition = new Vector3Int(x, _currentYPosition + _levelHeight, 0);
            _confinerTilemap.SetTile(upperPosition, _confinerTileUpper);
            Vector3Int upperPosition1 = new Vector3Int(x, _currentYPosition + _levelHeight + 1, 0);
            _confinerTilemap.SetTile(upperPosition1, _confinerTileUpper);
            _currentYPosition--;
        }
        _currentXPosition += chunkLenX - 2;
        GenerateStreightChunk(1);
    }

    private void GenerateCurvedUPEnemy(int chunkLen, int currentXPos, int currentYPos)
    {
        int randomUpIndex = _random.Next(0, 2);
        int randomDownIndex = _random.Next(0, 2);
        if (randomUpIndex == 0)
        {
            Instantiate(_upperEnemy, new Vector3(currentXPos + chunkLen/2, currentYPos + _levelHeight + chunkLen/2, 0), Quaternion.identity);
        }
        if (randomDownIndex == 0) 
        {
            Instantiate(_lowerEnemy, new Vector3(currentXPos + chunkLen/2, currentYPos + chunkLen/2, 0), Quaternion.identity);
        }
    }

    private void GenerateCurvedDOWNEnemy(int chunkLen, int currentXPos, int currentYPos)
    {
        int randomUpIndex = _random.Next(0, 3);
        int randomDownIndex = _random.Next(0, 3);
        if (randomUpIndex == 0)
        {
            Instantiate(_upperEnemy, new Vector3(currentXPos + chunkLen / 2, currentYPos + _levelHeight - chunkLen / 2, 0), Quaternion.identity);
        }
        if (randomDownIndex == 0)
        {
            Instantiate(_lowerEnemy, new Vector3(currentXPos + chunkLen / 2, currentYPos - chunkLen / 2, 0), Quaternion.identity);
        }
    }

    private void GenerateEnemies(int chunkLenX, int currentXPos, int currentYPos)
    {
        if (chunkLenX < 7)
        {
            return;
        }
        int boulderProbability = _random.Next(0, 3);
        if (boulderProbability == 0)
        {
            Instantiate(_boulderStart, new Vector3(currentXPos, currentYPos, 0), Quaternion.identity);
            Instantiate(_boulderEnd, new Vector3(currentXPos + chunkLenX, currentYPos, 0), Quaternion.identity);
            return;
        }
        int upperEnemies = _random.Next(0, 2);
        int lowerEnemies = _random.Next(0, 3);
        int upperOffset = 1;
        int lowerOffset = 2;
        for (int i = 0; i < upperEnemies; i++)
        {
            Instantiate(_upperEnemy, new Vector3(currentXPos + upperOffset, currentYPos + _levelHeight, 0), Quaternion.identity);
            upperOffset += chunkLenX / upperEnemies;
        }
        for (int i = 0; i < lowerEnemies; i++)
        {
            Instantiate(_lowerEnemy, new Vector3(currentXPos + lowerOffset, currentYPos, 0), Quaternion.identity);
            lowerOffset += chunkLenX / lowerEnemies;
        }
    }

    private void GenerateStartingChunk()
    {
        _currentYPosition = 0;
        GenerateColliderBounds(-9, 20, _currentYPosition, _currentYPosition + _levelHeight);
        for (int x = -9; x < 20; x++)
        {
            Vector3Int lowerPosition = new Vector3Int(x, _currentYPosition - 1, 0);
            _confinerTilemap.SetTile(lowerPosition, _confinerTileLower);
            Vector3Int lowerPosition1 = new Vector3Int(x, _currentYPosition - 2, 0);
            _confinerTilemap.SetTile(lowerPosition1, _confinerTileLower);
            for (int y = 0; y < _levelHeight; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                _tilemap.SetTile(position, _groundTile);
                _confinerTilemap.SetTile(position, _confinerTileLower);
            }
            Vector3Int upperPosition0 = new Vector3Int(x, _currentYPosition + _levelHeight - 1, 0);
            _confinerTilemap.SetTile(upperPosition0, _confinerTileUpper);
            Vector3Int upperPosition = new Vector3Int(x, _currentYPosition + _levelHeight, 0);
            _confinerTilemap.SetTile(upperPosition, _confinerTileUpper);
            Vector3Int upperPosition1 = new Vector3Int(x, _currentYPosition + _levelHeight + 1, 0);
            _confinerTilemap.SetTile(upperPosition1, _confinerTileUpper);
        }
        _currentXPosition = 20;
    }

    private void GenerateColliderBounds(int xStart, int xEnd, int  yStart, int yEnd)
    {
        GameObject colliderChild = new GameObject("child");
        colliderChild.transform.parent = _colliderObject.transform;
        PolygonCollider2D collider = colliderChild.AddComponent<PolygonCollider2D>();
        Vector2[] path = new Vector2[4];
        path[0] = new Vector2(xStart, yStart);
        path[1] = new Vector2(xEnd, yStart);
        path[2] = new Vector2(xEnd, yEnd);
        path[3] = new Vector2(xStart, yEnd);
        collider.pathCount = 1;
        collider.SetPath(0, path);
        collider.compositeOperation = Collider2D.CompositeOperation.Merge;
    }

    private void GenerateCurvedColliderBounds(int xStart, int xEnd, int yEnd1, int yEnd2)
    {
        GameObject colliderChild = new GameObject("child");
        colliderChild.transform.parent = _colliderObject.transform;
        PolygonCollider2D collider = colliderChild.AddComponent<PolygonCollider2D>();
        Vector2[] path = new Vector2[4];
        path[0] = new Vector2(xStart, _currentYPosition);
        path[1] = new Vector2(xEnd, yEnd1);
        path[2] = new Vector2(xEnd, yEnd2);
        path[3] = new Vector2(xStart, _currentYPosition + _levelHeight);
        collider.pathCount = 1;
        collider.SetPath(0, path);
        collider.compositeOperation = Collider2D.CompositeOperation.Merge;
    }

    public int GetBoulderYPosition()
    {
        return _currentYPosition + (_levelHeight / 2);
    }
}
