using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class FinalProjectUIUpdater : MonoBehaviour
{
    [SerializeField]
    [Required]
    FinalProject finalProjectData = null;
    [SerializeField]
    [Required]
    Text progressText = null;
    // Update is called once per frame
    void Update()
    {
        progressText.text = "Progress: " + finalProjectData.CurrentProgress;
    }
}
