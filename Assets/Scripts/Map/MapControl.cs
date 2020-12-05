﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class MapControl : Singleton<MapControl> {

    public GameObject GreenBackground;

    public Map map;
    public List<MapCell> StorageList;
    public GameObject building_storage;
    public GameObject player;
    public GameObject tombstone;
    //Pre Misa na talent
    public RSObjects[] Foragables= {RSObjects.Bush_Berry_Purple};
    CellObjectFactory COF;
    public int treeMaxCount=75;
    public int mushroomMaxCount=15;
    public int maxBerryCount = 8;
    public int toxicMaxFungiCount = 12;
    public int rockMaxCount = 40;
    public int ironMaxCount = 12;
    public int coalMaxCount = 8;
    private void Start() {
        COF = CellObjectFactory.Instance;
        GenerateWorld();
        //Ak chces daco navyse spawnut tak skus tu
    }
    public void GenerateWorld()
    {
        StorageList = new List<MapCell>();
        map = new Map(35, 22, 10f, new Vector3(0, 0));
        CreateGameObject(1, 1, player);
        CreateGameObject(1, 2, player);
        CreateGameObject(1, 3, player);
        CreateGameObject(4, 1, player);
        CreateGameObject(16, 12, building_storage);

        //Water 
        /*
        CreateGameObject(0, 0, COF.water[4]);
        CreateGameObject(map.GetWidth()-1, 0, COF.water[5]);
        CreateGameObject(map.GetWidth() - 1, map.GetHeight() - 1, COF.water[6]);
        CreateGameObject(0, map.GetHeight()-1, COF.water[7]);
        for (int i = 1; i < map.GetWidth()-1; i++)
        {
            CreateGameObject(i, 0, COF.water[0]);
            CreateGameObject(i, map.GetHeight()-1, COF.water[2]);
        }
        for(int i = 1; i < map.GetHeight()-1; i++)
        {
            CreateGameObject(0, i, COF.water[3]);
            CreateGameObject(map.GetWidth()-1, i, COF.water[1]);
        }
        */

        //Gravel around water
        for (int i = 0; i < map.GetWidth(); i++)
        {
            int r = Random.Range(0, 3);
            if (r <= 1)
                COF.ProduceBGlObject(i, 0, BGObjects.Gravel);
            r = Random.Range(0, 3);
            if (r <= 1)
                COF.ProduceBGlObject(i, map.GetHeight()-1, BGObjects.Gravel);
        }
        for (int i = 0; i < map.GetHeight(); i++)
        {
            int r = Random.Range(0, 3);
            if (r <= 1)
            {
                COF.ProduceBGlObject(0, i, BGObjects.Gravel);
            }
            r = Random.Range(0, 3);
            if (r <= 1)
            {
                COF.ProduceBGlObject(map.GetWidth() - 1, i, BGObjects.Gravel);
            }
        }
        //Lake
        int x, y;
        int lakexSize=3;
        int lakeySize = 3;
        x = map.GetWidth() - 9;
        y = map.GetHeight() - 7;
        if (x != -1 && y != -1)
        {
            CreateGameObject(x, y, COF.water[4]);
            CreateGameObject(x + lakexSize, y, COF.water[5]);
            CreateGameObject(x + lakexSize, y + lakeySize, COF.water[6]);
            CreateGameObject(x, y + lakeySize, COF.water[7]);
            for (int i = x + 1; i < x + lakexSize; i++)
            {
                CreateGameObject(i, y, COF.water[0]);
                CreateGameObject(i, y + lakeySize, COF.water[2]);
            }
            for (int i = y + 1; i < y + lakeySize; i++)
            {
                CreateGameObject(x, i, COF.water[3]);
                CreateGameObject(x + lakexSize, i, COF.water[1]);
            }
            for (int i = x + 1; i < x + lakexSize; i++)
            {
                for (int d = y + 1; d < y + lakeySize; d++)
                {

                    CreateGameObject(i, d, COF.water[8]);

                    /*int r = Random.Range(0, 3);
                    if (r >= 0)
                    {
                        COF.ProduceBGlObject(i, d, BGObjects.lekno);
                    }*/

                }
            }
        }
        //Forest
        x = 0;
        y = 0;
        int forestxSize= 15;
        int forestySize = 20;
        int treeCount = 0;
        int totalCount = 0;
        List<char> spots=new List<char>();
        for(int i = 0; i < Mathf.FloorToInt(treeMaxCount * 0.8f); i++)
        {
            spots.Add('f');
            totalCount++;
            treeCount++;
        }
        for (int i = 0; i < toxicMaxFungiCount; i++)
        {
            spots.Add('t');
            totalCount++;
        }
        for (int i = 0; i < mushroomMaxCount; i++)
        {
            spots.Add('m');
            totalCount++;
        }
        for(int i = 0; i < maxBerryCount; i++)
        {
            spots.Add('b');
            totalCount++;
        }
        for(int i = totalCount; i < forestxSize * forestySize; i++)
        {
            spots.Add('x');
        }
        Debug.Log("Spots" + spots.Count);
        x = 2;
        y = 2;
        if(x!=-1 && y != -1)
        {
            for (int i = x; i < forestxSize; i++)
            {
                for (int d = y; d < forestySize; d++)
                {
                    int randomIndex = Random.Range(0, spots.Count);
                    if (spots[randomIndex] == 'f')
                    {
                        COF.ProduceResourceSource(i, d, RSObjects.Forest);
                    }
                    if (spots[randomIndex] == 'm')
                    {
                        COF.ProduceResourceSource(i, d, RSObjects.Mushroom);
                    }
                    if (spots[randomIndex] == 't')
                    {
                        COF.ProduceResourceSource(i, d, RSObjects.ToxicMushroom);
                    }
                    if (spots[randomIndex] == 'b')
                    {
                        COF.ProduceResourceSource(i, d, RSObjects.Bush_Berry_Purple);
                    }
                    spots.RemoveAt(randomIndex);
                }
            }
        }
        
        while (treeCount <= treeMaxCount)
        {
            x = Random.Range(1, map.GetWidth() - 2);
            y= Random.Range(1, map.GetHeight() - 2);
            if (map.Grid[x][y].CanBeEnteredByObject(true))
            {
                treeCount++;
                COF.ProduceResourceSource(x, y, RSObjects.Forest);
            }
        }

        for (int i = 1; i < 16; i++)
        {
            for (int d = 1; d < 21; d++)
            {
                int r = Random.Range(0, 4);
                if (r <= 1)
                {
                    COF.ProduceBGlObject(i, d, BGObjects.Grass);
                }
                if (r == 2)
                {
                    COF.ProduceBGlObject(i, d, BGObjects.Grass1);
                }
            }
        }
        
    }
    public GameObject CreateGameObject(int x, int y, GameObject toBeCreatedGO)
    {
        if (map.IsInBounds(x,y))
        {
            CellObject CellObjectComponent = toBeCreatedGO.GetComponent<CellObject>();
            if (CellObjectComponent.CanEnterCell(map.Grid[x][y]))
            {
                GameObject g = Instantiate(toBeCreatedGO);
                //Debug.LogError("GameObject instantiated in MapControl");
                map.CenterObject(x, y, g);
                map.SetValue(x, y, g);
                return g;
            }
            else
            {
                Debug.LogWarning("Space x: " + x + " y: " + y + " is already occupied!");
                return null;
            }
        }
        Debug.LogWarning($"Instantiation failed in MapControl");
        return null;
    }

    private GameObject CreateGameObject(Vector3 worldPosition, GameObject toBeCreatedGO)
    {
        map.GetXY(worldPosition, out int x, out int y);
        return CreateGameObject(x, y, toBeCreatedGO);
    }
    public void FindPlaceToSpawn(int xsize, int ysize, int spotsNeeded, out int x, out int y)
    {
        int maxTries = 5;
        int currentTry = 0;
        int freespots;

        while (currentTry < maxTries)
        {
            freespots = 0;
            x = 0;
            x = Random.Range(x + 1, map.GetWidth() - 2 - xsize);
            y = 0;
            y = Random.Range(y +1, map.GetHeight() - 2 - ysize);

            for (int i = x; i < x + xsize; i++)
            {
                for(int d = y; d < y + ysize; d++)
                {
                    if (map.IsInBounds(i, d))
                    {
                        if (map.Grid[i][d].CanBeEnteredByObject(true))
                        {
                            freespots++;
                        }
                    }
                    else
                    {
                        Debug.Log("SOmehow you are outside of bounds");
                    }
                   
                }
            }
            if (freespots >= spotsNeeded)
            {
                return;
            }
            currentTry++;
        }
        x = -1;
        y = -1;
        return;
    }
}
