using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour {
    //this script is a class which creates cells


    //Bool to check if a cell has been visited
    public bool m_isVisited = false;
    //reference each game object
    public GameObject northWall, southWall, eastWall, wastWall; 

}
