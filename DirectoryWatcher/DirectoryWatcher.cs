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
        using (FileSystemWatcher watcher = new FileSystemWatcher())
        {
            watcher.Path = PATH;

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
            
            Console.WriteLine("Press 'q' to quit watcher.");
            while (Console.Read() != 'q') ;
        }
    }

    // Define the event handlers.
    private static void OnChanged(object source, FileSystemEventArgs e)
    {
        Console.WriteLine($"{DateTime.Now}: {e.ChangeType}: {e.FullPath.Replace(PATH, string.Empty)}");
    }

    private static void OnRenamed(object source, RenamedEventArgs e)
    {
        Console.WriteLine($"{DateTime.Now}: {e.OldFullPath.Replace(PATH, string.Empty)} renamed to {e.FullPath.Replace(PATH, string.Empty)}");
    }
}