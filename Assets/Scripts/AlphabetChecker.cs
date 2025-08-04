using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlphabetChecker : TMP_InputValidator
{
    public override char Validate(ref string text, ref int pos, char ch)
    {
        if (char.IsLetter(ch))
        {
            text = text.Insert(pos, ch.ToString().ToUpper());
            pos++;
            return ch;
        }
        return '\0'; // Reject non-letter characters
    }
}
