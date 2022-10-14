using System;
using System.Diagnostics;
using System.IO;
using TaskDNS.Channels;
using TaskDNS.Channels.Interface;
using TaskDNS.Channels.Models;
using TaskDNS.Tools;

namespace TaskDNS.App
{
    public class CMD
    {
        private static int currentChildDirectory;
        private static int IdProcess;
        private static bool isActiveProcess;
        private static string path;
        private static string nextPath;

        private Process process;
        private ProcessingCommand getPath = new ProcessingCommand();


        public CMD()
        {
            path = @"C:\";
            nextPath = "";
        }

        public async Task StartAsync()
        {
            if (!isActiveProcess)
            {
                isActiveProcess = true;

                process.Start();

                IdProcess = process.Id;

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.WaitForExit();

                path = nextPath;

                await SendOutputClient();

                isActiveProcess = false;
            }
        }

        public async void Write(string command)
        {
            nextPath = getPath.Processing(path, command);
            ArgumentProcess(command);
        }

        public void ArgumentProcess(string command)
        {
            process = new Process();

            process.StartInfo.FileName = "cmd.exe";

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            process.StartInfo.WorkingDirectory = path;

            process.StartInfo.Arguments = "/C " + command;
            ///////////////////////////////////////////////

            /////////////////////////////////////////////////
            process.ErrorDataReceived += new DataReceivedEventHandler(async (sender, e) =>
            {
                if (!String.IsNullOrEmpty(e.Data))
                {
                    var command = new Error(e.Data);
                    await ChannelProvider.CommandChannel.Writer.WriteAsync(command);
                }
            });

            process.OutputDataReceived += new DataReceivedEventHandler(async (sender, e) =>
            {
                if (!String.IsNullOrEmpty(e.Data))
                {
                    var command = new Executive(e.Data);
                    await ChannelProvider.CommandChannel.Writer.WriteAsync(command);
                }
            });
        }

        public string GetPath()
        {
            return path ;
        }
        public string GetChildPath()
        {
            string localString = childDirecty();
            return localString;
        }

        public void CloseProcess()
        {
            if (isActiveProcess)
            {
                if(IdProcess != 0)
                {
                    Process.GetProcessById(IdProcess).Kill(true);
                    isActiveProcess = false;
                }
            }
        }
        private async Task SendOutputClient()
        {
            var complete = new Complete();
            await ChannelProvider.CommandChannel.Writer.WriteAsync(complete);
        }

        private string childDirecty()
        {
            DirectoryInfo mainDirectory = new DirectoryInfo(path);
            DirectoryInfo[] childDirectory = mainDirectory.GetDirectories();

            if (currentChildDirectory == (childDirectory.Length-1))
                currentChildDirectory = 0;
            else
                currentChildDirectory++;

            return  childDirectory[currentChildDirectory].FullName;
        }
    }
}