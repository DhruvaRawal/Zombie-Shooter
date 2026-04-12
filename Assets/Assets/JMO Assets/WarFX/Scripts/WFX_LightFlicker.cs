using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class WFX_LightFlicker : MonoBehaviour
{
    public float time = 0.05f;
    private float timer;
    private Coroutine flickerCoroutine;

    void OnEnable()
    {
        flickerCoroutine = StartCoroutine(Flicker());
    }

    void OnDisable()
    {
        if (flickerCoroutine != null)
            StopCoroutine(flickerCoroutine);
        GetComponent<Light>().enabled = false;
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            GetComponent<Light>().enabled = !GetComponent<Light>().enabled;
            do
            {
                timer -= Time.deltaTime;
                yield return null;
            }
            while (timer > 0);
            timer = time;
        }
    }
}