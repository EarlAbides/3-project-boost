using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Rigidbody rigidbody;
    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        HandleSpaceKey();

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back);
        }
    }

    private void HandleSpaceKey()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioSource.Play();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            audioSource.Stop();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            rigidbody.AddRelativeForce(Vector3.up);
        }
    }
}