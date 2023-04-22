using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField] private ShipPlayer player;
    void Update()
    {
        if (player)
        {
            transform.localScale = new Vector3(player.GetHealth() / player.GetMaxHealth(), 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(0, 1, 1);
        }
    }
}
