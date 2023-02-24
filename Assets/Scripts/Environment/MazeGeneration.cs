using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;



public class MazeGeneration : MonoBehaviour
{
    public GameObject p_mazeCell;
    public int mazeSize = 5;

    private GameObject[,] maze;
    private cellPos targetPos;
    private Queue<cellPos> history = new Queue<cellPos>();

    private struct cellPos
    {
        public int x;
        public int z;
    }

    void Start()
    {
        maze = new GameObject[mazeSize, mazeSize];
        targetPos.x = 0;
        targetPos.z = 0;
        PlaceCells();
        BreakWalls();
    }

    private void PlaceCells()
    {
        for (int x = 0; x < mazeSize; x++)
        {
            for (int z = 0; z < mazeSize; z++)
            {
                Vector3 position = new Vector3((x * 10), 0, (z * 10));
                GameObject cell = Instantiate(p_mazeCell, position, Quaternion.identity);
                maze[x,z] = cell;
            }
        }
    }

    private bool CheckCanMove()
    {
        bool ret = false;
        bool[] validDir = { false, false, false, false };
        if (targetPos.z + 1 < mazeSize)
        {
            if (!maze[targetPos.x, targetPos.z + 1].GetComponent<MazeCell>().GetTouched())
            {
                validDir[0] = true;
            }
        }
        if (targetPos.z - 1 >= 0)
        {
            if (!maze[targetPos.x, targetPos.z - 1].GetComponent<MazeCell>().GetTouched())
            {
                validDir[1] = true;
            }
        }
        if (targetPos.x + 1 < mazeSize)
        {
            if (!maze[targetPos.x + 1, targetPos.z].GetComponent<MazeCell>().GetTouched())
            {
                validDir[2] = true;
            }
        }
        if (targetPos.x - 1 >= 0)
        {
            if (!maze[targetPos.x - 1, targetPos.z].GetComponent<MazeCell>().GetTouched())
            {
                validDir[3] = true;
            }
        }

        if (validDir[0] || validDir[1] || validDir[2] || validDir[3])
        {
            ret = true;
        }
        return ret;
    }

    private void BreakWalls()
    {
        bool finished = false;
        int failsafeA = 0;
        int failsafeB = 0;
        while (!finished)
        {
            cellPos oldPos = targetPos;
            bool directionPicked = false;
            int dir;
            if (CheckCanMove())
            {
                while (!directionPicked)
                {
                    dir = Random.Range(0, 4);
                    //Debug.Log(dir);
                    switch (dir)
                    {
                        case 0:
                            if (targetPos.z + 1 < mazeSize && !maze[targetPos.x, targetPos.z + 1].GetComponent<MazeCell>().GetTouched())
                            {
                                directionPicked = true;
                                targetPos.z += 1;
                                maze[oldPos.x, oldPos.z].GetComponent<MazeCell>().HideWall(Direction.NORTH);
                                maze[targetPos.x, targetPos.z].GetComponent<MazeCell>().HideWall(Direction.SOUTH);
                                failsafeA = 0;
                            }
                            break;
                        case 1:
                            if (targetPos.z - 1 >= 0 && !maze[targetPos.x, targetPos.z - 1].GetComponent<MazeCell>().GetTouched())
                            {
                                directionPicked = true;
                                targetPos.z -= 1;
                                maze[oldPos.x, oldPos.z].GetComponent<MazeCell>().HideWall(Direction.SOUTH);
                                maze[targetPos.x, targetPos.z].GetComponent<MazeCell>().HideWall(Direction.NORTH);
                                failsafeA = 0;
                            }
                            break;
                        case 2:
                            if (targetPos.x + 1 < mazeSize && !maze[targetPos.x + 1, targetPos.z].GetComponent<MazeCell>().GetTouched())
                            {
                                directionPicked = true;
                                targetPos.x += 1;
                                maze[oldPos.x, oldPos.z].GetComponent<MazeCell>().HideWall(Direction.EAST);
                                maze[targetPos.x, targetPos.z].GetComponent<MazeCell>().HideWall(Direction.WEST);
                                failsafeA = 0;
                            }
                            break;
                        case 3:
                            if (targetPos.x - 1 >= 0 && !maze[targetPos.x - 1, targetPos.z].GetComponent<MazeCell>().GetTouched())
                            {
                                directionPicked = true;
                                targetPos.x -= 1;
                                maze[oldPos.x, oldPos.z].GetComponent<MazeCell>().HideWall(Direction.WEST);
                                maze[targetPos.x, targetPos.z].GetComponent<MazeCell>().HideWall(Direction.EAST);
                                failsafeA = 0;
                            }
                            break;
                    }
                    failsafeA++;
                    if (failsafeA > 50)
                    {
                        Debug.Log("Failed to pick a direction on " + targetPos.x + ", " + targetPos.z);
                        if (failsafeA > 100)
                        { 
                            return;
                        }
                        targetPos = history.Dequeue();
                        break;
                    }
                }
                history.Enqueue(oldPos);
            }
            else
            {
                //finished = true;
                
                if (history.Count > 0)
                {
                    targetPos = history.Dequeue();
                    failsafeB = 0;
                }
                else
                {
                    finished = true;
                }
                
            }
            failsafeB++;
            if (failsafeB > 100)
            {
                Debug.Log("Failed to fully generate maze");
                return;
            }
        }
    }
}
