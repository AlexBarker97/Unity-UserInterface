using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ColourFader : MonoBehaviour
{
    [SerializeField] private Image panelImage; // The Image component of the panel
    [SerializeField] private GameObject token1;
    [SerializeField] private GameObject token2;
    [SerializeField] private GameObject token3;

    private Coroutine fadeCoroutine;
    private float fadeDuration = 1f;
    private string fadeTarget = "grey";

    private bool boolToken1R, boolToken1Y, boolToken1B;
    private bool boolToken2R, boolToken2Y, boolToken2B;
    private bool boolToken3R, boolToken3Y, boolToken3B;

    private void Update()
    {
        CheckBooleanCombinations();
    }

    private void CheckBooleanCombinations()
    {
        boolToken1R = token1.GetComponent<DraggableToken>().selectedRed;
        boolToken1Y = token1.GetComponent<DraggableToken>().selectedYellow;
        boolToken1B = token1.GetComponent<DraggableToken>().selectedBlue;
        boolToken2R = token2.GetComponent<DraggableToken>().selectedRed;
        boolToken2Y = token2.GetComponent<DraggableToken>().selectedYellow;
        boolToken2B = token2.GetComponent<DraggableToken>().selectedBlue;
        boolToken3R = token3.GetComponent<DraggableToken>().selectedRed;
        boolToken3Y = token3.GetComponent<DraggableToken>().selectedYellow;
        boolToken3B = token3.GetComponent<DraggableToken>().selectedBlue;
        int redCount = (boolToken1R ? 1 : 0) + (boolToken2R ? 1 : 0) + (boolToken3R ? 1 : 0);
        int yellowCount = (boolToken1Y ? 1 : 0) + (boolToken2Y ? 1 : 0) + (boolToken3Y ? 1 : 0);
        int blueCount = (boolToken1B ? 1 : 0) + (boolToken2B ? 1 : 0) + (boolToken3B ? 1 : 0);

        if (redCount == 1 && yellowCount == 1 && blueCount == 1 && fadeTarget != "white")
        {
            FadeToSpecificColor(Color.white);
            fadeTarget = "white";
            Debug.Log("whitecalled");
        }
        else if (redCount > 0 && blueCount > 0 && yellowCount == 0 && fadeTarget != "purple")
        {
            FadeToSpecificColor(new Color(0.5f, 0f, 0.5f));
            fadeTarget = "purple";
        }
        else if (blueCount > 0 && yellowCount > 0 && redCount == 0 && fadeTarget != "green")
        {
            FadeToSpecificColor(Color.green);
            fadeTarget = "green";
        }
        else if (yellowCount > 0 && redCount > 0 && blueCount == 0 && fadeTarget != "orange")
        {
            FadeToSpecificColor(new Color(1f, 0.65f, 0f));
            fadeTarget = "orange";
        }
        else if (redCount > 0 && yellowCount == 0 && blueCount == 0 && fadeTarget != "red")
        {
            FadeToSpecificColor(Color.red);
            fadeTarget = "red";
        }
        else if (yellowCount > 0 && redCount == 0 && blueCount == 0 && fadeTarget != "yellow")
        {
            FadeToSpecificColor(Color.yellow);
            fadeTarget = "yellow";
        }
        else if (blueCount > 0 && redCount == 0 && yellowCount == 0 && fadeTarget != "blue")
        {
            FadeToSpecificColor(Color.blue);
            fadeTarget = "blue";
        }
        Debug.Log(fadeTarget);
    }

    public void FadeToSpecificColor(Color targetColor)
    {
        fadeCoroutine = StartCoroutine(FadeToColour(targetColor));
    }

    private IEnumerator FadeToColour(Color targetColor)
    {
        Color startColor = panelImage.color;
        float timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            panelImage.color = Color.Lerp(startColor, targetColor, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        //panelImage.color = targetColor;
    }
}