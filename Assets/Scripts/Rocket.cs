using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    enum State { Alive, Dying, Trancending }

    private new Rigidbody rigidbody;
    private AudioSource audioSource;
    private State currentState = State.Alive;
    private bool collisionsOff = false;

    [SerializeField] float rcsThrust = 100.0f;
    [SerializeField] float mainThrust = 100.0f;
    [SerializeField] float levelLoadDelay = 1.0f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip explosion;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem engineEffect;
    [SerializeField] ParticleSystem explosionEffect;
    [SerializeField] ParticleSystem successEffect;

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

        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
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
            engineEffect.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidbody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        engineEffect.Play();
    }

    private void RespondToRotateInput()
    {
        float rotationThisFrame = Time.deltaTime * rcsThrust;

        rigidbody.angularVelocity = Vector3.zero; // remove rotation due to physics

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * rotationThisFrame);
        }
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsOff = !collisionsOff;
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
                if (collisionsOff) return;
                PlayerDead();
                break;
        }
    }

    private void LevelComplete()
    {
        if (currentState != State.Trancending)
        {
            var delay = success.length + levelLoadDelay;

            audioSource.Stop();
            audioSource.PlayOneShot(success);
            successEffect.Play();
            currentState = State.Trancending;

            Invoke("LoadNextLevel", delay);
        }
    }

    private void PlayerDead()
    {
        if (currentState != State.Dying)
        {
            var delay = explosion.length + levelLoadDelay;

            audioSource.Stop();
            engineEffect.Stop();
            audioSource.PlayOneShot(explosion);
            explosionEffect.Play();
            currentState = State.Dying;

            Invoke("LoadNextLevel", delay);
        }
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex < SceneManager.sceneCountInBuildSettings && currentState != State.Dying)
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }

    }
}