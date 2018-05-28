using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WwiseEnumGenerator : MonoBehaviour {

    [SerializeField]
    string m_pathToWwise_IDs = "";

    string m_currentEnum = "";

    StreamWriter m_sw;
    StreamReader m_sr;

    // Use this for initialization
    void Start ()
    {
        Convert();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Convert()
    {
        ConstifyIdsFile();

        string path = Application.dataPath + "/" + m_pathToWwise_IDs;
        m_sr = new StreamReader(path + "Wwise_IDs.cs", true);
        if (File.Exists(path + "Wwise_IDs_Enum.cs"))
            File.Delete(path + "Wwise_IDs_Enum.cs");

        m_sw = new StreamWriter(path + "Wwise_IDs_Enum.cs", true);

        m_sw.WriteLine("using System.Collections;");
        m_sw.WriteLine("using System.Collections.Generic;");
        m_sw.WriteLine("using UnityEngine;");
        
        while (!m_sr.EndOfStream)
        {
            string line = m_sr.ReadLine();
            if(line.Contains("public class"))
            {
                SetCurrentEnum(line.Remove(0, 17));
            }
            else if(line.Contains("public const uint"))
            {
                AddToEnum(line.Remove(0, 26));
            }
        }
        m_sw.WriteLine("}");

        m_sw.Close();
        m_sr.Close();
    }

    // Transform static var to const and remove //
    private void ConstifyIdsFile()
    {
        string path = Application.dataPath + "/" + m_pathToWwise_IDs;
        StreamReader sr = new StreamReader(path + "Wwise_IDs.cs", true);

        if (File.Exists(path + "Wwise_IDs_Tmp.cs"))
            File.Delete(path + "Wwise_IDs_Tmp.cs");

        StreamWriter sw = new StreamWriter(path + "Wwise_IDs_Tmp.cs");
        
        while (!sr.EndOfStream)
        {
            string line = sr.ReadLine();
            if (line.Contains("static"))
            {
                line = line.Replace("static", "const");
            }
            if (line.Contains("//"))
            {
                for(uint i = 0; i < line.Length;++i)
                {
                    if (line.ToCharArray()[i] != '/' || (line.ToCharArray()[i] == '/' && i < line.Length - 1 && line.ToCharArray()[i + 1] != '/'))
                    {
                        sw.Write(line.ToCharArray()[i]);
                    }
                    else
                        break;
                }

                sw.WriteLine("");
                continue;
            }

            sw.WriteLine(line);
        }

        sr.Close();
        sw.Close();
        File.Delete(path + "Wwise_IDs.cs");
        File.Move(path + "Wwise_IDs_Tmp.cs", path + "Wwise_IDs.cs");
    }


    // Add id to current enum
    private void AddToEnum(string strToParse)
    {
        string[] splited = strToParse.Split(' ');
        m_sw.WriteLine("\t"+splited[0] + " = unchecked((int)AK." + m_currentEnum + "." + splited[0] + "),");
    }

    private void SetCurrentEnum(string enumName)
    {
        if(m_currentEnum != "")
        {
            m_sw.WriteLine("}");
        }
        m_sw.WriteLine("");

        m_sw.WriteLine("public enum Wwise_ID_Enum_" + enumName);
        m_sw.WriteLine("{");
        m_currentEnum = enumName;
    }
}
