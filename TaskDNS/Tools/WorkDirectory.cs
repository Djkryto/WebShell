namespace TaskDNS.Tools
{
    /// <summary>
    /// Класс отвечающий за обработку комманд.
    /// </summary>
    public class WorkDirectory
    {
        private const string Root = @"C:\";
        private const int CommandCharacterAmountCD = 3;

        /// <summary>
        /// Обработка команды с возвращаемым значением в виде нового пути.
        /// </summary>
        /// <param name="currentDirectory">Текущая директория.</param>
        /// <param name="command">Комманда.</param>
        /// <returns></returns>
        public string GetDirectory(string currentDirectory, string command)
        {
            var isCD = IsCommandCD(command);
            if (!isCD)
                return currentDirectory;

            var localCommand = FirstStageClearingExtraCharacters(command);

            localCommand = ClearAllSpaceDirectory(localCommand);

            return ReadyDirectory(currentDirectory, localCommand); 
            //int.TryParse(localCommand, out var result);
        }

        private bool IsCommandCD(string command)
        {
            command = ClearAllSpaceDirectory(command);
            var directory = command.Remove(2);

            return string.Equals(directory, "CD", StringComparison.OrdinalIgnoreCase);
        }

        private string ReadyDirectory(string path, string command)
        {
            switch (command.Length)
            {
                case 1:
                    return Root;
                case 2:
                    return command == ".." ? RemoveLastNameDirectory(path) : path;
                default:
                    path = ProcessDirectory(path, command);

                    return path;
            }
        }

        private string RemoveLastNameDirectory(string path)
        {
            if (path.Length <= CommandCharacterAmountCD)
                return path;

            path = path.Split('\\')
                    .Select(x => x.Trim())
                    .SkipLast(1)
                    .Aggregate((first, end) => $"{first}\\{end}")
                    .ToString();

            return path.Length < 3 ? Root : path;
        }

        private string ProcessDirectory(string path, string command)
        {
            var isExistence = CheckExistenceDirectory(path,command, out command);

            return isExistence ? OpenDirectory(path, command) : Root;
        }

        private string OpenDirectory(string path, string command)
        {
            var directory = command.Remove(CommandCharacterAmountCD);

            return directory == Root ? OpenWithDisk(path, command) : OpenNotDisk(path, command);
        }

        private static string OpenWithDisk(string path, string command)
        {
            return command.Length > path.Length ? command : path;
        }

        private string OpenNotDisk(string path,  string command)
        {
            if(command.Length > 3)
            {
                command = ClearAllPoint(command);
                if (command[0] == '/' || command[0] == '\\')
                    command = command.Remove(0, 1);
            }

            if (path.Length == 3)
                return Root + command;
          
            return path + "\\" + command;
        }

        private bool CheckExistenceDirectory(string path,string command,out string result)
        {
             TwoStageClearingExtraCharacter(command,out result);

            return result.Length > 1 && CheckExistsDirectory(result, path);
        }

       private bool CheckExistsDirectory(string command, string path)
       {
            var directory = command.Remove(CommandCharacterAmountCD);
           
            return directory == Root ? Directory.Exists(command) : Directory.Exists(path + "\\" + command);
       }

       private string ClearAllSpaceDirectory(string directory)
       {
            return directory.Split('\\')
                .Select(x => x.Trim())
                .Aggregate((first, end) => $"{first}\\{end}")
                .ToString();
       }

        private string ClearAllPoint(string localCommand)
        {
            if(localCommand.Remove(CommandCharacterAmountCD).IndexOf('\\') != -1)
                localCommand = localCommand.Replace(".", string.Empty);

            return localCommand;
        }

        private string TwoStageClearingExtraCharacter(string command, out string result)
        {
            var localCommand = command.Remove(CommandCharacterAmountCD);

            result = localCommand == Root ? command.Trim('.') : ClearAllPoint(command);

            if (result[0] == '/' || result[0] == '\\')
                result = result.Remove(0, 1);

            return result;
        }

        private string FirstStageClearingExtraCharacters(string command)
        {
            if (string.IsNullOrEmpty(command))
                return "CD";

            command = command.Trim()
                .Remove(0,2)
                .Trim()
                .Replace('/', '\\')
                .Replace('"', ' ')
                .Trim();
            
            return string.IsNullOrEmpty(command) ? "CD" : command;
        }
    }
}
