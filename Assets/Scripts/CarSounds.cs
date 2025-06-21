using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSounds : MonoBehaviour
{
    public float minSpeed;
    public float maxSpeed;
    private float currentSpeed;

    private Rigidbody carRb;
    private AudioSource carAudio;

    public float minPitch;
    public float maxPitch;
    private float pitchFromCar;
    public float maxVolume;

    void Start()
    {
        carAudio = GetComponent<AudioSource>();
        carRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        EngineSound();
    }

    void EngineSound()
    {
        currentSpeed = carRb.velocity.magnitude;
        pitchFromCar = carRb.velocity.magnitude / 30f;

        if (currentSpeed < minSpeed)
        {
            carAudio.pitch = minPitch;
            carAudio.volume = 0;
        }

        if (currentSpeed > minSpeed)
        {
            carAudio.pitch = minPitch + pitchFromCar;
            carAudio.volume = pitchFromCar;

            if (carAudio.pitch > maxPitch)
            {
                carAudio.pitch = maxPitch;
            }
            if (carAudio.volume > maxVolume)
            {
                carAudio.volume = maxVolume;
            }
        }
    }
}
