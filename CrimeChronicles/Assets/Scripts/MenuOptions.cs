using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuOptions : MonoBehaviour
{
    [SerializeField] private GameObject pastConversations;
    [SerializeField] private GameObject inputField;
    [SerializeField] private TMP_InputField inputFieldText;
    [SerializeField] private GameObject outputField;
    [SerializeField] private TextMeshProUGUI outputFieldText;
    [SerializeField] private GameObject backButton;

    [SerializeField] private GameObject suspectOptions;
    [SerializeField] private GameObject suspectOptionsAgain;
    [SerializeField] private GameObject helperOptions;
    [SerializeField] private GameObject helperOptionsAgain;
    [SerializeField] private GameObject helperOptionsNext;

    [SerializeField] private Starter starter;
    public string currentName;
    public int currentNumber;

    [SerializeField] private GameObject loader;

    [SerializeField] private GameObject[] npcs;
    [SerializeField] private GameObject[] npcOptions;

    private Vector3 initialPosition;
    private Vector3 initialScale;

    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject nextCanvas;
    [SerializeField] private GameObject previousCanvas;
    [SerializeField] private GameObject accuseCanvas;

    [SerializeField] private TextMeshProUGUI accuseCanvasText;

    [SerializeField] private TextMeshProUGUI pastConversationsText;
    [SerializeField] private TextMeshProUGUI pastConversationsTitle;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void OnStartInspection()
    {
        currentNumber = 5;
        loader.SetActive(true);
        OnQueryOpen("Inspector");
        inputField.SetActive(false);
        outputField.SetActive(true);
        loader.SetActive(true);
        StartCoroutine(starter.SendQueries("Inspector", "Greet me, intorduce yourself and tell me about the case, the crime scene ands the suspects. Don't reveal the murderer.", OnResponse));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public string ReadAllFileData(string name)
    {
        string filePath = Application.streamingAssetsPath + "/Scripts/contextful_parsing/data/" + name + ".txt";

        if (File.Exists(filePath))
        {
            string fileData = File.ReadAllText(filePath);

            return fileData;
        }

        return null;
    }

    public void OnQueryOpen(string name)
    {

        inputField.SetActive(true);
        inputFieldText.text = "";

        backButton.SetActive(true);

        int number;

        bool isInteger = int.TryParse(name, out number);

        if (isInteger)
        {
            suspectOptions.SetActive(true);
            helperOptions.SetActive(false);
            pastConversations.SetActive(true);

            for (int i = 0; i < 5; i++)
            {
                if(i == number)
                {
                    npcs[i].SetActive(true);
                    initialPosition = npcs[i].transform.position;
                    npcs[i].transform.position = new Vector3(-6f, 0.75f, 0f);

                    initialScale = npcs[i].transform.localScale;
                    npcs[i].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

                    pastConversationsText.text = ReadAllFileData("Suspect" + (i + 1).ToString());
                    pastConversationsTitle.text = starter.suspectNames[number] + "'s Past Conversation";
                }
                else
                {
                    npcs[i].SetActive(false);
                }
            }

            npcs[5].SetActive(false);
            npcs[6].SetActive(false);

            currentName = starter.suspectNames[number];
            currentNumber = number;
        }
        else
        {
            helperOptions.SetActive(true);
            suspectOptions.SetActive(false);
            pastConversations.SetActive(true);

            for (int i = 0; i < 5; i++)
            {
                npcs[i].SetActive(false);
            }

            if (name == "Inspector")
            {
                npcs[5].SetActive(true);
                npcs[6].SetActive(false);

                initialPosition = npcs[5].transform.position;
                npcs[5].transform.position = new Vector3(-6f, 0.75f, 0f);

                initialScale = npcs[5].transform.localScale;
                npcs[5].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

                pastConversationsText.text = ReadAllFileData("Inspector");
                pastConversationsTitle.text = "Inspector's Past Conversation";
                currentNumber = 5;
            }
            else 
            {
                if (name == "Sherlock Holmes")
                {
                    npcs[6].SetActive(true);
                    npcs[5].SetActive(false);

                    initialPosition = npcs[6].transform.position;
                    npcs[6].transform.position = new Vector3(-6f, 0.75f, 0f);

                    initialScale = npcs[6].transform.localScale;
                    npcs[6].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

                    pastConversationsText.text = ReadAllFileData("Sherlock");
                    pastConversationsTitle.text = "Sherlock's Past Conversation";

                    currentNumber = 6;
                }
            }

            currentName = name;
        }

        npcs[7].SetActive(false);


        helperOptionsAgain.SetActive(false);
        suspectOptionsAgain.SetActive(false);

        for(int i = 0; i < 7; i++)
        {
            npcOptions[i].SetActive(false);
        }
    }

    public void OnQueryClose()
    {
        pastConversations.SetActive(false);
        inputField.SetActive(false);
        outputField.SetActive(false);
        backButton.SetActive(false);

        helperOptions.SetActive(false);
        suspectOptions.SetActive(false);

        helperOptionsAgain.SetActive(false);
        suspectOptionsAgain.SetActive(false);

        for (int i = 0; i < 7; i++)
        {
            if (npcs[i].activeSelf)
            {
                npcs[i].transform.position = initialPosition;
                npcs[i].transform.localScale = initialScale;
            }
            npcs[i].SetActive(true);
        }

        npcs[7].SetActive(true);

        for (int i = 0; i < 7; i++)
        {
            npcOptions[i].SetActive(true);
        }
    }

    public void OnQuery()
    {
        outputFieldText.text = "";

        int number;

        bool isInteger = int.TryParse(currentName, out number);

        if (isInteger)
        {
            currentName = starter.suspectNames[number];
            currentNumber = number;
        } else
        {
            if(currentName == "Inspector")
            {
                currentNumber = 5;
            } else
            {
                if (currentName == "Sherlock Holmes")
                {
                    currentNumber = 6;
                }
                
            }
        }

        loader.SetActive(true);

        StartCoroutine(starter.SendQueries(currentName, inputFieldText.text, OnResponse));
    }

    void OnResponse(string response)
    {
        if (response != null)
        {
            if(helperOptions.activeSelf)
            {
                helperOptions.SetActive(false);
                helperOptionsAgain.SetActive(true);
            }

            if (suspectOptions.activeSelf)
            {
                suspectOptions.SetActive(false);
                suspectOptionsAgain.SetActive(true);
            }
            inputField.SetActive(false);
            outputField.SetActive(true);
            outputFieldText.text = currentName + ": " + response;
            if(currentNumber < 5)
            {
                AddDataToFile(currentName + ": " + response, "Suspect" + (currentNumber + 1).ToString());
                pastConversationsText.text = ReadAllFileData("Suspect" + (currentNumber + 1).ToString());
            } else
            {
                if(currentNumber == 5)
                {
                    AddDataToFile(currentName + ": " + response, "Inspector");
                    pastConversationsText.text = ReadAllFileData("Inspector");
                } else
                {
                    AddDataToFile(currentName + ": " + response, "Sherlock");
                    pastConversationsText.text = ReadAllFileData("Sherlock");
                }
            }
            
        }
        else
        {
            Debug.LogError("Failed to get a response");
        }

        loader.SetActive(false);
    }

    public void OnPastConversations(int num)
    {
        if(num < 5)
        {
            pastConversationsText.text = ReadAllFileData("Suspect" + (num + 1).ToString());
            pastConversationsTitle.text = starter.suspectNames[num] + "'s Past Conversation";
        } else
        {
            if(num == 5)
            {
                pastConversationsText.text = ReadAllFileData("Inspector");
                pastConversationsTitle.text = "Inspector's Past Conversation";
            } else
            {
                if (num == 6)
                {
                    pastConversationsText.text = ReadAllFileData("Sherlock");
                    pastConversationsTitle.text = "Sherlock's Past Conversation";
                }
            }
        }
        pastConversations.SetActive(true);
        
    }

    public void OnPastConversationsClose()
    {
        pastConversations.SetActive(false);
    }

    void AddDataToFile(string data, string fileName)
    {
        string filePath = Application.streamingAssetsPath + "/Scripts/contextful_parsing/data/" + fileName + ".txt";

        if (File.Exists(filePath))
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(data);
            }
        }
        else
        {
            UnityEngine.Debug.Log("Error in file edit");
        }
    }

    public void OnAgainHelper()
    {
        helperOptions.SetActive(true);
        inputField.SetActive(true);
        inputFieldText.text = "";
        outputField.SetActive(false);
        helperOptionsAgain.SetActive(false);
    }

    public void OnAgainSuspect()
    {
        suspectOptions.SetActive(true);
        inputField.SetActive(true);
        inputFieldText.text = "";
        outputField.SetActive(false);
        suspectOptionsAgain.SetActive(false);
    }

    public void OnNextButton()
    {
        mainCanvas.SetActive(false);
        nextCanvas.SetActive(true);
    }

    public void OnNextBackButton()
    {
        mainCanvas.SetActive(true);
        nextCanvas.SetActive(false);
    }

    public void OnNextNextButton()
    {
        loader.SetActive(true);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void OnAccuseButtonOuter(string name)
    {
        int number;

        bool isInteger = int.TryParse(name, out number);
        currentName = starter.suspectNames[number];

        mainCanvas.SetActive(false);
        accuseCanvas.SetActive(true);

        accuseCanvasText.text = "You are about to accuse " + currentName + ". Do you want to continue?\r\n(Note: This will end the story and reveal the murderer)";
    }

    public void OnAccuseButtonInner()
    {
        int number;

        bool isInteger = int.TryParse(currentName, out number);

        if (isInteger)
        {
            accuseCanvasText.text = "You are about to accuse " + starter.suspectNames[number] + ". Do you want to continue?\r\n(Note: This will end the story and reveal the murderer)";
        }
        else{
            accuseCanvasText.text = "You are about to accuse " + currentName + ". Do you want to continue?\r\n(Note: This will end the story and reveal the murderer)";
        }
        mainCanvas.SetActive(false);
        accuseCanvas.SetActive(true);
    }


    public void OnAccuseButtonOuterAccuse()
    {
        UnityEngine.Debug.Log(currentName);
    }

    public void OnAccuseButtonOuterBack()
    {
        mainCanvas.SetActive(true);
        accuseCanvas.SetActive(false);
    }

    public void OnAccuseFinal()
    {
        outputFieldText.text = "";
        string accusedName = accuseCanvasText.text.Split(" ")[5];
        loader.SetActive(true);
        currentNumber = 6;
        currentName = "Sherlock Holmes";
        OnQueryOpen("Sherlock Holmes");
        accuseCanvas.SetActive(false);
        mainCanvas.SetActive(true);
        backButton.SetActive(false);
        inputField.SetActive(false);
        outputField.SetActive(true);
        pastConversations.SetActive(false);
        helperOptions.SetActive(false);
        helperOptionsAgain.SetActive(false);
        loader.SetActive(true);
        helperOptionsNext.SetActive(true);
        StartCoroutine(starter.SendQueries("Sherlock Holmes Reveal", "I announce " + accusedName + " as the murderer. Am I right?", OnResponse));
    }

    public void OnPreviousButton()
    {
        mainCanvas.SetActive(false);
        previousCanvas.SetActive(true);
    }

    public void OnPreviousBackButton()
    {
        mainCanvas.SetActive(true);
        previousCanvas.SetActive(false);
    }

    public void OnPreviousNextButton()
    {
        loader.SetActive(true);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex - 1);
    }
}
