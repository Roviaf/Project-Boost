using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f; // setting period to 2 seconds

    //todo remove from inspector later
    [Range(0, 1)][SerializeField]float movementFactor; //0 for not moved, 1 for fully moved.

    Vector3 startingPos; // X,Y,Z (vector3)

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position; // obtains the current position
    }

    // Update is called once per frame
    void Update()
    {
        

        if (period <= Mathf.Epsilon) {return; }// protects period equals to 0
        float cycles = Time.time / period; //grows continually from 0
        const float tau = Mathf.PI * 2f; // about 6.28
        float rawSinWave = Mathf.Sin(cycles * tau);// goes from -1 to +1
        movementFactor = rawSinWave / 2f + 0.5f; // prevents from going below 0
        Vector3 offset = movementVector * movementFactor; // Movement predifined times the result
        transform.position = startingPos + offset; // inital position plus the offset


    }
}
