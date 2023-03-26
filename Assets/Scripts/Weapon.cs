using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Team team;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float knockbackAmount;
    [SerializeField] private int shotCooldown;
    [SerializeField] private float shotSpread;
    [SerializeField] private int shotAmount;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public Team GetTeam()
    {
        return team;
    }
    public void SetTeam(Team newTeam)
    {
        team = newTeam;
    }
}
