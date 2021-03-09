using UnityEngine;

[CreateAssetMenu(fileName = "Final Project", menuName = "Game Settings/Final Project Data", order = 1)]
public class FinalProject : ScriptableObject
{
    [SerializeField]
    float currentProgress;
    [SerializeField]
    float maxProgress;
    private void Awake()
    {
        this.Reset();
    }

    public float CurrentProgress { get { return currentProgress; } }

    public void IncreaseProgress(float increaseAmount)
    {
        currentProgress += increaseAmount;
    }
    public bool IsProjectDone()
    {
        return currentProgress >= maxProgress;
    }
    public void Reset()
    {
        currentProgress = 0;
    }
}
