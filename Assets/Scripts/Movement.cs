using System.Timers;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;

    [SerializeField] float thrustStrength = 3f;
    [SerializeField] float rotationStrength = 10f;

    [SerializeField] ParticleSystem mainBoosterParticle;
    [SerializeField] ParticleSystem leftBoosterParticle;
    [SerializeField] ParticleSystem rightBoosterParticle;

    [SerializeField] AudioClip mainEngineSFX;

    Rigidbody rb;
    AudioSource audioSource;

    void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        ProcessTrust();
        ProcessRotation();
    }

    private void ProcessTrust()
    {
        if (thrust.IsPressed())
        {
            StartThrusting();
        }
        else
        {
            mainBoosterParticle.Stop();
            audioSource.Stop();
        }
    }

    private void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);
        mainBoosterParticle.Play();
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngineSFX);
        }
    }

    private void ProcessRotation()
    {
        float rotationInput = rotation.ReadValue<float>();

        if (rotationInput < 0)
        {
            TurnLeft();
        }

        else if (rotationInput > 0)
        {
            TurnRight();
        }

        else
        {
            rightBoosterParticle.Stop();
            leftBoosterParticle.Stop();
        }
    }

    private void TurnRight()
    {
        ApplyRotation(-rotationStrength);
        rightBoosterParticle.Stop();
        leftBoosterParticle.Play();
    }

    private void TurnLeft()
    {
        ApplyRotation(rotationStrength);
        leftBoosterParticle.Stop();
        rightBoosterParticle.Play();
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.fixedDeltaTime);

        rb.freezeRotation = false;
    }
}
