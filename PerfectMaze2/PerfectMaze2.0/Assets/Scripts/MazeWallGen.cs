using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MazeWallGen : MonoBehaviour
{
    /// <summary>
    /// Script spawns walls
    /// </summary>
    /// 

    public InputField GetWidth;
    public InputField GetLength;
    public GameObject Wall;                               //Game object to hold Wall prefab
    public GameObject Floor;                              //Game object to hold Floor prefab
    public GameObject CellCentre;                         //Game object to hold Cell centre prefab
    public int wallWidth = 5;                             //Width of the maze
    public int wallLength = 5;                            //Length of the maze
    public GameObject[] CellsVertical;                    //Placeholder for vertical walls
    public GameObject[] CellsHorizontal;                  //Placeholder for horizontal walls

    private GameObject WallHolderHorizontal;              //Placeholder name
    private GameObject WallHolderVertical;              //Placeholder name
    private GameObject CellCubes;                        //Placeholder for cell cubes



    //Provides Maze script with User input
    public void ApplySize()
    {

        Destroy(WallHolderHorizontal);                    //Destroys vertical wall holder
        Destroy(WallHolderVertical);                      //Destroys horizontal wall holder
        Destroy(CellCubes);
       
        wallWidth = int.Parse(GetWidth.text);             //Assigns wallWidth with user input
        wallLength = int.Parse(GetLength.text);           //Assigns wallLength with user input

        Debug.Log(wallWidth + "" + wallLength);

        GenerateGrid();                                   //Calls Generate method
    }
    public void QuitApplicaiton()
    {
        Debug.Log("Application Quit");
        Application.Quit();
    }


    // Use this for initialization
    //void Start()
    //{
    //    GenerateGrid();
    //}

    /// <summary>
    /// Method produces a grid in rows and columns
    /// </summary>
    void GenerateGrid()
    {
        Instantiate(Floor);                             //Spawns floor prefab
        WallHolderHorizontal = new GameObject();        //New placeholder gameobject
        WallHolderHorizontal.name = "Horizontal Maze Walls";

        WallHolderVertical = new GameObject();           //Creates a new placeholder gameobject
        WallHolderVertical.name = "Vertical Maze Walls";

        CellCubes = new GameObject();
        CellCubes.name = "Cell Centre Cube";

        GameObject tempWall;                             //Creates a temp game object to hold walls
        GameObject tempWall2;                            //Creates a second temp game object to hold walls
        GameObject CenterOfCell;                         //Creates a centre of cell cube

        //For loop creates Vertical walls
        for (int x = 0; x <= wallLength; x++)
        {
            for (int y = 0; y < wallWidth; y++)
            {
                tempWall = Instantiate(Wall, new Vector3((x - 0.5f), 0.0f, y), Quaternion.identity) as GameObject;
                if (y == 0)
                {
                    tempWall.tag = "OuterWestWall";
                }
                else if (y == wallLength)
                {
                    Debug.Log("We are at east wall");
                    tempWall.tag = "OuterEastWall";
                }
                tempWall.transform.parent = WallHolderVertical.transform;
            }

        }

        //For loop creates horizontal walls
        for (int i = 0; i < wallLength; i++)
        {
            for (int y = 0; y <= wallWidth; y++)
            {
                tempWall2 = Instantiate(Wall, new Vector3(i, 0.0f, (y - 0.5f)), Quaternion.Euler(0.0f, 90.0f, 0.0f)) as GameObject;
                if (y == 0)
                {
                    tempWall2.tag = "OuterSouthWall";
                }
                else if (y == wallLength)
                {
                    tempWall2.tag = "OuterNorthWall";
                }
                tempWall2.transform.parent = WallHolderHorizontal.transform;

            }
        }
        //Spawn a cube to act as the cell centre
        for (int x = 0; x < wallLength; x++)
        {
            for (int y = 0; y < wallWidth; y++)
            {
                CenterOfCell = Instantiate(CellCentre, new Vector3(x, 0.0f, y), Quaternion.identity);
                CenterOfCell.transform.parent = CellCubes.transform;
            }
        }
    }
}

