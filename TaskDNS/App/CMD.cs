using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using TaskDNS.Channels;
using TaskDNS.Channels.Interface;
using TaskDNS.Channels.Models;
using TaskDNS.Tools;


namespace TaskDNS.App
{
    public class CMD
    {
        private delegate bool ConsoleCtrlDelegate(uint type);
        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate handler, bool add);
        [DllImport("kernel32.dll")]
        internal static extern bool GenerateConsoleCtrlEvent(uint dwCtrlEvent, int dwProcessGroupId);

        private static string _directory = @"C:\";

        private static Process _process;
        private static ProcessingCommand _processing = new ProcessingCommand();

        public CMD()
        {
            Start();
        }

        public void Write(string command)
        {
            _directory = _processing.Processing(_directory,command);
            _process.StandardInput.WriteLine(command);
        }
        /// <summary>
        ///  Отправка клиенту текущую директорию.
        /// </summary>
        public string GetDirectory()
        {
            return _directory;
        }
        /// <summary>
        ///  Отправляем клиенту список катологов текущей директории.
        /// </summary>
        public string[] GetDirectories()
        {
            return childDirecty();
        }
        /// <summary>
        ///  Остановка выполняемой команды.
        /// </summary>
        public void Stop()
        {
            SetConsoleCtrlHandler(null, true);
            GenerateConsoleCtrlEvent(0, _process.Id);
            Thread.Sleep(10);
            SetConsoleCtrlHandler(null, false);

            SendOutputClient();
        }

        private async Task SendOutputClient()
        {
            var complete = new Complete();
            await ChannelProvider.CommandChannel.Writer.WriteAsync(complete);
        }
        /// <summary>
        ///  Получаем список катологов из текущей директории.
        /// </summary>
        private string[] childDirecty()
        {
            DirectoryInfo mainDirectory = new DirectoryInfo(_directory);
            DirectoryInfo[] childDirectory = mainDirectory.GetDirectories();
            string[] chilldArray = childDirectory.Select(x => x.FullName).ToArray();
            return chilldArray;
        }

        private void Start() 
        {
            _process = new Process();

            _process.StartInfo.FileName = "cmd.exe";
            _process.StartInfo.CreateNoWindow = false;
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.RedirectStandardError = true;
            _process.StartInfo.RedirectStandardInput = true;
            _process.StartInfo.RedirectStandardOutput = true;
            _process.StartInfo.WorkingDirectory = _directory;
            ///////////////////////////////////////////////////
            ///
            ///////////////////////////////////////////////
            _process.ErrorDataReceived += new DataReceivedEventHandler(async (sender, e) =>
            {
                if (!String.IsNullOrEmpty(e.Data))
                {
                    var command = new Error(e.Data);
                    await ChannelProvider.CommandChannel.Writer.WriteAsync(command);
                }
                else await SendOutputClient();
            });

            _process.OutputDataReceived += new DataReceivedEventHandler(async (sender, e) =>
            {
                if (!String.IsNullOrEmpty(e.Data))
                {
                    var command = new Executive(e.Data);
                    await ChannelProvider.CommandChannel.Writer.WriteAsync(command);
                }
                else await SendOutputClient();
            });
            //////////////////////////////////////
            ///
            ////////////////////////////////////
            _process.Start();
            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();

            SendOutputClient();
        }
    }
}