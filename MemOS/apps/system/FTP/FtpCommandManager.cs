﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cosmos.System;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.Listing;
using Cosmos.System.Network.IPv4;
using Cosmos.System.Network.IPv4.TCP;

namespace MemOS.FtpServer
{
    // <summary>
    //  FtpCommandManager class. Used to handle incoming FTP commands.
    // </summary>
    internal class FtpCommandManager
    {

        // Cosmos Virtual Filesystem.

        internal CosmosVFS FileSystem { get; set; }


        // Base path.

        internal string BaseDirectory { get; set; }


        // Current path.

        internal string CurrentDirectory { get; set; }


        // Create new instance of the <see cref="FtpCommandManager"/> class.

        // <param name="fs">Cosmos Virtual Filesystem.</param>
        // <param name="directory">Base directory used by the FTP server.</param>
        internal FtpCommandManager(CosmosVFS fs, string directory)
        {
            FileSystem = fs;
            CurrentDirectory = directory;
            BaseDirectory = directory;
        }


        // Process incoming FTP command.
        internal void ProcessRequest(FtpClient ftpClient, FtpCommand command)
        {
            if (command.Command == "USER")
            {
                ProcessUser(ftpClient, command);
            }
            else if (command.Command == "PASS")
            {
                ProcessPass(ftpClient, command);
            }
            else
            {
                if (ftpClient.IsConnected())
                {
                    switch (command.Command)
                    {
                        case "CWD":
                            ProcessCwd(ftpClient, command);
                            break;
                        case "SYST":
                            ftpClient.SendReply(215, "CosmosOS");
                            break;
                        case "CDUP":
                            ProcessCdup(ftpClient, command);
                            break;
                        case "QUIT":
                            ProcessQuit(ftpClient, command);
                            break;
                        case "DELE":
                            ProcessDele(ftpClient, command);
                            break;
                        case "PWD":
                            ProcessPwd(ftpClient, command);
                            break;
                        case "PASV":
                            ProcessPasv(ftpClient, command);
                            break;
                        case "PORT":
                            ProcessPort(ftpClient, command);
                            break;
                        case "HELP":
                            ftpClient.SendReply(200, "Help done.");
                            break;
                        case "NOOP":
                            ftpClient.SendReply(200, "Command OK.");
                            break;
                        case "RETR":
                            ProcessRetr(ftpClient, command);
                            break;
                        case "STOR":
                            ProcessStor(ftpClient, command);
                            break;
                        case "RMD":
                            ProcessRmd(ftpClient, command);
                            break;
                        case "MKD":
                            ProcessMkd(ftpClient, command);
                            break;
                        case "LIST":
                            ProcessList(ftpClient, command);
                            break;
                        case "TYPE":
                            ftpClient.SendReply(200, "Command OK.");
                            break;
                        default:
                            ftpClient.SendReply(500, "Unknown command.");
                            break;
                    }
                }
            }
        }


        // Process USER command.
        internal void ProcessUser(FtpClient ftpClient, FtpCommand command)
        {
            if (String.IsNullOrEmpty(command.Content))
            {
                ftpClient.SendReply(501, "Syntax error in parameters or arguments.");
                return;
            }
            if (command.Content == "anonymous")
            {
                ftpClient.Username = command.Content;
                ftpClient.Connected = true;
                ftpClient.SendReply(230, "User logged in, proceed.");
            }
            else if (String.IsNullOrEmpty(ftpClient.Username))
            {
                ftpClient.Username = command.Content;
                ftpClient.SendReply(331, "User name okay, need password.");
            }
            else
            {
                ftpClient.SendReply(550, "Requested action not taken.");
            }
        }


        // Process PASS command.
        internal void ProcessPass(FtpClient ftpClient, FtpCommand command)
        {
            if (String.IsNullOrEmpty(command.Content))
            {
                ftpClient.SendReply(501, "Syntax error in parameters or arguments.");
                return;
            }
            if (ftpClient.Username == "anonymous")
            {
                ftpClient.SendReply(530, "Login incorrect.");
            }
            else if (String.IsNullOrEmpty(ftpClient.Username))
            {
                ftpClient.SendReply(332, "Need account for login.");
            }
            else
            {
                ftpClient.Password = command.Content;
                ftpClient.Connected = true;
                ftpClient.SendReply(230, "User logged in, proceed.");
            }
        }


        // Process CWD command.
        internal void ProcessCwd(FtpClient ftpClient, FtpCommand command)
        {
            if (String.IsNullOrEmpty(command.Content))
            {
                ftpClient.SendReply(501, "Syntax error in parameters or arguments.");
                return;
            }
            try
            {
                if (command.Content.StartsWith("\\"))
                {
                    //Client asking for a path
                    if (command.Content == "\\")
                    {
                        CurrentDirectory = BaseDirectory;
                    }
                    else
                    {
                        CurrentDirectory = BaseDirectory + command.Content;
                    }
                }
                else
                {
                    //Client asking for a folder in current directory
                    if (CurrentDirectory == BaseDirectory)
                    {
                        CurrentDirectory += command.Content;
                    }
                    else
                    {
                        CurrentDirectory += "\\" + command.Content;
                    }
                }

                if (Directory.Exists(CurrentDirectory))
                {
                    ftpClient.SendReply(250, "Requested file action okay.");
                }
                else
                {
                    ftpClient.SendReply(550, "Requested action not taken.");
                }
            }
            catch
            {
                ftpClient.SendReply(550, "Requested action not taken.");
            }
        }


        // Process PWD command.
        internal void ProcessPwd(FtpClient ftpClient, FtpCommand command)
        {
            //Replace 0:/ for FTP client
            int i = CurrentDirectory.IndexOf(":") + 2;
            var tmp = CurrentDirectory.Substring(i);

            if (tmp.Length == 0)
            {
                ftpClient.SendReply(257, "/ created.");
            }
            else
            {
                ftpClient.SendReply(257, "\"/" + tmp + "\" created.");
            }
        }


        // Process PASV command.
        internal void ProcessPasv(FtpClient ftpClient, FtpCommand command)
        {
            /*
                TODO: - Fix new TCP SYN connection (https://stackoverflow.com/questions/67824462/why-does-my-ftp-client-open-multiple-control-connection)
                      - Find port dynamically.
            */

            ushort port = 20;
            var address = ftpClient.Control.StateMachine.LocalEndPoint.Address.ToByteArray();

            ftpClient.SendReply(200, $"Entering Passive Mode ({address[0]},{address[1]},{address[2]},{address[3]},{port / 256},{port % 256})");

            ftpClient.DataListener = new TcpListener(port);
            ftpClient.DataListener.Start();

            ftpClient.Mode = TransferMode.PASV;
        }


        // Process PORT command.
        internal void ProcessPort(FtpClient ftpClient, FtpCommand command)
        {
            var splitted = command.Content.Split(',');

            ftpClient.Data = new TcpClient(ftpClient.Port);

            ftpClient.Address = new Address((byte)int.Parse(splitted[0]), (byte)int.Parse(splitted[1]), (byte)int.Parse(splitted[2]), (byte)int.Parse(splitted[3]));
            ftpClient.Port = Int32.Parse(splitted[4]) * 256 + Int32.Parse(splitted[5]);

            ftpClient.SendReply(200, "Entering Active Mode.");

            ftpClient.Mode = TransferMode.ACTV;
        }


        // Process LIST command.
        internal void ProcessList(FtpClient ftpClient, FtpCommand command)
        {
            try
            {
                if (ftpClient.Mode == TransferMode.NONE)
                {
                    ftpClient.SendReply(425, "Can't open data connection.");
                }
                else if (ftpClient.Mode == TransferMode.ACTV)
                {
                    ftpClient.Data.Connect(ftpClient.Address, ftpClient.Port);

                    DoList(ftpClient, command);
                }
                else if (ftpClient.Mode == TransferMode.PASV)
                {
                    ftpClient.Data = ftpClient.DataListener.AcceptTcpClient();
                    ftpClient.DataListener.Stop();

                    DoList(ftpClient, command);
                }
            }
            catch
            {
                ftpClient.SendReply(425, "Can't open data connection.");
            }
        }


        // Make a file/directory listing and send it to FTP data connection.
        private void DoList(FtpClient ftpClient, FtpCommand command)
        {
            var directory_list = FileSystem.GetDirectoryListing(CurrentDirectory + "\\" + command.Content);

            var sb = new StringBuilder();
            foreach (var directoryEntry in directory_list)
            {
                if (directoryEntry.mEntryType == DirectoryEntryTypeEnum.Directory)
                {
                    sb.Append("d");
                }
                else
                {
                    sb.Append("-");
                }
                sb.Append("rwxrwxrwx 1 unknown unknown ");
                sb.Append(directoryEntry.mSize);
                sb.Append(" Jan 1 09:00 ");
                sb.AppendLine(directoryEntry.mName);
            }

            ftpClient.Data.Send(Encoding.ASCII.GetBytes(sb.ToString()));

            ftpClient.Data.Close();

            ftpClient.SendReply(226, "Transfer complete.");
        }


        // Process DELE command.
        internal void ProcessDele(FtpClient ftpClient, FtpCommand command)
        {
            if (String.IsNullOrEmpty(command.Content))
            {
                ftpClient.SendReply(501, "Syntax error in parameters or arguments.");
                return;
            }
            try
            {
                if (File.Exists(CurrentDirectory + "\\" + command.Content))
                {
                    File.Delete(CurrentDirectory + "\\" + command.Content);
                    ftpClient.SendReply(250, "Requested file action okay, completed.");
                }
                else
                {
                    ftpClient.SendReply(550, "Requested action not taken.");
                }
            }
            catch
            {
                ftpClient.SendReply(550, "Requested action not taken.");
            }
        }


        // Process RMD command.
        internal void ProcessRmd(FtpClient ftpClient, FtpCommand command)
        {
            if (String.IsNullOrEmpty(command.Content))
            {
                ftpClient.SendReply(501, "Syntax error in parameters or arguments.");
                return;
            }
            try
            {
                if (Directory.Exists(CurrentDirectory + "\\" + command.Content))
                {
                    Directory.Delete(CurrentDirectory + "\\" + command.Content, true);
                    ftpClient.SendReply(200, "Command okay.");
                }
                else
                {
                    ftpClient.SendReply(550, "Requested action not taken.");
                }
            }
            catch
            {
                ftpClient.SendReply(550, "Requested action not taken.");
            }
        }


        // Process MKD command.
        internal void ProcessMkd(FtpClient ftpClient, FtpCommand command)
        {
            if (String.IsNullOrEmpty(command.Content))
            {
                ftpClient.SendReply(501, "Syntax error in parameters or arguments.");
                return;
            }
            try
            {
                if (Directory.Exists(CurrentDirectory + "\\" + command.Content))
                {
                    ftpClient.SendReply(550, "Requested action not taken.");
                }
                else
                {
                    Directory.CreateDirectory(CurrentDirectory + "\\" + command.Content);
                    ftpClient.SendReply(200, "Command okay.");
                }
            }
            catch
            {
                ftpClient.SendReply(550, "Requested action not taken.");
            }
        }


        // Process CDUP command.
        internal void ProcessCdup(FtpClient ftpClient, FtpCommand command)
        {
            try
            {
                var root = FileSystem.GetDirectory(CurrentDirectory);

                if (CurrentDirectory.Length > 3)
                {
                    CurrentDirectory = root.mParent.mFullPath;
                    ftpClient.SendReply(250, "Requested file action okay.");
                }
                else
                {
                    ftpClient.SendReply(550, "Requested action not taken.");
                }
            }
            catch
            {
                ftpClient.SendReply(550, "Requested action not taken.");
            }
        }


        // Process STOR command.
        internal void ProcessStor(FtpClient ftpClient, FtpCommand command)
        {
            if (String.IsNullOrEmpty(command.Content))
            {
                ftpClient.SendReply(501, "Syntax error in parameters or arguments.");
                return;
            }
            try
            {
                if (ftpClient.Mode == TransferMode.NONE)
                {
                    ftpClient.SendReply(425, "Can't open data connection.");
                }
                else if (ftpClient.Mode == TransferMode.ACTV)
                {
                    ftpClient.Data.Connect(ftpClient.Address, ftpClient.Port);

                    DoStor(ftpClient, command);
                }
                else if (ftpClient.Mode == TransferMode.PASV)
                {
                    Global.mDebugger.Send("Test.");

                    ftpClient.Data = ftpClient.DataListener.AcceptTcpClient();
                    ftpClient.DataListener.Stop();

                    DoStor(ftpClient, command);
                }
            }
            catch
            {
                ftpClient.SendReply(425, "Can't open data connection.");
            }
        }


        // Receive file from FTP data connection and write it to filesystem.
        private void DoStor(FtpClient ftpClient, FtpCommand command)
        {
            var ep = new EndPoint(Address.Zero, 0);
            var data = ftpClient.Data.Receive(ref ep);

            try
            {
                File.WriteAllBytes(CurrentDirectory + "\\" + command.Content, data);
            }
            catch
            {
                ftpClient.SendReply(550, "Requested action not taken.");
            }

            ftpClient.Data.Close();

            ftpClient.SendReply(226, "Transfer complete.");
        }


        // Process RETR command.
        internal void ProcessRetr(FtpClient ftpClient, FtpCommand command)
        {
            if (String.IsNullOrEmpty(command.Content))
            {
                ftpClient.SendReply(501, "Syntax error in parameters or arguments.");
                return;
            }
            try
            {
                if (ftpClient.Mode == TransferMode.NONE)
                {
                    ftpClient.SendReply(425, "Can't open data connection.");
                }
                else if (ftpClient.Mode == TransferMode.ACTV)
                {
                    ftpClient.Data.Connect(ftpClient.Address, ftpClient.Port);

                    DoRetr(ftpClient, command);
                }
                else if (ftpClient.Mode == TransferMode.PASV)
                {
                    ftpClient.Data = ftpClient.DataListener.AcceptTcpClient();
                    ftpClient.DataListener.Stop();

                    DoRetr(ftpClient, command);
                }
            }
            catch
            {
                ftpClient.SendReply(425, "Can't open data connection.");
            }
        }


        // Read file from filesystem and send it to FTP data connection.
        private void DoRetr(FtpClient ftpClient, FtpCommand command)
        {
            try
            {
                var data = File.ReadAllBytes(CurrentDirectory + "\\" + command.Content);

                ftpClient.Data.Send(data);
            }
            catch
            {
                ftpClient.SendReply(550, "Requested action not taken.");
            }

            ftpClient.Data.Close();

            ftpClient.SendReply(226, "Transfer complete.");
        }


        // Process QUIT command.
        internal void ProcessQuit(FtpClient ftpClient, FtpCommand command)
        {
            ftpClient.SendReply(221, "Service closing control connection.");

            ftpClient.Control.Close();
        }
    }
}
