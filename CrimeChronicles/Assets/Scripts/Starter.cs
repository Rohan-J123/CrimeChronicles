using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Diagnostics;
using System.IO;

public class Starter : MonoBehaviour
{
    private string url = "http://localhost:8080/";
    public string[] suspectNames;

    [SerializeField] private TextMeshProUGUI suspect1_name;
    [SerializeField] private TextMeshProUGUI suspect2_name;
    [SerializeField] private TextMeshProUGUI suspect3_name;
    [SerializeField] private TextMeshProUGUI suspect4_name;
    [SerializeField] private TextMeshProUGUI suspect5_name;

    [SerializeField] private GameObject loader;

    [SerializeField] private MenuOptions menuOptions;

    public void CreateScenario(string[] commands)
    {
        string combinedCommands = string.Join(" && ", commands);

        string assetsPath = Application.streamingAssetsPath;

        ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/c " + combinedCommands)
        {
            WorkingDirectory = assetsPath,
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        Process process = new Process
        {
            StartInfo = processInfo
        };

        process.Start();
        process.WaitForExit();
    }

    private IEnumerator SendRequest(string name, string query)
    {
        yield return new WaitForSeconds(5f);
        string jsonData = "{\"user\": \"user\", \"name\": \"" + name + "\", \"query\": \"" + query + "\"}";
        UnityWebRequest request = UnityWebRequest.Post(url, "POST");

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string[] names = request.downloadHandler.text.Trim('[', ']', '"').Split(',');
            List<string> genders = new List<string>();

            suspect1_name.text = names[0].Split('(')[0].TrimStart().TrimEnd();
            suspect2_name.text = names[1].Split('(')[0].TrimStart().TrimEnd();
            suspect3_name.text = names[2].Split('(')[0].TrimStart().TrimEnd();
            suspect4_name.text = names[3].Split('(')[0].TrimStart().TrimEnd();
            suspect5_name.text = names[4].Split('(')[0].TrimStart().TrimEnd();

            string[] temp = { suspect1_name.text, suspect2_name.text, suspect3_name.text, suspect4_name.text, suspect5_name.text };

            suspectNames = temp;

            menuOptions.OnStartInspection();
            loader.SetActive(false);
        }
        else
        {
            UnityEngine.Debug.LogError("Error: " + request.error);
        }
        request.Dispose();
    }

    public IEnumerator SendQueries(string name, string query, System.Action<string> callback)
    {
        string jsonData = "{\"user\": \"user\", \"name\": \"" + name + "\", \"query\": \"" + query + "\"}";
        UnityWebRequest request = UnityWebRequest.Post(url, "POST");

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        for(int i = 0; i < suspectNames.Length; i++)
        {
            if (suspectNames[i].Contains(name))
            {
                AddDataToFile("Detective: " + query, "Suspect" + (i + 1).ToString());
            }
        }

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            callback?.Invoke(request.downloadHandler.text.Replace("\n", "").Replace("\r", ""));
        }
        else
        {
            UnityEngine.Debug.LogError("Error: " + request.error);
            callback?.Invoke(null);
        }
        request.Dispose();
    }

    void AddDataToFile(string data, string fileName)
    {
        string filePath = Application.streamingAssetsPath + "/Scripts/contextful_parsing/data/"+ fileName+".txt";

        if (File.Exists(filePath))
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(data);
            }
        } else
        {
            UnityEngine.Debug.Log("Error in file edit");
        }
    }

    private IEnumerator StartScenarioAndQuery()
    {
        loader.SetActive(true);

        string[] commands = new string[]
        {
            "cd Scripts",
            "cd contextful_parsing",
            "python story.py"
        };

        CreateScenario(commands);
        yield return null;
        StartCoroutine(SendRequest("Computer", "Give me names of suspects in the format: [First Name(Gender), First Name(Gender), First Name(Gender), First Name(Gender), First Name(Gender)]"));
    }

    void Start()
    {
        StartCoroutine(StartScenarioAndQuery());
        ClearFileData();
    }

    public void ClearFileData()
    {
        string filePath;

        for (int number = 1; number < 6; number++)
        {
            filePath = Application.streamingAssetsPath + "/Scripts/contextful_parsing/data/Suspect" + number + ".txt";
            File.WriteAllText(filePath, string.Empty);
        };

        filePath = Application.streamingAssetsPath + "/Scripts/contextful_parsing/data/Sherlock.txt";
        File.WriteAllText(filePath, string.Empty);

        filePath = Application.streamingAssetsPath + "/Scripts/contextful_parsing/data/Inspector.txt";
        File.WriteAllText(filePath, string.Empty);
    }
}
