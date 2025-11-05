using UnityEngine;

public class ShardPickup : MonoBehaviour
{
    [SerializeField] private int shardValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShardManagerTMP.Instance.AddShard(shardValue);
            Destroy(gameObject);
        }
    }
}