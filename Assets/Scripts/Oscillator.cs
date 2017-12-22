using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField]
    Vector3 movement = new Vector3(10.0f, 10.0f, 10.0f);

    [SerializeField]
    float period = 2.0f;

    Vector3 startingPos;
    float movementFactor;

    // Use this for initialization
    void Start()
    {
        startingPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period > Mathf.Epsilon)
        {
            PerformOscillation();
        }
    }

    private void PerformOscillation()
    {
        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2;
        float rawSineValue = Mathf.Sin(cycles * tau); // Goes from -1.0 to 1.0

        movementFactor = rawSineValue / 2.0f + 0.5f;
        gameObject.transform.position = startingPos + (movementFactor * movement);
    }
}
