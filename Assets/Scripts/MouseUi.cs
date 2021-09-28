using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseUi : MonoBehaviour
{
    [System.NonSerialized]
    public CursorAnimated currentCursor;

    public CursorAnimated[] cursors;
    private float _timer; private int frame = 0;

    private void Start()
    {
        currentCursor = cursors[0];
    }
    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer < 0)
        {
            _timer = currentCursor.timer;
            frame++; if (frame == currentCursor.texture.Length) frame = 0;
            Cursor.SetCursor(currentCursor.texture[frame], currentCursor.offset, CursorMode.Auto);
        }

    }

    [System.Serializable]
    public class CursorAnimated
    {
        public Texture2D[] texture;
        public float timer;
        public Vector2 offset;
    }

}
