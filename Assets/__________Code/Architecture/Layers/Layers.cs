using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layers : MonoBehaviour
{
    public static LayerMask Ground { get; }
    public static LayerMask GroundAndPlatforms { get; }
    public static LayerMask Items { get; }
    public static LayerMask CharactersAndItems { get; }
    public static LayerMask CharactersAndGround { get; }
    public static LayerMask PlayerAndGround { get; }
    public static LayerMask UI { get; }

    static Layers()
    {
        Ground = LayerMask.GetMask("Ground");
        GroundAndPlatforms = LayerMask.GetMask("Ground", "Platforms");
        Items = LayerMask.GetMask("Items");
        CharactersAndItems = LayerMask.GetMask("Characters", "Items");
        CharactersAndGround = LayerMask.GetMask("Characters", "Ground");
        PlayerAndGround = LayerMask.GetMask("Player", "Ground");
        UI = LayerMask.GetMask("UI");
    }
}
