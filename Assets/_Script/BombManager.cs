using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    [SerializeField] Transform Player;
    [SerializeField] public int timer = 999;
    [SerializeField] int bibIntervalSpeed = 5;
    [SerializeField] float intensity = 5;
    [SerializeField] float intensityDropRate = 1;

    [HideInInspector] public bool hasClue = false;
    [HideInInspector] public bool hasDefuse = false;
    Bomb bomb;
    Transform clue;
    Transform direction;
    Vector3 lookTo;
    private void Awake()
    {
        setBomb();
        setClue();
        direction = Player.Find("DirectionPivot");
        lookTo = bomb.bomb.transform.position;
    }
    IEnumerator Start()
    {
        bomb.Light.enabled = true;
        bomb.bomb.SetActive(true);
        
        while (Time.time <= timer && !hasDefuse)
        {
            bomb.Light.intensity = intensity;
            bomb.Audio.Play();
            yield return new WaitForSeconds(bibIntervalSpeed - bibIntervalSpeed * (Time.time / timer));
        }
        direction.gameObject.SetActive(false);
        bomb.Light.enabled = false;
        
    }
    private void Update()
    {
        LOS_Bomb();
        direction.LookAt(lookTo);
        if(hasClue)
        {
            lookTo = bomb.bomb.transform.position;
            clue.gameObject.SetActive(false);
        }
    }
    void setBomb()
    {
        bomb = new Bomb();
        var bombs = GameObject.FindGameObjectsWithTag("Bombs");

        int randomIndex = Random.Range(0, bombs.Length - 1);
        GameObject bombObject = bombs[randomIndex].transform.GetChild(0).gameObject;
        Light light = bombs[randomIndex].transform.GetChild(1).GetComponent<Light>();
        bomb.bomb = bombObject;
        bomb.Light = light;
        bomb.Audio = bombs[randomIndex].GetComponent<AudioSource>();

        foreach (var bombsObj in bombs)
        {
            bombsObj.SetActive(false);
        }
        bombs[randomIndex].SetActive(true);
        
    }
    void setClue()
    {
        var clues = GameObject.FindGameObjectsWithTag("Interact");
        int randomIndex = Random.Range(0, clues.Length - 1);
        GameObject clue = clues[randomIndex];
        foreach(var clueObj in clues)
        {
            clueObj.SetActive(false);
        }
        clue.SetActive(true);
        this.clue = clue.transform;
    }
    private void LOS_Bomb()
    {
        if (hasClue) return;
        Ray ray = new Ray(Player.position, (bomb.bomb.transform.position + Vector3.up) - Player.position);
        Debug.DrawRay(ray.origin, ray.direction);
        if (!Physics.Raycast(ray, Vector3.Distance(Player.position, bomb.bomb.transform.position + Vector3.up)))
        {
            lookTo = clue.position;
            Debug.Log("LOSBOMB");
        }
    }
    private void FixedUpdate()
    {
        bomb.Light.intensity -= intensityDropRate;
    }
}
public class Bomb
{
    public GameObject bomb;
    public Light Light;
    public AudioSource Audio;
}
