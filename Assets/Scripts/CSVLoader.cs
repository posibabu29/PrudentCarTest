using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVLoader : MonoBehaviour
{
    public List<PartData> partDataList = new List<PartData>();

    void Start()
    {
        //string path = Path.Combine(Application.dataPath, "Resources/data.csv");
        LoadCSV();
    }

    public void LoadCSV()
    {


        TextAsset csvData = Resources.Load<TextAsset>("data"); // omit the file extension

        //if (csvData != null)
        //{
        //    string[] lines = csvData.text.Split(new char[] { '\n' });
        //    foreach (string line in lines)
        //    {
        //        Debug.Log(line); // Process each line as needed
        //    }
        //}
        //else
        //{
        //    Debug.LogError("CSV file not found!");
        //}


        //string[] lines = File.ReadAllLines(filePath);

        string[] lines = csvData.text.Split(new char[] { '\n' });
        
        // Skip the header and loop through the lines
        for (int i = 1; i < lines.Length-1; i++)
        {

            Debug.Log(lines[i]);
            string[] entries = lines[i].Split(',');
            PartData partData = new PartData
            {
                Part = entries[0],
                Description = entries[1]
            };
            partDataList.Add(partData);
        }
    }

    public string GetDescription(string partName)
    {
        foreach (var part in partDataList)
        {
            if (part.Part == partName)
            {
                return part.Description;
            }
        }
        return "Part not found.";
    }
}
[System.Serializable]
public class PartData
{
    public string Part;
    public string Description;
    
}