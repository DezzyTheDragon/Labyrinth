using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 10;
    public float speed = 0.1f;
    public float lifeTime = 1f;

    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Life());
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(this.gameObject.transform.forward * speed * Time.deltaTime);
        Debug.DrawRay(this.gameObject.transform.position, this.gameObject.transform.forward);
        transform.position += direction * speed * Time.deltaTime;

        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position, transform.TransformDirection(Vector3.forward), out hit, 1))
        {
            if (hit.transform.tag != "Enemy" && hit.transform.tag != "Player")
            {
                Debug.Log("Destroyed on " + hit.transform.name);
                NetworkServer.Destroy(this.gameObject);
            }
        }
    }

    public void SetDirection(Vector3 dir)
    {
        
        direction = dir;
        transform.eulerAngles = new Vector3(0, GetAngle(dir), 0);
    }

    private float GetAngle(Vector3 vec)
    {
        float n = Mathf.Atan2(vec.x, vec.z) * Mathf.Rad2Deg;
        if (n < 0)
        {
            n += 360;
        }
        return n;
    }

    public IEnumerator Life()
    {
        yield return new WaitForSeconds(lifeTime);
        NetworkServer.Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            health.DamagePlayer(damage);
            NetworkServer.Destroy(this.gameObject);
        }
        else if(other.tag != "Enemy")
        {
            //Debug.Log("I hit " + other.name);
            //NetworkServer.Destroy(this.gameObject);
        }
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hello " + collision.transform.name);
    }*/
}
