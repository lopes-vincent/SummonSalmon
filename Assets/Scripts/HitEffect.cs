using System.Collections;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(AutoDestroy());
    }
    
    IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
