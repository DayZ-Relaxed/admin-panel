namespace DayZRelaxed.Helpers
{
    public class LogParser
    {
        string LogFilePath;
        string ActiveLogFilePath;
        string LogFileName;

        public LogParser(int mapId)
        {
            var mapSettings = "settings-map0.json";
            if (mapId == 1) mapSettings = "settings-map1.json";

            var config = new ConfigurationBuilder().AddJsonFile(mapSettings).Build();

            this.LogFilePath = config.GetValue<string>("DayZLogs:LogFilesPath");
            this.ActiveLogFilePath = config.GetValue<string>("DayZLogs:LogFilesPathActive");
            this.LogFileName = config.GetValue<string>("DayZLogs:LogFileName");
        }

        public List<String> getDirs()
        {
            List<String> dirs = Directory.GetDirectories(this.LogFilePath, "", SearchOption.TopDirectoryOnly).ToList<String>();
            dirs.Add(this.ActiveLogFilePath);
            return dirs;
        }

        public string getFileName(string dirName)
        {
            return @$"{dirName}\{this.LogFileName}";
        }
    }
}
