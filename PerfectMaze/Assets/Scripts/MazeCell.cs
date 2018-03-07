using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour {

    //Bool to check if a cell has been visited
    public bool m_isVisited = false;
    //reference each game object
    public GameObject northWall, southWall, eastWall, wastWall, floor; 

}
