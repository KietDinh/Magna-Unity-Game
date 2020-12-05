using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour
{
    public float speed = 4;
    public int grenadeDamage = 1;
    public Vector3 launchOffset;
    public bool thrown;

    [SerializeField] GameObject exploParticle;
    [SerializeField] private AudioClip bombExplodeSFX;


    // Start is called before the first frame update
    void Start()
    {
        if (thrown)
        {
            var direction = transform.right + Vector3.up;
            GetComponent<Rigidbody2D>().AddForce(direction * speed, ForceMode2D.Impulse);
        }
        transform.Translate(launchOffset);

        Destroy(gameObject, 5); //Destroy automatically after 5 seconds
    }

    // Update is called once per frame
    void Update()
    {
        if (!thrown)
        {
            transform.position += -transform.right * speed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Ground"))
        {
            AudioSource.PlayClipAtPoint(bombExplodeSFX, transform.position);
            var enemy = collision.collider.GetComponent<EnemyController>();
            //var boss = collision.collider.GetComponent<Boss>();
            if (enemy != null)
            {
                enemy.TakeDamage(this.grenadeDamage);
            }
            /*
            if (enemy != null)
            {
                enemy.TakeDamage(this.grenadeDamage);
            }
            */
            
            GameObject spawnedParticle = Instantiate(exploParticle, transform.position, transform.rotation);
            Destroy(spawnedParticle, 1);
            Destroy(gameObject);
        }
        
    }

    public void explode()
    {
        AudioSource.PlayClipAtPoint(bombExplodeSFX, transform.position);
        GameObject spawnedParticle = Instantiate(exploParticle, transform.position, transform.rotation);
        Destroy(spawnedParticle, 1);
        Destroy(gameObject);
    }

}
