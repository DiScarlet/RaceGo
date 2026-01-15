using UnityEngine;

public class CarHandler : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform gameModel;

    //Max values
    private float maxSteerVelocity = 2;
    private float maxForwardVelocity = 30;

    //Multiplyers
    private float accelerationMultiplyer = 3;
    private float breakMultiplyer = 15;
    private float steeringInputMultiplyer = 5;

    //Input 
    Vector2 input = Vector2.zero;

    private void Update()
    {
        //Rotate the model when turning
        gameModel.transform.rotation = Quaternion.Euler(0, rb.linearVelocity.x * 5, 0);
    }

    private void FixedUpdate()
    {
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
            rb.AddForce(rb.transform.forward * accelerationMultiplyer * input.y);
    }

    private void Brake()
    {
        if (rb.linearVelocity.z <= 0)
            return;

        rb.AddForce(rb.transform.forward * breakMultiplyer * input.y);
    }

    private void Steer()
    {
        if(Mathf.Abs(input.x) > 0)
        {
            //Move car sideways
            float speedBaseSteeringLimit = rb.linearVelocity.z / 5.0f;
            speedBaseSteeringLimit = Mathf.Clamp01(speedBaseSteeringLimit);

            rb.AddForce(rb.transform.right * steeringInputMultiplyer * input.x * speedBaseSteeringLimit);

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

    public void SetInput(Vector2 inputVector)
    {
        inputVector.Normalize();

        input = inputVector;
    }
}
