using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShadeController : MonoBehaviour {
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
            dialog.introDialog = "Hello small human.  I hope your are not too cold here.";

            dialog.questions = new string[numQuestions];
            dialog.answers = new string[numQuestions][];

            dialog.questions[dialogIndex] = "What are you?";
            dialog.answers[dialogIndex] = new string[2];
            dialog.answers[dialogIndex][0] = "I am nothing now.  I was once the Pontifax-- The Bridge-Builder.  We created something beautiful here.";
            dialog.answers[dialogIndex][1] = "I am trapped now as punishment, on this dark, wet island.";

            dialogIndex++;
            dialog.questions[dialogIndex] = "Punishment for what?";
            dialog.answers[dialogIndex] = new string[2];
            dialog.answers[dialogIndex][0] = "For being the son of the Remaker.  For siding with him in the war.  For killing.";
            dialog.answers[dialogIndex][1] = "For not believing that the world needs to be controlled.  For many, many things.";

            dialogIndex++;
            dialog.questions[dialogIndex] = "Who am I?";
            dialog.answers[dialogIndex] = new string[3];
            dialog.answers[dialogIndex][0] = "I do not know you, I am sorry.";
            dialog.answers[dialogIndex][1] = "Neither can I tell you what to do.  There are other shades, they may know more than I.  If you find the one they called The Messenger,  I believe that you will know who you are-- he knew a great many things.";
            dialog.answers[dialogIndex][2] = " If you find one called The Artist, tell her “The chasm fills my hands” and she will know what that means. She will be able to help you.  ";


            dialogIndex++;
            dialog.questions[dialogIndex] = "What happened to this place?";
            dialog.answers[dialogIndex] = new string[1];
            dialog.answers[dialogIndex][0] = "It was so beautiful.  We tore it apart.  The warmth is gone.  The sun... ";


            dialogIndex++;
            dialog.questions[dialogIndex] = "What do I do now?";
            dialog.answers[dialogIndex] = new string[2];
            dialog.answers[dialogIndex][0] = "I do not know where you should go, but if you do not want to stay here, you will need to rebuild one of my portals.";
            dialog.answers[dialogIndex][1] = "Find the five rune fragments, and they will open a new portal where you first fell in here.";

            dialogIndex++;
            dialog.questions[dialogIndex] = "Goodbye";
            dialog.answers[dialogIndex] = new string[1];
            dialog.answers[dialogIndex][0] = "Good luck.  Stay warm";

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
