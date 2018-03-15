using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGen : MonoBehaviour
{

    ///<Summery>
    ///The Maze script.
    ///Contains all methods to produce a perfect maze.
    ///</Summery>

    [System.Serializable]//Allows the class to be seen in the editor.
    public class Cells //Creates a public class Cell to with a bool and 4 game objects.
    {
        public bool m_isVisited = false;
        public GameObject m_North;       //Wall 1
        public GameObject m_East;        //Wall 2
        public GameObject m_West;        //Wall 3
        public GameObject m_South;       //Wall 4
    }

    //Public variables
    public GameObject Wall;
    public GameObject Floor;
    public int m_WallX = 5;
    public int m_WallY = 5;
    public int m_CurrentCell = 0;
    public float m_WallLength = 1.0f;   //Length of the cube's Z axis.

    //Private variables 
    private Vector3 m_StartPosition;
    private GameObject WallHolder;      //Creates a parent object to hold walls.
    private Cells[] m_Cells;            //refernce to the MazeCell class.
    private List<int> m_LastCell;
    private int m_TotalCells;
    private int m_CellsVisited = 0;     //Number of cells visited by the system
    private int m_CurrentNeighbour = 0; //Interger for the current neighbour
    private int m_BackUp = 0;
    private int m_BreakableWall = 0;
    private bool m_StartedBuild = false;

    //Start function to generate the maze
    void Start()
    {
        GenMaze();
    }

    //Method generates walls in order of rows then columns
    private void GenMaze()
    {
        Instantiate(Floor);             //Produces a ground plane

        WallHolder = new GameObject();
        WallHolder.name = "Grid";       //Used to keep the hierarchy clean andd hold all wall objects

        m_StartPosition = new Vector3((-m_WallX / 2) + m_WallLength / 2, 0.0f, (-m_WallY / 2) + m_WallLength / 2);//Assigns bottom right of the origin point as start point
        Vector3 m_CellPos = m_StartPosition;//Assigns the vector position of the start point to the cell position 
        GameObject tempWall;            //provides a temporary wall gameobject

        //Walls for X Axis / rows
        for (int i = 0; i < m_WallY; i++)//for less than the length of column variable
        {
            for (int j = 0; j <= m_WallX; j++)//for each wall of the row variable
            {
                m_CellPos = new Vector3(m_StartPosition.x + (j * m_WallLength) - m_WallLength / 2, 0.0f, m_StartPosition.z + (i * m_WallLength) - m_WallLength / 2); //Obtain as a new vector, the position of the starting cell and increment it by 1 to produce the walls.
                tempWall = Instantiate(Wall, m_CellPos, Quaternion.identity) as GameObject;//Spawn game object at cellPosition and Spawns the object with the original orientation
                if (j == 0)
                {
                    tempWall.tag = "OuterWestWall";
                }
                else if (j == m_WallY)
                {
                    tempWall.tag = "OuterEastWall";
                }
                tempWall.transform.parent = WallHolder.transform;//Assigns the wall as a child of the holder game object
            }
        }
        //Walls for Y Axis / columns
        for (int i = 0; i <= m_WallY; i++)//For each column (Y Axis)
        {
            for (int j = 0; j < m_WallX; j++)//for each row(X Axis)
            {
                m_CellPos = new Vector3(m_StartPosition.x + (j * m_WallLength), 0.0f, m_StartPosition.z + (i * m_WallLength) - m_WallLength);//Obtain as a new vector, the position of the starting cell with minus wall length to assign walls to the edge and increment it by 1 to produce the walls. 
                tempWall = Instantiate(Wall, m_CellPos, Quaternion.Euler(0.0f, 90.0f, 0.0f)) as GameObject;//Spawn wall as a new game object
                tempWall.transform.parent = WallHolder.transform;//Assigns the wall as a child of the holder game object
            }
        }
        InitialiseCells();                                      //After walls have been created, intitate cell creation from assigning North, East, South and West walls.
    }

    //Method assigns walls to a cell and which direction the wall is facing in comparison to the cell.
    void InitialiseCells()
    {
        m_LastCell = new List<int>();                           //Instatiate List of cells
        m_LastCell.Clear();                                     //Ensure the list is empty

        m_TotalCells = m_WallX * m_WallY;                       //Find the amount of total cells.
        GameObject[] mazeWalls;                                 //Array of walls within the maze.
        int m_wallChildren = WallHolder.transform.childCount;   //Counts the number of child objects within WallHolder.
        mazeWalls = new GameObject[m_wallChildren];             //Assigning Gameobjects to each Wall child within wallHolder.
        m_Cells = new Cells[m_WallX * m_WallY];                 //Provides a number of cells within the maze.
        int m_CellProcess = 0;                                  //variable of the active cell.
        int m_HorizontalWalls = 0;                              //Horizontal wall counter.
        int m_CellCount = 0;                                    //Used to check if the row has ended.                                  

        //Finds all children within Holder
        for (int i = 0; i < m_wallChildren; i++)
        {
            mazeWalls[i] = WallHolder.transform.GetChild(i).gameObject; // Assigns each child within Wall holder into mazeWall array as a seperate value.
        }

        //Place walls into cells
        for (int i = 0; i < m_Cells.Length; i++)
        {
            m_Cells[i] = new Cells();                           //Sets each cell within the maze to a new cell with each wall GameObject.
            m_Cells[i].m_East = mazeWalls[m_CellProcess];       //Assign each east wall to the cell
            m_Cells[i].m_South = mazeWalls[m_HorizontalWalls + (m_WallX + 1) * m_WallY];//Gets the first south facing wall after all vertical walls.
            if (m_CellCount == m_WallX)                         //last element on the X axis.
            {
                m_CellProcess += 2;                             //Skip a cell to make the edge wall.
                m_CellCount = 0;                                //Resets the cell count to signify a new row.
            }
            m_CellCount++;                                      //Increment the cell count meaning we have not reached the end of the row.
            m_HorizontalWalls++;                                //Increments 

            m_Cells[i].m_West = mazeWalls[m_CellProcess];       //Assigns west wall
            m_Cells[i].m_North = mazeWalls[(m_HorizontalWalls + (m_WallX + 1) * m_WallY) + m_WallX - 1];//Assigns north wall

        }
        InstatiateMaze();                                       //Calls InstantiateMaze method
    }

    //Method checks if the maze has been generated + Uses a DFS to design the maze
    void InstatiateMaze()
    {
        if (m_CellsVisited < m_TotalCells)                      //Checking that the created cells variable hasn't reach total cells
        {
            if (m_StartedBuild)                                 //If the maze has started building
            {
                FindCell();
                if(m_Cells[m_CurrentNeighbour].m_isVisited == false && m_Cells[m_CurrentCell].m_isVisited == true) //Checking if the neighbour hasn't been visited but the current cell has
                {
                    Debug.Log("Found Visited Cells");
                    DestroyWall();                              //Calls Destroy method
                    m_Cells[m_CurrentNeighbour].m_isVisited = true; //Setting the current neighbour to true
                    m_CellsVisited++;                               //Increase Cells visited
                    m_LastCell.Add(m_CurrentCell);
                    m_CurrentCell = m_CurrentNeighbour;

                    if (m_LastCell.Count > 0)
                    {
                        m_BackUp = m_LastCell.Count - 1;         //Top value of the Cell stack.
                    }
                }
            }
            else //If there is no starting cell
            {
                m_CurrentCell = Random.Range(0, m_TotalCells);  //Pick a starting cell at random
                //move a gameobject to starting cell here.
                //gameobject.transform.position = new Vector3(m_cells[m_CurrentCell].x, 0, m_cells[m_CurrentCell].z)
                m_Cells[m_CurrentCell].m_isVisited = true;      //Assign the current cell to visited
                m_CellsVisited++;                               //Increment visited cells
                m_StartedBuild = true;                          //Update Started bool to true
            }

            Invoke("InstatiateMaze", 0.5f);                     //Activating the maze
        }
    }

    void DestroyWall()
    {
        Debug.Log("Destroying Wall");
        switch (m_BreakableWall)
        {
            //Checks that you're not destroying an edge wall
            case 1 : Destroy(m_Cells[m_CurrentCell].m_North); break;    //Destroy north wall then breaks switch statement
            case 2 : Destroy(m_Cells[m_CurrentCell].m_East); break;     //Destroy east wall then breaks switch statement
            case 3 : Destroy(m_Cells[m_CurrentCell].m_West); break;     //Destroy west wall then breaks switch statement
            case 4 : Destroy(m_Cells[m_CurrentCell].m_South); break;    //Destroy south wall then breaks switch statement

        }
    }
    void FindCell()
    {
        int m_foundNeighbour = 0;
        int[] m_Neighbours = new int[4];                //Cell only has 4 directions (N,E,S,W).
        int m_Check = 0;
        int[] m_ConnectedWall = new int[4];             //Connecting walls to the cell
        m_Check = ((m_CurrentCell + 1) / m_WallX);      //Checking if the cell is in a corner
        m_Check -= 1;
        m_Check *= m_WallX;
        m_Check += m_WallX;

        //x + y * width = i
        //y = (i - x)/width
        //x = i - y*width

        //If Cell is on the west side
        if (m_CurrentCell + 1 < m_TotalCells && (m_CurrentCell + 1) != m_Check)
        {
            if (m_Cells[m_CurrentCell + 1].m_isVisited == false)//Checking if the cell has not been visited 
            {
                m_Neighbours[m_foundNeighbour] = m_CurrentCell + 1; //Increase the total of visited cells
                m_ConnectedWall[m_foundNeighbour] = 3;              //Assigning connecting wall West
                m_foundNeighbour++;
            }
        }
        //East side
        if (m_CurrentCell - 1 >= 0 && m_CurrentCell != m_Check)
        {
            if (m_Cells[m_CurrentCell - 1].m_isVisited == false)//Checking if the cell has not been visited 
            {
                m_Neighbours[m_foundNeighbour] = m_CurrentCell - 1;
                m_ConnectedWall[m_foundNeighbour] = 2;
                m_foundNeighbour++;
            }
        }

        //North side
        if (m_CurrentCell + m_WallX < m_TotalCells)
        {
            if (m_Cells[m_CurrentCell + m_WallX].m_isVisited == false)//Checking if the cell has not been visited 
            {
                m_Neighbours[m_foundNeighbour] = m_CurrentCell + m_WallX; //Increase the total of visited cells
                m_ConnectedWall[m_foundNeighbour] = 1;
                m_foundNeighbour++;
            }
        }
        //South side
        if (m_CurrentCell - m_WallX >= 0)
        {
            if (m_Cells[m_CurrentCell - m_WallX].m_isVisited == false)//Checking if the cell has not been visited 
            {
                m_Neighbours[m_foundNeighbour] = m_CurrentCell - m_WallX; //Increase the total of visited cells
                m_ConnectedWall[m_foundNeighbour] = 4;
                m_foundNeighbour++;
            }
        }
 
        if (m_foundNeighbour != 0)
        {
            int m_StartingCell = Random.Range(0, m_foundNeighbour);  //Givs the method a random neighbour
            m_CurrentNeighbour = m_Neighbours[m_StartingCell];       //Assigns the current neighbour to the new starting neighbour
            m_BreakableWall = m_ConnectedWall[m_StartingCell];
        }
        else
        {
            if (m_BackUp > 0)
            {
                m_CurrentCell = m_LastCell[m_BackUp];
                m_BackUp--;
            }
        }        

    }

}


