using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 10;
    public float speed = 10f;
    public float lifeTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Life());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(this.gameObject.transform.forward * speed * Time.deltaTime);
        Debug.DrawRay(this.gameObject.transform.position, this.gameObject.transform.forward);
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
        }
        else 
        {
            NetworkServer.Destroy(this.gameObject);
        }
    }
}
