using UnityEngine;
using UnityEngine.UI;

public class DateInputMask : MonoBehaviour
{
    public InputField dateInputField;

    void Start()
    {
        dateInputField.onValueChanged.AddListener(FormatDateInput);
    }

    private void FormatDateInput(string input)
    {
        input = new string(System.Text.RegularExpressions.Regex.Replace(input, "[^0-9]", "").ToCharArray());

        if (input.Length > 8)
        {
            input = input.Substring(0, 8);
        }

        if (input.Length > 2)
        {
            input = input.Insert(2, "/");
        }
        if (input.Length > 5)
        {
            input = input.Insert(5, "/");
        }

        dateInputField.text = input;

        dateInputField.caretPosition = input.Length;
    }

    void OnDestroy()
    {
        dateInputField.onValueChanged.RemoveListener(FormatDateInput);
    }
}
