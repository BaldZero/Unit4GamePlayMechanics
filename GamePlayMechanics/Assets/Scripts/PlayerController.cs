using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float hangTime;
    public float smashSpeed;
    public float explosionForce;
    public float explosionRadius;
    public float speed = 5.0f;

    private Rigidbody playerRb;
    private GameObject focalPoint;
    private float powerupStrength = 15f;
    private GameObject tmpRocket;
    private Coroutine powerupCountdown;

    public bool hasPowerup;
    bool smashing = false;
    float floorY;
    public PowerUp.PowerUpType currentPowerup = PowerUp.PowerUpType.None;
    

    public GameObject rocketPrefab;

    public GameObject powerupIndicator;

    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("FocalPoint");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
        powerupIndicator.transform.Rotate(Vector3.up, 150 * Time.deltaTime);

        if(currentPowerup == PowerUp.PowerUpType.Rockets && Input.GetKeyDown(KeyCode.F))
        {
            LaunchRockets();
        }

        if(currentPowerup == PowerUp.PowerUpType.Slam && Input.GetKeyDown(KeyCode.Space) && !smashing)
        {
            smashing = true;
            StartCoroutine(Slam());

        }

        // destroys the player if they fall off
        if (transform.position.y < -10)
        {
            gameManager.gameOver = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //checks to see if the object is a powerup
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            currentPowerup = other.gameObject.GetComponent<PowerUp>().powerUpType;

            powerupIndicator.SetActive(true);
            Destroy(other.gameObject);
            if(powerupCountdown != null)
            {
                StopCoroutine(powerupCountdown);
            }
            StartCoroutine(PowerupCounttdownRoutine());
        }

    }

    IEnumerator PowerupCounttdownRoutine()
    {
        yield return new WaitForSeconds(7);
        powerupIndicator.SetActive(false);
        currentPowerup = PowerUp.PowerUpType.None;
        hasPowerup = false;
    }

    IEnumerator Slam()
    {
        var enemies = FindObjectsOfType<Enemy>();

        //Store the y position before jumping up
        floorY = transform.position.y;

        //Calculate the amount of time we are in air
        float jumpTime = Time.time + hangTime; 

        while(Time.time < jumpTime)
        {
            //move player up while keeping their x velocity
            playerRb.velocity = new Vector2(playerRb.velocity.x, smashSpeed);
            yield return null;
        }

        //Now move the player down
        while(transform.position.y > floorY)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, -smashSpeed * 2);
            yield return null;
        }

        //Go through all the enemies in scene
        for(int i = 0; i < enemies.Length; i++)
        {
            //apply a force originating from the players position
            if(enemies[i] != null)
            {
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 0.0f, ForceMode.Impulse); 

            }

        }
        smashing = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && currentPowerup == PowerUp.PowerUpType.Forceslam)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 pushPlayer = collision.gameObject.transform.position - transform.position;

            enemyRigidbody.AddForce(pushPlayer * powerupStrength, ForceMode.Impulse);
            Debug.Log("Collided with: " + collision.gameObject.name + " with powerup set to" + currentPowerup.ToString());

        }
    }

    void LaunchRockets()
    {
        foreach(var enemy in FindObjectsOfType<Enemy>())
        {
            tmpRocket = Instantiate(rocketPrefab, transform.position + Vector3.up, Quaternion.identity);
            tmpRocket.GetComponent<RocketBehavior>().Fire(enemy.transform);

        }
    }
}
