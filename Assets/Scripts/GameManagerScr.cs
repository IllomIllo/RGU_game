using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Game
{
    public List<Card> EnemyDeck, PlayerDeck;


    public Game()
    {
        EnemyDeck = GiveDeckCard();
        PlayerDeck = GiveDeckCard();

    }

    List<Card> GiveDeckCard()
    {
        List<Card> list = new List<Card>();
        for (int i = 0; i < 35; i++)
            list.Add(CardManager.AllCards[Random.Range(0, CardManager.AllCards.Count)]);
        return list;
    }
}

public class GameManagerScr : MonoBehaviour
{
    public static GameManagerScr Instance;
    public Game CurrentGame;
    public Transform EnemyHand, PlayerHand,
                     EnemyField, PlayerField;
    public GameObject CardPref;
    int Turn, TurnTime = 30;
    public TextMeshProUGUI TurnTimeTxt;
    public Button EndTurnBtn;

    public int PlayerMana = 10, EnemyMana = 10;
    public TextMeshProUGUI PlayerManaTxt, EnemyManaTxt;

    public int PlayerHP, EnemyHP;
    public TextMeshProUGUI PlayerHPTxt, EnemyHPTxt;

    public GameObject ResultGO;
    public TextMeshProUGUI ResultTxt;

    public AttackedHero EnemyHero, PlayerHero;

    public List<CardController> PlayerHandCards = new List<CardController>(),
                                PlayerFieldCards = new List<CardController>(),
                                EnemyHandCards = new List<CardController>(),
                                EnemyFieldCards = new List<CardController>();

    public bool IsPlayerTurn
    {
        get
        {
            return Turn % 2 == 0;
        }
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void StartGame()
    {
        Turn = 0;
        EndTurnBtn.interactable = true;

        CurrentGame = new Game();

        GiveHandCards(CurrentGame.EnemyDeck, EnemyHand);
        GiveHandCards(CurrentGame.PlayerDeck, PlayerHand);

        PlayerMana = EnemyMana = 10;
        PlayerHP = EnemyHP = 30;

        ShowHp();
        ShowMana();

        ResultGO.SetActive(false);

        StartCoroutine(TurnFunc());
    }

    public void RestartGame()
    {
        StopAllCoroutines();

        foreach (var card in PlayerHandCards)
            Destroy(card.gameObject);
        foreach (var card in PlayerFieldCards)
            Destroy(card.gameObject);
        foreach (var card in EnemyHandCards)
            Destroy(card.gameObject);
        foreach (var card in EnemyFieldCards)
            Destroy(card.gameObject);

        PlayerHandCards.Clear();
        PlayerFieldCards.Clear();
        EnemyHandCards.Clear();
        EnemyFieldCards.Clear();

        StartGame();
    }

    void Start()
    {
        StartGame();
    }

    void GiveHandCards(List<Card> deck, Transform hand)
    {
        int i = 0;
        while (i++ < 4)
            GiveCardToHand(deck, hand);
    }

    void GiveCardToHand(List<Card> deck, Transform hand)
    {
        if (deck.Count == 0) 
            return;

        CreateCardPref(deck[0], hand);

        deck.RemoveAt(0);
    }

    void CreateCardPref(Card card, Transform hand)
    {
        GameObject cardGO = Instantiate(CardPref, hand, false);
        CardController cardC = cardGO.GetComponent<CardController>();

        cardC.Init(card, hand == PlayerHand);

        if (cardC.IsPlayerCard)
            PlayerHandCards.Add(cardC);
        else
            EnemyHandCards.Add(cardC);
    }

    IEnumerator TurnFunc()
    {
        TurnTime = 30;
        TurnTimeTxt.text = TurnTime.ToString();

        foreach (var card in PlayerHandCards)
            card.Info.HighlightCard(false);

        CheckCardsForManaAvaliability();

        if (IsPlayerTurn)
        {
            foreach (var card in PlayerFieldCards)
            {

                card.Card.CanAttack = true;
                card.Info.HighlightCard(true);
                card.Ability.OnNewTurn();
            }

            while(TurnTime-- > 0)
            {
                TurnTimeTxt.text = TurnTime.ToString();
                yield return new WaitForSeconds(1);
            }

            ChangeTurn();
        }
        else
        {
            foreach (var card in EnemyFieldCards)
            {
                card.Card.CanAttack = true;
                card.Ability.OnNewTurn();
            }

            StartCoroutine(EnemyTurn(EnemyHandCards));
            
        }
    }

    IEnumerator EnemyTurn(List<CardController> cards)
    {
        yield return new WaitForSeconds(1);
        int count = cards.Count == 1 ? 1:
                Random.Range(0, cards.Count);
        

        for (int i = 0; i < count; i++)
        {
            if (EnemyFieldCards.Count > 5 ||
                EnemyMana == 0 ||
                EnemyHandCards.Count ==0)
                break;

            List<CardController> cardsList = cards.FindAll(x => EnemyMana >= x.Card.Manacost);

            if (cardsList.Count == 0)
                break;

            cardsList[0].GetComponent<CardMovementScr>().MoveToField(EnemyField);

            yield return new WaitForSeconds(.51f);

            cardsList[0].transform.SetParent(EnemyField);

            cardsList[0].OnCast();
        }

        yield return new WaitForSeconds(1);

        while(EnemyFieldCards.Exists(x => x.Card.CanAttack))
        {
            var activeCard = EnemyFieldCards.FindAll(x => x.Card.CanAttack)[0];
            bool hasProvacation = PlayerFieldCards.Exists(x => x.Card.IsProvacation);

            if (hasProvacation ||
                Random.Range(0, 2) == 0 &&
                PlayerFieldCards.Count > 0)
            {
                CardController enemy;

                if (hasProvacation)
                    enemy = PlayerFieldCards.Find(x => x.Card.IsProvacation);
                else
                    enemy = PlayerFieldCards[Random.Range(0, PlayerFieldCards.Count)];

                Debug.Log(activeCard.Card.Name + " (" + activeCard.Card.Attack + ";" + activeCard.Card.Defense + ") " + "--->" +
                          enemy.Card.Name + " (" + enemy.Card.Attack + ";" + enemy.Card.Defense + ") ");

                activeCard.Movement.MoveToTarget(enemy.transform);
                yield return new WaitForSeconds(.75f);

                CardsFight(enemy, activeCard);
            }
            else
            {
                Debug.Log(activeCard.Card.Name + " (" + activeCard.Card.Attack + ") Attacked Hero");

                activeCard.GetComponent<CardMovementScr>().MoveToTarget(PlayerHero.transform);
                yield return new WaitForSeconds(.75f);

                DamageHero(activeCard, false);

            }

            yield return new WaitForSeconds(.2f);
        }

        yield return new WaitForSeconds(1);
        ChangeTurn();
    }

    public void ChangeTurn()
    {
        StopAllCoroutines();
        Turn++;

        EndTurnBtn.interactable = IsPlayerTurn;

        if (IsPlayerTurn)
        {
            GiveNewCards();

            PlayerMana = EnemyMana = 10;

            ShowMana();
        }
        StartCoroutine(TurnFunc());
    }

    void GiveNewCards()
    {
        GiveCardToHand(CurrentGame.EnemyDeck, EnemyHand);
        GiveCardToHand(CurrentGame.PlayerDeck, PlayerHand);
    }

    public void CardsFight(CardController attacker, CardController defender)
    {
        defender.Card.GetDamage(attacker.Card.Attack);
        attacker.OnDamageDeal();
        defender.OnTakeDamage(attacker);

        attacker.Card.GetDamage(defender.Card.Attack);
        attacker.OnTakeDamage();

        attacker.CheckForAlive();
        defender.CheckForAlive();
    }

    void ShowMana()
    {
        PlayerManaTxt.text = PlayerMana.ToString();
        EnemyManaTxt.text = EnemyMana.ToString();
    }

    void ShowHp()
    {
        EnemyHPTxt.text = EnemyHP.ToString();
        PlayerHPTxt.text = PlayerHP.ToString();
    }

    public void ReduceMana(bool playerMana, int manacost)
    {
        if (playerMana)
            PlayerMana = Mathf.Clamp(PlayerMana - manacost, 0, int.MaxValue);
        else
            EnemyMana = Mathf.Clamp(EnemyMana - manacost, 0, int.MaxValue);

        ShowMana();
    }

    public void DamageHero(CardController card, bool isEnemyAttacked)
    {
        if (isEnemyAttacked)
            EnemyHP = Mathf.Clamp(EnemyHP - card.Card.Attack, 0, int.MaxValue);
        else
            PlayerHP = Mathf.Clamp(PlayerHP - card.Card.Attack, 0, int.MaxValue);

        ShowHp();
        card.OnDamageDeal();
        CheckForResult();
    }

    void CheckForResult()
    {
        if (EnemyHP == 0 || PlayerHP == 0)
        {
            ResultGO.SetActive(true);

            StopAllCoroutines();

            if (EnemyHP == 0)
                ResultTxt.text = "WIN gratz";
            else
                ResultTxt.text = "-25";
        }

        
    }

    public void CheckCardsForManaAvaliability()
    {
        foreach (var card in PlayerHandCards)
            card.Info.HighlightManaAvaliability(PlayerMana);
    }

    public void HighlightTargets(bool highlight)
    {
        List<CardController> targets = new List<CardController>();

        if (EnemyFieldCards.Exists(x => x.Card.IsProvacation))
            targets = EnemyFieldCards.FindAll(x => x.Card.IsProvacation);
        else
        {
            targets = EnemyFieldCards;
            EnemyHero.HighlightAsTarget(highlight);
        }

        foreach (var card in targets)
            card.Info.HighlightAsTarget(highlight);
    }
}
//11:55