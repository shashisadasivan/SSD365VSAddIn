using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSD365VSAddIn.Settings
{
    class FetchSettings
    {
        private static string GetSettingsFileName()
        {
            string fileName = String.Empty;

            // get the current model name
            fileName = "SSD365VSAddIn_" + Common.CommonUtil.GetCurrentModel().Name + ".json";


            return fileName;
        }

        private static string GetSettingsFolder()
        {
            string assemblyFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return assemblyFolder;
        }
        private static string GetFilePath()
        {
            string fileName = GetSettingsFileName();
            string settingsFolder = GetSettingsFolder();
            var filePath = Path.Combine(settingsFolder, fileName);

            return filePath;
        }

        public static void SaveSettings(ModelSettings modelSettings)
        {
            var filePath = GetFilePath();

            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (var sw = new StreamWriter(filePath))
            {
                using (var jsonWriter = new JsonTextWriter(sw))
                {
                    serializer.Serialize(jsonWriter, modelSettings);
                }
            }
        }

        public static ModelSettings FindOrCreateSettings()
        {
            ModelSettings modelSettings = new ModelSettings();
            var filePath = GetFilePath();

            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            if (File.Exists(filePath))
            {
                // read the file
                using (var sr = new StreamReader(filePath))
                {
                    using (var jsonReader = new JsonTextReader(sr))
                    {
                        modelSettings = serializer.Deserialize<ModelSettings>(jsonReader);
                    }
                }
            }
            else
            {
                // create the file
                using (var sw = new StreamWriter(filePath))
                {
                    using (var jsonWriter = new JsonTextWriter(sw))
                    {
                        serializer.Serialize(jsonWriter, modelSettings);
                    }
                }
            }

            return modelSettings;
        }

    }
}
