using UnityEngine;
using System.Collections;

public class PlayerDialog : MonoBehaviour {

    public string[] messages;
    public int messageFocus = 0;
    public bool active = false;
    public Vector2 messageOffset = new Vector2(-7.5f, 10.0f);
    public float focusXOffset = -8.5f;
    public int messageHeight = 100;
    public int messageWidth = 200;
    Dialog dialog;

    // Use this for initialization
    void Start ()
    {
	
	}
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnGUI()
    {
        if (active)
        {
            Rect textArea;
            Vector3 guiLocation = Camera.main.WorldToScreenPoint(transform.position + new Vector3(messageOffset.x, messageOffset.y));

            for (int messageIndex = 0; messageIndex < messages.Length; messageIndex++)
            {
                textArea = new Rect(guiLocation.x, Screen.height - guiLocation.y + messageIndex * messageWidth, messageHeight, messageWidth);
                GUI.Label(textArea, messages[messageIndex]);
            }
            textArea = new Rect(guiLocation.x + focusXOffset, Screen.height - guiLocation.y + messageFocus * messageWidth, 20, 20);
            GUI.Label(textArea, "*");
        }
    }

    public void IncrementMessage()
    {
        messageFocus = (messageFocus + 1) % messages.Length;
    }

    public void DecrementMessage()
    {
        messageFocus = messageFocus == 0 ? messages.Length-1 : messageFocus-1;
    }
    
}
