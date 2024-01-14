using CSSkinScrapper.Interop;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

//#1 read summary from method below (FileStream.Close()) -> so call saveFile.Dispose ?

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

        public SaveFile Load()
        {
            return LoadAsync().GetAwaiter().GetResult();
        }

        public void Save(SaveFile objectToSave)
        {
            SaveAsync(objectToSave).GetAwaiter().GetResult();
        }

        private async Task SaveAsync(SaveFile objectToSave)
        {
            Console.WriteLine("\nSaving json file.");
            await using FileStream saveFile = File.OpenWrite(jsonPath);
            await JsonSerializer.SerializeAsync(saveFile, objectToSave, serializerOptions);
            //TODO: #1 at top
            saveFile.Close();
        }

        private async Task<SaveFile> LoadAsync()
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
                //TODO: #1 at top
                newFile.Close();
                return newSave;
            }
            else
            {
                Console.WriteLine("Loading save File.\n");

                StreamReader sr = new StreamReader(jsonPath);
                string json = sr.ReadToEnd();
                //TODO: #1 at top
                sr.Close();
                SaveFile? s = JsonSerializer.Deserialize<SaveFile>(json, serializerOptions);
                s.runCount++;
                await SaveAsync(s);
                return s;
            }
        }
    }
}