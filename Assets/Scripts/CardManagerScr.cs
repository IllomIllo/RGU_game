using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Card
{
    public string Name;
    public Sprite Logo;
    public int Attack, Defense, Manacost;
    public bool CanAttack;
    public bool IsPlaced;

    public bool IsAlive
    {
        get
        {
            return Defense > 0;
        }
    }

    public Card(string name, string logoPath, int attack, int defense, int manacost)
    {
        Name = name;
        Logo = Resources.Load<Sprite>(logoPath);
        Attack = attack;
        Defense = defense;
        Manacost = manacost;
        CanAttack = false;
        IsPlaced = false;
    }

    public void ChangeAttackState(bool can)
    {
        CanAttack = can;
    }

    public void GetDamage(int dmg)
    {
        Defense -= dmg;
    }


}

public static class CardManager
{
    public static List<Card> AllCards = new List<Card>();
}
public class CardManagerScr : MonoBehaviour
{
    // список карт
    public void Awake()
    {
        CardManager.AllCards.Add(new Card("LD","Sprites/Cards/LD", 5, 4, 40));
        CardManager.AllCards.Add(new Card("Brother","Sprites/Cards/Brother", 5, 5, 3));
        CardManager.AllCards.Add(new Card("Decan","Sprites/Cards/Decan", 9, 5, 7));
        CardManager.AllCards.Add(new Card("Otlichnik","Sprites/Cards/Otlichnik", 1, 5, 2));
        CardManager.AllCards.Add(new Card("Starosta","Sprites/Cards/Starosta", 2, 14, 5));
        CardManager.AllCards.Add(new Card("Starshecursnik","Sprites/Cards/Starshecursnik", 1, 1, 1));
        CardManager.AllCards.Add(new Card("Laborant","Sprites/Cards/Laborant", 2, 4, 2));

    }
}
   

