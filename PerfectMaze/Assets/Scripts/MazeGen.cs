using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGen : MonoBehaviour
{
    //Variables for the Generation script
    public GameObject Wall;
    public int m_WallX = 5;
    public int m_WallY = 5;
    public float m_MazeSize = 1.0f;//Length of the cube z axis

    private Vector3 m_StartPosition;
    private GameObject WallHolder;
    

    void Start()
    {
        GenMaze();
    }
    private void GenMaze()
    {
        WallHolder = new GameObject();
        WallHolder.name = "Grid";

        m_StartPosition = new Vector3((-m_WallX / 2) + m_MazeSize / 2, 0.0f, (-m_WallY / 2) + m_MazeSize / 2);
        Vector3 m_CellPos = m_StartPosition;
        GameObject tempWall;

        //Walls for X Axis
        for (int i = 0; i < m_WallY; i++)//For each row (x Axis)
        {
            for (int j = 0; j <= m_WallX; j++)//for each column(y Axis)
            {
                m_CellPos = new Vector3(m_StartPosition.x + (j * m_MazeSize) - m_MazeSize / 2, 0.0f, m_StartPosition.z + (i * m_MazeSize) - m_MazeSize / 2);
                tempWall = Instantiate(Wall,m_CellPos,Quaternion.identity) as GameObject;//Spawn game object
                tempWall.transform.parent = WallHolder.transform;
            }
        }
        //Walls for Y Axis
        for (int i = 0; i <= m_WallY; i++)//For each row (x Axis)
        {
            for (int j = 0; j < m_WallX; j++)//for each column(y Axis)
            {
                m_CellPos = new Vector3(m_StartPosition.x + (j * m_MazeSize), 0.0f, m_StartPosition.z + (i * m_MazeSize) - m_MazeSize);
                tempWall = Instantiate(Wall, m_CellPos, Quaternion.Euler(0.0f, 90.0f, 0.0f)) as GameObject;//Spawn wall as a new game object
                tempWall.transform.parent = WallHolder.transform;
            }
        }
    }

}


