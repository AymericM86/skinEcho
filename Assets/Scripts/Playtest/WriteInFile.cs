using UnityEngine;
using System.IO;

public class WriteInFile {

    private static int s_filesInFolder = -1;

    public static void Write(string directory, string text)
    {
        string path = Application.dataPath + "/../" + directory;

        if (s_filesInFolder == -1)
        {
            var info = new DirectoryInfo(path);
            s_filesInFolder = info.GetFiles().Length;
        }
        path += "Playtester_" + s_filesInFolder + ".txt";

        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(text);
        writer.Close();
    }
	
}
