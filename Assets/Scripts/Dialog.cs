using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dialog : MonoBehaviour
{
  public string[] questions;
  public string[][] answers;
  public string introDialog;
  public string betweenQuestions;
  public int currentSelection;

  public Vector2 messageOffset = new Vector2(-15f, 0f);
  public Vector2 playerQuestionsOffset = new Vector2(-1, 3);

  public float questionSeparator = 1.0f;
  public float messageTransitionDelay = 2;
  public bool dialogPlaying = false;
  private float dialogTimer = 0;
  public int exitOption = 0;

  public playerController playerController = null;
  public ObjectSensor playerSensor = null;
  public GameObject player;

  public DialogState dialogState = DialogState.WAITING_TO_START;

  public enum DialogState
  {
    WAITING_TO_START,
    WAITING_FOR_PLAYER,
    WAITING_FOR_QUESTION,
    TALKING,
    WAITING_TO_RESET
  };

  void Start ()
  {
    if (playerSensor == null)
    {
      Debug.LogError("Set up playerSensor first");
    } 
    if (playerController == null)
    {
      Debug.LogError("Set up playerController first");
    }
  }

  void Update () {
    switch (dialogState)
    {
      case DialogState.WAITING_TO_START:
        if (playerSensor.colliding)
        {
          dialogState = DialogState.WAITING_FOR_PLAYER;
        }
        break;

      case DialogState.WAITING_FOR_PLAYER:
        if (!playerSensor.colliding)
        {
          dialogState = DialogState.WAITING_FOR_PLAYER;
        }
        else if (playerController.StartDialog(this))
        {
          dialogState = DialogState.WAITING_FOR_QUESTION;
          dialogTimer = 0;
          currentSelection = questions.Length-1;
        }
        break;

      case DialogState.WAITING_FOR_QUESTION:
        dialogTimer += Time.deltaTime;
        break;

      case DialogState.TALKING:
        dialogTimer += Time.deltaTime;
        break;

      case DialogState.WAITING_TO_RESET:
        if (!playerSensor.colliding)
        {
          dialogState = DialogState.WAITING_FOR_PLAYER;
          playerController.EndDialog();
        }
        break;
    }
  }

  void OnGUI()
  {
    bool displayDialog = false;
    string currentMessage = "";
    int answerIndex = 0;
    int questionIndex = 0;
    float questionX = 0;
    float questionY = 0;

    switch (dialogState)
    { 
      case DialogState.WAITING_FOR_QUESTION:
        displayDialog = true;
        currentMessage = introDialog;
                
        for (int messageIndex = 0; messageIndex < questions.Length; messageIndex++)
        {
          questionX = player.transform.position.x + playerQuestionsOffset[0];
          questionY = player.transform.position.y + playerQuestionsOffset[1] + ((questions.Length - messageIndex - 1) * questionSeparator);
            drawText(questions[messageIndex], questionX, questionY, 300, 500);
        }
        questionX = player.transform.position.x + playerQuestionsOffset[0] - 1;
        questionY = player.transform.position.y + playerQuestionsOffset[1] + (currentSelection * questionSeparator);
        drawText("*", questionX, questionY, 20, 20);
        break;

      case DialogState.TALKING:
        displayDialog = true;
        answerIndex = Mathf.FloorToInt(dialogTimer/ messageTransitionDelay);
        questionIndex = questions.Length - currentSelection - 1;

        if (answerIndex < answers[questionIndex].Length)
        {
            currentMessage = answers[questionIndex][answerIndex];
        }
        else if (currentSelection == 0) //Asking last question.
        {
            dialogState = DialogState.WAITING_TO_RESET;
            playerController.EndDialog();
            displayDialog = false;
        }
        else
        {
            dialogState = DialogState.WAITING_FOR_QUESTION;
            displayDialog = false;
        }
        break;
    }

    //Display speaker's text
    if (displayDialog)
    {
      drawText(currentMessage, transform.position.x + messageOffset.x, transform.position.y + messageOffset.y, 400, 400);
    }
  }

  public void IncrementMessage()
  {
    if (dialogState == DialogState.WAITING_FOR_QUESTION)
    {
      currentSelection -= 1;
      if (currentSelection < 0)
      {
        currentSelection = questions.Length-1;
      }
    }
  }

  public void DecrementMessage()
  {
    if (dialogState == DialogState.WAITING_FOR_QUESTION)
    {
      currentSelection += 1;
      currentSelection = currentSelection % questions.Length;
    }
  }

  public void SelectDialogOption()
  {
    if (dialogState == DialogState.WAITING_FOR_QUESTION)
    {
      dialogTimer = 0;
      dialogState = DialogState.TALKING;
    }
  }

  private void drawText(string text, float x, float y, float width, float height)
  {
      Vector3 guiLocation = Camera.main.WorldToScreenPoint(new Vector3(x, y));
      Rect textArea = new Rect(guiLocation.x, Screen.height - guiLocation.y, width, height);
      GUI.Label(textArea, text);
  }
}
