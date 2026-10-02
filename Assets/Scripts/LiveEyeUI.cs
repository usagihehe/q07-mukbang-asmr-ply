using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LiveEyeUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI numberView;
    private int number = 999;
    private Coroutine coroutine;
    WaitForSeconds wait = new WaitForSeconds(0.5f);
    public void Start()
    {
        coroutine = StartCoroutine(OnNumberIncrease());
    }

    IEnumerator OnNumberIncrease()
    {
        while (true)
        {
            number += Random.Range(-10, 50);
            numberView.text = number.ToString();
            yield return wait;
        }

    }

    public void OnDisable()
    {
        StopCoroutine(coroutine);
    }
}
