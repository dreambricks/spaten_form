using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine.SceneManagement;

public class FormValidator : MonoBehaviour
{
    public InputField nomeInput;
    public InputField emailInput;
    public InputField cpfInput;
    public InputField telefoneInput;

    public Text nomeErrorText;
    public Text emailErrorText;
    public Text cpfErrorText;
    public Text telefoneErrorText;

    public Button confirmar;

    private void Start()
    {
        confirmar.onClick.AddListener(() => ValidateForm());

        nomeInput.onValueChanged.AddListener(delegate { ClearErrorText(nomeErrorText); });
        emailInput.onValueChanged.AddListener(delegate { ClearErrorText(emailErrorText); });
        cpfInput.onValueChanged.AddListener(delegate { ClearErrorText(cpfErrorText); });
        telefoneInput.onValueChanged.AddListener(delegate { ClearErrorText(telefoneErrorText); });
    }

    public void ValidateForm()
    {
        bool isFormValid = true;

        if (string.IsNullOrEmpty(nomeInput.text) || nomeInput.text.Length < 3)
        {
            nomeErrorText.text = "Nome inválido.";
            isFormValid = false;
        }
        else
        {
            nomeErrorText.text = "";
            PlayerPrefs.SetString("nome", nomeInput.text);
        }

        // if (string.IsNullOrEmpty(emailInput.text) || !IsValidEmail(emailInput.text))
        // {
        //     emailErrorText.text = "Email inválido.";
        //     isFormValid = false;
        // }
        // else
        // {
        //     emailErrorText.text = "";
        //     PlayerPrefs.SetString("email", emailInput.text);
        // }

        if (string.IsNullOrEmpty(cpfInput.text) || !IsValidCPF(cpfInput.text))
        {
            cpfErrorText.text = "CPF inválido.";
            isFormValid = false;
        }
        else
        {
            cpfErrorText.text = "";
            PlayerPrefs.SetString("cpf", cpfInput.text);
        }

        // if (string.IsNullOrEmpty(telefoneInput.text) || !IsValidTelefone(telefoneInput.text))
        // {
        //     telefoneErrorText.text = "Telefone inválido.";
        //     isFormValid = false;
        // }
        // else
        // {
        //     telefoneErrorText.text = "";
        //     PlayerPrefs.SetString("telefone", telefoneInput.text);
        // }

        if (isFormValid)
        {
            SavePlayerDataToCSV();
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("SampleScene");
        }
    }

    private bool IsValidEmail(string email)
    {
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailPattern);
    }

    private bool IsValidCPF(string cpf)
    {
        string cpfPattern = @"^\d{3}\.\d{3}\.\d{3}\-\d{2}$";
        return Regex.IsMatch(cpf, cpfPattern) && IsCpfValidLogic(cpf);
    }

    private bool IsCpfValidLogic(string cpf)
    {
        cpf = cpf.Replace(".", "").Replace("-", "");

        if (cpf.Length != 11 || new string(cpf[0], 11) == cpf)
            return false;

        int[] mult1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] mult2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCpf = cpf.Substring(0, 9);
        int sum = 0;

        for (int i = 0; i < 9; i++)
        {
            sum += int.Parse(tempCpf[i].ToString()) * mult1[i];
        }

        int remainder = sum % 11;
        remainder = remainder < 2 ? 0 : 11 - remainder;

        if (remainder != int.Parse(cpf[9].ToString()))
            return false;

        tempCpf += remainder;
        sum = 0;

        for (int i = 0; i < 10; i++)
        {
            sum += int.Parse(tempCpf[i].ToString()) * mult2[i];
        }

        remainder = sum % 11;
        remainder = remainder < 2 ? 0 : 11 - remainder;

        return remainder == int.Parse(cpf[10].ToString());
    }

    private bool IsValidTelefone(string telefone)
    {
        string telefonePattern = @"^\(\d{2}\) \d{5}-\d{4}$";
        return Regex.IsMatch(telefone, telefonePattern);
    }

    private void ClearErrorText(Text errorText)
    {
        errorText.text = "";
    }


    public void SavePlayerDataToCSV()
    {
        string nome = PlayerPrefs.GetString("nome", "N/A");
        string idade = PlayerPrefs.GetString("nascimento", "N/A");
        string email = PlayerPrefs.GetString("email", "N/A");
        string cpf = PlayerPrefs.GetString("cpf", "N/A");
        string telefone = PlayerPrefs.GetString("telefone", "N/A");

        string csvLine = $"{nome},{idade},{email},{cpf},{telefone}";

        string filePath = Path.Combine(Application.persistentDataPath, "player_data.csv");

        if (!File.Exists(filePath))
        {
            string header = "Nome,DtNascimento,Email,CPF,Telefone";
            File.WriteAllText(filePath, header + "\n");
        }

        File.AppendAllText(filePath, csvLine + "\n");

        Debug.Log("Dados salvos em: " + filePath);
    }
}
