using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace iniSync
{
    class FuncParser
    {
        static List<string> cacheFile = new List<string>();
        static int startIndex = -1;
        static int enbIndex = -1;
        static int lineIndex = -1;

        public static bool keyExists(string path, string section, string key, bool flush = true)
        {
            startIndex = -1;
            enbIndex = -1;
            lineIndex = -1;
            bool findSection = false;
            bool findKey = false;
            if (File.Exists(path))
            {
                cacheFile = new List<string>(File.ReadAllLines(path));
                for (int i = 0; i < cacheFile.Count; i++)
                {
                    if (!findSection && cacheFile[i] == "[" + section + "]")
                    {
                        findSection = true;
                        startIndex = i;
                        enbIndex = i;
                    }
                    else if (findSection && cacheFile[i].StartsWith("[") && cacheFile[i].EndsWith("]"))
                    {
                        break;
                    }
                    else if (findSection && cacheFile[i].Length > 0)
                    {
                        if (cacheFile[i].StartsWith(key + "="))
                        {
                            findKey = true;
                            lineIndex = i;
                            break;
                        }
                        else
                        {
                            enbIndex = i;
                        }
                    }
                }
            }
            if (flush)
            {
                cacheFile.Clear();
            }
            return findKey;
        }

        public static string stringRead(string path, string section, string key, bool flush = true)
        {
            string outString = null;
            if (keyExists(path, section, key, false))
            {
                outString = cacheFile[lineIndex].Remove(0, cacheFile[lineIndex].IndexOf('=') + 1);
                if (outString.Length == 0)
                {
                    outString = null;
                }
            }
            if (flush)
            {
                cacheFile.Clear();
            }
            return outString;
        }

        public static void iniWrite(string path, string section, string key, string value)
        {
            bool readyToWrite = false;
            string line = stringRead(path, section, key, false);
            if (lineIndex != -1)
            {
                if (line != value)
                {
                    cacheFile[lineIndex] = key + "=" + value;
                    readyToWrite = true;
                }
            }
            else
            {
                if (startIndex != -1 && enbIndex != -1)
                {
                    cacheFile[enbIndex] += Environment.NewLine + key + "=" + value;
                    readyToWrite = true;
                }
                else
                {
                    File.AppendAllText(path, Environment.NewLine + "[" + section + "]" + Environment.NewLine + key + "=" + value + Environment.NewLine);
                }
            }
            if (readyToWrite)
            {
                try
                {
                    File.WriteAllLines(path, cacheFile, new UTF8Encoding(false));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось записать файл: " + path + Environment.NewLine + ex.Message);
                }
            }
            cacheFile.Clear();
        }
    }
}
