using UnityEngine;

public class RandomizeObject : MonoBehaviour
{
    [SerializeField] Vector3 localRotMin = Vector3.zero;
    [SerializeField] Vector3 localRotMax = Vector3.zero;

    [SerializeField] float localScaleMultMin = 0.8f;
    [SerializeField] float localScaleMultMax = 1.5f;

    Vector3 localScaleOriginal = Vector3.one;

    private void Start()
    {
        localScaleOriginal = transform.localScale;
    }

    void OnEnabled()
    {
        transform.localRotation = Quaternion.Euler(Random.Range(localRotMin.x, localRotMax.x),
                                                   Random.Range(localRotMin.y, localRotMax.y),
                                                   Random.Range(localRotMin.z, localRotMax.z));

        transform.localScale = localScaleOriginal * Random.Range(localScaleMultMin, localScaleMultMax);
    }
}
