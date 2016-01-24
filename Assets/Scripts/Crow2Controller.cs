using UnityEngine;
using System.Collections;

public class Crow2Controller : MonoBehaviour {
    private Dialog dialog;
    private int numQuestions = 6;

    // Use this for initialization
    void Start()
    {
        int dialogIndex = 0;

        dialog = GetComponent<Dialog>();
        if (dialog == null)
        {
            Debug.LogError("Could not find Dialog component");
        }
        else
        {
            dialog.introDialog = "Hello little friend!  I am afraid you must have taken a wrong turn somewhere to end up here..  Who knows? Maybe this is exactly where you are meant to be.";

            dialog.questions = new string[numQuestions];
            dialog.answers = new string[numQuestions][];

            dialog.questions[dialogIndex] = "Where am I?";
            dialog.answers[dialogIndex] = new string[2];
            dialog.answers[dialogIndex][0] = "You are in the Underworld, my new friend.";
            dialog.answers[dialogIndex][1] = "Is it what you expected?";

            dialogIndex++;
            dialog.questions[dialogIndex] = "Who are you?";
            dialog.answers[dialogIndex] = new string[2];
            dialog.answers[dialogIndex][0] = "What do you want to know?  My symbolic verbal signifier?  The sum total of my habits and accomplishments?";
            dialog.answers[dialogIndex][1] = "My name is Fogul, but I think what you actually want to know is-- What am I? ";

            dialogIndex++;
            dialog.questions[dialogIndex] = "What are you?";
            dialog.answers[dialogIndex] = new string[2];
            dialog.answers[dialogIndex][0] = "I am a Crow.  Your people have forgotten us, but we were brothers once. Sometimes one of you finds their way down here, usually as part of some punishment.  Sometimes it’s just an awful sense of direction.";
            dialog.answers[dialogIndex][1] = "Either way, there's no going back. ";

            dialogIndex++;
            dialog.questions[dialogIndex] = "What do I do?";
            dialog.answers[dialogIndex] = new string[2];
            dialog.answers[dialogIndex][0] = "That’s up to you. I wonder the same thing myself most days. Most humans die quickly here-- it is not a friendly place for the earth-bound.  There are objects of power here, however, left over from when the world was different.";
            dialog.answers[dialogIndex][1] = "You might survive if you find them.  Objects, fragments, gold.";

            dialogIndex++;
            dialog.questions[dialogIndex] = "Who am I?";
            dialog.answers[dialogIndex] = new string[2];
            dialog.answers[dialogIndex][0] = "If you don’t remember, then does it matter who you were?  You are a clean slate-- find out who you are for yourself.";
            dialog.answers[dialogIndex][1] = "The mind is weak.  My past self is in a haze.  I discover newly every day who I am. I suggest you do the same.";

            dialogIndex++;
            dialog.questions[dialogIndex] = "Goodbye";
            dialog.answers[dialogIndex] = new string[1];
            dialog.answers[dialogIndex][0] = "I'm afraid that I wasn't much help.";

        }
    }
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
