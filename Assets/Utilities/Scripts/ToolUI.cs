using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;

public class ToolUI : Singleton<ToolUI>
{
    public enum ToolUIState
    {
        Hidden,
        ShowManipulation,
        ShowNavigationX,
        ShowNavigationY
    }
    ToolUIState currentTool = ToolUIState.Hidden;

    private GameObject manipulationCue;
    private Renderer maniRend;

    private void Awake()
    {  
        manipulationCue = GameObject.Find("ManipulationCue");
        manipulationCue.SetActive(false);
        //maniRend = manipulationCue.GetComponent<Renderer>();
    }

    public void ToolState(string toolState)
    {
        if (toolState == "manipulation")
        {
            manipulationCue.SetActive(true);
        }
    }
}
