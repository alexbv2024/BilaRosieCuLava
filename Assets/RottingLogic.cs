using System;
using UnityEngine;

public class RottingLogic : MonoBehaviour
{
    private void OnMouseDown()
    {
        new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
