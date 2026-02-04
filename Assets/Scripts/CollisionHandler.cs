using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float delayAmount = 0.5f;
    [SerializeField] AudioClip crashedSFX;
    [SerializeField] AudioClip finishedSFX;
    [SerializeField] ParticleSystem crashedParticle;
    [SerializeField] ParticleSystem finishedParticle;

    AudioSource audioSource;

    bool isControllable = true;
    bool isColladable = true;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();  

    }

    void Update()
    {
        RespondToDebugKeys(); 
    }
    void OnCollisionEnter(Collision collision)
    {
        if(!isControllable || !isColladable) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("It's a Friend");
                break;

            case "Fuel":
                Debug.Log("Watch Out!");
                break;

            case "Finish":
                FinishedState();
                break;

            default:
                StartCrashSequence();
                break;

        }
    }

    void RespondToDebugKeys()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            NextLevel();
        }
        else if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            isColladable = !isColladable;
        }
    }

    void FinishedState()
    {
        isControllable = false;

        finishedParticle.Play();
        GetComponent<Movement>().enabled = false;
        audioSource.PlayOneShot(finishedSFX);
        Invoke("NextLevel", delayAmount);
    }

    void StartCrashSequence()
    {
        isControllable = false;

        crashedParticle.Play();
        GetComponent<Movement>().enabled = false;
        audioSource.PlayOneShot(crashedSFX);
        Invoke("ReloadLevel", delayAmount);
    }
    void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    void NextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;

        int nextScene = currentScene+1;
        
        if (nextScene == SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }

        SceneManager.LoadScene(nextScene);
        
    }
}
