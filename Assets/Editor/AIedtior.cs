using UnityEditor;
using UnityEngine;
using UnityEditor.Graphs;

using System.Collections.Generic;

public class uGraphGUI : GraphGUI // overrride intance by kao
{

}

public class Example : EditorWindow
{
    static Example example;
    Graph stateMachineGraph;
    GraphGUI stateMachineGraphGUI;

    //LIST for draw
    List<Rect> AIblock = new List<Rect>();

    enum GenericMenuState
    {
        add,
        delete,
        link,
    }

    [MenuItem("KaoTest/Edtior")]
    static void Do()
    {
        example = GetWindow<Example>();
    }

    void OnEnable()
    {
        if (stateMachineGraph == null)
        {
            stateMachineGraph = ScriptableObject.CreateInstance<Graph>();
            stateMachineGraph.hideFlags = HideFlags.HideAndDontSave;
        }
        if (stateMachineGraphGUI == null)
        {
            stateMachineGraphGUI = (GetEditor(stateMachineGraph));
        }
    }

    void OnDisable()
    {
        example = null;
    }

    void OnGUI()
    {
        if (example && stateMachineGraphGUI != null)
        {
            stateMachineGraphGUI.BeginGraphGUI(example, new Rect(0, 0, example.position.width, example.position.height));
            //            stateMachineGraphGUI.OnGraphGUI ();
            stateMachineGraphGUI.EndGraphGUI();

        }

        //right click menu
        //GenericMenu API
        Event currentEvent = Event.current;
        if (currentEvent.type == EventType.ContextClick)
        {
            // Now create the menu, add items and show it
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("ADD"), false, SelectGenericMenuCB, GenericMenuState.add);
            menu.AddItem(new GUIContent("DELETE"), false, SelectGenericMenuCB, GenericMenuState.delete);
            menu.AddSeparator("");
            menu.ShowAsContext();
            currentEvent.Use();   
        }

        //Draw block
        if (AIblock.Count > 0)
        {
            BeginWindows();
            for (int i = 0; i < AIblock.Count; i++)
            {
                AIblock[i] = GUI.Window(i, AIblock[i], DrawNodeWindow, "AI" + i.ToString());   // Updates the Rect's when these are dragged
            }
            EndWindows();
        }
    }

    void DrawNodeWindow(int id)
    {
        //right click
        Event currentEvent = Event.current;
        //&& AIblock[id].Contains(currentEvent.mousePosition)
        if (currentEvent.type == EventType.mouseDown && currentEvent.button == 1 )
        {
            // Now create the menu, add items and show it
            GenericMenu menuForAI = new GenericMenu();
            menuForAI.AddItem(new GUIContent("LINK"), false, SelectGenericMenuCB, GenericMenuState.link);
            menuForAI.ShowAsContext();
            currentEvent.Use();
        }
        else
        {
            GUI.DragWindow();
        }
    }

    void SelectGenericMenuCB(object obj)
    {       
        switch ((GenericMenuState)obj)
        {
            case GenericMenuState.add:
                AddBlock();
                break;

            case GenericMenuState.delete:

                break;
        };
        
    }

    GraphGUI GetEditor(Graph graph)
    {
        /* GraphGUI graphGUI = CreateInstance("GraphGUI") as GraphGUI;
         graphGUI.graph = graph;*/

        uGraphGUI graphGUI = new uGraphGUI();
        graphGUI.graph = graph;
        graphGUI.hideFlags = HideFlags.HideAndDontSave;
        return graphGUI;
    }

    void AddBlock()
    {
        AIblock.Add(new Rect(0, 0, 100, 100));
    }

}

