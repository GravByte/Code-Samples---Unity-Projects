using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    //----------- Public&Serialised Variables
    [Header("Settings")]
    [SerializeField]
    bool _showLogs;

    [SerializeField]
    string _Prefix;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject[] _tilePrefabs;

    [SerializeField]
    private int numTilesOnScreen = 6;

    [SerializeField]
    private List<GameObject> _activeTiles;

    //----------- Private variables
    private Transform _playerTransform;

    private float _zOffset = -20.0f;
    private readonly float _tileLength = 20f;
    private readonly float _safeZone = 30.0f;
    private int lastPrefabIndex = 0;



    //----------- Start is called before the first frame update
    private void Start()
    {
        //Disable logger if  not debug build
        Debug.unityLogger.logEnabled = Debug.isDebugBuild;

        //----------- spawn and delete tiles
        _activeTiles = new List<GameObject>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        //----------- Determines how many tiles are on screen and spawns new ones when there are less than the preset number.
        //----------- First 3 tiles are blank to cover camera movement time.
        for (int i = 0; i < numTilesOnScreen; i++)
        {
            if (i < 3)
                SpawnTile(0);
            else
                SpawnTile();
        }

    }

    private void Update()
    {
        //----------- Adds tiles to end while deleting tile behind player
        if (_playerTransform.position.z - _safeZone > (_zOffset - numTilesOnScreen * _tileLength))
        {
            SpawnTile();
            DeleteTile();
        }
    }

    //----------- Instantiates new tile gameobjects in an array
    private void SpawnTile(int prefabIndex = -1)
    {
        GameObject go;
        if (prefabIndex == -1)
            go = Instantiate(_tilePrefabs[RandomPrefabIndex()]) as GameObject;
        else
            go = Instantiate(_tilePrefabs [prefabIndex]) as GameObject;

        go.transform.SetParent(transform);
        go.transform.position = Vector3.forward * _zOffset;
        _zOffset += _tileLength;
        _activeTiles.Add(go);
    }

    //----------- Removes tiles no longer needed
    private void DeleteTile()
    {
        Destroy(_activeTiles[0]);
        _activeTiles.RemoveAt(0);
    }

    //----------- Randomises selected tile prefabs from the prefab array index
    private int RandomPrefabIndex()
    {
        if (_tilePrefabs.Length <= 1)
            return 0;

        int randomIndex = lastPrefabIndex;
        while(randomIndex == lastPrefabIndex)
        {
            randomIndex = Random.Range(0, _tilePrefabs.Length);
        }

        lastPrefabIndex = randomIndex;
        return randomIndex;
    }

    //-------- Logging Control Method
    public void Log(object message)
    {
        if (_showLogs)
            Debug.Log(_Prefix + message);
    }

}
