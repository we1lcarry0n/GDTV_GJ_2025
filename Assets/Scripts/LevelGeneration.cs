using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using Random = System.Random;
using UnityEngine.InputSystem;

public class LevelGeneration : MonoBehaviour
{
    [SerializeField] private int _levelUpgradeLength;
    [SerializeField] private int _minChunkLenX;
    [SerializeField] private int _maxChunkLenX;
    [SerializeField] private TileBase _groundTile;
    [SerializeField] private Tilemap _tilemap;

    [SerializeField] private int _levelHeight;

    private Random _random;
    private int _currentXPosition = 20;
    private int _currentYPosition = 0;

    private void Start()
    {
        //_random = new Random();
    }

    [ContextMenu("Fill tilemap")]
    public void FillTestTilemap()
    {
        _random = new Random();
        _tilemap.ClearAllTiles();
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
            //GenerateUpgradeRoom
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

    private void MergeAllCameraConstraints()
    {

    }

    private void DecorateLevelChunk(int chunkLenX, int straightIndex)
    {
        // straight index here is 0 = str, 1 = upward, -1 = downward
        // take out the prefab from random
        // Get that prefab length
        // take out prefabs untill the chunk is decorated from left to right
    }

    private void GenerateStreightChunk(int chunkLenX)
    {
        for (int x = _currentXPosition; x < _currentXPosition + chunkLenX; x++)
        {
            for (int y = _currentYPosition; y < _currentYPosition + _levelHeight; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                _tilemap.SetTile(position, _groundTile);
            }
        }
        _currentXPosition += chunkLenX;
    }

    private void GenerateUpwardChunk(int chunkLenX)
    {
        GenerateStreightChunk(1);
        for (int x = _currentXPosition; x < _currentXPosition + chunkLenX - 2; x++)
        {
            for (int y = _currentYPosition; y < _currentYPosition + _levelHeight; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                _tilemap.SetTile(position, _groundTile);
            }
            _currentYPosition++;
        }
        _currentXPosition += chunkLenX - 2;
        GenerateStreightChunk(1);
    }

    private void GenerateDownwardChunk(int chunkLenX)
    {
        GenerateStreightChunk(1);
        for (int x = _currentXPosition; x < _currentXPosition + chunkLenX - 2; x++)
        {
            for (int y = _currentYPosition; y < _currentYPosition + _levelHeight; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                _tilemap.SetTile(position, _groundTile);
            }
            _currentYPosition--;
        }
        _currentXPosition += chunkLenX - 2;
        GenerateStreightChunk(1);
    }

    private void GenerateStartingChunk()
    {
        for (int x = -9; x < 20; x++)
        {
            for (int y = 0; y < _levelHeight; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                _tilemap.SetTile(position, _groundTile);
            }
        }
        _currentXPosition = 20;
        _currentYPosition = 0;
    }

    public int GetBoulderYPosition()
    {
        return _currentYPosition + (_levelHeight / 2);
    }
}
