using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GempaManager : MonoBehaviour
{
    [SerializeField] GameObject npc;
    HeartManager heart;
    HideUnderTable _HideUnderTable;
    public float DamageInterfal = 1f;
    public float TimeGempa = 20f;

    private void Awake()
    {
        _HideUnderTable = GameObject.FindAnyObjectByType<HideUnderTable>();
        heart = GameObject.FindAnyObjectByType<HeartManager>();
    }

    IEnumerator Start()
    {
        float time = Time.time + TimeGempa;
        npc.SetActive(false);
        while (Time.time <= time)
        {
            yield return new WaitForSeconds(DamageInterfal);
            if (!_HideUnderTable.currentlyHiding) heart.ReduceHeart(1);
        }
        npc.SetActive(true);
    }
}
