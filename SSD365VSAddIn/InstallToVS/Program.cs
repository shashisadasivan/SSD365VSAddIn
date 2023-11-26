﻿using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace InstallToVS
{
    public class FileUnblocker
    {
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteFile(string name);

        public bool Unblock(string fileName)
        {
            return DeleteFile(fileName + ":Zone.Identifier");
        }
    }
    class Program
    {
        private const string DllName1     = "SSD365VSAddIn.dll";
        private const string DllName2     = "SSD365VSAddIn.pdb";

        private const string AddinFolder = "AddinExtensions";

        static void Main(string[] args)
        {
            try
            {
                FileUnblocker unblocker = new FileUnblocker();
                string extensionFolderName = FindExtensionFolder();
                Console.WriteLine($"VS extension folder: {extensionFolderName}");
                string sourcePath = Path.Combine(Environment.CurrentDirectory, DllName1);
                unblocker.Unblock(sourcePath);
                string targetPath = Path.Combine(extensionFolderName, DllName1);
                File.Copy(sourcePath, targetPath, true);

                sourcePath = Path.Combine(Environment.CurrentDirectory, DllName2);
                unblocker.Unblock(sourcePath);
                targetPath = Path.Combine(extensionFolderName, DllName2);
                File.Copy(sourcePath, targetPath, true);

                Console.WriteLine("Setup finished");

            }
            catch (Exception ee)
            {
                Console.Error.WriteLine(ee);
                Console.Error.WriteLine("Seems that an issue prevented me from doing my job :(");
            }
        }
        private static string FindExtensionFolder()
        {
            String path;

            path = Environment.GetEnvironmentVariable("DynamicsVSTools");

            if (string.IsNullOrEmpty(path))
            {
                throw new ApplicationException("Could not find D365FO tools in Windows registry.");
            }

            return Path.Combine(path, AddinFolder);
        }
    }
}
