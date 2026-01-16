using UnityEngine;

public class RandomizeObject : MonoBehaviour
{
    [SerializeField] Vector3 localRotMin = Vector3.zero;
    [SerializeField] Vector3 localRotMax = Vector3.zero;

    [SerializeField] float localScaleMultMin = 0.8f;
    [SerializeField] float localScaleMultMax = 1.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnabled()
    {
        transform.localRotation = Quaternion.Euler(Random.Range(localRotMin.x, localRotMax.x),
                                                   Random.Range(localRotMin.y, localRotMax.y),
                                                   Random.Range(localRotMin.z, localRotMax.z));

        transform.localScale = transform.localScale * Random.Range(localScaleMultMin, localScaleMultMax);
    }
}
