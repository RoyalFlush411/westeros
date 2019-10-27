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

    //twitch plays
    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} cycle [Cycles through all displays] | !{0} cycle <sigil/forename/house/words/seat> [Cycles through the specified display] | !{0} submit <sigil> <forename> <house> <words> <seat> [Submits the specified house stats] | Spaces are NOT allowed";
    #pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        string[] parameters = command.Split(' ');
        if (Regex.IsMatch(parameters[0], @"^\s*cycle\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            if (parameters.Length == 2)
            {
                if (Regex.IsMatch(parameters[1], @"^\s*sigil\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
                {
                    yield return null;
                    yield return new WaitForSeconds(0.5f);
                    for (int i = 0; i < 5; i++)
                    {
                        buttons[0].OnInteract();
                        yield return "trycancel Sigil cycling cancelled due to a cancel request.";
                        yield return new WaitForSeconds(2f);
                    }
                }
                if (Regex.IsMatch(parameters[1], @"^\s*forename\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
                {
                    yield return null;
                    yield return new WaitForSeconds(0.5f);
                    for (int i = 0; i < 5; i++)
                    {
                        buttons[1].OnInteract();
                        yield return "trycancel Forename cycling cancelled due to a cancel request.";
                        yield return new WaitForSeconds(2f);
                    }
                }
                if (Regex.IsMatch(parameters[1], @"^\s*house\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
                {
                    yield return null;
                    yield return new WaitForSeconds(0.5f);
                    for (int i = 0; i < 5; i++)
                    {
                        buttons[2].OnInteract();
                        yield return "trycancel House cycling cancelled due to a cancel request.";
                        yield return new WaitForSeconds(2f);
                    }
                }
                if (Regex.IsMatch(parameters[1], @"^\s*words\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
                {
                    yield return null;
                    yield return new WaitForSeconds(0.5f);
                    for (int i = 0; i < 5; i++)
                    {
                        buttons[3].OnInteract();
                        yield return "trycancel Words cycling cancelled due to a cancel request.";
                        yield return new WaitForSeconds(2f);
                    }
                }
                if (Regex.IsMatch(parameters[1], @"^\s*seat\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
                {
                    yield return null;
                    yield return new WaitForSeconds(0.5f);
                    for (int i = 0; i < 5; i++)
                    {
                        buttons[4].OnInteract();
                        yield return "trycancel Seat cycling cancelled due to a cancel request.";
                        yield return new WaitForSeconds(2f);
                    }
                }
            }
            else if(parameters.Length == 1)
            {
                yield return null;
                yield return new WaitForSeconds(0.5f);
                for (int i = 0; i < 5; i++)
                {
                    buttons[0].OnInteract();
                    yield return "trycancel Cycling cancelled due to a cancel request.";
                    yield return new WaitForSeconds(2f);
                }
                yield return new WaitForSeconds(0.5f);
                for (int i = 0; i < 5; i++)
                {
                    buttons[1].OnInteract();
                    yield return "trycancel Cycling cancelled due to a cancel request.";
                    yield return new WaitForSeconds(2f);
                }
                yield return new WaitForSeconds(0.5f);
                for (int i = 0; i < 5; i++)
                {
                    buttons[2].OnInteract();
                    yield return "trycancel Cycling cancelled due to a cancel request.";
                    yield return new WaitForSeconds(2f);
                }
                yield return new WaitForSeconds(0.5f);
                for (int i = 0; i < 5; i++)
                {
                    buttons[3].OnInteract();
                    yield return "trycancel Cycling cancelled due to a cancel request.";
                    yield return new WaitForSeconds(2f);
                }
                yield return new WaitForSeconds(0.5f);
                for (int i = 0; i < 5; i++)
                {
                    buttons[4].OnInteract();
                    yield return "trycancel Cycling cancelled due to a cancel request.";
                    yield return new WaitForSeconds(2f);
                }
            }
            yield break;
        }
        if (Regex.IsMatch(parameters[0], @"^\s*submit\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            if(parameters.Length == 6)
            {
                yield return null;
                string temp = "";
                string temp2 = parameters[1].ToLower();
                int count = 0;
                while (!temp.Equals(temp2))
                {
                    buttons[0].OnInteract();
                    temp = sigilDisplayed.rend.material.name.ToLower();
                    temp = temp.Replace(" (instance)", "");
                    if (count == 5)
                    {
                        yield return "sendtochaterror The Sigil '"+parameters[1]+"' is not an option!";
                        for(int i = 0; i < sigilOptions.Length; i++)
                        {
                            string thing = "";
                            thing = sigilOptions[i].name.ToLower();
                            thing = thing.Replace(" (instance)", "");
                            if (thing.Equals(temp2))
                            {
                                yield return "sendtochaterror Since this is a valid Sigil, an unsubmittable penalty will be applied.";
                                yield return "unsubmittablepenalty";
                                yield break;
                            }
                        }
                        yield return "sendtochaterror Since this is not a valid Sigil, an unsubmittable penalty will not be applied.";
                        yield break;
                    }
                    count++;
                    yield return new WaitForSeconds(0.1f);
                }
                string tempp = "";
                string tempp2 = parameters[2].ToLower();
                int count2 = 0;
                while (!tempp.Equals(tempp2))
                {
                    buttons[1].OnInteract();
                    tempp = currentlyDisplayed[0].textMesh.text.ToLower();
                    if (count2 == 5)
                    {
                        yield return "sendtochaterror The Forename '" + parameters[2] + "' is not an option!";
                        for (int i = 0; i < forenameOptions.Length; i++)
                        {
                            string thing = "";
                            thing = forenameOptions[i].ToLower();
                            if (thing.Equals(tempp2))
                            {
                                yield return "sendtochaterror Since this is a valid Forename, an unsubmittable penalty will be applied.";
                                yield return "unsubmittablepenalty";
                                yield break;
                            }
                        }
                        yield return "sendtochaterror Since this is not a valid Forename, an unsubmittable penalty will not be applied.";
                        yield break;
                    }
                    count2++;
                    yield return new WaitForSeconds(0.1f);
                }
                string temppp = "";
                string temppp2 = parameters[3].ToLower();
                int count3 = 0;
                while (!temppp.Equals(temppp2))
                {
                    buttons[2].OnInteract();
                    temppp = currentlyDisplayed[1].textMesh.text.ToLower();
                    if (count3 == 5)
                    {
                        yield return "sendtochaterror The House '" + parameters[3] + "' is not an option!";
                        for (int i = 0; i < familyNameOptions.Length; i++)
                        {
                            string thing = "";
                            thing = familyNameOptions[i].ToLower();
                            if (thing.Equals(temppp2))
                            {
                                yield return "sendtochaterror Since this is a valid House Name, an unsubmittable penalty will be applied.";
                                yield return "unsubmittablepenalty";
                                yield break;
                            }
                        }
                        yield return "sendtochaterror Since this is not a valid House Name, an unsubmittable penalty will not be applied.";
                        yield break;
                    }
                    count3++;
                    yield return new WaitForSeconds(0.1f);
                }
                string tempppp = "";
                string tempppp2 = parameters[4].ToLower();
                int count4 = 0;
                while (!tempppp.Equals(tempppp2))
                {
                    buttons[3].OnInteract();
                    tempppp = currentlyDisplayed[2].textMesh.text.ToLower();
                    tempppp = tempppp.Replace(" ", "");
                    if (count4 == 5)
                    {
                        yield return "sendtochaterror The Words '" + parameters[4] + "' is not an option!";
                        for (int i = 0; i < wordsOptions.Length; i++)
                        {
                            string thing = "";
                            thing = wordsOptions[i].ToLower();
                            if (thing.Equals(tempppp2))
                            {
                                yield return "sendtochaterror Since this is a valid set of Words, an unsubmittable penalty will be applied.";
                                yield return "unsubmittablepenalty";
                                yield break;
                            }
                        }
                        yield return "sendtochaterror Since this is not a valid set of Words, an unsubmittable penalty will not be applied.";
                        yield break;
                    }
                    count4++;
                    yield return new WaitForSeconds(0.1f);
                }
                string temppppp = "";
                string temppppp2 = parameters[5].ToLower();
                int count5 = 0;
                while (!temppppp.Equals(temppppp2))
                {
                    buttons[4].OnInteract();
                    temppppp = currentlyDisplayed[3].textMesh.text.ToLower();
                    temppppp = temppppp.Replace(" ", "");
                    if (count5 == 5)
                    {
                        yield return "sendtochaterror The Seat '" + parameters[5] + "' is not an option!";
                        for (int i = 0; i < seatOptions.Length; i++)
                        {
                            string thing = "";
                            thing = seatOptions[i].ToLower();
                            if (thing.Equals(temppppp2))
                            {
                                yield return "sendtochaterror Since this is a valid Seat, an unsubmittable penalty will be applied.";
                                yield return "unsubmittablepenalty";
                                yield break;
                            }
                        }
                        yield return "sendtochaterror Since this is not a valid Seat, an unsubmittable penalty will not be applied.";
                        yield break;
                    }
                    count5++;
                    yield return new WaitForSeconds(0.1f);
                }
                submitButton.OnInteract();
            }
            yield break;
        }
    }
}
