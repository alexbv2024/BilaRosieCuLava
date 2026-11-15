using UnityEngine;

public class LavaTrigger : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        LogicaMovement playerMovement = collision.gameObject.GetComponentInParent<LogicaMovement>();

        if (playerMovement != null || collision.gameObject.CompareTag("Player"))
        {
            if (playerMovement != null)
            {
                playerMovement.DieInLava();
            }

            gameObject.SetActive(false);
        }
    }
}