using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class MVPText : MonoBehaviour
{
    TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    /// <summary>
    /// テキストを更新する
    /// </summary>
    /// <param name="t"></param>
    public void SetText(string t)
    {
        if (_text)
        {
            _text.text = t;
        }
    }
}