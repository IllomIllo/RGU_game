using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Card
{
    public string Name;
    public Sprite Logo;
    public int Attack, Defense;

    public Card(string name, string logoPath, int attack, int defense)
    {
        Name = name;
        Logo = Resources.Load<Sprite>(logoPath);
        Attack = attack;
        Defense = defense;
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
        CardManager.AllCards.Add(new Card("LD","Sprites/Cards/LD", 5, 5));
        CardManager.AllCards.Add(new Card("Brother","Sprites/Cards/Brother", 5, 5));
        CardManager.AllCards.Add(new Card("Decan","Sprites/Cards/Decan", 5, 5));
        CardManager.AllCards.Add(new Card("Otlichnik","Sprites/Cards/Otlichnik", 5, 5));
        CardManager.AllCards.Add(new Card("Starosta","Sprites/Cards/Starosta", 5, 5));
        CardManager.AllCards.Add(new Card("Starshecursnik","Sprites/Cards/Starshecursnik", 5, 5));
        CardManager.AllCards.Add(new Card("Laborant","Sprites/Cards/Laborant", 5, 5));
    }
}
   

