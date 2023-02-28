using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using Mirror;
using Unity.AI.Navigation;

public class MazeGeneration : NetworkBehaviour
{
    public GameObject p_mazeCell;
    public GameObject p_enemy;
    public GameObject p_healing;
    public GameObject p_rifle;
    public GameObject p_exit;
    public GameObject mazeParent;
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
        if (isServer)
        { 
            PlaceCells();
            BreakWalls();
            SpawnItems();
        }
    }

    private void SpawnItems()
    {
        int enemyCount = Random.Range(1, 6);
        int healingItemsCount = Random.Range(0, 3);
        int rifleProbability = Random.Range(1, 101);
        int exitLocation = Random.Range(0, mazeSize + 1);
        {
            Vector3 pos = new Vector3(exitLocation * 10, 0, (mazeSize - 1) * 10);
            GameObject exit = Instantiate(p_exit, pos, Quaternion.identity);
            NetworkServer.Spawn(exit);
        }
        for (int e = 0; e < enemyCount; e++)
        {
            int x = Random.Range(0, mazeSize);
            int z = Random.Range(0, mazeSize);
            Vector3 pos = new Vector3(x * 10, 0, z * 10);
            GameObject newEnemy = Instantiate(p_enemy, pos, Quaternion.identity);
            NetworkServer.Spawn(newEnemy);

        }
        for (int h = 0; h < healingItemsCount; h++)
        {
            int x = Random.Range(0, mazeSize);
            int z = Random.Range(0, mazeSize);
            Vector3 pos = new Vector3(x * 10, 0, z * 10);
            GameObject newHealth = Instantiate(p_healing, pos, Quaternion.identity);
            NetworkServer.Spawn(newHealth);
        }
        if (rifleProbability < 25)
        {
            int x = Random.Range(0, mazeSize);
            int z = Random.Range(0, mazeSize);
            Vector3 pos = new Vector3(x * 10, 0, z * 10);
            GameObject newRifle = Instantiate(p_rifle, pos, Quaternion.identity);
            NetworkServer.Spawn(newRifle);
        }
    }

    private void PlaceCells()
    {
        for (int x = 0; x < mazeSize; x++)
        {
            for (int z = 0; z < mazeSize; z++)
            {
                Vector3 position = new Vector3((x * 10) - 5, 0, (z * 10) - 5);
                GameObject cell = Instantiate(p_mazeCell, position, Quaternion.identity);
                cell.transform.parent = mazeParent.transform;
                NetworkServer.Spawn(cell);
                maze[x,z] = cell;
            }
        }

        mazeParent.GetComponent<NavMeshSurface>().BuildNavMesh();
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
