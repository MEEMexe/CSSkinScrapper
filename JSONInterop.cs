using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
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
            exeDirPath = AppDomain.CurrentDomain.BaseDirectory;
            exeDirPath = exeDirPath.Replace("CSSkinScrapper.exe", "");
            jsonPath = exeDirPath + "ScrapperSettings.json";
        }

        public async Task Save(SaveFile objectToSave)
        {
            await using FileStream saveFile = File.OpenWrite(jsonPath);
            await JsonSerializer.SerializeAsync(saveFile, objectToSave, serializerOptions);
            saveFile.Close();
        }

        public async Task<SaveFile> Load()
        {
            if (!File.Exists(jsonPath))
            {
                Console.WriteLine("Creating new save File.\n");

                SaveFile newSave = new SaveFile()
                {
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
                return JsonSerializer.Deserialize<SaveFile>(json, serializerOptions);
            }
        }
    }

    public class SaveFile
    {
        public int runCount {  get; set; }
        public int skinCount { get; set; }
        public string filePath { get; set; }
        public List<string> skinNames { get; set; }
        public List<string> skinApiNames { get; set; }
        public List<string> skinBuyPrice { get;set; }

        public SaveFile()
        {
            runCount = 0;
            skinCount = 0;
            filePath = "";
            skinNames = new List<string>();
            skinApiNames = new List<string>();
            skinBuyPrice = new List<string>();
        }
    }
}
