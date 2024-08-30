using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed = 5.0f;

    private Rigidbody playerRb;
    private GameObject focalPoint;
    private float powerupStrength = 12f;

    public bool hasPowerup;

    public GameObject powerupIndicator;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("FocalPoint");
    }

    // Update is called once per frame
    void Update()
    {
        //movement
        float verticalInput = Input.GetAxis("Vertical");
        //moves the player around based on the focal point
        playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed);

        float horizontalInput = Input.GetAxis("Horizontal");
        //same here
        playerRb.AddForce(focalPoint.transform.right * horizontalInput * speed);

        //sets the position of the powerup to follow the player
        powerupIndicator.transform.position = transform.position + new Vector3(0f, -0.3f, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        //checks to see if the object is a powerup
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            powerupIndicator.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerupCounttdownRoutine());
        }
    }

    IEnumerator PowerupCounttdownRoutine()
    {
        yield return new WaitForSeconds(7);
        powerupIndicator.SetActive(false);
        hasPowerup = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 pushPlayer = collision.gameObject.transform.position - transform.position;

            enemyRigidbody.AddForce(pushPlayer * powerupStrength, ForceMode.Impulse);
            Debug.Log("Collided witth: " + collision.gameObject.name + " with powerup set to" + hasPowerup);

        }
    }
}
