using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using TaskDNS.Channels;
using TaskDNS.Channels.Interface;
using TaskDNS.Channels.Models;
using TaskDNS.Tools;

namespace TaskDNS.App.Processes
{
    /// <summary>
    /// Класс открывающий процесс cmd.exe.
    /// </summary>
    public class CMD
    {
        private delegate bool ConsoleCtrlDelegate(uint type);
        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate handler, bool add);
        [DllImport("kernel32.dll")]
        internal static extern bool GenerateConsoleCtrlEvent(uint dwCtrlEvent, int dwProcessGroupId);

        private static string _directory = @"C:\";

        private static readonly ProcessCommand _processCommand = new ();
        private static Process _process;

        /// <summary>
        /// .ctor
        /// </summary>
        public CMD()
        {
            Start();
        }

        /// <summary>
        /// Запись в командную строку.
        /// </summary>
        public void Write(string command)
        {
            _directory = _processCommand.GetDirectory(_directory, command);
            _process.StandardInput.WriteLine(command);
        }
        /// <summary>
        /// Отправка клиенту данные о текущую директорию.
        /// </summary>
        public string GetDirectory()
        {
            return _directory;
        }

        /// <summary>
        ///  Отправляем клиенту список подкатологов текущей директории.
        /// </summary>
        public string[] GetDirectories()
        {
            return Directories();
        }

        /// <summary>
        /// Остановка выполняемой команды.
        /// </summary>
        public async Task StopAsync()
        {
            SetConsoleCtrlHandler(null, true);
            GenerateConsoleCtrlEvent(0, _process.Id);
            Thread.Sleep(10);
            SetConsoleCtrlHandler(null, false);

            await WriteInChannelAsync(new Complete(),null);
        }

        private static string[] Directories()
        {
            var mainDirectory = new DirectoryInfo(_directory);
            var directories = mainDirectory.GetDirectories();
            var directoriesArray = directories.Select(x => x.FullName).ToArray();
            return directoriesArray;
        }

        private static void Start()
        {
            _process = new Process();

            _process.StartInfo.FileName = "cmd.exe";
            _process.StartInfo.CreateNoWindow = false;
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.RedirectStandardError = true;
            _process.StartInfo.RedirectStandardInput = true;
            _process.StartInfo.RedirectStandardOutput = true;
            _process.StartInfo.WorkingDirectory = _directory;

            _process.OutputDataReceived += new DataReceivedEventHandler(async (sender, e) =>
            {
                var command = new Executive(e.Data);
                await WriteInChannelAsync(command, command.Output);
            });

            _process.ErrorDataReceived += new DataReceivedEventHandler(async (sender, e) =>
            {
                var command = new Error(e.Data);
                await WriteInChannelAsync(command, command.Output);
            });

            _process.Start();
            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();
        }

        private static async Task WriteInChannelAsync(ICommandWithStatus commandWithStatus,string textCommand)
        {
            if(string.IsNullOrEmpty(textCommand))
                await ChannelProvider.CommandChannel.Writer.WriteAsync(new Complete());
            
            await ChannelProvider.CommandChannel.Writer.WriteAsync(commandWithStatus);
        }
    }
}