using QuestionnaireTeamBot.Controllers;
using System.Text.Json;

QuestionnaireTeamBot.Models.Settings settings = null;
try
{
    string json = File.ReadAllText("settings.json");
    settings = JsonSerializer.Deserialize<QuestionnaireTeamBot.Models.Settings>(json);
}
catch (Exception ex)
{
    Console.WriteLine($"Error read settings.json: {ex.Message}");
    try
    {
        string fileName = "settings.json";
        string jsonString = JsonSerializer.Serialize(new QuestionnaireTeamBot.Models.Settings());
        File.WriteAllText(fileName, jsonString);
    }
    catch (Exception ex2)
    {
        Console.WriteLine($"Error create settings.json: {ex.Message}");
    }
}

if (settings != null)
{
    QuestionnaireTeamBot.Config.Settings = settings;
    try
    {
        var temp = new Listener();
        temp.Lisen();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error start bot: {ex.Message}");
    }
}
Console.ReadLine();