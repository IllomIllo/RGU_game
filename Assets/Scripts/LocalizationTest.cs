using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationTest : MonoBehaviour
{
    [SerializeField]
    private LocalizedText text;

    public void LocalizeText()
    {
        text.Localize("Test2_key");
    }
}
