using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private float currentTime = 0;
    private float maxTime = 0;
    private bool paused = false;
    private bool countingDown = true;

    public bool CountingDown { get => countingDown; set => countingDown = value; }
    public float CurrentTime { get => currentTime; set => currentTime = value; }
    public float MaxTime { get => maxTime; set => maxTime = value; }
    public bool Paused { get => paused; set => paused = value; }


    public void Init(float max)
    {
        MaxTime = max;
        CurrentTime = MaxTime;
    }

    public void Tick()
    {
        if (!Paused)
        {
            if (CountingDown)
            {
                CurrentTime -= Time.deltaTime;
            }
            else
            {
                CurrentTime += Time.deltaTime;
            }
        }
    }

    public void ResetTimer()
    {
        CurrentTime = MaxTime;
    }

    public void TogglePause()
    {
        Paused = !Paused;
    }
}