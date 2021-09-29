using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MouseUi : MonoBehaviour
{
    [System.NonSerialized]
    public CursorAnimated currentCursor;

    public static Action<String> hooveringItem;

    public CursorAnimated[] cursors;
    private float _timer; private int frame = 0;
    private bool animate = false; private bool inConvo;

    private void Start()
    {
        currentCursor = cursors[0];
        Cursor.SetCursor(currentCursor.texture[frame], currentCursor.offset, CursorMode.Auto);
        hooveringItem += ChangeCursor;

        ConvController.startConvo += x => InConversation();
        ConvController.endConvo += OutConversation;
    }
    private void Update()
    {
        if (animate && !inConvo)
        {
            _timer -= Time.deltaTime;
            if (_timer < 0)
            {
                _timer = currentCursor.timer;
                frame++; if (frame == currentCursor.texture.Length) frame = 0;
                Cursor.SetCursor(currentCursor.texture[frame], currentCursor.offset, CursorMode.Auto);
            }
        }

    }
    void InConversation()
    {
        inConvo = true;
        currentCursor = cursors[0];
    }

    void OutConversation()
    {
        inConvo = false;
    }
    void ChangeCursor(string thing)
    {
        if (thing == "idle")
        {
            currentCursor = cursors[0];
            Cursor.SetCursor(currentCursor.texture[frame], currentCursor.offset, CursorMode.Auto);
            animate = false;
            return;
        }

        if (thing == "obj") currentCursor = cursors[0];
        if (thing == "item") currentCursor = cursors[1];
        if (thing == "npc") currentCursor = cursors[2];
        animate = true;
    }


    [System.Serializable]
    public class CursorAnimated
    {
        public Texture2D[] texture;
        public float timer;
        public Vector2 offset;
    }

}
