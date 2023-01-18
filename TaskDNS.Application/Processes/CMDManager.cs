using TaskDNS.Application.Processes.Channels;
using System.Runtime.InteropServices;
using TaskDNS.Application.Model;
using System.Diagnostics;

namespace TaskDNS.Application.Processes
{
    /// <summary>
    /// Класс открывающий процесс cmd.exe.
    /// </summary>
    public class CMDManager : IDisposable
    {
        private delegate bool ConsoleCtrlDelegate(uint type);
        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate handler, bool add);
        [DllImport("kernel32.dll")]
        internal static extern bool GenerateConsoleCtrlEvent(uint dwCtrlEvent, int dwProcessGroupId);

        private readonly string _connectionId;

        private string _directory = @"C:\";

        private Process _process;

        /// <summary>
        /// .ctor
        /// </summary>
        public CMDManager(string connectionId)
        {
            _connectionId = connectionId;
            Start();
        }

        /// <summary>
        /// Запись в командную строку.
        /// </summary>
        public void Write(string command)
        {
            _directory = ProcessPathHandler.GetDirectory(_directory, command);
            _process.StandardInput.WriteLine(command);
        }

        /// <summary>
        /// Отправка клиенту данные о текущую директорию.
        /// </summary>
        public string GetDirectory() => _directory;

        /// <summary>
        ///  Отправляем клиенту список подкатологов текущей директории.
        /// </summary>
        public string[] GetDirectories() => Directories();

        /// <summary>
        /// Остановка выполняемой команды.
        /// </summary>
        public async Task StopAsync()
        {
            SetConsoleCtrlHandler(null, true);
            GenerateConsoleCtrlEvent(0, _process.Id);
            Thread.Sleep(10);
            SetConsoleCtrlHandler(null, false);

            await WriteInChannelAsync(CommandExecutionResult.Success(string.Empty, _connectionId));
        }

        private string[] Directories()
        {
            var mainDirectory = new DirectoryInfo(_directory);
            var directories = mainDirectory.GetDirectories();
            var directoriesArray = directories.Select(x => x.FullName).ToArray();
            return directoriesArray;
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

            _process.OutputDataReceived += new DataReceivedEventHandler(async (sender, e) =>
            {
                await WriteInChannelAsync(CommandExecutionResult.Executive(e.Data, _connectionId));
            });

            _process.ErrorDataReceived += new DataReceivedEventHandler(async (sender, e) =>
            {
                await WriteInChannelAsync(CommandExecutionResult.Error(e.Data, _connectionId));
            });

            _process.Start();
            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();
        }

        private async Task WriteInChannelAsync(CommandExecutionResult commandExecutionResult)
        {
            if (string.IsNullOrEmpty(commandExecutionResult.Output))
            {
                await CommandChannelProvider.CommandResultChannel.Writer.WriteAsync(CommandExecutionResult.Success(string.Empty, _connectionId));
                return;
            }

            await CommandChannelProvider.CommandResultChannel.Writer.WriteAsync(commandExecutionResult);
        }

        /// <summary>
        /// Удаление консоли пользователя.
        /// </summary>
        public void Dispose() => _process.Dispose();
    }
}