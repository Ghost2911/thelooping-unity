using System.Linq;
using System.Runtime;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int width = 16;
    public int height = 12;
    public int bossCount = 3;
    public Slots slots;
    private List<Vector3Int> possibleBossTiles = new List<Vector3Int>();
    public void Awake()
    {
        Create();
    }

    public void Create()
    {
        int[,] arr = new int[width, height];
        AddPath(ref arr);
        AddForks(ref arr);
        AddEvents(ref arr);

        for (int j = 0; j < width; j++)
        {
            for (int i = 0; i < height; i++)
            {
                GameObject tile = TileFromResources(arr[i, j]);
                Instantiate(tile, new Vector3(48 * j, 0f, 48 * i), tile.transform.rotation, this.transform);
            }
        }
    }

    void AddPath(ref int[,] arr)
    {
        int startPos, endPos, distBetweenHorizontal;
        int forkPos = Random.Range(2, width-2);

        for (int i = 0; i < height; i += distBetweenHorizontal)
        {
            distBetweenHorizontal = Random.Range(2, 3);
            startPos = Random.Range(0, forkPos-1)+1;
            endPos = Random.Range(forkPos+1, width)-1;

            possibleBossTiles.Add(new Vector3Int(i, startPos-1, 4));
            possibleBossTiles.Add(new Vector3Int(i, endPos+1, 6));

            //horizontal path
            arr[i, startPos-1] = 4;
            arr[i, endPos+1] = 6;

            for (int j = startPos; j <= endPos; j++)
                if (arr[i,j]==0)
                    arr[i, j] = 1;

            if (i + 2 < height)
            {
                int[] list = Enumerable.Range(startPos, endPos - startPos + 1).Where(a => a != forkPos).ToArray();
                if (list.Length != 0)
                {
                    Debug.Log(list);
                    forkPos = list[Random.Range(0, list.Length)];
                    arr[i + 1, forkPos] = 3;
                    arr[i, forkPos] = 23;
                    arr[i + 2, forkPos] = 17;
                }
            }
        }
    }

    void AddForks(ref int[,] arr)
    {
        for (int i = 0; i < (height-1); i++)
        {
            for (int j = 0; j < (width-1); j++)
            {
                if (arr[i, j] == 0 && arr[i, j + 1] == 3 && Random.Range(0, 3) == 0)
                {
                    arr[i, j] = 4;
                    arr[i, j+1] = 19;
                }
                if (arr[i, j] == 3 && arr[i, j + 1] == 0 && Random.Range(0, 3) == 0)
                {
                    arr[i, j] = 21;
                    arr[i, j+1] = 6;

                }
                if (arr[i, j] == 0 && arr[i + 1, j] == 1 && Random.Range(0, 3) == 0)
                {
                    arr[i, j] = 2;
                    arr[i+1, j] = 17;
                }
                if (arr[i, j] == 1 && arr[i + 1, j] == 0 && Random.Range(0, 3) == 0)
                {
                    arr[i, j] = 23;
                    arr[i+1, j] = 8;
                    possibleBossTiles.Add(new Vector3Int(i+1, j, 8));
                }
            }
        }

        //Boss tiles replace fork
        List<int> spinNumbers = new List<int>();
        while(spinNumbers.Count < bossCount)
        {
            Vector3Int possibleTile = possibleBossTiles[Random.Range(0,possibleBossTiles.Count)];
            possibleBossTiles.Remove(possibleTile);
            Object[] maps = Resources.LoadAll("Tiles/Bosses/" + possibleTile.z).Where(a => 
                !spinNumbers.Contains(System.Convert.ToInt32(a.name))).ToArray();

            if (maps.Length > 0)
            {
                int tileNum = System.Convert.ToInt32(maps[Random.Range(0, maps.Length)].name);
                spinNumbers.Add(tileNum);
                arr[possibleTile.x,possibleTile.y]=tileNum;
            }
        }

        //Debug boss tiles - send to Spins
        slots.SetSpinResults(spinNumbers);
    }

    public GameObject TileFromResources(int tileNum)
    {
        Object[] maps =  (tileNum<900)?Resources.LoadAll("Tiles/"+tileNum):
            Resources.LoadAll("Tiles/Bosses").Where(a => a.name == tileNum.ToString()).ToArray();
        int totalMapCount = maps.Length;
        Resources.UnloadUnusedAssets();

        return maps[Random.Range(0, totalMapCount)] as GameObject;
    }

    void AddEvents(ref int[,] arr)
    {
        for (int i = 0; i < height; i++)
            for (int j = 0; j < width; j++)
                if (arr[i, j] == 0 && Random.Range(0, 4) == 0)
                    arr[i, j] = 5;
    }
}
