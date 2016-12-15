// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
////css_args -d, "e:\dev\NppGit\", -c, Release, -s, RC
//css_reference System.IO.Compression.FileSystem.dll

using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Compression;

class Script
{
    private const string DEPLOY_DIR = "Deploy";
    private const string DEPLOY_DLL_DIR = "NppKate";
    private const string DEPLOY_ARCHIVE = "NppKate_{0}{1}.zip";
    private static readonly IList<string> _exclude = new ReadOnlyCollection<string>(
            new List<string> {
                ".pdb", ".lib", ".exp", ".config", ".xml", "linux", "osx", ".iobj", ".ipdb"
            });
    private static readonly IList<string> _rootFiles = new ReadOnlyCollection<string>(
            new List<string> {
                "NppKate.dll"
            });
    
    private static string _rootDir = "";
    private static string _config = "Release";
    private static string _suffix = "Release";
    private static bool _fromEnv = true;
    private static string _version = "";
    private static string _branch = "";
    
    [STAThread]
    static public void Main(string[] args)
    {
        // Считываем параметры
        System.Console.WriteLine("Start KatePack with args: ");
        for (int i = 0; i < args.Length; i++)
        {
            if ("-d".Equals(args[i]))
            {
                _rootDir = args[++i];
                System.Console.WriteLine("Root directory: {0}", _rootDir);
            }
            else if ("-c".Equals(args[i]))
            {
                _config = args[++i];
                System.Console.WriteLine("Config: {0}", _config);
            }
            else if ("-s".Equals(args[i]))
            {
                _suffix = args[++i];
                System.Console.WriteLine("Suffix: {0}", _suffix);
            }
            else if ("-e".Equals(args[i]))
            {
                _fromEnv = false;
            }
        }
        if (_fromEnv)
        {
            System.Console.WriteLine("Load params from environment variables");
            _rootDir = System.Environment.GetEnvironmentVariable("APPVEYOR_BUILD_FOLDER");
            System.Console.WriteLine("Root directory: {0}", _rootDir);
            _config = System.Environment.GetEnvironmentVariable("CONFIGURATION");
            System.Console.WriteLine("Config: {0}", _config);
            _version = System.Environment.GetEnvironmentVariable("APPVEYOR_BUILD_VERSION");
            System.Console.WriteLine("Build version: {0}", _version);

            _branch = System.Environment.GetEnvironmentVariable("APPVEYOR_REPO_BRANCH");

            _suffix = (_branch == "master" ? "" : _branch == "develop" ? "b" : "a") + "_" 
                + System.Environment.GetEnvironmentVariable("PLATFORM");
            System.Console.WriteLine("Suffix: {0}", _suffix);
            
        }
        var deployName = string.Format(DEPLOY_ARCHIVE, _version, _suffix);
        System.Console.WriteLine("ZIP_NAME: {0}", deployName);
        System.Diagnostics.Process.Start("appveyor", "SetVariable -Name \"ZIP_NAME\" -Value \"" + deployName + "\"");
        
        if (string.IsNullOrEmpty(_rootDir)) {
            _rootDir = System.IO.Directory.GetCurrentDirectory();
        }
        // Задаем рабочие папки
        var configDir = new DirectoryInfo(Path.Combine(_rootDir, "bin", _config));
        
        System.Console.WriteLine("Search work directories...");
        var pathDir = Path.Combine(_rootDir, "bin", DEPLOY_DIR);
        if (!Directory.Exists(pathDir))
        {
            System.Console.WriteLine("Create directory {0}", pathDir);
            Directory.CreateDirectory(pathDir);
        }
        var deployDir = new DirectoryInfo(pathDir);
        pathDir = Path.Combine(_rootDir, "bin", DEPLOY_DIR, DEPLOY_DLL_DIR);
        if (!Directory.Exists(pathDir))
        {
            System.Console.WriteLine("Create directory {0}", pathDir);
            Directory.CreateDirectory(pathDir);
        }
        var deployDll = new DirectoryInfo(pathDir);
        
        ClearDir(deployDir);
        System.Console.WriteLine("Copying files...");
        CopyFiles(configDir, deployDir, deployDll);
        Archive(deployDir, deployName);
    }
    
    private static void ClearDir(DirectoryInfo dirInfo) 
    {
        System.Console.WriteLine("Clear directory: {0}", dirInfo.FullName);
        foreach(var f in dirInfo.EnumerateFiles()) 
        {
            try 
            {
                System.Console.WriteLine("Delete file {0}", f.Name);
                f.Delete();
            } 
            catch (Exception e) 
            {
                System.Console.WriteLine("{0}\r\n{1}\r\n{2}\r\n", e.Message, e.InnerException, e.StackTrace);
            }
        }
        foreach (var d in dirInfo.EnumerateDirectories())
        {
            ClearDir(d);
        }
    }
    
    private static void CopyFiles(DirectoryInfo source, DirectoryInfo deployDir, DirectoryInfo deployDll)
    {
        foreach(var f in source.GetFiles()) 
        {
            if (!_exclude.Contains(f.Extension)) 
            {
                var destination = "";
                if (_rootFiles.Contains(f.Name)) 
                    destination = Path.Combine(deployDir.FullName, f.Name);
                else 
                    destination = Path.Combine(deployDll.FullName, f.Name);
                System.Console.WriteLine("Copying files: {0} -> {1}", f.FullName, destination);
                f.CopyTo(destination);
            }
        }

        foreach(var d in source.GetDirectories())
        {
            CopyDirectories(d, deployDll);
        }
    }
    
    private static void CopyDirectories(DirectoryInfo source, DirectoryInfo destination) 
    {
        if (_exclude.Contains(source.Name))
            return;

        var dest = Path.Combine(destination.FullName, source.Name);
        System.Console.WriteLine("Copying files: {0} -> {1}", source.FullName, dest);
        if (!Directory.Exists(dest))
        {
            Directory.CreateDirectory(dest);
        }
        foreach (var f in source.GetFiles())
        {
            if (!_exclude.Contains(f.Extension))
            {
                var destF = Path.Combine(dest, f.Name);
                System.Console.WriteLine("Copying files: {0} -> {1}", f.FullName, destF);
                f.CopyTo(destF);
            }
        }
        foreach (var d in source.GetDirectories())
        {
            CopyDirectories(d, new DirectoryInfo(dest));
        }
    }
    
    private static void Archive(DirectoryInfo deployDir, string deployName) 
    {
        System.Console.WriteLine("Compressing directory: {0}", deployDir.FullName);
        var archName = Path.Combine(_rootDir, "bin", deployName);
        if (File.Exists(archName)) {
            System.Console.WriteLine("File exists: {0}", archName);
            new FileInfo(archName).Delete();
        }
        ZipFile.CreateFromDirectory(deployDir.FullName, archName, CompressionLevel.Optimal, false);
        System.Console.WriteLine("Archive created: {0}", archName);
    }
}   
