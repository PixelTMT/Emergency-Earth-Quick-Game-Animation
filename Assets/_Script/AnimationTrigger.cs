using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] bool ShakeScreen = false;
    public float ScreenShakeTime = 1;
    public float ScreenShakeScale = 0.2f;
    bool hasTrigger = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTrigger)
        {
            hasTrigger = true;
            animator.SetTrigger("isTrigger");
            if (ShakeScreen)
                StartCoroutine(other.GetComponent<CharaControl>().ScreenShake(ScreenShakeTime, ScreenShakeScale));
        }
    }
}
