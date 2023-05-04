using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Card
{
    public enum AbilityType
    {
        NO_ABILITY,
        INSTANT_ACTIVE,
        DOUBLE_ATTACK,
        SHIELD,
        PROVACATION,
        REGENERATION_EACH_TURN,
        COUNTER_ATTACK
    }


    public string Name;
    public Sprite Logo;
    public int Attack, Defense, Manacost;
    public bool CanAttack;
    public bool IsPlaced;


    public List<AbilityType> Abilities;

    public int TimesDealedDamage;

    public bool IsAlive
    {
        get
        {
            return Defense > 0;
        }
    }
    public bool HasAbility
    {
        get
        {
            return Abilities.Count > 0;
        }
    }
    public bool IsProvacation
    {
        get
        {
            return Abilities.Exists(x => x == AbilityType.PROVACATION);
        }
    }

    public Card(string name, string logoPath, int attack, int defense, int manacost,
                AbilityType abilityType = 0)
    {
        Name = name;
        Logo = Resources.Load<Sprite>(logoPath);
        Attack = attack;
        Defense = defense;
        Manacost = manacost;
        CanAttack = false;
        IsPlaced = false;

        Abilities = new List<AbilityType>();

        if (abilityType != 0)
            Abilities.Add(abilityType);

        TimesDealedDamage = 0;
    }

    public void GetDamage(int dmg)
    {
        if (dmg > 0)
        {
            if (Abilities.Exists(x => x == AbilityType.SHIELD))
                Abilities.Remove(AbilityType.SHIELD);
            else
                Defense -= dmg;
        }
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
        CardManager.AllCards.Add(new Card("LD","Sprites/Cards/LD", 5, 4, 3));
        CardManager.AllCards.Add(new Card("Brother","Sprites/Cards/Brother", 5, 5, 3));
        CardManager.AllCards.Add(new Card("Decan","Sprites/Cards/Decan", 9, 5, 7));
        CardManager.AllCards.Add(new Card("Otlichnik","Sprites/Cards/Otlichnik", 1, 5, 2));
        CardManager.AllCards.Add(new Card("Starosta","Sprites/Cards/Starosta", 2, 14, 5));
        CardManager.AllCards.Add(new Card("Starshecursnik","Sprites/Cards/Starshecursnik", 1, 1, 1));
        CardManager.AllCards.Add(new Card("Laborant","Sprites/Cards/Laborant", 2, 4, 2));

        CardManager.AllCards.Add(new Card("provacation", "Sprites/Cards/Provacation", 2, 12, 4, Card.AbilityType.PROVACATION));
        CardManager.AllCards.Add(new Card("regeneration", "Sprites/Cards/Regeneration", 4, 4, 4, Card.AbilityType.REGENERATION_EACH_TURN));
        CardManager.AllCards.Add(new Card("doubleAttack", "Sprites/Cards/DB_Attack", 3, 8, 7, Card.AbilityType.DOUBLE_ATTACK));
        CardManager.AllCards.Add(new Card("instantActive", "Sprites/Cards/I_Active", 2, 4, 3, Card.AbilityType.INSTANT_ACTIVE));
        CardManager.AllCards.Add(new Card("Shield", "Sprites/Cards/Shield_Have", 6, 2, 5, Card.AbilityType.SHIELD));
        CardManager.AllCards.Add(new Card("counterAttack", "Sprites/Cards/Counter_Attack", 4, 6, 2, Card.AbilityType.COUNTER_ATTACK));
        


    }
}
   

