using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
                SaveFile? s = JsonSerializer.Deserialize<SaveFile>(json, serializerOptions);
                s.runCount++;
                await Save(s);
                return s;
            }
        }
    }

    public class SaveFile
    {
        [JsonInclude]
        public int runCount;
        [JsonInclude]
        public string filePath;
        [JsonInclude]
        public List<Skin> skinList { get; private set; }

        [JsonIgnore]
        public int skinCount => skinList.Count;

        public SaveFile()
        {
            runCount = 0;
            filePath = "";
            skinList = new List<Skin>();
        }
    }
}