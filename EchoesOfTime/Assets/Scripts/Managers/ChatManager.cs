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
        nextMessageDelay -= Time.deltaTime;
        UpdateLifespans();

        Debug.Log(currentMessages.Count);

        if(nextMessageDelay <= 0)
        {
            currentMessages.Enqueue(chatData.GetMessage(ChatData.Emotion.RANDOM));
            currentMessagesLife.Enqueue(messageLifespan);
            nextMessageDelay = Random.Range(minMessageDelay, maxMessageDelay);
        }

        UpdateText();
    }

    private void UpdateLifespans()
    {
        for (int i = currentMessagesLife.Count; i > 0; i--)
        {
            float life = currentMessagesLife.Dequeue();
            currentMessagesLife.Enqueue(life - Time.deltaTime);
        }
        while(currentMessagesLife.Count > 0 && currentMessagesLife.Last() <= 0)
        {
            currentMessagesLife.Dequeue();
            currentMessages.Dequeue();
        }
    }

    private void UpdateText()
    {
        chatUI.text = "";
        foreach(var message in currentMessages.Reverse())
        {
            chatUI.text += $"<color=#{message.Item1.Color.ToHexString()}>{message.Item1.Name}</color> {message.Item2}\n";
        }
    }
}
