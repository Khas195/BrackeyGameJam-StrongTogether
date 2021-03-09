using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AINeed : MonoBehaviour
{
    [SerializeField] float satisfactionFallOffPerSecond = 1f;
    [SerializeField] float timeUntilNeedMin = 10f;
    [SerializeField] float timeUntilNeedMax = 20f;
    [SerializeField] List<Need> possibleNeeds;
    [SerializeField] SpriteRenderer needDisplay;

    private Need currentNeed = null;
    private AISatisfaction aISatisfaction;
    private Timer needTimer;
    private bool isSatisfyingNeed = false;


    public void Init(AISatisfaction aISatisfaction)
    {
        this.aISatisfaction = aISatisfaction;
        needTimer.Init(Random.Range(timeUntilNeedMin, timeUntilNeedMax));
        needDisplay.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(currentNeed == null)
        {
            needTimer.Tick();

            if(needTimer.CurrentTime <= 0f)
            {
                ChooseNewNeed();
                needTimer.Init(Random.Range(timeUntilNeedMin, timeUntilNeedMax));
            }
        }
        else if (!isSatisfyingNeed)
        {
            aISatisfaction.ChangeSatisfactionBy(-satisfactionFallOffPerSecond);
        }
    }

    public void SatisfyNeed(Need need)
    {
        if(need == currentNeed)
        {
            aISatisfaction.ChangeSatisfactionBy(need.satisfactionValue);

            needDisplay.gameObject.SetActive(false);

            isSatisfyingNeed = true;
        }
    }

    public void EndNeedSatisfaction(Need need)
    {
        needTimer.ResetTimer();
        currentNeed = null;

        isSatisfyingNeed = false;
    }

    private void ChooseNewNeed()
    {
        currentNeed = possibleNeeds[Random.Range(0, possibleNeeds.Count)];
        DisplayNeed();
    }

    private void DisplayNeed()
    {
        needDisplay.sprite = currentNeed.icon;
        needDisplay.gameObject.SetActive(true);
    }
}
