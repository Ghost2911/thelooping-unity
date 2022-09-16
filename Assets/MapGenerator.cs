using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [HideInInspector]
    public static MapGenerator instance;
    public int tileSize = 48;
    public int mapSize = 10;
    public int bossCount = 3;
    public Slots slots;
    public bool isPrebuild = false;
    private int[,] arr;
    private List<Vector3Int> possibleBossTiles = new List<Vector3Int>();
    private Vector2Int mainTile;
    private float offset;
    public void Awake()
    {
        if (instance == null)
            instance = this;
        offset = -((mapSize+2)/2-0.5f)*tileSize;
        if (!isPrebuild)
        {
            GenerateMap();
            TilesInstantiate();
        }
    }

    public void Start()
    {
        if (!isPrebuild)
        {
            ReplaceCharacters();
        }
    }


    public void GenerateMap()
    {
        arr = new int[mapSize + 2, mapSize + 2];
        AddPath(ref arr);
        AddMainStructures(ref arr);
        AddForks(ref arr);
    }

    public void TilesInstantiate()
    {
        GameObject tile;
        for (int j = 0; j < mapSize + 2; j++)
        {
            for (int i = 0; i < mapSize + 2; i++)
            {
                tile = TileFromResources(arr[i, j]);
                Instantiate(tile, new Vector3(offset + tileSize * j, 0f, offset + tileSize * i), tile.transform.rotation, this.transform);
            }
        }
    }

    void Clear()
    {
        for (int i = 0; i < mapSize + 2; i++)
            for (int j = 0; j < mapSize + 2; j++)
                arr[i, j] = 0;
        foreach (Transform child in transform)
            GameObject.Destroy(child.gameObject);
    }

    void AddPath(ref int[,] arr)
    {
        int startPos, endPos, forkPos = 0;

        //MAIN PATH
        for (int i = 1; i < mapSize + 1; i += 2)
        {
            startPos = 1;
            endPos = mapSize;

            possibleBossTiles.Add(new Vector3Int(i, startPos, 4));
            possibleBossTiles.Add(new Vector3Int(i, endPos, 6));

            //horizontal path
            arr[i, startPos] = 4;
            arr[i, endPos] = 6;

            for (int j = startPos + 1; j < endPos; j++)
                arr[i, j] = 1;

            if (forkPos!=0)
                arr[i, forkPos] = 17;

            if (i + 2 < mapSize)
            {
                int[] list = Enumerable.Range(startPos + 1, endPos - 2).Where(a => a != forkPos).ToArray();
                forkPos = list[Random.Range(0, list.Length)];
                arr[i, forkPos] = 23;
                arr[i + 1, forkPos] = 3;
            }
        }

        //DARKNESS ZONE
        arr[0, 0] = -1;
        arr[mapSize + 1, mapSize + 1] = -9;
        arr[0, mapSize + 1] = -3;
        arr[mapSize + 1, 0] = -7;

        for (int i = 1; i < mapSize + 1; i++)
        {
            arr[i, 0] = -4;
            arr[i, mapSize + 1] = -6;
            arr[0, i] = -2;
            arr[mapSize + 1, i] = -8;
        }
    }

    void AddForks(ref int[,] arr)
    {
        try
        {
            for (int i = 1; i < mapSize; i++)
            {
                for (int j = 1; j < mapSize; j++)
                {
                    if (arr[i, j] == 0 && arr[i, j + 1] == 3 && Random.Range(0, 3) == 0)
                    {
                        arr[i, j] = 4;
                        arr[i, j + 1] = 19;
                    }
                    if (arr[i, j] == 3 && arr[i, j + 1] == 0 && Random.Range(0, 3) == 0)
                    {
                        arr[i, j] = 21;
                        arr[i, j + 1] = 6;
                    }
                    if (arr[i, j] == 0 && arr[i + 1, j] == 1 && Random.Range(0, 3) == 0)
                    {
                        arr[i, j] = 2;
                        arr[i + 1, j] = 17;
                    }
                    if (arr[i, j] == 1 && arr[i + 1, j] == 0 && Random.Range(0, 3) == 0)
                    {
                        arr[i, j] = 23;
                        arr[i + 1, j] = 8;
                        possibleBossTiles.Add(new Vector3Int(i + 1, j, 8));
                    }
                }
            }


            //Boss tiles replace fork
            List<int> spinNumbers = new List<int>();
            while (spinNumbers.Count < bossCount)
            {
                Vector3Int possibleTile = possibleBossTiles[Random.Range(0, possibleBossTiles.Count)];
                possibleBossTiles.Remove(possibleTile);
                Object[] maps = Resources.LoadAll("Tiles/Bosses/" + possibleTile.z).Where(a =>
                    !spinNumbers.Contains(System.Convert.ToInt32(a.name))).ToArray();

                if (maps.Length > 0)
                {
                    int tileNum = System.Convert.ToInt32(maps[Random.Range(0, maps.Length)].name);
                    spinNumbers.Add(tileNum);
                    arr[possibleTile.x, possibleTile.y] = tileNum;
                }
            }

            //Debug boss tiles - send to Spins
            slots.SetSpinResults(spinNumbers);
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public void ReplaceCharacters()
    {
        List<GameObject> spawns = GameObject.FindGameObjectsWithTag("SpawnHero").ToList();
        foreach (GameObject character in GlobalSettings.instance.characters)
        {
            int spawnNum = Random.Range(0, spawns.Count);
            character.transform.localPosition = spawns[spawnNum].transform.position;
            Debug.Log(spawns[spawnNum].transform.position);
            spawns.RemoveAt(spawnNum);
        }
    }

    public GameObject TileFromResources(int tileNum)
    {
        Object[] maps = (tileNum < 900) ? Resources.LoadAll("Tiles/" + tileNum) : Resources.LoadAll("Tiles/Bosses").Where(a => a.name == tileNum.ToString()).ToArray();
        int totalMapCount = maps.Length;
        return maps[Random.Range(0, totalMapCount)] as GameObject;
    }

    void AddMainStructures(ref int[,] arr)
    {
        List<Vector2Int> mainTilePos = new List<Vector2Int>();

        for (int i = 1; i < mapSize+2; i++)
            for (int j = 1; j < mapSize+2; j++)
            {
                if (arr[i, j] == 1 && arr[i, j + 1] == 1)
                     mainTilePos.Add(new Vector2Int(i, j));
                else if (arr[i, j] == 0 && Random.Range(0,5)>1)
                {
                    //добавить тайл для пустого места
                }
            }
        mainTile = mainTilePos[Random.Range(0,mainTilePos.Count)];
        arr[mainTile.x, mainTile.y] = 50;
        arr[mainTile.x, mainTile.y+1] = 51;
    }
}
