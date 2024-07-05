using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManageLogin : MonoBehaviour
{
    //attached to EmptyHolder on Login Scene

    public InputField nameInput;

    public static string SessionID;
    public static string Username;
    public static string Lesson;

    string charlist = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456790";

    private void Awake()  //on first starting, set the SessionID
    {
        SessionID = "";
        for (int i = 0; i < 8; i++)
        {
            char c = charlist[Random.Range(0, charlist.Length)];
            SessionID = SessionID + c.ToString();
        }
    }

    public void CheckNameAndGo(string LessonInput)
    {
        if (nameInput.text.Length > 0)
        {
            Username = nameInput.text;
            Lesson = LessonInput;
            SceneManager.LoadScene("SampleScene");
        }

        else
        {
            nameInput.placeholder.GetComponent<Text>().text = "Must provide some info";
        }
    }

    public void ReticulocyteLogin(string LessonInput)
    {
        if (nameInput.text.Length > 0)
        {
            Username = nameInput.text;
            Lesson = LessonInput;
            SceneManager.LoadScene("Reticulocyte");
        }

        else
        {
            nameInput.placeholder.GetComponent<Text>().text = "Must provide some info";
        }
    }

    public void ChemotaxisLogin(string LessonInput)
    {
        if (nameInput.text.Length > 0)
        {
            Username = nameInput.text;
            Lesson = LessonInput;
            SceneManager.LoadScene("Chemotaxis");
        }

        else
        {
            nameInput.placeholder.GetComponent<Text>().text = "Must provide some info";
        }
    }
}
