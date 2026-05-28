using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Image = UnityEngine.UIElements.Image;

public class TitleController : MonoBehaviour
{
    //Inspector Drag references
    [SerializeField] private Animator modelAnimator;
    [SerializeField] private TextMeshPro[] texts = new TextMeshPro[3];
    private int textIndex;
    [SerializeField] private float fadeTime= 0.5f;
    private float fadeTimer;
    private bool fading;
    [SerializeField] private RawImage fadeImage;

    private void Start()
    {
        //set the model to start waving
        modelAnimator.SetBool("Waving", true);
    }

    private void OnAnyKey()
    {
        //if we are already fading
        if (fading) return;
        
        //if we are not fading, start
        textIndex += 1;
        fadeTimer = 0;
        fading = true;
    }

    private void Update()
    {
        if (!fading) return;

        if (fadeTimer < fadeTime)
        {
            //temp variable the last text to FADE OUT
            var tempColor0 = texts[textIndex-1].color;
            
            //? operator to check if we are at the end of texts array, if yes set the fadeScreen for FADE IN
            //if not set the upcoming text for FADE IN
            var tempColor1 = textIndex >= texts.Length ? fadeImage.color : texts[textIndex].color;
            
            //lerping new values
            tempColor0 = new Color(tempColor0.r,tempColor0.g,tempColor0.b ,Mathf.Lerp(1, 0, fadeTimer / fadeTime));
            tempColor1 = new Color(tempColor1.r,tempColor1.g,tempColor1.b ,Mathf.Lerp(0, 1, fadeTimer / fadeTime));
            
            //plugging temp values back in
            texts[textIndex - 1].color = tempColor0;
            
            //plugging FADE IN dependent on if it's text or fadeScreen
            if (textIndex >= texts.Length) fadeImage.color = tempColor1;
            else texts[textIndex].color = tempColor1;
            
            fadeTimer += Time.deltaTime;
        }

        else
        {
            //Reset Timer
            fadeTimer = 0;
            
            //Go to next Scene if black screen faded in fully
            if (textIndex >= texts.Length) SceneManager.LoadScene("SampleScene");
            
            //exit loop
            fading = false;
        }
            
        
        
    }
}
