using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { NORTH, SOUTH, EAST, WEST }

public class MazeCell : MonoBehaviour
{
    [Header("Walls Objects")]
    public GameObject northWall;
    public GameObject southWall;
    public GameObject eastWall;
    public GameObject westWall;

    private bool touched = false;

    public bool GetTouched()
    {
        return touched;
    }

    public void HideWall(Direction dir)
    {
        switch (dir)
        {
            case Direction.NORTH:
                northWall.SetActive(false);
                break;
            case Direction.SOUTH:
                southWall.SetActive(false);
                break;
            case Direction.EAST:
                eastWall.SetActive(false);
                break;
            case Direction.WEST:
                westWall.SetActive(false);
                break;
        }
        touched = true;
    }
}
