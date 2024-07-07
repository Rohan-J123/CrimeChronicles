using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using TMPro;


public class Server : MonoBehaviour
{
    private Process dockerProcess;
    [SerializeField] private TMP_InputField apiKeyInput;

    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject apiCanvas;

    void Start()
    {
        string[] commands = new string[]
            {
                "cd Scripts",
                "cd contextful_parsing",
                "python story.py",
                "docker compose up --build"
            };
        ExecuteCommands(commands);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ExecuteCommands(string[] commands)
    {
        string combinedCommands = string.Join(" && ", commands);

        string assetsPath = Application.dataPath;

        ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/c " + combinedCommands)
        {
            WorkingDirectory = assetsPath,
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        dockerProcess = new Process
        {
            StartInfo = processInfo
        };

        dockerProcess.Start();
    }

    public void StopContainers()
    {
        dockerProcess.Kill();
        dockerProcess.WaitForExit();
        dockerProcess.Close();
    }

    void OnApplicationQuit()
    {
        StopContainers();
    }
}
