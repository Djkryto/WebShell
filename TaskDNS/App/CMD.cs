using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        private ProcessingCommand processing = new ProcessingCommand();

        private StreamWriter streamWriter;
        public CMD()
        {
            StartProcess();

            streamWriter = process.StandardInput;

            path = @"C:\";
            nextPath = "";
        }

        public async void Write(string command)
        {
            nextPath = processing.Processing(path, command);
            streamWriter.WriteLine(command);
        }

        public string GetPath()
        {
            return path;
        }

        public string[] GetChildPath()
        {
            return childDirecty();
        }

        public async Task CloseProcess()
        {
            //
        }

        private async Task SendOutputClient()
        {
            var complete = new Complete();
            await ChannelProvider.CommandChannel.Writer.WriteAsync(complete);
        }

        private string[] childDirecty()
        {
            DirectoryInfo mainDirectory = new DirectoryInfo(path);
            DirectoryInfo[] childDirectory = mainDirectory.GetDirectories();
            string[] chilldArray = childDirectory.Select(x => x.FullName).ToArray();

            return chilldArray;
        }

        private void StartProcess() 
        {
            process = new Process();

            process.StartInfo.FileName = "cmd.exe";

            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.UseShellExecute = false;

            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;

            process.StartInfo.WorkingDirectory = path;
            ///////////////////////////////////////////////////
            ///
            ///////////////////////////////////////////////
            process.ErrorDataReceived += new DataReceivedEventHandler(async (sender, e) =>
            {
                if (!String.IsNullOrEmpty(e.Data))
                {
                    var command = new Error(e.Data);
                    await ChannelProvider.CommandChannel.Writer.WriteAsync(command);
                }
                else await SendOutputClient();
            });

            process.OutputDataReceived += new DataReceivedEventHandler(async (sender, e) =>
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
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            SendOutputClient();
        }
    }
}