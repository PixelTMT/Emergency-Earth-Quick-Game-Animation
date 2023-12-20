using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Webhook : MonoBehaviour
{
    public string webhookUrl = "https://discord.com/api/webhooks/1159708366000631848/Qda4yiEa_5PcbhVyzMtNbq30KKwQWPZ96wFOxzs_1XdoYAvugoGEqwFU4BVMlIjCBaXC";

    public string message = "Upoc";
    DateTime time = DateTime.Now;
    int ids;
    private void Start()
    {
        SendWebhook();
    }
    public void SendWebhook()
    {
        time = DateTime.Now;
        ids = UnityEngine.Random.Range(0, int.MaxValue);
        DontDestroyOnLoad(gameObject);
        StartCoroutine(PostToDiscord($"Started {message} with ID : {ids}"));
    }

    IEnumerator PostToDiscord(string msg)
    {
        // Create a JSON payload for the Discord webhook
        string jsonPayload = "{\"content\":\"" + msg + "\"}";

        // Create a UnityWebRequest and set its method to POST
        UnityWebRequest request = new UnityWebRequest(webhookUrl, "POST");

        // Set the request's upload handler and download handler
        byte[] jsonBytes = new System.Text.UTF8Encoding().GetBytes(jsonPayload);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        // Set the request's content type
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request
        yield return request.SendWebRequest();
    }
    private void OnDisable()
    {
        // Create a JSON payload for the Discord webhook
        string jsonPayload = "{\"content\":\"" + $"End {DateTime.Now - time} {message} with ID : {ids}" + "\"}";

        // Create a UnityWebRequest and set its method to POST
        UnityWebRequest request = new UnityWebRequest(webhookUrl, "POST");

        // Set the request's upload handler and download handler
        byte[] jsonBytes = new System.Text.UTF8Encoding().GetBytes(jsonPayload);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        // Set the request's content type
        request.SetRequestHeader("Content-Type", "application/json");
        request.SendWebRequest();
    }
}
