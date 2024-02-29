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

        // SceneWeaver.GetInstance().LoadDialogueSet("Tests/TestFactsUpdateByLine 1");
        // SceneWeaver.GetInstance().LoadDialogueSet("Tests/TestChoicesAndLinesRender");
        SceneWeaver.GetInstance().LoadDialogueSet("Tests/check_basic_line_ordering");
        // SceneWeaver.GetInstance().LoadDialogueSet("Tests/check_choices may_branch");
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
