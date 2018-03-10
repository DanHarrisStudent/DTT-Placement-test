using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGen : MonoBehaviour
{
    [System.Serializable]//Allow the class to be seen in the editor
    public class Cells
    {
        public bool m_isVisited = false;
        public GameObject m_North;
        public GameObject m_East;
        public GameObject m_West;
        public GameObject m_South;
    }

    //Variables for the Generation script
    public GameObject Wall;
    public GameObject Floor;
    public int m_WallX = 5;
    public int m_WallY = 5;
    public float m_WallLength = 1.0f;//Length of the cube z axis
    public int m_CurrentCell = 0;

    private Vector3 m_StartPosition;
    private GameObject WallHolder;//Creates a parent object to hold walls
    private Cells[] m_Cells;//refernce to the MazeCell class
    private int m_TotalCells;

    void Start()
    {
        GenMaze();
    }
    private void GenMaze()
    {
        Instantiate(Floor);
        WallHolder = new GameObject();
        WallHolder.name = "Grid";

        m_StartPosition = new Vector3((-m_WallX / 2) + m_WallLength / 2, 0.0f, (-m_WallY / 2) + m_WallLength / 2);
        Vector3 m_CellPos = m_StartPosition;
        GameObject tempWall;

        //Walls for X Axis
        for (int i = 0; i < m_WallY; i++)//For each row (x Axis)
        {
            for (int j = 0; j <= m_WallX; j++)//for each column(y Axis)
            {
                m_CellPos = new Vector3(m_StartPosition.x + (j * m_WallLength) - m_WallLength / 2, 0.0f, m_StartPosition.z + (i * m_WallLength) - m_WallLength / 2);
                tempWall = Instantiate(Wall, m_CellPos, Quaternion.identity) as GameObject;//Spawn game object
                tempWall.transform.parent = WallHolder.transform;
            }
        }
        //Walls for Y Axis
        for (int i = 0; i <= m_WallY; i++)//For each row (x Axis)
        {
            for (int j = 0; j < m_WallX; j++)//for each column(y Axis)
            {
                m_CellPos = new Vector3(m_StartPosition.x + (j * m_WallLength), 0.0f, m_StartPosition.z + (i * m_WallLength) - m_WallLength);
                tempWall = Instantiate(Wall, m_CellPos, Quaternion.Euler(0.0f, 90.0f, 0.0f)) as GameObject;//Spawn wall as a new game object
                tempWall.transform.parent = WallHolder.transform;
            }
        }
        InitialiseCells();
    }

    void InitialiseCells()
    {
        GameObject[] mazeWalls;//Array of walls within the maze.
        int m_wallChildren = WallHolder.transform.childCount;//Counts the number of child objects within WallHolder.
        mazeWalls = new GameObject[m_wallChildren];//Assigning Gameobject mazeWall the length of wallChildren.
        m_Cells = new Cells[m_WallX * m_WallY];//Provides a number of cells within the maze.
        int m_CellProcess = 0;
        int m_HorizontalWalls = 0;//Horizontal wall counter
        int m_CellCount = 0;

        //Finds all children within Holder
        for (int i = 0; i < m_wallChildren; i++)
        {
            mazeWalls[i] = WallHolder.transform.GetChild(i).gameObject;
        }

        //Place walls into cells
        for (int i = 0; i < m_Cells.Length; i++)
        {
            m_Cells[i] = new Cells();//Sets each cell within the maze to a new cell with each wall GameObject.
            m_Cells[i].m_East = mazeWalls[m_CellProcess];//Assign each west wall to the cell
            m_Cells[i].m_South = mazeWalls[m_HorizontalWalls + (m_WallX + 1) * m_WallY];//Gets the first south facing wall after all vertical walls.

            if (m_CellCount == m_WallX)//last element on the Xaxis
            {
                m_CellProcess += 2;//Skip a cell to make the edge wall 
                m_CellCount = 0;
            }
            m_CellCount++;
            m_HorizontalWalls++;

            m_Cells[i].m_West = mazeWalls[m_CellProcess];//Assigns west wall
            m_Cells[i].m_North = mazeWalls[(m_HorizontalWalls + (m_WallX + 1) * m_WallY) + m_WallX - 1];//Assigns north wall

        }
        InstatiateMaze();
    }

    void InstatiateMaze()
    {
        FindCell();
    }
    void FindCell()
    {
        m_TotalCells = m_WallX * m_WallY;
        int m_foundNeighbour = 0;
        int[] m_Neighbours = new int[4];//Cell only has 4 directions (N,E,S,W).
        int m_Check = 0;
        m_Check = ((m_CurrentCell + 1) / m_WallX);//Checking if the cell is in a corner
        m_Check -= 1;
        m_Check *= m_WallX;
        m_Check += m_WallX;

        //If Cell is on the west
        if (m_CurrentCell + 1 < m_TotalCells && (m_CurrentCell+1) != m_Check)
        {
            if(m_Cells[m_CurrentCell +1].m_isVisited == false)//Checking if the cell has not been visited 
            {
                m_Neighbours[m_foundNeighbour] = m_CurrentCell + 1; //Increase the total of visited cells

                m_foundNeighbour++;
            }
        }
        //East Side
        if (m_CurrentCell - 1 >= 0 && m_CurrentCell != m_Check)
        {
            if (m_Cells[m_CurrentCell - 1].m_isVisited == false)//Checking if the cell has not been visited 
            {
                m_Neighbours[m_foundNeighbour] = m_CurrentCell - 1; //Increase the total of visited cells

                m_foundNeighbour++;
            }
        }
        //North side
        if (m_CurrentCell + m_WallX < m_TotalCells)
        {
            if (m_Cells[m_CurrentCell + m_WallX].m_isVisited == false)//Checking if the cell has not been visited 
            {
                m_Neighbours[m_foundNeighbour] = m_CurrentCell + m_WallX; //Increase the total of visited cells

                m_foundNeighbour++;
            }
        }

        if (m_CurrentCell - m_WallX >= 0)
        {
            if (m_Cells[m_CurrentCell - m_WallX].m_isVisited == false)//Checking if the cell has not been visited 
            {
                m_Neighbours[m_foundNeighbour] = m_CurrentCell - m_WallX; //Increase the total of visited cells

                m_foundNeighbour++;
            }
        }
        for (int i = 0; i < m_foundNeighbour; i++)
        {
            Debug.Log(m_Neighbours[i]);

        }

    }

}


