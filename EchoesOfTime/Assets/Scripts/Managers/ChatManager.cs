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

    [SerializeField] private float minMessageDelayBurst;
    [SerializeField] private float maxMessageDelayBurst;

    private bool isBursting = false;
    private ChatData.Emotion burstEmotion;

    [SerializeField] private float burstDuration;
    [SerializeField] private float curBurstDuration;

    [SerializeField] private int maxMessageCount;

    [SerializeField] private float messageLifespan;

    private Queue<(ChatData.User, string)> currentMessages = new Queue<(ChatData.User, string)>();
    private Queue<float> currentMessagesLife = new Queue<float>();

    [SerializeField] private TMP_Text chatUI;

    void Start()
    {
        if (instance != null)
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
        if (isBursting) curBurstDuration += Time.deltaTime;
        UpdateLifespans();

        if (curBurstDuration > burstDuration)
        {
            isBursting = false;
        }

        if (nextMessageDelay <= 0)
        {
            currentMessages.Enqueue((isBursting)
                ? chatData.GetMessage(burstEmotion)
                : chatData.GetMessage(ChatData.Emotion.NONE)
                );
            currentMessagesLife.Enqueue(messageLifespan);
            nextMessageDelay = Random.Range(
                (!isBursting) ? minMessageDelay : minMessageDelayBurst,
                (!isBursting) ? maxMessageDelay : maxMessageDelayBurst
                );
        }

        UpdateText();

        while (currentMessages.Count > maxMessageCount)
        {
            currentMessages.Dequeue();
            currentMessagesLife.Dequeue();
        }
    }

    private void UpdateLifespans()
    {
        for (int i = currentMessagesLife.Count; i > 0; i--)
        {
            float life = currentMessagesLife.Dequeue();
            if (life <= 0) currentMessages.Dequeue();
            else currentMessagesLife.Enqueue(life - Time.deltaTime);
        }
    }

    public void MessageBurst(ChatData.Emotion emotion)
    {
        isBursting = true;
        burstEmotion = emotion;
        curBurstDuration = 0;
    }

    private void UpdateText()
    {
        chatUI.text = "";
        foreach (var message in currentMessages)
        {
            chatUI.text += $"<color=#{message.Item1.Color.ToHexString()}>{message.Item1.Name}</color> {message.Item2}\n";
        }
    }
}
