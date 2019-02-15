using System;
using System.IO;
using System.Security.Permissions;

public class DirectoryWatcher
{
    static string PATH = string.Empty;

    public static void Main()
    {
        string[] args = Environment.GetCommandLineArgs();

        // If a directory is not specified, exit program.
        if (args.Length != 2)
        {
            // Display the proper way to call the program.
            Console.WriteLine("Please pass diretory path");
            return;
        }
        PATH = args[1];
        Run();
    }

    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    private static void Run()
    {
        // Create a new FileSystemWatcher and set its properties.
        foreach (string path in PATH.Split(','))
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            {
                Console.WriteLine("Watching: " + path);
                watcher.Path = path.Trim();

                // Watch for changes in LastAccess and LastWrite times, and
                // the renaming of files or directories.
                //watcher.NotifyFilter = NotifyFilters.LastAccess
                //                     | NotifyFilters.LastWrite
                //                     | NotifyFilters.FileName
                //                     | NotifyFilters.DirectoryName;

                watcher.Filter = "*.*";
                watcher.IncludeSubdirectories = true;

                // Add event handlers.
                watcher.Changed += OnChanged;
                watcher.Created += OnChanged;
                watcher.Deleted += OnChanged;
                watcher.Renamed += OnRenamed;

                watcher.EnableRaisingEvents = true;

            }
        }
        Console.WriteLine("Press 'q' to quit watcher.");
        while (Console.Read() != 'q') ;
    }

    // Define the event handlers.
    private static void OnChanged(object source, FileSystemEventArgs e)
    {
        Console.WriteLine($"{DateTime.Now}: {e.ChangeType}: {TrimDirectory(e.FullPath)}");
    }

    private static void OnRenamed(object source, RenamedEventArgs e)
    {
        Console.WriteLine($"{DateTime.Now}: {TrimDirectory(e.OldFullPath)} renamed to {TrimDirectory(e.FullPath)}");
    }

    static string TrimDirectory(string fullPath)
    {
        foreach (string mainPath in PATH.Split(','))
            if (fullPath.StartsWith(mainPath.Trim()))
                return fullPath.Replace(mainPath.Trim(), string.Empty);
        return fullPath;
    }
}