using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Reflection;
using System;

namespace CSSkinScrapper
{
    internal class JSONInterop
    {
        private static JsonSerializerOptions serializerOptions = new JsonSerializerOptions { WriteIndented = true };
        
        private string exeDirPath;
        private string jsonPath;

        public JSONInterop()
        {
            exeDirPath = Assembly.GetExecutingAssembly().Location;
            exeDirPath = exeDirPath.Replace("CSSkinScrapper.dll", "");
            jsonPath = exeDirPath + "ScrapperSettings.json";
        }

        public async void Save(SaveFile objectToSave)
        {
            string json = JsonSerializer.Serialize(objectToSave, serializerOptions);
            await using FileStream saveFile = File.OpenWrite(jsonPath);
            await JsonSerializer.SerializeAsync(saveFile, json, serializerOptions);
            saveFile.Close();
        }

        public async Task<SaveFile> Load()
        {
            if (!File.Exists(jsonPath))
            {
                Console.WriteLine("Creating new save File.\n");

                SaveFile newSave = new SaveFile()
                {
                    skinCount = 0,
                    filePath = exeDirPath
                };

                await using FileStream newFile = File.Create(jsonPath);
                await JsonSerializer.SerializeAsync(newFile, newSave, serializerOptions);
                newFile.Close();
                return newSave;
            }
            else
            {
                Console.WriteLine("Loading save File.\n");

                StreamReader sr = new StreamReader(jsonPath);
                string json = sr.ReadToEnd();
                sr.Close();
                return JsonSerializer.Deserialize<SaveFile>(json);
            }
        }
    }

    public class SaveFile
    {
        public int skinCount { get; set; }
        public string filePath { get; set; }
        public List<string> skinNames { get; set; }
        public List<string> skinApiNames { get; set; }
        public List<string> skinBuyPrice { get;set; }

        public SaveFile()
        {
            skinCount = 0;
            filePath = "";
            skinNames = new List<string>();
            skinApiNames = new List<string>();
            skinBuyPrice = new List<string>();
        }
    }
}
