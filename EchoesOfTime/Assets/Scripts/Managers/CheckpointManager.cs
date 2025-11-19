using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private static CheckpointManager instance;
    public static CheckpointManager Instance {  get { return instance; } }

    public Player player;

    [SerializeField] private Checkpoint[] checkpoints;
    [SerializeField] private int currentCheckpoint = 0;

    void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }

    public void SetCheckPoint(Checkpoint checkpoint)
    {
        for(int i = 0; i < checkpoints.Length; i++) 
            if(checkpoints[i] == checkpoint)
                currentCheckpoint = i;
    }

    public void Respawn()
    {
        Debug.Log(player.transform.position);
        player.transform.position = checkpoints[currentCheckpoint].transform.position;
        Debug.Log(player.transform.position);
    }
}
