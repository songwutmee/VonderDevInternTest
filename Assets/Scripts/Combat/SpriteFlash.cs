using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlash : MonoBehaviour
{
    private SpriteRenderer sr;
    private Material originalMat;
    public Material flashMaterial; 

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalMat = sr.material;
    }

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        sr.material = flashMaterial;
        yield return new WaitForSeconds(0.1f);
        sr.material = originalMat;
    }
}