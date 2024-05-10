using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatNames
{
    public static string Life        => "Life";
    public static string Attack      => "Attack";
    public static string Defend      => "Defend";
    public static string MoveSpeed   => "MoveSpeed";
    public static string SprintSpeed => "SprintSpeed";
    public static string JumpForce   => "JumpForce";

    public static List<string> AllNames { get; } = new List<string>() 
    {
        Life,
        Attack, 
        Defend,
        MoveSpeed, 
        SprintSpeed, 
        JumpForce
    };
}
