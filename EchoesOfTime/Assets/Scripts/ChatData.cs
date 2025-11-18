using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChatData", menuName = "Scriptable Objects/ChatData")]
public class ChatData : ScriptableObject
{
    public enum Emotion : int
    {
        NONE = 0,
        EXCITED = 1,
        SCARED = 2,
        HATE = 3,
        RANDOM = 4
    }

    [System.Serializable]
    public struct User
    {
        [SerializeField] public string Name;
        [SerializeField] public Color Color;
    }

    [Header("Users")]
    public List<User> users;

    [Header("Messages")]
    public List<string> defaultMessages;
    public List<string> excitedMessages;
    public List<string> scaredMessages;
    public List<string> hateMessages;

    public (User, string) GetMessage(Emotion emotion)
    {
        User user = users[Random.Range(0, users.Count)];
        string message;
        switch (emotion)
        {
            case Emotion.NONE:
                message = defaultMessages[Random.Range(0, defaultMessages.Count)]; 
                break;
            case Emotion.EXCITED:
                message = excitedMessages[Random.Range(0, excitedMessages.Count)];
                break;
            case Emotion.SCARED:
                message = scaredMessages[Random.Range(0, scaredMessages.Count)];
                break;
            case Emotion.HATE:
                message = hateMessages[Random.Range(0, hateMessages.Count)];
                break;
            case Emotion.RANDOM:
                return GetMessage((Emotion)Random.Range((int)Emotion.NONE, (int)Emotion.HATE));
            default:
                throw new System.Exception("Unavailable emotion");
        }
        return (user, message);
    }
}
