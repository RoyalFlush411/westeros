using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

    private bool hasActivated = false;
    private bool TwitchPlaysActive;

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
        GetComponent<KMBombModule>().OnActivate += OnActivate;
    }


    void Start()
    {
        PickCorrectAnswer();
        PickDecoys();
        LogData();
        if (!hasActivated)
        {
            sigilDisplayed.rend.material = sigilOptions[20];
            for (int i = 0; i < 4; i++)
            {
                currentlyDisplayed[i].textMesh.text = "";
            }
        }
    }

    void OnActivate()
    {
        hasActivated = true;
        sigilDisplayed.currentlyDisplayed = UnityEngine.Random.Range(0, 5);
        sigilDisplayed.tpDisplayText.text = (sigilDisplayed.currentlyDisplayed + 1).ToString();
        if (TwitchPlaysActive)
            sigilDisplayed.tpDisplay.SetActive(true);
        sigilDisplayed.rend.material = sigilDisplayed.sigilOptions[sigilDisplayed.currentlyDisplayed];

        for (int i = 0; i < 4; i++)
        {
            currentlyDisplayed[i].currentlyDisplayed = UnityEngine.Random.Range(0, 5);
            currentlyDisplayed[i].textMesh.text = currentlyDisplayed[i].textOptions[currentlyDisplayed[i].currentlyDisplayed];
        }
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

        if (hasActivated)
        {
            sigilDisplayed.currentlyDisplayed = UnityEngine.Random.Range(0, 5);
            sigilDisplayed.tpDisplayText.text = (sigilDisplayed.currentlyDisplayed + 1).ToString();
            sigilDisplayed.rend.material = sigilDisplayed.sigilOptions[sigilDisplayed.currentlyDisplayed];

            for (int i = 0; i < 4; i++)
            {
                currentlyDisplayed[i].currentlyDisplayed = UnityEngine.Random.Range(0, 5);
                currentlyDisplayed[i].textMesh.text = currentlyDisplayed[i].textOptions[currentlyDisplayed[i].currentlyDisplayed];
            }
        }
    }

    void LogData()
    {
        List<List<int>> randos = new List<List<int>>();
        for(int i = 0; i < 5; i++)
        {
            List<int> temp = new List<int>();
            for (int j = 0; j < 5; j++)
            {
                temp.Add(j);
            }
            randos.Add(temp);
        }
        for(int i = 0; i < 5; i++)
        {
            randos[i] = randos[i].Shuffle();
        }
        Debug.LogFormat("[Westeros #{0}] The possible sigils are: {1}, {2}, {3}, {4}, and {5}", moduleId, sigilDisplayed.sigilOptions[randos[0][0]].name, sigilDisplayed.sigilOptions[randos[0][1]].name, sigilDisplayed.sigilOptions[randos[0][2]].name, sigilDisplayed.sigilOptions[randos[0][3]].name, sigilDisplayed.sigilOptions[randos[0][4]].name);
        Debug.LogFormat("[Westeros #{0}] The possible forenames are: {1}, {2}, {3}, {4}, and {5}", moduleId, currentlyDisplayed[0].textOptions[randos[1][0]], currentlyDisplayed[0].textOptions[randos[1][1]], currentlyDisplayed[0].textOptions[randos[1][2]], currentlyDisplayed[0].textOptions[randos[1][3]], currentlyDisplayed[0].textOptions[randos[1][4]]);
        Debug.LogFormat("[Westeros #{0}] The possible houses are: {1}, {2}, {3}, {4}, and {5}", moduleId, currentlyDisplayed[1].textOptions[randos[2][0]], currentlyDisplayed[1].textOptions[randos[2][1]], currentlyDisplayed[1].textOptions[randos[2][2]], currentlyDisplayed[1].textOptions[randos[2][3]], currentlyDisplayed[1].textOptions[randos[2][4]]);
        Debug.LogFormat("[Westeros #{0}] The possible words are: \"{1}\", \"{2}\", \"{3}\", \"{4}\", and \"{5}\"", moduleId, currentlyDisplayed[2].textOptions[randos[3][0]].Replace("  ", " "), currentlyDisplayed[2].textOptions[randos[3][1]].Replace("  ", " "), currentlyDisplayed[2].textOptions[randos[3][2]].Replace("  ", " "), currentlyDisplayed[2].textOptions[randos[3][3]].Replace("  ", " "), currentlyDisplayed[2].textOptions[randos[3][4]].Replace("  ", " "));
        Debug.LogFormat("[Westeros #{0}] The possible seats are: {1}, {2}, {3}, {4}, and {5}", moduleId, currentlyDisplayed[3].textOptions[randos[4][0]].Replace("  ", " "), currentlyDisplayed[3].textOptions[randos[4][1]].Replace("  ", " "), currentlyDisplayed[3].textOptions[randos[4][2]].Replace("  ", " "), currentlyDisplayed[3].textOptions[randos[4][3]].Replace("  ", " "), currentlyDisplayed[3].textOptions[randos[4][4]].Replace("  ", " "));
        Debug.LogFormat("[Westeros #{0}] The correct sigil is: {1}", moduleId, correctAnswers[0]);
        Debug.LogFormat("[Westeros #{0}] The correct forename is: {1}", moduleId, correctAnswers[1]);
        Debug.LogFormat("[Westeros #{0}] The correct house is: {1}", moduleId, correctAnswers[2]);
        Debug.LogFormat("[Westeros #{0}] The correct words are: \"{1}\"", moduleId, correctAnswers[3].Replace("  ", " "));
        Debug.LogFormat("[Westeros #{0}] The correct seat is: {1}", moduleId, correctAnswers[4].Replace("  ", " "));
    }

    public void PressSubmit()
    {
        if(moduleSolved)
        {
            return;
        }
        submitButton.AddInteractionPunch();
        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, submitButton.transform);
        if(sigilDisplayed.currentlyDisplayed == 0 && currentlyDisplayed[0].currentlyDisplayed == 0 && currentlyDisplayed[1].currentlyDisplayed == 0 && currentlyDisplayed[2].currentlyDisplayed == 0 && currentlyDisplayed[3].currentlyDisplayed == 0)
        {
            Debug.LogFormat("[Westeros #{0}] You submitted {1}, {2}, {3}, \"{4}\", and {5}. Input correct, module disarmed.", moduleId, sigilDisplayed.sigilOptions[sigilDisplayed.currentlyDisplayed].name, currentlyDisplayed[0].textOptions[currentlyDisplayed[0].currentlyDisplayed], currentlyDisplayed[1].textOptions[currentlyDisplayed[1].currentlyDisplayed], currentlyDisplayed[2].textOptions[currentlyDisplayed[2].currentlyDisplayed].Replace("  ", " "), currentlyDisplayed[3].textOptions[currentlyDisplayed[3].currentlyDisplayed].Replace("  ", " "));
            GetComponent<KMBombModule>().HandlePass();
            moduleSolved = true;
            Audio.PlaySoundAtTransform("theme", transform);
            sigilDisplayed.rend.material = sigilOptions[20];
            for (int i = 0; i < 4; i++)
            {
                currentlyDisplayed[i].textMesh.text = "";
            }
            sigilDisplayed.tpDisplayText.text = "";
        }
        else
        {
            Debug.LogFormat("[Westeros #{0}] You submitted {1}, {2}, {3}, \"{4}\", and {5}. Input incorrect, Strike! Module reset.", moduleId, sigilDisplayed.sigilOptions[sigilDisplayed.currentlyDisplayed].name, currentlyDisplayed[0].textOptions[currentlyDisplayed[0].currentlyDisplayed], currentlyDisplayed[1].textOptions[currentlyDisplayed[1].currentlyDisplayed], currentlyDisplayed[2].textOptions[currentlyDisplayed[2].currentlyDisplayed].Replace("  ", " "), currentlyDisplayed[3].textOptions[currentlyDisplayed[3].currentlyDisplayed].Replace("  ", " "));
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
        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, pressedButton.transform);
        if(pressedButton.name == "Sigil")
        {
            pressedButton.GetComponent<SigilDisplay>().currentlyDisplayed++;
            pressedButton.GetComponent<SigilDisplay>().currentlyDisplayed = pressedButton.GetComponent<SigilDisplay>().currentlyDisplayed % 5;
            pressedButton.GetComponent<SigilDisplay>().tpDisplayText.text = (pressedButton.GetComponent<SigilDisplay>().currentlyDisplayed + 1).ToString();
            pressedButton.GetComponent<Renderer>().material = pressedButton.GetComponent<SigilDisplay>().sigilOptions[pressedButton.GetComponent<SigilDisplay>().currentlyDisplayed];
        }
        else
        {
            pressedButton.GetComponent<TextDisplay>().currentlyDisplayed++;
            pressedButton.GetComponent<TextDisplay>().currentlyDisplayed = pressedButton.GetComponent<TextDisplay>().currentlyDisplayed % 5;
            pressedButton.GetComponentInChildren<TextMesh>().text = pressedButton.GetComponent<TextDisplay>().textOptions[pressedButton.GetComponent<TextDisplay>().currentlyDisplayed];
        }
    }

    //twitch plays
    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} cycle (stat1) (stat2)... [Cycles through the specified display(s) showing Westerosi house stat(s) 'stat1' (and 'stat2' or more)] | !{0} submit <sigil> <forename> <house> <words> <seat> [Submits the specified Westerosi house stats where 'words' and 'seat' must have no spaces and 'sigil' must be a number from 1-5]";
    #pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        string[] parameters = command.Split(' ');
        if (Regex.IsMatch(parameters[0], @"^\s*cycle\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            if (parameters.Length == 1)
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        yield return "trycancel Cycling halted due to a request to cancel!";
                        yield return new WaitForSeconds(1.5f);
                        buttons[i].OnInteract();
                    }
                    yield return new WaitForSeconds(1f);
                }
            }
            else if (parameters.Length >= 2)
            {
                string[] types = { "sigil", "sigils", "forename", "forenames", "house", "houses", "word", "words", "seat", "seats" };
                int[] indexes = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4 };
                for(int i = 1; i < parameters.Length; i++)
                {
                    if (!types.Contains(parameters[i].ToLower()))
                    {
                        yield return "sendtochaterror The specified display '" + parameters[i] + "' is invalid!";
                        yield break;
                    }
                }
                for (int i = 1; i < parameters.Length; i++)
                {
                    int index = Array.IndexOf(types, parameters[i].ToLower());
                    for (int j = 0; j < 5; j++)
                    {
                        yield return "trycancel Cycling halted due to a request to cancel!";
                        yield return new WaitForSeconds(1.5f);
                        buttons[indexes[index]].OnInteract();
                    }
                    yield return new WaitForSeconds(1f);
                }
            }
            yield break;
        }
        if (Regex.IsMatch(parameters[0], @"^\s*submit\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            if (parameters.Length > 6)
            {
                yield return "sendtochaterror Too many parameters!";
            }
            else if (parameters.Length == 6)
            {
                bool madeit1 = false;
                bool madeit2 = false;
                for(int i = 0; i < 5; i++)
                {
                    if (parameters[1].EqualsIgnoreCase(sigilDisplayed.tpDisplayText.text))
                    {
                        madeit1 = true;
                        break;
                    }
                    else
                    {
                        buttons[0].OnInteract();
                        yield return new WaitForSeconds(0.1f);
                    }
                }
                if (!madeit1)
                {
                    yield return "sendtochaterror The sigil '" + parameters[1] + "' is out of range 1-5!";
                    yield break;
                }
                List<string[]> arrays = new List<string[]>();
                arrays.Add(forenameOptions);
                arrays.Add(familyNameOptions);
                arrays.Add(wordsOptions);
                arrays.Add(seatOptions);
                string[] names = { "forename", "house", "words", "seat" };
                for (int k = 1; k < 5; k++)
                {
                    madeit1 = false;
                    madeit2 = false;
                    for (int i = 0; i < 5; i++)
                    {
                        string checker;
                        if (k == 3 || k == 4)
                        {
                            checker = currentlyDisplayed[k-1].textOptions[currentlyDisplayed[k-1].currentlyDisplayed].Replace("  ", "");
                        }
                        else
                        {
                            checker = currentlyDisplayed[k-1].textOptions[currentlyDisplayed[k-1].currentlyDisplayed];
                        }
                        if (parameters[k+1].EqualsIgnoreCase(checker))
                        {
                            madeit1 = true;
                            break;
                        }
                        buttons[k].OnInteract();
                        yield return new WaitForSeconds(0.1f);
                    }
                    if (!madeit1)
                    {
                        int total;
                        if (k == 1)
                        {
                            total = 100;
                        }
                        else
                        {
                            total = 20;
                        }
                        for (int j = 0; j < total; j++)
                        {
                            string checker;
                            if (k == 3 || k == 4)
                            {
                                checker = arrays[k-1][j].Replace("  ", "");
                            }
                            else
                            {
                                checker = arrays[k-1][j];
                            }
                            if (checker.EqualsIgnoreCase(parameters[k+1]))
                            {
                                madeit2 = true;
                                yield return "sendtochaterror The " + names[k-1] + " '" + parameters[k+1] + "' is not an option!";
                                yield return "unsubmittablepenalty";
                            }
                        }
                        if (!madeit2)
                        {
                            yield return "sendtochaterror The " + names[k-1] + " '" + parameters[k+1] + "' does not exist!";
                        }
                        yield break;
                    }
                }
                submitButton.OnInteract();
            }
            else if (parameters.Length < 6)
            {
                yield return "sendtochaterror Please specify all Westerosi stats needed to submit!";
            }
            yield break;
        }
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        while (!sigilDisplayed.sigilOptions[sigilDisplayed.currentlyDisplayed].name.Equals(correctAnswers[0]))
        {
            buttons[0].OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
        for(int i = 0; i < 4; i++)
        {
            while (!currentlyDisplayed[i].textOptions[currentlyDisplayed[i].currentlyDisplayed].Equals(correctAnswers[i+1]))
            {
                buttons[i+1].OnInteract();
                yield return new WaitForSeconds(0.1f);
            }
        }
        submitButton.OnInteract();
    }
}
