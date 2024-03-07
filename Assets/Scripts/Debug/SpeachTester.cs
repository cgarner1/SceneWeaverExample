using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.UI;
using UnityEngine.UIElements;
using Assets.Scripts.TextSystem;
using Assets.Scripts.TextSystem.SceneWeaver;

public class SpeachTester : MonoBehaviour
{
    [SerializeField] UIDocument doc;
    
    void Start()
    {

        // SceneWeaver.GetInstance().LoadDialogueSet("Tests/TestFactsUpdateByLine_1");
        // SceneWeaver.GetInstance().LoadDialogueSet("Tests/TestChoicesAndLinesRender");
        // SceneWeaver.GetInstance().LoadDialogueSet("Tests/check_basic_line_ordering");
        // SceneWeaver.GetInstance().LoadDialogueSet("Tests/check_choices may_branch");
        SceneWeaver.GetInstance().LoadDialogueSet("Tests/check_events_fire_from_text");
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {

            // Debug.Log(v.name);
            UIController.Instance.InsertNextDialogueElement(1);
        }
    }
}
