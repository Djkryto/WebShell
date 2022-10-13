using System.IO;
using System.Text;
using TaskDNS.App;
using TaskDNS.Models;

namespace TaskDNS.Tools
{
    public class ProcessingCommand
    {
        private const string root = @"C:\";
        private bool isReturn = false;
        private bool isCd = false;
        private string nextPath;

        public string Processing(string path, string command)
        {
            nextPath = path;
            string localCommand = command;

            ClearSpace(ref localCommand);
            ClearDoubleQuote(ref localCommand);
            ClearCD(path,ref localCommand, ref isCd);

            if (isCd)
            {
                if (!isReturn)
                {
                    CheckVoidString(ref localCommand);
                    ClearDoubleQuote(ref localCommand);
                    localCommand = ClearSpaceInPathDirection(localCommand);
                    CheckSmallNumberCharacter(ref path, ref localCommand, ref nextPath, ref isReturn);
                }
                else
                {
                    nextPath = localCommand;
                    AddCD(ref localCommand);
                }
            }
            isReturn = false;

            return nextPath;
        }

        private void ClearSpace(ref string command)
        {
            if (command != "" && command != null)
            {
                command = command.Trim();
            }
            else
            {
                isReturn = true;
            }
        }

        private void ClearCD(string path,ref string command,ref bool isCD)
        {
            string localString = command[0].ToString() + command[1].ToString();
            if (string.Equals(localString,"CD",StringComparison.OrdinalIgnoreCase))
            {
                isCD = true;
                command = command.Remove(0, 2);
                command = command.Trim();

                if(command.Length == 0)
                {
                    nextPath = path;
                    isCD = false;
                }
            }
            else
            {
                isCd = false;
            }
        }

        private void ClearDoubleQuote(ref string command)
        {
            command = command.Replace('/', '\\');
            command = command.Replace('"', ' ').Trim();


            if(command.Length > 1)
                if (command[0] == '\\')
                {
                   command = command.Remove(0,1);
                }
        }

        private void CheckVoidString(ref string comman)
        {
            if(comman == null || comman == "")
            {
                comman = "CD";
                return;
            }
        }

        private void CheckSmallNumberCharacter(ref string path, ref string command, ref string nextPath, ref bool isReturn)
        {
            if (command.Length == 1)// Перебор простых команд CD 
            {
                if(command[0] == '/')
                {
                    nextPath = root;
                    isReturn = true;
                    return;
                }
                else
                {
                    nextPath = root;
                }
            }
            else if (command.Length == 2)
            {
                if (command == "..")
                {
                    LevelDownPath(ref path);

                    nextPath = path;
                    isReturn = true;
                    return;
                }
                else if(command == "./")
                {
                    isReturn = true;
                    return;
                }
            }
            else
            {
                CheckManyNumberCharacter(ref path, ref command, ref nextPath, ref isReturn);
            }
        }

        private void LevelDownPath(ref string nextPath)//Стираем путь до первого разделения
        {
            for (int i = nextPath.Length - 1; i > 0; i--)
            {
                string pathI = nextPath[i].ToString();

                if (pathI != @"\" && pathI != @"\\")
                {
                    nextPath = nextPath.Remove(i);
                }
                else
                {
                    if (nextPath.Length > 3)
                    {
                        nextPath = nextPath.Remove(i);
                    }
                    break;
                }
            }
        }

        private void CheckManyNumberCharacter(ref string path, ref string command, ref string nextPath, ref bool isReturn)// Проверка на количество длинны команды(потенциального пути)
        {
            if(command.Length >= 3)
            {
                CheckAccuracyPath(ref path, ref command, ref nextPath, ref isReturn);// Проверяем на существование
                if (!isReturn)
                {
                    OpenLenghtPath(ref path, ref command, ref nextPath, ref isReturn);//Открывает папку
                }
            }
        }

        private void OpenLenghtPath(ref string path, ref string command,ref string nextPath,ref bool isReturn)
        {
            string localCommnd = command[0].ToString() + command[1].ToString() + command[2].ToString();//Проверка первых символов на полный путь (C:\)
            
            if (localCommnd == root)// Если соответсвует то открываем полный путь
            {
                 FullPaht(ref path,ref command,ref nextPath,ref isReturn);
            }
            else //Если нет то открываем без C:\
            {

                ShortPath(ref path, ref command, ref nextPath, ref isReturn);
            }
        }

        private void FullPaht(ref string path,ref string command, ref string nextPath, ref bool isReturn)
        {
            if(path != command)
            {
                if(command.Length > path.Length)// Если путь комманды превышает длину текущего пути то помещаем конец команды в следующий путь
                {
                    nextPath = command;
                    isReturn = true;
                    return;
                }
                else if(command.Length == path.Length)// Если команды соответствуют то путь не изменится следовательно выходим из обработки
                {
                    isReturn = true;
                    return;
                }
            }
        }

        private void ShortPath(ref string path, ref string command, ref string nextPath, ref bool isReturn)
        {
            if(command.Length > 3)
            {
                command = command.Replace(".", string.Empty);
                if (command[0] == '/' || command[0] == '\\')
                {
                    command = command.Remove(0, 1);
                }
            }


            if (path.Length == 3)
                nextPath = root + command;
            else
                nextPath = path + "\\" + command;
            
            isReturn = true;
        }

        private void AddCD(ref string command)
        {
            command = "CD " + command;
        }

        private void CheckAccuracyPath(ref string path, ref string command, ref string nextPath,ref bool isReturn)
        {
            string localCommnd = "";
            if (command.Length >= 3)
            {
                localCommnd = command[0].ToString() + command[1].ToString() + command[2].ToString();

                if(localCommnd == root)
                {
                    command = command.Trim('.');
                }
                else
                {
                    command = command.Replace(".", string.Empty);
                }
                if (command[0] == '/' || command[0] == '\\')
                {
                    command = command.Remove(0, 1);
                }
                if(command.Length == 1)
                {
                    nextPath = root;
                    isReturn = true;
                    return;
                }
            }

            if(command.Length >= 3){
                localCommnd = command[0].ToString() + command[1].ToString() + command[2].ToString();
            }

            if (localCommnd == root)// Если соответсвует то открываем полный путь
            {
                try
                {
                    DirectoryInfo mainDirectory = new DirectoryInfo(command);
                    DirectoryInfo[] childDirectory = mainDirectory.GetDirectories();
                }
                catch
                {

                    isReturn = true;
                }
            }
            else
            {
                try
                {
                    DirectoryInfo mainDirectory = new DirectoryInfo(path + "\\" + command);
                    DirectoryInfo[] childDirectory = mainDirectory.GetDirectories();
                }
                catch
                {
                    nextPath = root;
                    isReturn = true;
                }
            }
        }

       private string ClearSpaceInPathDirection(string path)
       {
            string localPath = path;
            char[] arrayChar = path.ToCharArray();

            for(int i = 0;i < localPath.Length; i++)
            {
                if (arrayChar[i] == '\\')
                {
                    if ( i - 1 < localPath.Length && i + 1 < localPath.Length)
                    {
                        if (localPath[i - 1] == ' ')
                        {
                            localPath = localPath.Remove(i - 1, 1);
                            if (localPath[i] == ' ')
                            {
                                localPath = localPath.Remove(i, 1);
                            }
                        }
                        else
                        {
                            if (localPath[i+1] == ' ')
                            {
                                localPath = localPath.Remove(i+1, 1);
                            }
                        }
                    }
                }
            }

            return localPath;
       }


        //    public void Processing(ref string path, ref string command, ref int countChildDirectory, ref string nextPath)
        //    {
        //        string localCommand = command;
        //        //DirectFolders(path);
        //        while (command.Contains('"'))
        //        {
        //            command = command.Remove(command.IndexOf('"'), 1);
        //        }

        //        string localString = "";
        //        bool isCD = false;

        //        if (localCommand[0] == ' ')
        //        {
        //            localCommand = localCommand.Remove(0, 1);
        //        }

        //        localString = localCommand.Remove(2);

        //        isCD = CheckCommand(localString);

        //        if (isCD)
        //        {
        //            if (CheckJustCD(ref command))
        //            {
        //                return;
        //            }

        //            string bufferLine = localCommand.Remove(0, 2);
        //            string processCommand = bufferLine;

        //            ClearSpace(ref processCommand);

        //            if (CheckReturnRootDirecotry(ref path, processCommand))
        //                return;

        //            TypeCommand(ref path,ref processCommand,ref nextPath);

        //            command = processCommand;
        //        }
        //        countChildDirectory = 0;
        //    }

        //    private void ProcessingCD()
        //    {

        //    }
        //    private bool CheckJustCD(ref string command)
        //    {
        //        string localString = command;
        //        bool stop = false;

        //        int i = 0;

        //        while (!stop)
        //        {
        //            if (localString[i] == ' ')
        //            {
        //                localString = localString.Remove(i, 1);
        //            }
        //            else
        //            {
        //                stop = true;
        //            }

        //            i++;
        //        }

        //        if(localString.Length == 2)
        //        {
        //            command = localString;
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }

        //    private bool CheckCommand(string command)
        //    {
        //        if (string.Equals(command, "CD", StringComparison.OrdinalIgnoreCase))
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }

        //    private bool CheckFullDirectory(string path)
        //    {
        //        if (path[0] == 'C')
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }

        //    private bool CheckLevelDown(string path)
        //    {
        //        if (path == "..")
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }

        //    private bool CheckOpenFolder(ref string path)
        //    {
        //        bool noHaveCharacter = true;
        //        string localString = "./"; 
        //        for (int i = 0; i < path.Length - 1;i++)
        //        {
        //            if(i != path.Length - 1)
        //            {
        //                string pathCharacter = path[i].ToString() + path[i + 1].ToString();
        //                if (localString == pathCharacter)
        //                {
        //                    path = path.Remove(i,2);
        //                }
        //            }
        //        }

        //        for(int i = 0; i < path.Length; i++)
        //        {
        //            string pathI = path[i].ToString();

        //            if (pathI == "/" && pathI == @"\" && pathI == @"\\"&& pathI == ".")
        //            {
        //                noHaveCharacter = false;
        //            }
        //            else
        //            {
        //                noHaveCharacter = true;
        //            }
        //        }

        //        return noHaveCharacter;
        //    }

        //    private void TypeCommand(ref string path,ref string command, ref string nextPath)
        //    {
        //        if (CheckFullDirectory(command))
        //        {
        //            nextPath = command;
        //        }
        //        else if (CheckLevelDown(command))
        //        {
        //            LevelDown(ref path,ref command);
        //        }
        //        else if (CheckLongDirectory(command)) 
        //        {
        //            LongDirectory(ref path,ref command,ref nextPath);
        //        }
        //        else if (CheckLongCharacter(path))
        //        {
        //            ClearPath(ref path);
        //        }
        //        else if (CheckOpenFolder(ref command))
        //        {
        //            OpenFolder(ref path,ref command,ref nextPath);
        //        }
        //    }


        //    private void LevelDown(ref string path,ref string command)
        //    {
        //        if (path.Length > 3)
        //        {
        //            path = path.Remove(path.Length - 1, 1);

        //            for (int i = path.Length - 1; i > 0; i--)
        //            {
        //                string pathI = path[i].ToString();

        //                if (pathI != "/" && pathI != @"\" && pathI != @"\\")
        //                {
        //                    path = path.Remove(i);
        //                }
        //                else
        //                {
        //                    command = "CD " + command; 
        //                    break;
        //                }
        //            }
        //        }
        //    }

        //    private void OpenFolder(ref string path,ref string command, ref string nextPath)
        //    {
        //        nextPath += command;
        //        string localString = command[0].ToString() + command[1].ToString();

        //        if (!string.Equals(localString, "CD", StringComparison.OrdinalIgnoreCase))
        //            command = "CD " + command;

        //        checkAccuracyPath(ref path,ref command, ref nextPath);
        //    }

        //    private bool CheckLongCharacter(string path)
        //    {
        //        string localString = "../.";

        //        for (int i = 0; i < path.Length - 1; i++)
        //        {
        //            if(i > path.Length - 4 && path.Length > 3)
        //            {
        //                string pathI = path[i].ToString() + path[i + 1].ToString() + path[i + 2].ToString() + path[i + 3].ToString();
        //                if (pathI == localString)
        //                {
        //                    return true;
        //                }
        //                else
        //                {
        //                    return false;
        //                }
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //        return false;
        //    }

        //    private bool CheckLongDirectory(string path)
        //    {
        //        string localString = "";
        //        string[] commands = new string[20]; 
        //        bool noHaveCharacter = true;
        //        int postionCommans = 0;
        //        for (int i = 0; i < path.Length - 1; i++)
        //        {
        //            if (i != path.Length - 1)
        //            {
        //                string pathI = path[i].ToString();
        //                if (pathI != "/" && pathI != @"\" && pathI != @"\\" ) 
        //                {
        //                   commands[postionCommans] += pathI;
        //                }
        //                else
        //                {
        //                    commands[postionCommans] = commands[postionCommans];
        //                    postionCommans++;
        //                }
        //            }
        //        }

        //        if (postionCommans > 0)
        //            return true;
        //        else
        //            return false;
        //    }

        //    private void LongDirectory(ref string path,ref string command,ref string nextPath)
        //    {
        //        string localString = "";
        //        string[] commands = new string[11];

        //        bool noHaveCharacter = true;
        //        int postionCommans = 0;

        //        for (int i = 0; i < command.Length; i++)
        //        {
        //            if (i != command.Length)
        //            {
        //                string pathI = command[i].ToString();
        //                if (pathI != "/" && pathI != @"\" && pathI != @"\\" && pathI != ".")
        //                {
        //                    commands[postionCommans] += pathI;
        //                }
        //                else
        //                {
        //                    postionCommans++;
        //                }
        //            }
        //        }
        //        command = @"CD C:\";
        //        for (int i = 0; i < commands.Length - 1; i++)
        //        {
        //            if (commands[i] != "" && commands[i] != null)
        //            {
        //                command += commands[i];
        //            }
        //        }

        //        WritePath(ref path, commands, ref nextPath);
        //    }

        //    private void ClearSpace(ref string command)
        //    {
        //        if(command != "" && command != null)
        //        {
        //            bool stop = false;

        //            int i = 0;

        //            while (!stop)
        //            {
        //                if (command[i] == ' ')
        //                {
        //                    command = command.Remove(i, 1);
        //                }
        //                else
        //                {
        //                    stop = true;
        //                }

        //                if (command.Length - 1 == i)
        //                {
        //                    stop = true;
        //                }

        //                i++;
        //            }
        //        }
        //    }

        //    private void WritePath(ref string path, string[] commands,ref string nextPath)
        //    {
        //        path = root;
        //        for (int i = 0; i < commands.Length - 1; i++)
        //        {
        //            if (commands[i] != "" && commands[i] != null)
        //            {
        //                nextPath += "\\"+ commands[i];
        //            }
        //        }
        //    }

        //    private bool CheckReturnRootDirecotry(ref string path,string command)
        //    {
        //       if(command == "/")
        //       {
        //            path = root;
        //            return true;
        //       }
        //       else
        //       {
        //            return false;
        //       }
        //    }

        //    private void ClearPath(ref string path)
        //    {
        //        path = root;
        //    }

        //    private void checkAccuracyPath(ref string path,ref string command,ref string nextPath)
        //    {
        //        try
        //        {
        //            DirectoryInfo mainDirectory = new DirectoryInfo(path + nextPath);
        //            DirectoryInfo[] childDirectory = mainDirectory.GetDirectories();
        //        }
        //        catch
        //        {
        //            path = root;
        //            nextPath = "";
        //        }
        //    }
        //}
    }
}
