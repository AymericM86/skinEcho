using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EndGame : GameSequence
{
    public override bool IsTerminated()
    {

        WriteInFile.Write("PlaytestDatas/", "Time to complete total :" + PlaytestVibrationSpot.s_totalTimeElapsed + " Average time in area : " + PlaytestVibrationSpot.s_totalTimeInSpot / PlaytestVibrationSpot.s_numberOfSpots);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

        return true;
    }
}
