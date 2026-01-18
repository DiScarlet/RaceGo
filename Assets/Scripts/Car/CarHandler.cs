using System;
using System.Collections;
using UnityEngine;

public class CarHandler : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform gameModel;
    [SerializeField] MeshRenderer carMeshRenderer;
    [SerializeField] ExplodeHandler explodeHandler;

    [Header("SFX")]
    [SerializeField] AudioSource carEngineAS;
    [SerializeField] AnimationCurve carPitchAnimationCurve;
    [SerializeField] AudioSource carSkidAS;
    [SerializeField] AudioSource carCrashAS;

    //Max values
    private float maxSteerVelocity = 2;
    private float maxForwardVelocity = 30;
    private float carMaxSpeedPercentage = 0;

    //Multiplyers
    private float accelerationMultiplier = 3;
    private float brakingMultiplier = 15;
    private float steeringPowerMultiplier = 5;

    //Input 
    private Vector2 input = Vector2.zero;

    //Emissive property
    private int _EmissionColor = Shader.PropertyToID("_EmissionColor");
    private Color emissiveColor = Color.red;
    private float emisiveColorMultiplier = 0f;

    //Exploded state
    private bool isExploded = false;

    private bool isPlayer = true;

    //Stats
    private float carStartPositionZ;
    private float distanceTravelled = 0;
    public float DistanceTravelled => distanceTravelled;

    //Events
    public event Action<CarHandler> OnPlayerCrashed;


    private void Start()
    {
        isPlayer = CompareTag("Player");

        if (isPlayer)
            carEngineAS.Play();

        carStartPositionZ = transform.position.z;
    }

    private void Update()
    {
        //Check if exploded
        if (isExploded)
        {
            FadeOutCarAudio();
            return;
        }

            //Rotate the model when turning
            gameModel.transform.rotation = Quaternion.Euler(0, rb.linearVelocity.x * 5, 0);

        if(carMeshRenderer != null)
        {
            float desiredCarEmissiveMyltipler = 0f;

            if (input.y <= 0)
                desiredCarEmissiveMyltipler = 4.0f;

            emisiveColorMultiplier = Mathf.Lerp(emisiveColorMultiplier, desiredCarEmissiveMyltipler, Time.deltaTime * 4);

            carMeshRenderer.material.SetColor(_EmissionColor, emissiveColor * emisiveColorMultiplier);
        }

        UpdateCarAudio();

        //Update distance travelled
        distanceTravelled = transform.position.z - carStartPositionZ;
    }

    private void FixedUpdate()
    {
        //Check if exploded
        if (isExploded)
        {
            rb.linearDamping = rb.linearVelocity.z * 0.1f;
            rb.linearDamping = Mathf.Clamp(rb.linearDamping, 1.5f, 10);

            //Move away from where exploded
            rb.MovePosition(Vector3.Lerp(transform.position, new Vector3(0, 0, transform.position.z), Time.deltaTime * 0.5f));

            return;
        }

        if (input.y > 0)
            Accelerate();
        else
            rb.linearDamping = 0.2f;

        if (input.y < 0)
            Brake();
        

        Steer();

        //Car never goes backwards
        if (rb.linearVelocity.z <= 0)
            rb.linearVelocity = Vector3.zero;
    }
    private void Accelerate()
    {
        rb.linearDamping = 0;

        if(rb.linearVelocity.x <= maxForwardVelocity)
            rb.AddForce(rb.transform.forward * accelerationMultiplier * input.y);
    }

    private void Brake()
    {
        if (rb.linearVelocity.z <= 0)
            return;

        rb.AddForce(rb.transform.forward * brakingMultiplier * input.y);
    }

    private void Steer()
    {
        if(Mathf.Abs(input.x) > 0)
        {
            //Move car sideways
            float speedBaseSteeringLimit = rb.linearVelocity.z / 5.0f;
            speedBaseSteeringLimit = Mathf.Clamp01(speedBaseSteeringLimit);

            rb.AddForce(rb.transform.right * steeringPowerMultiplier * input.x * speedBaseSteeringLimit);

            //Normalize x velocity
            float normalizedX = rb.linearVelocity.x / maxSteerVelocity;
            normalizedX = Mathf.Clamp(normalizedX, -1.0f, 1.0f);

            //Turn speed limits
            rb.linearVelocity = new Vector3(normalizedX * maxSteerVelocity, 0, rb.linearVelocity.z);
        }
        else
        {
            //Autocenter the car
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, new Vector3(0, 0, rb.linearVelocity.z), Time.fixedDeltaTime * 3);
        }
    }

    private void UpdateCarAudio()
    {
        if (!isPlayer)
            return;

        carMaxSpeedPercentage = rb.linearVelocity.z / maxForwardVelocity;

        carEngineAS.pitch = carPitchAnimationCurve.Evaluate(carMaxSpeedPercentage);

        if (input.y < 0 && carMaxSpeedPercentage > 0.2f)
        {
            if (!carSkidAS.isPlaying)
            {
                carSkidAS.Play();
            }

            carSkidAS.volume = Mathf.Lerp(carSkidAS.volume, 1.0f, Time.deltaTime * 10);
        }
        else
        {
            carSkidAS.volume = Mathf.Lerp(carSkidAS.volume, 0.0f, Time.deltaTime * 10);
        }
    }
    private void FadeOutCarAudio()
    {
        if (!isPlayer)
            return;

        carEngineAS.volume = Mathf.Lerp(carEngineAS.volume, 0, Time.deltaTime * 10);
        carSkidAS.volume = Mathf.Lerp(carSkidAS.volume, 0.0f, Time.deltaTime * 10);
    }

    public void SetInput(Vector2 inputVector)
    {
        inputVector.Normalize();

        input = inputVector;
    }

    public void SetMaxSpeed(float newMaxSpeed)
    {
        maxForwardVelocity = newMaxSpeed;
    }

    IEnumerator SlowDownTimeCO()
    {
        while (Time.timeScale > 0.4f)
        {
            Time.timeScale -= Time.deltaTime * 2;

            yield return null;
        }
        yield return new WaitForSeconds(0.5f);

        while (Time.timeScale <= 1.0f)
        {
            Time.timeScale += Time.deltaTime;

            yield return null;
        }

        Time.timeScale = 1.0f;
    }

    //Events
    private void OnCollisionEnter(Collision collision)
    {
        //NPC only explode when hit by player or car part
        if(!isPlayer)
        {
            if (collision.transform.root.CompareTag("Untagged"))
                return;

            if (collision.transform.root.CompareTag("CarNPC"))
                return;
        }
        Vector3 velocity = rb.linearVelocity;
        explodeHandler.Explode(velocity * 45);

        isExploded = true;


        carCrashAS.volume = carMaxSpeedPercentage;
        carCrashAS.volume = Mathf.Clamp(carCrashAS.volume, 0.25f, 1.0f);

        carCrashAS.pitch = carMaxSpeedPercentage;
        carCrashAS.pitch = Mathf.Clamp(carCrashAS.volume, 0.3f, 1.0f);

        carCrashAS.Play();

        Debug.Log("PLAYER CRASHED!!");
        //Trigger crash action
        OnPlayerCrashed?.Invoke(this);

        StartCoroutine(SlowDownTimeCO());
    }
}
