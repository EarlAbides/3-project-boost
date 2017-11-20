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
            Thrust();
            Rotate();
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
                print("Hit Finish");
                currentState = State.Trancending;
                currentLevel++;
                if (currentLevel == 2)
                {
                    currentLevel = 0;
                }
                Invoke("LoadNextLevel", 1f);
                break;
            default:
                print("Dead!");
                currentState = State.Dying;
                currentLevel = 0;
                Invoke("LoadNextLevel", 1f);
                break;
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(currentLevel);
    }
    
    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidbody.AddRelativeForce(Vector3.up * mainThrust);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            audioSource.Stop();
        }
    }

    private void Rotate()
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