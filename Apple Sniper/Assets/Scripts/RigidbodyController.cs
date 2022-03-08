using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RigidbodyController : MonoBehaviour
{
    [SerializeField] private float maxplayerHealth = 10f;
    public static float playerHealth=1f;
    private float healthBonus = 0.5f;

    private int score = 0;
    public static int scoreStreak = 0;
    [Space]

    private Vector3 movementInput;
    private Vector3 wishdir;
    private Vector2 lookInput;

    private float cameraVertical;
    private float cameraHorizontal;

    [SerializeField] private float sensitivity;
    [SerializeField] private float speed;
    [Space]
    [SerializeField] private Rigidbody body;
    [SerializeField] private Transform eyes;
    [Space]
    [SerializeField] private float minViewAngle = -60f, maxViewAngle = 60f;
    [Space]
    [SerializeField] private float reloadDelay = 1f;
    [Space]
    [SerializeField] private AudioClip shootSoundSource;
    [SerializeField] private AudioClip hitSoundSource;
    [SerializeField] private AudioSource audioSource;

    private bool isLoaded = true;
    private Apple hitObject;

    private float eyerot = 0;
    private float dt;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            sensitivity = PlayerPrefs.GetFloat("Sensitivity");
        }

        Cursor.lockState = CursorLockMode.Locked;

        playerHealth = maxplayerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.GameIsPaused)
        {
            return;
        }

        updateUIText();

        if (playerHealth <= 0)
        {
            AppleSniperGamemode.resetGame();
        }

        dt = Time.deltaTime;

        movementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        lookInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        wishdir = new Vector3(eyes.transform.TransformDirection(movementInput).x, 0f, eyes.transform.TransformDirection(movementInput).z);

        move();
        look();



        if (Input.GetAxis("Fire1") > 0.5f)
        {
            shoot();
        }
    }

    void move()
    {
        eyerot += 2f * body.velocity.magnitude * dt;
        eyerot %= 2f * Mathf.PI;
        body.velocity = new Vector3(wishdir.x * speed, body.velocity.y, wishdir.z * speed);
    }
    void look()
    {
        cameraVertical -= lookInput.y * sensitivity;
        cameraVertical = Mathf.Clamp(cameraVertical, minViewAngle, maxViewAngle);
        cameraHorizontal += lookInput.x * sensitivity;
        eyes.transform.localRotation = Quaternion.Euler(cameraVertical, cameraHorizontal, 2f * Mathf.Sin(eyerot));
    }
    void shoot()
    {
        RaycastHit shotHit;
        Physics.Raycast(eyes.position, eyes.transform.TransformDirection(Vector3.forward), out shotHit, Mathf.Infinity);
        Debug.DrawRay(eyes.position, eyes.transform.TransformDirection(Vector3.forward) * shotHit.distance, Color.yellow);

        if (isLoaded)
        {
            isLoaded = false;
            Invoke("reload", reloadDelay);

            audioSource.clip = shootSoundSource;
            audioSource.Play();

            if (Physics.Raycast(eyes.position, eyes.transform.TransformDirection(Vector3.forward), Mathf.Infinity))
            {
                if (shotHit.transform.tag == "Apple" && shotHit.collider != null)
                {
                    AudioSource.PlayClipAtPoint(hitSoundSource, shotHit.transform.position);
                    hitObject = (Apple)shotHit.collider.gameObject.GetComponent(typeof(Apple));
                    Vector3 prePose = eyes.transform.TransformDirection(Vector3.forward) * 1000;
                    hitObject.shootAndDestroy(prePose);

                    score += 1 + scoreStreak;
                        
                    scoreStreak++;

                    if (playerHealth < maxplayerHealth - healthBonus)
                    {
                        playerHealth += healthBonus;
                    }
                    else
                    {
                        playerHealth = maxplayerHealth;
                    }
                    
                    print("Apple Hit");
                }
            }

        }
    }
    void reload()
    {
        print("GunLoaded");
        isLoaded = true;
    }

    public void updateUIText()
    {
        string tempString = "";
        for (int i = 0; i < playerHealth; i++)
        {
            tempString = tempString + "+";
        }
        HealthText.healthColor = new Vector3(1f,playerHealth / maxplayerHealth, playerHealth / maxplayerHealth);
        HealthText.healthString = tempString;

        ScoreText.score = score;

        StreakText.scoreStreak = scoreStreak;
    }
    public void setSensitivity(float newSensitivity)
    {
        sensitivity = newSensitivity;
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
    }
    public float getSensitivity()
    {
        return sensitivity;
    }
}
