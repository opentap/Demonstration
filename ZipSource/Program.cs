using System;
using System.Collections;
using System.Collections.Generic;

using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;

namespace ZipSource
{
    class Program
    {
        static void Main(string[] args)
        {

            var extensions = new List<string>(){ ".cs", ".csproj", ".xml", ".TapPlan", ".txt" };
            var excludePaths = new List<string>() { "Help", "packages", "ZipSource", "obj", "bin", ".git" };
            List<string> sourceFiles = Directory.EnumerateFiles(args[0], "*.*", SearchOption.AllDirectories)
                .Where(f => extensions.Contains(Path.GetExtension(f), StringComparer.InvariantCultureIgnoreCase) && !excludePaths.Any(path => f.Contains(path))).ToList();
            
            //Adding some extra files 
            File.WriteAllText("Demos.sln", EditSolutionFile());
            sourceFiles.Add("Demos.sln");
            CreateZipFile(args[1], args[0], sourceFiles);
        }

        public static string EditSolutionFile()
        {
            if(File.Exists("Demos.sln"))
            {
                File.Delete("Demos.sln");
            }
            
            string str = File.ReadAllText("..\\..\\..\\Demos.sln");
            str = str.Replace("\tProjectSection(ProjectDependencies) = postProject\r\n" +
                "\t\t{FD33E5E1-C1A2-4113-8F0B-6D50924FDAFB} = {FD33E5E1-C1A2-4113-8F0B-6D50924FDAFB}\r\n" + 
                "\tEndProjectSection\r\n" +
                "EndProject\r\n" +
                "Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"ZipSource\", \"ZipSource\\ZipSource.csproj\", \"{FD33E5E1-C1A2-4113-8F0B-6D50924FDAFB}\"\r\n", "");
            str = str.Replace("{FD33E5E1-C1A2-4113-8F0B-6D50924FDAFB}.Debug|Any CPU.ActiveCfg = Debug|Any CPU", "");
            str = str.Replace("{FD33E5E1-C1A2-4113-8F0B-6D50924FDAFB}.Debug|Any CPU.Build.0 = Debug|Any CPU", "");
            str = str.Replace("{FD33E5E1-C1A2-4113-8F0B-6D50924FDAFB}.Release|Any CPU.ActiveCfg = Release|Any CPU", "");
            str = str.Replace("{FD33E5E1-C1A2-4113-8F0B-6D50924FDAFB}.Release|Any CPU.Build.0 = Release|Any CPU", "");
            str = str.Replace("\tGlobalSection(ExtensibilityGlobals) = postSolution\r\n\t\tSolutionGuid = {70A3904E-2398-43F2-86A6-DF42AB055FA0}\r\n" +
                "\tEndGlobalSection\r\nEndGlobal\r\n", "");
            return str;
        }

        public static void CreateZipFile(string fileName, string directoryPath, IEnumerable<string> files)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            var zip = ZipFile.Open(fileName, ZipArchiveMode.Update);
            foreach (var file in files)
            {
                // Add the entry for each file
                zip.CreateEntryFromFile(file, file.Replace(directoryPath, ""), CompressionLevel.Optimal);
            }
            Console.WriteLine("Zip created: " + fileName);
            // Dispose of the object when we are done
            zip.Dispose();
        }
    }
}
