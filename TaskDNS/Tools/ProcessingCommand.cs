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
        private const string root = @"C:\";

        public string Processing(string path, string command)
        {
            var isCD = isCommandCD(command);
            if (!isCD)
                return path;

            var localCommand = ClearCharacters(command);

            localCommand = ClearSpaceInPath(localCommand);

            return ProcessingPath(path, localCommand); 
        }

        private bool isCommandCD(string command)
        {
            command = ClearSpaceInPath(command);
            string localString = command.Remove(2);

            if (string.Equals(localString, "CD", StringComparison.OrdinalIgnoreCase))
                return true;
            else
                return false;
        }

        private string ProcessingPath(string path, string command)
        {
            if (command.Length == 1)// Перебор простых команд CD 
            {
                if(command[0] == '/')
                    return root;
                else
                    return root;
            }
            else if (command.Length == 2)
            {
                if (command == "..")
                    return LevelDownPath(path);
                else if(command == "./")
                    return path;
            }
            else
                path = ProcessingDirectory(path, command);

            return path;
        }

        private string LevelDownPath(string path)//Стираем путь до первого разделения
        {
            if(path.Length <= 3)
                return path;

            return RemoveLastName(path);
        }

        private string RemoveLastName(string path)
        {
           path = path.Split('\\')
                    .Select(x => x.Trim())
                    .SkipLast(1)
                    .Aggregate((first, end) => $"{first}\\{end}")
                    .ToString();

            if (path.Length < 3)
                return root;
            else
                return path;
        }

        private string ProcessingDirectory(string path, string command)// Проверка на количество длинны команды(потенциального пути)
        {
            bool isExistence = CheckAccuracyDirectory(ref path, ref command);// Проверяем на существование

            if (isExistence)
               return OpenDirectory(path, command);//Открывает папку

            return path;
        }

        private string OpenDirectory(string path, string command)
        {
            string localCommnd = command.Remove(3);

            if (localCommnd == root)// Проверка первых символов на полный путь (C:\)
                return FullPaht(path, command);
            else 
                return ShortPath(path, command);
        }

        private string FullPaht(string path, string command)
        {
            if(path != command)
                if(command.Length > path.Length)// Если путь комманды превышает длину текущего пути то помещаем конец команды в следующий путь
                    return command;

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
                return root + command;
            else
                return path + "\\" + command;
        }

        private bool CheckAccuracyDirectory(ref string path,ref string command)
        {
            ///Обработка
            command = ProcessingOnAccuracyCommand(command);

            bool isProccesingResult = ProcessingOnAccuracyPath(command);

            if (isProccesingResult)
                return ProcessingDirectory(command,ref path);
            else
            {
                path = root;
                return false;
            }
        }

       private bool ProcessingDirectory(string command,ref string path)
       {
            string localCommand = command.Remove(3);
            if (localCommand == root)//Проверка на полный путь включаня название диска
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
                    path = root;
                    return false;
                }
            }
       }

       private string ClearSpaceInPath(string path)
       {
            return path.Split('\\')
                .Select(x => x.Trim())
                .Aggregate((first, end) => $"{first}\\{end}")
                .ToString();
       }

        private string ClearAllPoint(string localCommand)
        {
            if(localCommand.Remove(3).IndexOf('\\') != -1)
                localCommand = localCommand.Replace(".", string.Empty);
            return localCommand;
        }

        private string ProcessingOnAccuracyCommand(string command)
        {
            string localCommand = command.Remove(3);

            if (localCommand == root)
                command = command.Trim('.');
            else
                command = ClearAllPoint(command);

            if (command[0] == '/' || command[0] == '\\')
                command = command.Remove(0, 1);

            return command;
        }

        private bool ProcessingOnAccuracyPath( string command)
        {
            if (command.Length == 1)
                return false;
            else
                return true;
        }

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


        private string checkIsNameDirectory(string command)
        {
            bool word = false;
            for(int i = 0; i <command.Length; i++)
            {
               if(command[i] != '.' || command[i] != '*'|| command[i] != '/')
               {
                    word = true;
                    command.Remove(0, i);
               }
            }

            return command;
        }
    }
}
