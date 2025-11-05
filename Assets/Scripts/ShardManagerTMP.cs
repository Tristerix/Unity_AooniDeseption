using UnityEngine;
using TMPro;

public class ShardManagerTMP : MonoBehaviour
{
    public static ShardManagerTMP Instance;

    [SerializeField] private TextMeshProUGUI shardText;
    private int currentShards = 0;
    private int totalShards = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        CountTotalShards();
        UpdateUI();
    }

    private void CountTotalShards()
    {
        // シーン内にある全てのシャード数を数える
        totalShards = FindObjectsOfType<ShardPickup>().Length;
    }

    public void AddShard(int amount)
    {
        currentShards += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (currentShards <= totalShards)
        {
            shardText.text = $"Shards: {currentShards} / {totalShards}";
        }
        if (currentShards == totalShards)
        {
            shardText.text = $"Complete. Return to the entrance.";
        }
    }
}
