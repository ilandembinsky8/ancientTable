using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;

public class LoadingBar : MonoBehaviour
{
    [SerializeField] private Image _fillBar;

    private Action _onLoadingComplete;

    public void StartBarLoading(float loadingTime)
    {
        StartCoroutine(FillBar(loadingTime));
    }

    /// <summary>
    /// Pass anything you would like to happen once the bar finishes loading
    /// </summary>
    /// <param name="onLoadingComplete"></param>
    public void SetActionOnLoadingComplete(Action onLoadingComplete)
    {
        _onLoadingComplete = onLoadingComplete;
    }

    private IEnumerator FillBar(float loadingTime)
    {
        float elapsedTime = 0f;
        while (elapsedTime < loadingTime)
        {
            elapsedTime += Time.deltaTime;
            float fillAmount = Mathf.Clamp01(elapsedTime / loadingTime);
            _fillBar.fillAmount = fillAmount;
            yield return null;
        }
        _fillBar.fillAmount = 1f;

        _onLoadingComplete?.Invoke();
    }

}