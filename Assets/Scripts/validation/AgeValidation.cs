using UnityEngine;
using UnityEngine.UI;
using System;

public class AgeValidation : MonoBehaviour
{
    public InputField birthDateInput;
    public Text resultText;

    public Button button;

    [SerializeField] private GameObject ageScreen;

    [SerializeField] private GameObject form;

    private void Start()
    {
        button.onClick.AddListener(() => ValidateAge());
    }

    public void ValidateAge()
    {
        DateTime birthDate;
        if (DateTime.TryParse(birthDateInput.text, out birthDate))
        {
            int age = CalculateAge(birthDate);
            if (age >= 18)
            {
                resultText.text = "Idade validada: maior de idade.";
                PlayerPrefs.SetString("nascimento", birthDateInput.text);
                Debug.Log(birthDateInput.text);
                form.SetActive(true);
                ageScreen.SetActive(false);
            }
            else
            {
                resultText.text = "É preciso ter mais de 18 anos para participar";
                if (ColorUtility.TryParseHtmlString("#FF6464", out Color redColor))
                {
                    resultText.color = redColor;
                }
            }
        }
        else
        {
            resultText.text = "Data de nascimento inválida. Por favor, insira uma data válida.";
            resultText.color = Color.yellow; // Opcional: muda a cor do texto para amarelo
        }

        Invoke("ClearResultText", 4f);
    }

    private void ClearResultText()
    {
        resultText.text = "";
    }

    private int CalculateAge(DateTime birthDate)
    {
        DateTime today = DateTime.Today;
        int age = today.Year - birthDate.Year;
        if (birthDate > today.AddYears(-age)) age--;
        return age;
    }
}
