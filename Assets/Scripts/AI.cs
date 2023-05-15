using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public void MakeTurn()
    {
        StartCoroutine(EnemyTurn(GameManagerScr.Instance.EnemyHandCards));
    }


    IEnumerator EnemyTurn(List<CardController> cards)
    {
        yield return new WaitForSeconds(1);
        int count = cards.Count == 1 ? 1 :
                Random.Range(0, cards.Count);


        for (int i = 0; i < count; i++)
        {
            if (GameManagerScr.Instance.EnemyFieldCards.Count > 5 ||
                GameManagerScr.Instance.CurrentGame.Enemy.Mana == 0 ||
                GameManagerScr.Instance.EnemyHandCards.Count == 0)
                break;

            List<CardController> cardsList = cards.FindAll(x => GameManagerScr.Instance.CurrentGame.Enemy.Mana >= x.Card.Manacost);

            if (cardsList.Count == 0)
                break;

            cardsList[0].GetComponent<CardMovementScr>().MoveToField(GameManagerScr.Instance.EnemyField);

            yield return new WaitForSeconds(.51f);

            cardsList[0].transform.SetParent(GameManagerScr.Instance.EnemyField);

            cardsList[0].OnCast();
        }

        yield return new WaitForSeconds(1);

        while (GameManagerScr.Instance.EnemyFieldCards.Exists(x => x.Card.CanAttack))
        {
            var activeCard = GameManagerScr.Instance.EnemyFieldCards.FindAll(x => x.Card.CanAttack)[0];
            bool hasProvacation = GameManagerScr.Instance.PlayerFieldCards.Exists(x => x.Card.IsProvacation);

            if (hasProvacation ||
                Random.Range(0, 2) == 0 &&
                GameManagerScr.Instance.PlayerFieldCards.Count > 0)
            {
                CardController enemy;

                if (hasProvacation)
                    enemy = GameManagerScr.Instance.PlayerFieldCards.Find(x => x.Card.IsProvacation);
                else
                    enemy = GameManagerScr.Instance.PlayerFieldCards[Random.Range(0, GameManagerScr.Instance.PlayerFieldCards.Count)];

                Debug.Log(activeCard.Card.Name + " (" + activeCard.Card.Attack + ";" + activeCard.Card.Defense + ") " + "--->" +
                          enemy.Card.Name + " (" + enemy.Card.Attack + ";" + enemy.Card.Defense + ") ");

                activeCard.Card.CanAttack = false; //!!!!!!!!!!!!!!!!!!!!!!   

                activeCard.Movement.MoveToTarget(enemy.transform);
                yield return new WaitForSeconds(.75f);

                GameManagerScr.Instance.CardsFight(enemy, activeCard);
            }
            else
            {
                Debug.Log(activeCard.Card.Name + " (" + activeCard.Card.Attack + ") Attacked Hero");

                activeCard.Card.CanAttack = false; //!!!!!!!!!!!!!!!!!!!!!!   

                activeCard.GetComponent<CardMovementScr>().MoveToTarget(GameManagerScr.Instance.PlayerHero.transform);
                yield return new WaitForSeconds(.75f);

                GameManagerScr.Instance.DamageHero(activeCard, false);

            }

            yield return new WaitForSeconds(.2f);
        }

        yield return new WaitForSeconds(1);
        GameManagerScr.Instance.ChangeTurn();
    }
}
