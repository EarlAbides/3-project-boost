using UnityEngine;

public class Rocket : MonoBehaviour
{
    private new Rigidbody rigidbody;
    private AudioSource audioSource;

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
        Thrust();
        Rotate();
    }


    private void Thrust()
    {
        float thrustThisFrame = Time.deltaTime * mainThrust;

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