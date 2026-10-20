using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Rigidbody playerRb;
    public float speed = 5.0f;
    private GameObject focalPoint;
    public GameObject powerupIndicator;
    public bool hasPowerup = false;
    private float powerUpStrength = 15.0f;

    private UIManager uiManagerScript;
    public AudioSource playerAudio;
    public AudioClip collisionSound;
    public AudioClip lossSound;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
        focalPoint = GameObject.Find("Focal Point");
        uiManagerScript = GameObject.Find("UIManager").GetComponent<UIManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!uiManagerScript.paused)
        {

            float forwardInput = Input.GetAxis("Vertical");

            playerRb.AddForce(focalPoint.transform.forward * forwardInput * speed);

            powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
            if (transform.position.y < -10)
            {
                playerAudio.PlayOneShot(lossSound, uiManagerScript.volume);
                uiManagerScript.UpdateScore();
                uiManagerScript.onSaveGamePress();
                uiManagerScript.onMenuPress();
            }
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup") && !hasPowerup)
        {
            Destroy(other.gameObject);
            hasPowerup = true;
            powerupIndicator.gameObject.SetActive(true);
            StartCoroutine(PowerupCountdownRoutine());
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (hasPowerup)
            {
                Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
                Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;
                enemyRigidbody.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Impulse);

            }
            playerAudio.PlayOneShot(collisionSound, uiManagerScript.volume);
        }
        
    }
}
