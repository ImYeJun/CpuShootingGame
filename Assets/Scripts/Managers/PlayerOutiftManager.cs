using System.Collections.Generic;
using UnityEngine;

public class PlayerOutiftManager : MonoBehaviour
{
    [SerializeField] private List<PlayerOutfit> equippedOutfits = new List<PlayerOutfit>();
    [SerializeField] private PlayerOutfit currentOutift;

    public void SetPlayerOutfit(int outfitCode){
        // outfitCode corresponds to equippedOutfits' index;
    }
}
