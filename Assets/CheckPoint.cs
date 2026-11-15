using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Transform spawnPointPosition;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LogicaMovement playerMovement = collision.GetComponent<LogicaMovement>();
            if (playerMovement != null)
            {
                playerMovement.SetCheckPoint(spawnPointPosition.position);
            }
        }
    }
}
