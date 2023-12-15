using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUnderTable : MonoBehaviour
{
    public float LerpSpeed = 5f;
    public Vector3 offset = Vector3.zero;

    Vector3[] lastTransform;
    [HideInInspector] public bool isHiding = false;
    Transform player;

    float delayTimeHide;
    private void Awake()
    {
        lastTransform = new Vector3[2];
        player = GameObject.Find("Player").transform;
        lastTransform[0] = player.position;
        lastTransform[1] = player.rotation.eulerAngles;
    }
    private void Update()
    {
        if (isHiding)
        {
            player.position = Vector3.Lerp(player.position, this.transform.position + offset, LerpSpeed * Time.deltaTime);
            if (Input.GetKeyDown(KeyCode.E) && Time.time > delayTimeHide)
            {
                StartCoroutine(unhide());
                isHiding = false;
            }
        }

    }
    IEnumerator unhide()
    {
        float time = Time.time + 1;
        while (Time.time < time)
        {
            player.position = Vector3.Lerp(player.position, lastTransform[0], LerpSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && !isHiding)
        {
            delayTimeHide = Time.time + 1f;
            isHiding = true;
        }
    }
}
