using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{

    private static ChatManager instance;
    public static ChatManager Instance { get => instance; }

    [SerializeField] private ChatData chatData;
    [SerializeField] private float nextMessageDelay;

    [SerializeField] private float minMessageDelay;
    [SerializeField] private float maxMessageDelay;

    [SerializeField] private int maxMessageCount;

    [SerializeField] private float messageLifespan;

    private Queue<(ChatData.User, string)> currentMessages = new Queue<(ChatData.User, string)>();
    private Queue<float> currentMessagesLife = new Queue<float>();

    [Header("UI References")]
    [SerializeField] private GameObject chatMessagePrefab; 
    [SerializeField] private Transform chatContainer;      //Vertical layout
    private float nextMessageTimer;
    [SerializeField] private TMP_Text chatUI;

    void Start()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        HandleBackgroundChat();
    }

    private void HandleBackgroundChat()
    {
        nextMessageTimer -= Time.deltaTime;

        if (nextMessageTimer <= 0)
        {
           
            (ChatData.User user, string message) = chatData.GetMessage(ChatData.Emotion.RANDOM);

            CreateVisualMessage(user.Name, message, user.Color);

            nextMessageTimer = Random.Range(minMessageDelay, maxMessageDelay);
        }
    }

    public void OnTrickPerformed(string trickName, int score)
    {
      

        ChatData.User randomUser = chatData.users[Random.Range(0, chatData.users.Count)];

       
        string hypeMessage = $"WOAH! {trickName}!! <color=yellow>+{score}</color>";

      
        CreateVisualMessage(randomUser.Name, hypeMessage, randomUser.Color);

      
        nextMessageTimer = Random.Range(2f, 4f);
    }

  
    public void OnFail()
    {
       
        (ChatData.User user, string message) = chatData.GetMessage(ChatData.Emotion.HATE);
        CreateVisualMessage(user.Name, message, user.Color);
    }

    private void CreateVisualMessage(string name, string text, Color color)
    {
      
        GameObject newMsgObj = Instantiate(chatMessagePrefab, chatContainer);

       
        ChatMessageController controller = newMsgObj.GetComponent<ChatMessageController>();
        if (controller != null)
        {
            controller.Setup(name, text, color, messageLifespan);
        }

       
        if (chatContainer.childCount > maxMessageCount)
        {
          
            Destroy(chatContainer.GetChild(0).gameObject);
        }
    }
}
