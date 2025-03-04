using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private GameObject focalPoint;

    public float speed = 5.0f;
    public float jumbForce;
    public float shockwaveForce;

    public bool hasPowerup;
    private float powerupStrength = 15.0f;
    public GameObject powerupIndicator;
    public GameObject bullet;



    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);

        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup")) 
        {
            hasPowerup = true;
            powerupIndicator.gameObject.SetActive(true);
            
            

            if (other.gameObject.name.Equals("Bullet Powerup"))
            {
                StartCoroutine(BulletPowerupRoutine());
            }
            else if (other.gameObject.name.Equals("Jump Powerup"))
            {

            }
            else if (other.gameObject.name.Equals("Powerup"))
            {
                StartCoroutine(PowerupCountdownRoutine());
            }
            Destroy(other.gameObject);
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
    }

    IEnumerator BulletPowerupRoutine()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            Vector3 direction = (enemies[i].transform.position - transform.position).normalized;
            GameObject bullets = Instantiate(bullet, transform.position, enemies[i].transform.rotation);
            bullets.GetComponent<Rigidbody>().AddForce(direction * 500);
        }
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRigidBody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);

            Debug.Log("Collided with " + collision.gameObject.name + " with powerup set to " + hasPowerup);
            enemyRigidBody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }
    }

}
