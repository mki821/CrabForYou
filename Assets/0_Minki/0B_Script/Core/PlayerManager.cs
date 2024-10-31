using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    public Player Player { get; set; }
    public Transform PlayerTrm { get; set; }
}
