using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody;
    AudioSource audioSource;
    

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float rcsRotation = 100f;
    [SerializeField] float LoadLevelDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip victory;
    [SerializeField] AudioClip death;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem victoryParticles;
    [SerializeField] ParticleSystem deathParticles;

    bool CollisionsDisabled = true;
    bool isTransitioning = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTransitioning)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
        if (Debug.isDebugBuild) { Respondtodebug(); }

    }
    void Respondtodebug()
    {
        if (Input.GetKeyDown(KeyCode.L)) { LoadNextScene(); }
        if (Input.GetKeyDown(KeyCode.C)) { CollisionsDisabled = !CollisionsDisabled; }

    }
    void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || !CollisionsDisabled) { return; }
        mainEngineParticles.Stop();
        switch (collision.gameObject.tag)
        {

            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }
    private void StartSuccessSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(victory);
        victoryParticles.Play();
        Invoke("LoadNextScene", LoadLevelDelay);
    }
    private void StartDeathSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        deathParticles.Play();
        Invoke("LoadFirstLevel", LoadLevelDelay);
    }

    

    private void LoadFirstLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (currentSceneIndex != 0) { SceneManager.LoadScene(1); }
        else { SceneManager.LoadScene(0); }
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        //if(nextSceneIndex == Scene.Manager.sceneCountInBuildSettings){nextSceneIndex=0;}
        if (nextSceneIndex != 8)
        {
            SceneManager.LoadScene(nextSceneIndex); 
        }
        else { SceneManager.LoadScene(0); }

    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space)) // can thrust while rotating
        {
            ApplyThrust();

                

        }
        else
        {
            StopApplyingThrust();
        }
    }

    private void StopApplyingThrust()
    {
        audioSource.Stop();
        mainEngineParticles.Stop();
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * rcsThrust * Time.deltaTime);
        if (!audioSource.isPlaying) { audioSource.PlayOneShot(mainEngine); }
        mainEngineParticles.Play();

    }

    private void RespondToRotateInput()
    {
        
        if (Input.GetKey(KeyCode.A))
        {
            RotateManually(rcsRotation * Time.deltaTime);
        }

        else if (Input.GetKey(KeyCode.D))
        {
            RotateManually(-rcsRotation * Time.deltaTime);
        }
        

    }

    private void RotateManually(float rotationThisFrame)
    {
        rigidBody.freezeRotation = true; // take manual control of rotation
        transform.Rotate(Vector3.forward * rotationThisFrame);
        rigidBody.freezeRotation = false; //resume physics control of rotation
    }
}
