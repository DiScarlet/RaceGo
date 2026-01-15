using UnityEngine;

public class CarHandler : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    //Multiplyers
    private float accelerationMultiplyer = 3;
    private float breakMultiplyer = 15;
    private float steeringInputMultiplyer = 5;

    //Input 
    Vector2 input = Vector2.zero;

    private void FixedUpdate()
    {
        if (input.y > 0)
            Accelerate();
        else
            rb.linearDamping = 0.2f;

        if (input.y < 0)
            Brake();
        

        Steer();
    }
    private void Accelerate()
    {
        rb.linearDamping = 0;

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
            rb.AddForce(rb.transform.right * steeringInputMultiplyer * input.x);
        }
    }

    public void SetInput(Vector2 inputVector)
    {
        inputVector.Normalize();

        input = inputVector;
    }
}
