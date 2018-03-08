using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGen : MonoBehaviour
{
    
    //Variables for the Generation script
    public GameObject Wall;
    public GameObject Floor;
    public int m_WallX = 5;
    public int m_WallY = 5;
    public float m_WallLength = 1.0f;//Length of the cube z axis

    private Vector3 m_StartPosition;
    private GameObject WallHolder;//Creates a parent object to hold walls
    private MazeCell[] m_Cells;//refernce to the MazeCell class

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
                tempWall = Instantiate(Wall,m_CellPos,Quaternion.identity) as GameObject;//Spawn game object
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
        GameObject[] allWalls;//Array of walls within the maze
        int m_wallChildren = WallHolder.transform.childCount;//Counts the number of child objects within WallHolder 

        //Finds all children within Holder
        for (int i = 0; i < m_wallChildren; i++)
        {

            allWalls[i] = WallHolder.transform.GetChild(i).gameObject;
        }
    }

}


