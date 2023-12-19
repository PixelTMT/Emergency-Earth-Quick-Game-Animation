using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class BombManager : MonoBehaviour
{
    [SerializeField] Transform Player;
    [SerializeField] public int timer = 999;
    [SerializeField] int bibIntervalSpeed = 5;
    [SerializeField] float intensity = 5;
    [SerializeField] float intensityDropRate = 1;

    [SerializeField] TextMeshProUGUI TimerText;
    [SerializeField] GameObject MissionParent;
    [SerializeField] TextMeshProUGUI MissionText;

    [HideInInspector] public bool hasClue = false;
    [HideInInspector] public bool hasDefuse = false;
    [HideInInspector] public bool bombFound = false;
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
        TimerText.gameObject.SetActive(true);
        MissionParent.SetActive(true);
        bomb.Light.enabled = true;
        bomb.bomb.SetActive(true);
        timer += (int)Time.time;
        while (Time.time <= timer && !hasDefuse)
        {
            bomb.Light.intensity = intensity;
            bomb.Audio.Play();
            yield return new WaitForSeconds(bibIntervalSpeed - bibIntervalSpeed * (Time.time / timer));
        }
        if(!hasDefuse) GameObject.FindAnyObjectByType<HeartManager>().ReduceHeart(5);
        direction.gameObject.SetActive(false);
        bomb.Light.enabled = false;
        TimerText.gameObject.SetActive(false);
        MissionParent.SetActive(false);
    }
    private void Update()
    {
        LOS_Bomb();
        direction.LookAt(lookTo);
        if(hasClue)
        {
            lookTo = bomb.bomb.transform.position;
            MissionText.text = "You have found the clue code, return to the location of the bomb and defuse it.";
            clue.gameObject.SetActive(false);
        }
        else if (bombFound)
        {
            MissionText.text = "Now look for clues in the form of codes to defuse the BOMB, the clues are located on the table rack.";
        }
        else
        {
            MissionText.text = "Find the location of the bomb by hearing the sound or following the arrow";
        }
        TimerText.text = $"Time : {Convert.ToInt32(timer - Time.time)}";
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
            bombFound = true;
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
