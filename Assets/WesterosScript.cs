using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;

public class WesterosScript : MonoBehaviour
{
    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMSelectable[] buttons;
    public KMSelectable submitButton;
    public SigilDisplay sigilDisplayed;
    public TextDisplay[] currentlyDisplayed;

    public Material[] sigilOptions;
    public String[] forenameOptions;
    public String[] familyNameOptions;
    public String[] wordsOptions;
    public String[] seatOptions;

    private List<int> chosenIndices = new List<int>();
    private List<int> tempChosenIndices = new List<int>();
    private int correctIndex = 0;
    public String[] correctAnswers;

    //Logging
    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    void Awake()
    {
        moduleId = moduleIdCounter++;
        foreach (KMSelectable button in buttons)
        {
            KMSelectable pressedButton = button;
            button.OnInteract += delegate () { PressButton(pressedButton); return false; };
        }
        submitButton.OnInteract += delegate () { PressSubmit(); return false; };
    }


    void Start()
    {
        PickCorrectAnswer();
        PickDecoys();
    }

    void PickCorrectAnswer()
    {
        correctIndex = UnityEngine.Random.Range(0,20);
        correctAnswers[0] = sigilOptions[correctIndex].name;
        int forenameIndex = UnityEngine.Random.Range(0,5);
        correctAnswers[1] = forenameOptions[(correctIndex * 5) + forenameIndex];
        correctAnswers[2] = familyNameOptions[correctIndex];
        correctAnswers[3] = wordsOptions[correctIndex];
        correctAnswers[4] = seatOptions[correctIndex];
        for(int i = 1; i < 5; i++)
        {
            currentlyDisplayed[i-1].textOptions[0] = correctAnswers[i];
        }
        sigilDisplayed.sigilOptions[0] = sigilOptions[correctIndex];
        for(int i = 0; i < 5; i++)
        {
            chosenIndices.Add(correctIndex);
        }
        Debug.LogFormat("[Westeros #{0}] Submit {1}.", moduleId, string.Join(", ", correctAnswers.Select((x) => x).ToArray()));
    }

    void PickDecoys()
    {
        for(int i = 0; i < 4; i++)
        {
            int decoyIndex = UnityEngine.Random.Range(0,20);
            while(chosenIndices.Where((x) => x.Equals(decoyIndex)).Count() > 3 || tempChosenIndices.Contains(decoyIndex))
				    {
                decoyIndex = UnityEngine.Random.Range(0,20);
            }
            chosenIndices.Add(decoyIndex);
            tempChosenIndices.Add(decoyIndex);
            int nameIndex = UnityEngine.Random.Range(0,5);
            currentlyDisplayed[0].textOptions[i+1] = forenameOptions[(decoyIndex * 5) + nameIndex];
        }
        tempChosenIndices.Clear();

        for(int i = 0; i < 4; i++)
        {
            int decoyIndex = UnityEngine.Random.Range(0,20);
            while(chosenIndices.Where((x) => x.Equals(decoyIndex)).Count() > 3 || tempChosenIndices.Contains(decoyIndex))
				    {
                decoyIndex = UnityEngine.Random.Range(0,20);
            }
            chosenIndices.Add(decoyIndex);
            tempChosenIndices.Add(decoyIndex);
            currentlyDisplayed[1].textOptions[i+1] = familyNameOptions[decoyIndex];
        }
        tempChosenIndices.Clear();

        for(int i = 0; i < 4; i++)
        {
            int decoyIndex = UnityEngine.Random.Range(0,20);
            while(chosenIndices.Where((x) => x.Equals(decoyIndex)).Count() > 3 || tempChosenIndices.Contains(decoyIndex))
				    {
                decoyIndex = UnityEngine.Random.Range(0,20);
            }
            chosenIndices.Add(decoyIndex);
            tempChosenIndices.Add(decoyIndex);
            currentlyDisplayed[2].textOptions[i+1] = wordsOptions[decoyIndex];
        }
        tempChosenIndices.Clear();

        for(int i = 0; i < 4; i++)
        {
            int decoyIndex = UnityEngine.Random.Range(0,20);
            while(chosenIndices.Where((x) => x.Equals(decoyIndex)).Count() > 3 || tempChosenIndices.Contains(decoyIndex))
				    {
                decoyIndex = UnityEngine.Random.Range(0,20);
            }
            chosenIndices.Add(decoyIndex);
            tempChosenIndices.Add(decoyIndex);
            currentlyDisplayed[3].textOptions[i+1] = seatOptions[decoyIndex];
        }
        tempChosenIndices.Clear();

        for(int i = 0; i < 4; i++)
        {
            int decoyIndex = UnityEngine.Random.Range(0,20);
            while(chosenIndices.Where((x) => x.Equals(decoyIndex)).Count() > 3 || tempChosenIndices.Contains(decoyIndex))
				    {
                decoyIndex = UnityEngine.Random.Range(0,20);
            }
            chosenIndices.Add(decoyIndex);
            tempChosenIndices.Add(decoyIndex);
            sigilDisplayed.sigilOptions[i+1] = sigilOptions[decoyIndex];
        }
        tempChosenIndices.Clear();

        sigilDisplayed.currentlyDisplayed = UnityEngine.Random.Range(0,5);
        sigilDisplayed.rend.material = sigilDisplayed.sigilOptions[sigilDisplayed.currentlyDisplayed];

        for(int i = 0; i < 4; i++)
        {
            currentlyDisplayed[i].currentlyDisplayed = UnityEngine.Random.Range(0,5);
            currentlyDisplayed[i].textMesh.text = currentlyDisplayed[i].textOptions[currentlyDisplayed[i].currentlyDisplayed];
        }
    }

    public void PressSubmit()
    {
        if(moduleSolved)
        {
            return;
        }
        submitButton.AddInteractionPunch();
        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        if(sigilDisplayed.currentlyDisplayed == 0 && currentlyDisplayed[0].currentlyDisplayed == 0 && currentlyDisplayed[1].currentlyDisplayed == 0 && currentlyDisplayed[2].currentlyDisplayed == 0 && currentlyDisplayed[3].currentlyDisplayed == 0)
        {
            Debug.LogFormat("[Westeros #{0}] Input correct. Module disarmed.", moduleId);
            GetComponent<KMBombModule>().HandlePass();
            moduleSolved = true;
            Audio.PlaySoundAtTransform("theme", transform);
        }
        else
        {
            Debug.LogFormat("[Westeros #{0}] Strike! You submitted {1}, {2}, {3}, {4}, {5}. That is incorrect. Module reset.", moduleId, sigilDisplayed.sigilOptions[sigilDisplayed.currentlyDisplayed].name, currentlyDisplayed[0].textOptions[currentlyDisplayed[0].currentlyDisplayed], currentlyDisplayed[1].textOptions[currentlyDisplayed[1].currentlyDisplayed], currentlyDisplayed[2].textOptions[currentlyDisplayed[2].currentlyDisplayed], currentlyDisplayed[3].textOptions[currentlyDisplayed[3].currentlyDisplayed]);
            GetComponent<KMBombModule>().HandleStrike();
            chosenIndices.Clear();
            Start();
        }
    }

    void PressButton(KMSelectable pressedButton)
    {
        if(moduleSolved)
        {
            return;
        }
        pressedButton.AddInteractionPunch(0.5f);
        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        if(pressedButton.name == "Sigil")
        {
            pressedButton.GetComponent<SigilDisplay>().currentlyDisplayed++;
            pressedButton.GetComponent<SigilDisplay>().currentlyDisplayed = pressedButton.GetComponent<SigilDisplay>().currentlyDisplayed % 5;
            pressedButton.GetComponent<Renderer>().material = pressedButton.GetComponent<SigilDisplay>().sigilOptions[pressedButton.GetComponent<SigilDisplay>().currentlyDisplayed];
        }
        else
        {
            pressedButton.GetComponent<TextDisplay>().currentlyDisplayed++;
            pressedButton.GetComponent<TextDisplay>().currentlyDisplayed = pressedButton.GetComponent<TextDisplay>().currentlyDisplayed % 5;
            pressedButton.GetComponentInChildren<TextMesh>().text = pressedButton.GetComponent<TextDisplay>().textOptions[pressedButton.GetComponent<TextDisplay>().currentlyDisplayed];
        }
    }
}
