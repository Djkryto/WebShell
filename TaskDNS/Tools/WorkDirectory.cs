namespace TaskDNS.Tools
{
    /// <summary>
    /// Класс отвечающий за обработку комманд.
    /// </summary>
    public class WorkDirectory
    {
        private const string Root = @"C:\";
        private const int CharacterAmoutCDAndSpace = 3;
        private const int CharacterAmoutCD = 2;
        /// <summary>
        /// Обработка команды с возвращаемым значением в виде нового пути.
        /// </summary>
        /// <param name="currentDirectory">Текущая директория работы процесса.</param>
        /// <param name="commandClient">Комманда от клиента.</param>
        /// <returns></returns>
        public string GetDirectory(string currentDirectory, string commandClient)
        {
            var isCd = IsCommandCd(commandClient);
            if (!isCd)
                return currentDirectory;

            var directory = FirstStageClearingExtraCharacters(commandClient);

            directory = ClearAllSpaceDirectory(directory);

            return ReadyDirectory(currentDirectory, directory); 
        }

        private static bool IsCommandCd(string commandClient)
        {
            commandClient = ClearAllSpaceDirectory(commandClient);
            var directory = commandClient.Remove(CharacterAmoutCD);

            return string.Equals(directory, "CD", StringComparison.OrdinalIgnoreCase);
        }

        private static string ReadyDirectory(string currentDirectory, string directory)
        {
            switch (directory.Length)
            {
                case 1:
                    return Root;
                case 2:
                    return directory == ".." ? RemoveLastNameDirectory(currentDirectory) : currentDirectory;
                default:
                    return ProcessDirectory(currentDirectory, directory);
            }
        }

        private static string RemoveLastNameDirectory(string currentDirectory)
        {
            if (currentDirectory.Length <= CharacterAmoutCDAndSpace)
                return currentDirectory;

            currentDirectory = currentDirectory.Split('\\')
                    .Select(x => x.Trim())
                    .SkipLast(1)
                    .Aggregate((first, end) => $"{first}\\{end}")
                    .ToString();

            return currentDirectory.Length < 3 ? Root : currentDirectory;
        }

        private static string ProcessDirectory(string currentDirectory, string directory)
        {
            var isExistence = CheckExistenceDirectory(currentDirectory,directory, out directory);

            return isExistence ? OpenDirectory(currentDirectory, directory) : Root;
        }

        private static string OpenDirectory(string currentDirectory, string directory)
        {
            var readyDirectory = directory.Remove(CharacterAmoutCDAndSpace);

            return readyDirectory == Root ? OpenWithDisk(currentDirectory, directory) : OpenNotDisk(currentDirectory, directory);
        }

        private static string OpenWithDisk(string currentDirectory, string directory)
        {
            return directory.Length > currentDirectory.Length ? directory : currentDirectory;
        }

        private static string OpenNotDisk(string currentDirectory,  string directory)
        {
            if(directory.Length > 3)
            {
                directory = ClearAllPoint(directory);
                if (directory[0] == '/' || directory[0] == '\\')
                    directory = directory.Remove(0, 1);
            }

            if (currentDirectory.Length == 3)
                return Root + directory;
          
            return currentDirectory + "\\" + directory;
        }

        private static bool CheckExistenceDirectory(string currentDirectory,string directory,out string result)
        {
             TwoStageClearingExtraCharacter(directory,out result);

            return result.Length > 1 && CheckExistsDirectory(result, currentDirectory);
        }

       private static bool CheckExistsDirectory(string directory, string currentDirectory)
       {
            var readyDirectory = directory.Remove(CharacterAmoutCDAndSpace);
           
            return readyDirectory == Root ? Directory.Exists(directory) : Directory.Exists(currentDirectory + "\\" + directory);
       }

       private static string ClearAllSpaceDirectory(string directory)
       {
            return directory.Split('\\')
                .Select(x => x.Trim())
                .Aggregate((first, end) => $"{first}\\{end}")
                .ToString();
       }

        private static string ClearAllPoint(string directory)
        {
            if(directory.Remove(CharacterAmoutCDAndSpace).IndexOf('\\') != -1)
                directory = directory.Replace(".", string.Empty);

            return directory;
        }

        private static void TwoStageClearingExtraCharacter(string directory, out string result)
        {
            var readyDirectory = directory.Remove(CharacterAmoutCDAndSpace);
            
            result = readyDirectory == Root ? directory.Trim('.') : ClearAllPoint(directory);

            if (result[0] == '/' || result[0] == '\\')
                result = result.Remove(0, 1);
        }

        private static string FirstStageClearingExtraCharacters(string directory)
        {
            if (string.IsNullOrEmpty(directory))
                return "CD";

            directory = directory.Trim()
                .Remove(0, CharacterAmoutCD)
                .Trim()
                .Replace('/', '\\')
                .Replace('"', ' ')
                .Trim();
            
            return string.IsNullOrEmpty(directory) ? "CD" : directory;
        }
    }
}
