using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Assets.Scripts.TextSystem.SceneWeaver;
using Assets.Scripts.TextSystem.Interfaces;
using Assets.Scripts.TextSystem.Models.Dialogue;
using Assets.Scripts.TextSystem.Choices;

public class UIController : MonoBehaviour
{
    private static UIController _instance;
    public static UIController Instance { get { return _instance;  } }
    [SerializeField] UIDocument doc;
    [SerializeField] TypeWriter typeWriter;
    private VisualElement v;
    private UIManager manager;

    private void Awake()
    {
       if(_instance != null && _instance != this)
       {
            Destroy(this.gameObject);
       } else
       {
            _instance = this;
       }

    }

    void Start()
    {
        v = doc.rootVisualElement;

        manager = v.Q<UIManager>();
        if (manager == null)
        {
            SceneWeaver.GetInstance().DialogueSetExausted += OnDialogueSetExhausted;
            Debug.Log("Element:" + v.ElementAt(0));
            manager = (UIManager)v.ElementAt(0);

        }
    }

    public void InsertVisualElementIntoDialogueSecition(VisualElement element)
    {
        manager.InsertVisualElementIntoDialogueSecition(element);
    }

    public void InsertNextDialogueElement(int dSetId)
    {
        if (typeWriter.isWriting || typeWriter.IsAwaitingChoice)
        {
            // typeWriter.FinishLine(); -> only finish line based on a setting in game. Some game designs will not allow dialogue to finish, others will.
            return;
        }

        IDialogueModel line = SceneWeaver.GetInstance().GetNextDialogueElement(dSetId); // we don't want to call this when we see a choice!!!
        if (line is null) return;

        // todo -> this is DEFINTELY broken lol
        if (line.IsFinalNodeInDSet)
        {
            Debug.LogWarning("FINAL NODE IN DSET DELIVERED. TODO:HANDLE THIS");
        }

        manager.InsertVisualElementIntoDialogueSecition(line.Renderer.GetVisualElement()); // must insert the new line first
        if (line is DialogueLine)
        {
            typeWriter.BeginTypeWriting((DialogueLine)line);
        } else if(line is ChoiceSet)
        {
            // make it so we can ONLY progress on click.
            typeWriter.IsAwaitingChoice = true;
        }
    }

    // TODO -> not properly set up to fire.
    public void OnDialogueSetExhausted(object source, System.EventArgs e)
    {
        typeWriter.BeginTypeWriting(new DialogueLine("DSET IS EMPTY. RETURN TO GAMEPLAY")); // todo -> make this debug only
    }

    // TODO
    public void InsertNextDialogueElementFromChoice(OptionDialogueNode chosenOption)
    {
        throw new System.NotImplementedException("Need to re-implement inserting choices in Sceneweaver.");
        return;
        /*
        IDialogueModel line = SceneWeaver.GetInstance().GetNextDialogueElementFromOption(chosenOption);
        manager.InsertVisualElementIntoDialogueSecition(line.Renderer.GetVisualElement());
        line.Renderer.StartTypeWriting();
        if (line is DialogueLine)
        {
            typeWriter.SetDLine((DialogueLine)line);
        }
        */
    }

    public void UpdateMostRecentTextBoxElement(string text)
    {
        manager.UpdateMostRecentTextBoxElement(text);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
