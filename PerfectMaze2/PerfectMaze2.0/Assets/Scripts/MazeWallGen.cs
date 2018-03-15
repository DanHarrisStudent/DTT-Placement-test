using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeWallGen : MonoBehaviour
{
    /// <summary>
    /// Script spawns walls
    /// </summary>
    /// 


    public GameObject Wall;
    public int wallWidth = 5;                             //Width of the maze
    public int wallLength = 5;                            //Length of the maze
    public GameObject[] CellsVertical;
    public GameObject[] CellsHorizontal;



    private GameObject WallHolderHorizontal;
    private GameObject WallHolderVertical;



    // Use this for initialization
    void Start()
    {
        GenerateGrid();
    }
    void GenerateGrid()
    {
        WallHolderHorizontal = new GameObject();
        WallHolderHorizontal.name = "Horizontal Maze Walls";

        WallHolderVertical = new GameObject();
        WallHolderVertical.name = "Vertical Maze Walls";

        GameObject tempWall;
        GameObject tempWall2;


        for (int x = 0; x <= wallWidth; x++)
        {
            for (int y = 0; y < wallLength; y++)
            {
                tempWall = Instantiate(Wall, new Vector3((x - 0.5f), 0.0f, y), Quaternion.identity);
                tempWall.transform.parent = WallHolderVertical.transform;
            }
            for (int i = 0; i < wallLength; i++)
            {
                for (int y = 0; y <= wallWidth; y++)
                {
                    tempWall2 = Instantiate(Wall, new Vector3(i, 0.0f, (y - 0.5f)), Quaternion.Euler(0.0f, 90.0f, 0.0f));
                    tempWall2.transform.parent = WallHolderHorizontal.transform;

                }
            }
        }
    }
}
