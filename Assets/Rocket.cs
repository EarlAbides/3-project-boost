using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    private new Rigidbody rigidbody;
    private AudioSource audioSource;
    private int currentLevel;
    private State currentState = State.Alive;

    enum State { Alive, Dying, Trancending }

    [SerializeField]
    float rcsThrust = 100.0f;

    [SerializeField]
    float mainThrust = 100.0f;

    [SerializeField]
    AudioClip mainEngine;

    [SerializeField]
    AudioClip explosion;

    [SerializeField]
    AudioClip levelComplete;

    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != State.Dying)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // Do Nothing
                break;
            case "Finish":
                LevelComplete();
                break;
            default:
                PlayerDead();
                break;
        }
    }

    private void LevelComplete()
    {
        if (currentState != State.Trancending)
        {
            var delay = levelComplete.length;

            audioSource.Stop();
            audioSource.PlayOneShot(levelComplete);
            currentState = State.Trancending;
            currentLevel++;
            if (currentLevel == 2)
            {
                currentLevel = 0;
            }

            Invoke("LoadNextLevel", delay);
        }
    }

    private void PlayerDead()
    {
        if (currentState != State.Dying)
        {
            var delay = explosion.length;

            audioSource.Stop();
            audioSource.PlayOneShot(explosion);
            currentState = State.Dying;
            currentLevel = 0;

            Invoke("LoadNextLevel", delay);
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(currentLevel);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            audioSource.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidbody.AddRelativeForce(Vector3.up * mainThrust);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
    }

    private void RespondToRotateInput()
    {
        float rotationThisFrame = Time.deltaTime * rcsThrust;

        // Take manual control of rotation
        rigidbody.freezeRotation = true;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * rotationThisFrame);
        }

        // Resume rotation control
        rigidbody.freezeRotation = false;
    }
}