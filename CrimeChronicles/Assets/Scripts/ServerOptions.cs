using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using System.IO;
using UnityEngine.Networking;
using TMPro;
using System.Linq;
using Unity.VisualScripting;

public class ServerOptions : MonoBehaviour
{
    [SerializeField] private GameObject startGame;
    [SerializeField] private GameObject startGameDisabled;

    [SerializeField] private TextMeshProUGUI textMain;

    [SerializeField] private GameObject loader;

    private string url = "http://localhost:8080/";
    private Process dockerProcess;

    [SerializeField] private TMP_InputField apiKeyInput;

    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject apiCanvas;

    [SerializeField] private GameObject infoText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ConnectServer("", ""));
        string filePath = Application.streamingAssetsPath + "/Scripts/contextful_parsing/.env";

        if (File.Exists(filePath))
        {
            string fileData = File.ReadAllText(filePath);

            if(fileData == "OPENAI_API_KEY=")
            {
                OnApiKeyOpen();
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ExecuteCommands(string[] commands)
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

        dockerProcess = new Process
        {
            StartInfo = processInfo
        };

        dockerProcess.Start();
    }

     public IEnumerator ConnectServer(string name, string query)
    {

        string jsonData = "{\"user\": \"user\", \"name\": \"" + name + "\", \"query\": \"" + query + "\"}";
        UnityWebRequest request = UnityWebRequest.Post(url, "POST");

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            startGame.SetActive(true);
            startGameDisabled.SetActive(false);
            loader.SetActive(false);
        } else
        {
            startGame.SetActive(false);
            startGameDisabled.SetActive(true);
        }
        request.Dispose();
        yield return new WaitForSeconds(2f);
        StartCoroutine(ConnectServer(name, query));
    }

    public void StartServer()
    {
        loader.SetActive(true);
        string[] commands = new string[]
            {
                "cd Scripts",
                "cd contextful_parsing",
                "docker compose up --build"
            };
        ExecuteCommands(commands);
    }

    public void OnStartGame()
    {
        loader.SetActive(true);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void OnStartGameDisabled()
    {
        StartCoroutine(ServerNotConnected());
    }

    IEnumerator ServerNotConnected()
    {
        textMain.text = "Server not yet connected";
        yield return new WaitForSeconds(1f);
        textMain.text = "";
    }

    //public void StopContainers()
    //{
    //    dockerProcess.Kill();
    //    dockerProcess.WaitForExit();
    //    dockerProcess.Close();
    //}

    //void OnApplicationQuit()
    //{
    //    StopContainers();
    //}

    //private void OnDestroy()
    //{
    //    StopContainers();
    //}

    public void OnCloseServer()
    {
        loader.SetActive(true);
        string[] commands = new string[]
            {
                "cd Scripts",
                "cd contextful_parsing",
                "docker-compose stop"
            };
        ExecuteCommands(commands);
    }

    public void OnApiKeyOpen()
    {
        apiCanvas.SetActive(true);
        mainCanvas.SetActive(false);
    }

    public void OnApiKeyCancel()
    {
        apiCanvas.SetActive(false);
        mainCanvas.SetActive(true);
    }

    public void WriteAPIKey()
    {
        string filePath = Application.streamingAssetsPath + "/Scripts/contextful_parsing/.env";

        File.WriteAllText(filePath, string.Empty);

        File.WriteAllText(filePath, "OPENAI_API_KEY=" + apiKeyInput.text);

        apiCanvas.SetActive(false);
        mainCanvas.SetActive(true);
    }

    public void OnInfoClick()
    {
        if (infoText.activeSelf)
        {
            infoText.SetActive(false);
        }
        else
        {
            infoText.SetActive(true);
        }
    }
}
