using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using TaskDNS.App;
using TaskDNS.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TaskDNS.Tools
{
    public class ProcessingCommand
    {
        private const string Root = @"C:\";

        /// <summary>
        /// Главный метод обработки команды.
        /// </summary>
        public string Processing(string currentDirectory, string command)
        {
            var isCD = isCommandCD(command);
            if (!isCD)
                return currentDirectory;

            var localCommand = ClearCharacters(command);

            localCommand = ClearSpaceInPath(localCommand);

            return ProcessingPath(currentDirectory, localCommand); 
        }
        /// <summary>
        /// Проверка на команду CD
        /// </summary>
        private bool isCommandCD(string command)
        {
            command = ClearSpaceInPath(command);
            string localString = command.Remove(2);

            if (string.Equals(localString, "CD", StringComparison.OrdinalIgnoreCase))
                return true;
            else
                return false;
        }
        /// <summary>
        /// Обработка пути
        /// </summary>
        private string ProcessingPath(string path, string command)
        {
            if (command.Length == 1)
            {
                if(command[0] == '/')
                    return Root;
                else
                    return Root;
            }
            else if (command.Length == 2)
            {
                if (command == "..")
                    return ProcessingPathOnRemoveLastName(path);
                else if(command == "./")
                    return path;
            }
            else
                path = ProcessingDirectoryOnAccuracy(path, command);

            return path;
        }
        /// <summary>
        /// Проверка перед стиранием пути до первого разделения.
        /// </summary>
        private string ProcessingPathOnRemoveLastName(string path)
        {
            if(path.Length <= 3)
                return path;

            return RemoveLastNamePath(path);
        }
        /// <summary>
        /// Стирание пути до первого разделения.
        /// </summary>
        private string RemoveLastNamePath(string path)
        {
           path = path.Split('\\')
                    .Select(x => x.Trim())
                    .SkipLast(1)
                    .Aggregate((first, end) => $"{first}\\{end}")
                    .ToString();

            if (path.Length < 3)
                return Root;
            else
                return path;
        }

        private string ProcessingDirectoryOnAccuracy(string path, string command)
        {
            bool isExistence = CheckAccuracyDirectory(ref path, ref command);

            if (isExistence)
               return OpenDirectory(path, command);//Открывает папку

            return path;
        }

        private string OpenDirectory(string path, string command)
        {
            string localCommnd = command.Remove(3);

            if (localCommnd == Root)
                return FullPaht(path, command);
            else 
                return ShortPath(path, command);
        }

        private static string FullPaht(string path, string command)
        {
            if(path != command)
            {
                if(command.Length > path.Length)
                    return command;
            }

            return path;
        }

        private string ShortPath(string path,  string command)
        {
            if(command.Length > 3)
            {
                command = ClearAllPoint(command);
                if (command[0] == '/' || command[0] == '\\')
                    command = command.Remove(0, 1);
            }

            if (path.Length == 3)
                return Root + command;
            else
                return path + "\\" + command;
        }
        /// <summary>
        /// Метод для обработки и проверки команды на существование директории.
        /// </summary>
        private bool CheckAccuracyDirectory(ref string path,ref string command)
        {
            ///Обработка
            command = ProcessingOnAccuracyCommand(command);
            bool isProccesingResult = ProcessingOnAccuracyPath(command);

            if (isProccesingResult)
                return ProcessingDirectory(command,ref path);
            else
            {
                path = Root;
                return false;
            }
        }
       /// <summary>
       /// Проверка на существование директории.
       /// </summary>
       private bool ProcessingDirectory(string command,ref string path)
       {
            string localCommand = command.Remove(3);
            if (localCommand == Root)//Проверка на полный путь включаня название диска
            {
                try
                {
                    DirectoryInfo mainDirectory = new DirectoryInfo(command);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else//Если не полный путь то к текушему пути добавляем комманду
            {
                try
                {
                    DirectoryInfo mainDirectory = new DirectoryInfo(path + "\\" + command);
                    return true;
                }
                catch
                {
                    path = Root;
                    return false;
                }
            }
       }
        /// <summary>
        /// Удаление все пробелы из пути.
        /// </summary>
       private string ClearSpaceInPath(string path)
       {
            return path.Split('\\')
                .Select(x => x.Trim())
                .Aggregate((first, end) => $"{first}\\{end}")
                .ToString();
       }
        /// <summary>
        /// Удаление всех точек.
        /// </summary>
        private string ClearAllPoint(string localCommand)
        {
            if(localCommand.Remove(3).IndexOf('\\') != -1)
                localCommand = localCommand.Replace(".", string.Empty);

            return localCommand;
        }
        /// <summary>
        /// Обработка команды перед проверка на существование директории.
        /// </summary>
        private string ProcessingOnAccuracyCommand(string command)
        {
            string localCommand = command.Remove(3);

            if (localCommand == Root)
                command = command.Trim('.');
            else
                command = ClearAllPoint(command);

            if (command[0] == '/' || command[0] == '\\')
                command = command.Remove(0, 1);

            return command;
        }
        /// <summary>
        /// Проверка длинны команды.
        /// </summary>
        private static bool ProcessingOnAccuracyPath(string command)
        {
            if (command.Length == 1)
                return false;
            else
                return true;
        }
        /// <summary>
        /// Удаление лишних символов.
        /// </summary>
        private string ClearCharacters(string command)
        {
            if (string.IsNullOrEmpty(command))
                return "CD";

            command = command.Trim()
                .Remove(0,2)
                .Trim()
                .Replace('/', '\\')
                .Replace('"', ' ')
                .Trim();

            if (string.IsNullOrEmpty(command))
                return "CD";

            return command;
        }
    }
}
