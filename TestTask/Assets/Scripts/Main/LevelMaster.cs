using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class LevelMaster : MonoBehaviour
{
    [SerializeField]
    LevelInfo Levels;

    [SerializeField]
    SetInfo Sets;

    [SerializeField]
    GameObject pauseBackground;

    [SerializeField]
    Restart res;

    [SerializeField]
    PlaygroundMaster plMaster;

    int currenLevel = 0;

    List<string> answers = new List<string>();

    public UnityEvent PauseEvent = new UnityEvent();
    public void newLevel()
    {
        if (currenLevel < Levels.levels.Count)
        {
            Collection currentSet;

            currentSet = SetSelection(Levels.levels[currenLevel].type);
            answers.Add(GenerateAnswer(currentSet));

            if (answers[answers.Count - 1] != "NAN")
            {
                plMaster.Init(answers[answers.Count - 1], currentSet, Levels.levels[currenLevel].columns, Levels.levels[currenLevel].lines);
            }
            else
            {
                Restart();
            }
        }
        else
        {
            Restart();
        }

        currenLevel += 1;
    }

    private Collection SetSelection(int index)
    {
        int result;

        if (index >= 0 && index < Sets.sets.Count)
        {
            result = index;
        }
        else
        {
            result = Random.Range(0, 100) % Sets.sets.Count;
        }

        return Sets.sets[result];
    }

    private string GenerateAnswer(Collection set)
    {
        string result = "";
        bool isCorrect = false;

        for (int i = 0; i < 15; i++)
        {
            result = set.elements[Random.Range(0, set.elements.Count - 1)].name;

            isCorrect = true;
            for (int j = 0; j < answers.Count; j++)
            {
                if (result == answers[j])
                {
                    isCorrect = false;
                    break;
                }
            }

            if (isCorrect)
            {
                break;
            }
        }

        if (!isCorrect)
        {
            for (int i = 0; i < set.elements.Count; i++)
            {
                isCorrect = true;
                for (int j = 0; j < answers.Count; j++)
                {
                    if (set.elements[i].name == answers[j])
                    {
                        isCorrect = false;
                        break;
                    }
                }

                if (isCorrect)
                {
                    result = set.elements[i].name;
                    break;
                }
            }
        }

        if (isCorrect)
        {
            return result;
        }
        else
        {
            return "NAN";
        }
    }

    private void Restart()
    {
        pauseBackground.SetActive(true);

        PauseEvent.Invoke();
    }

    private void Endgame()
    {
        currenLevel = 0;
        answers.Clear();
    }

    private void Start()
    {
        plMaster.newGame.AddListener(newLevel);
        res.RestartEvent.AddListener(Endgame);
        newLevel();
    }
}
