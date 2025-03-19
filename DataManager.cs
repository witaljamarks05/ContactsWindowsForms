using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public static class DataManager
{
    private static readonly string filePath = "contacts.json";

    public static List<Contact> LoadContacts()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Contact>>(json);
        }
        return new List<Contact>();
    }

    public static void SaveContacts(List<Contact> contacts)
    {
        string json = JsonConvert.SerializeObject(contacts, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }
}