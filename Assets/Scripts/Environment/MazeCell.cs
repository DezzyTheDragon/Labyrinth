using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

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
                //NetworkServer.Destroy(northWall);
                break;
            case Direction.SOUTH:
                southWall.SetActive(false);
                //NetworkServer.Destroy(southWall);
                break;
            case Direction.EAST:
                eastWall.SetActive(false);
                //NetworkServer.Destroy(eastWall);
                break;
            case Direction.WEST:
                westWall.SetActive(false);
                //NetworkServer.Destroy(westWall);
                break;
        }
        touched = true;
    }
}
