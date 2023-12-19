using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUnderTable : MonoBehaviour
{
    public float LerpSpeed = 5f;
    public Vector3 offset = Vector3.zero;
    [SerializeField] GameObject HideTips;

    Vector3[] lastTransform;
    [HideInInspector] public bool isHiding = false;
    [HideInInspector] public bool currentlyHiding = false;
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
        if (Input.GetKeyDown(KeyCode.E) && isHiding && !currentlyHiding)
        {
            currentlyHiding = true;
            delayTimeHide = Time.time + 2f;
            StartCoroutine(hide());
        }
        else if (Input.GetKeyDown(KeyCode.E) && Time.time > delayTimeHide && currentlyHiding)
        {
            currentlyHiding = false;
            StartCoroutine(unhide());
        }

    }
    IEnumerator hide()
    {
        float time = Time.time + 2;

        while (Time.time < time)
        {
            player.position = Vector3.Lerp(player.position, this.transform.position + offset, LerpSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HideTips.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HideTips.SetActive(false);
            isHiding = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isHiding = true;
        }
    }
}
