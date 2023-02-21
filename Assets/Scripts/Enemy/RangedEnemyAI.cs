using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyAI : MonoBehaviour
{
    public float range = 1f;
    public float attackRange = 2f;
    public float cooldown = 2f;
    public GameObject projectilePrefab;
    public GameObject shootSpawn;

    private NavMeshAgent navAgent;
    private GameObject targetPlayer = null;
    private float lastFired = 0f;
    

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //Check if there is a player in range
        if (targetPlayer != null)
        {
            Move();
            Attack();
        }
    }

    //Handle the movment of the enemy
    private void Move()
    {
        //The enemy can not see through walls so a ray is cast from the enemy to the player
        //  to check if there is something in the way
        bool inSight = false;
        Vector3 direction = targetPlayer.transform.position - transform.position;
        Vector3 castPos = transform.position;
        castPos.y = 1;
        RaycastHit hit;
        if (Physics.Raycast(castPos, direction, out hit, 10f))
        {
            if (hit.transform.tag == "Player")
            {
                inSight = true;
            }
            else
            {
                Debug.Log(hit.transform.name + " is in the way!");
            }
        }
        if (inSight)
        {
            //SetDestination moves the enemy to that position and as far as I can tell there is
            //  no version that has a range so to avoid having the enemy move right on top of
            //  the player alter the destination 
            float xOffset;
            float zOffset;
            if (direction.x > 0)
            {
                xOffset = range;
            }
            else
            {
                xOffset = -range;
            }
            if (direction.z > 0)
            {
                zOffset = range;
            }
            else
            {
                zOffset = -range;
            }
            Vector3 destination = new Vector3(targetPlayer.transform.position.x - xOffset, targetPlayer.transform.position.y, targetPlayer.transform.position.z - zOffset);
            navAgent.SetDestination(destination);
            Vector3 target = targetPlayer.transform.position;
            this.transform.LookAt(target);
            Debug.DrawRay(this.transform.position, this.transform.forward);
        }
    }

    private void Attack()
    {
        Vector3 direction = targetPlayer.transform.position - transform.position;
        Vector3 target = targetPlayer.transform.position;
        target.y = 1;
        shootSpawn.transform.LookAt(target);
        Debug.DrawRay(shootSpawn.transform.position, shootSpawn.transform.forward);
        if (Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.z, 2)) < attackRange)
        {
            if (lastFired <= Time.time)
            {
                lastFired = Time.time + cooldown;
                GameObject projectile = Instantiate(projectilePrefab, shootSpawn.transform.position, shootSpawn.transform.rotation);
                projectile.GetComponent<Projectile>().SetDirection(direction.normalized);
                NetworkServer.Spawn(projectile);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            targetPlayer = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        targetPlayer = null;
    }
}
