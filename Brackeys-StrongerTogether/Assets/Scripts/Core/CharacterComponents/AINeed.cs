using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AINeed : MonoBehaviour, ISatisfier, IWorker
{
    [SerializeField] float satisfactionFallOffPerSecond = 1f;
    [SerializeField] float timeUntilNeedMin = 10f;
    [SerializeField] float timeUntilNeedMax = 20f;
    [SerializeField] List<NeedData> possibleNeeds;
    [SerializeField] SpriteRenderer needDisplay;

    private NeedData currentNeed = null;
    private AISatisfaction aISatisfaction;
    private Timer needTimer = new Timer();
    private bool isSatisfyingNeed = false;


    public void Init(AISatisfaction aISatisfaction)
    {
        this.aISatisfaction = aISatisfaction;
        needTimer.Init(Random.Range(timeUntilNeedMin, timeUntilNeedMax));
        needDisplay.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (currentNeed == null)
        {
            needTimer.Tick();

            if (needTimer.CurrentTime <= 0f)
            {
                ChooseNewNeed();
                needTimer.Init(Random.Range(timeUntilNeedMin, timeUntilNeedMax));
            }
        }
        else if (!isSatisfyingNeed)
        {
            aISatisfaction.ChangeSatisfactionBy(-satisfactionFallOffPerSecond * Time.deltaTime);
        }
    }

    public void SatisfyNeed(NeedData need)
    {
        if (need == currentNeed)
        {
            aISatisfaction.ChangeSatisfactionBy(need.SatisfyAmount);

            needDisplay.gameObject.SetActive(false);

            isSatisfyingNeed = true;
        }
    }

    public void EndNeedSatisfaction(NeedData need)
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

    public void Satisfy(NeedData needData)
    {
        this.SatisfyNeed(needData);
    }

    public bool HasActiveNeed()
    {
        return currentNeed != null;
    }
}
