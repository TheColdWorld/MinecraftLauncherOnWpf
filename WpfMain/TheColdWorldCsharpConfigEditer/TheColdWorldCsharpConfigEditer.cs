// Author:TheColdWorld
// License: GNU GENERAL PUBLIC LICENSE V3
namespace TheColdWorldCsharpConfigEditer
{
    public static class TheColdWorldCsharpConfigEditer
    {
        /// <summary>
        /// 寻找某个头的所有键
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        /// <param name="findHeadle">头</param>
        /// <param name="result">键</param>
        /// <returns>是否取到结果<br/>是为true,否则为false</returns>
        public static bool FindAllKey(in string FilePath, in string findHeadle, out string[] result)
        {
            string[] FileLines = System.IO.File.ReadAllLines(FilePath, System.Text.Encoding.UTF8);
            string[] temp1;
            string[] temp2 = new string[0];
            bool Fonded = false;
            for (int i = 0; i < FileLines.Length && !Fonded; i++)
            {
                if (FileLines[i] == findHeadle + ":" && FileLines[i + 1] == "{")
                {
                    string str = string.Empty;
                    for (int ii = 0; true; ii++)
                    {
                        str = FileLines[i + 2 + ii];
                        if (str == "}") break;
                        else
                        {
                            temp1 = new string[ii + 1];
                            for (int iii = 0; iii < temp2.Length; iii++)
                            {
                                temp1[iii] = temp2[iii];
                            }
                            temp1[temp1.Length - 1] = str;
                            temp2 = temp1;
                        }
                    }
                    Fonded = true;
                }
            }
            if (Fonded) result = temp2;
            else result = System.Array.Empty<string>();
            return Fonded;
        }
        /// <summary>
        /// 寻找某个头的某个键的值<br/>依赖于TheColdWorldCsharpConfigEditer.TheColdWorldCsharpConfigEditer.FindAllKey(in string FilePath, in string findHeadle, out string[] result)
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        /// <param name="findHeadle">头</param>
        /// <param name="findKey">键</param>
        /// <param name="value">值</param>
        /// <returns>是否取到结果<br/>是为true,否则为false</returns>
        public static bool FindValue(in string FilePath, in string findHeadle, in string findKey, out string value)
        {
            string[] temp1;
            bool Returns = false;
            if (!FindAllKey(FilePath, findHeadle, out temp1))
            {
                value = null;
                return false;
            }
            else
            {
                foreach (string Key__Value in temp1)
                {
                    if (Key__Value[0] == '/' && Key__Value[1] == '/') continue;
                    string[] temp2 = Key__Value.Split(new string[] { "=" }, System.StringSplitOptions.None);
                    if (temp2[0] == findKey)
                    {
                        if (temp2.Length > 2)
                        {
                            string temp3 = string.Empty;
                            for (int i = 1; i < temp2.Length; i++)
                            {
                                if (i == temp2.Length - 1)
                                {
                                    temp3 += temp2[i] + ":";
                                }
                                else
                                {
                                    temp3 += temp2[i];
                                }
                            }
                            value = temp3;
                        }
                        else
                        {
                            value = temp2[1];
                        }
                        Returns = true;
                        return Returns;
                    }
                }
                if (!Returns)
                {
                    value = null;
                    return false;
                }
            }
            value = null;
            return false;
        }
        /// <summary>
        /// 寻找某个头的某个键的值<br/>依赖于TheColdWorldCsharpConfigEditer.TheColdWorldCsharpConfigEditer.FindAllKey(in string FilePath, in string findHeadle, out string[] result)<br/>
        /// 和TheColdWorldCsharpConfigEditer.TheColdWorldCsharpConfigEditer.FindValue(in string FilePath, in string findHeadle, in string findKey, out string value)
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        /// <param name="findHeadle">头</param>
        /// <param name="findKey">键</param>
        /// <param name="value">值</param>
        /// <returns>是否取到结果<br/>是为true,否则为false</returns>
        public static bool FindValue(in string FilePath, in string findHeadle, in string findKey, out int value)
        {
            string str;
            if (!FindValue(FilePath, findHeadle, findKey, out str))
            {
                value = int.MinValue;
                return false;
            }
            else
            {
                return int.TryParse(str, out value);
            }
        }
        /// <summary>
        /// 寻找某个头的某个键的值<br/>依赖于TheColdWorldCsharpConfigEditer.TheColdWorldCsharpConfigEditer.FindAllKey(in string FilePath, in string findHeadle, out string[] result)<br/>
        /// 和TheColdWorldCsharpConfigEditer.TheColdWorldCsharpConfigEditer.FindValue(in string FilePath, in string findHeadle, in string findKey, out string value)
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        /// <param name="findHeadle">头</param>
        /// <param name="findKey">键</param>
        /// <param name="value">值</param>
        /// <returns>是否取到结果<br/>是为true,否则为false</returns>
        public static bool FindValue(in string FilePath, in string findHeadle, in string findKey, out double value)
        {
            string str;
            if (!FindValue(FilePath, findHeadle, findKey, out str))
            {
                value = double.MinValue;
                return false;
            }
            else
            {
                return double.TryParse(str, out value) ;
            }
        }
        /// <summary>
        /// 寻找某个头的某个键的值<br/>依赖于TheColdWorldCsharpConfigEditer.TheColdWorldCsharpConfigEditer.FindAllKey(in string FilePath, in string findHeadle, out string[] result)<br/>
        /// 和TheColdWorldCsharpConfigEditer.TheColdWorldCsharpConfigEditer.FindValue(in string FilePath, in string findHeadle, in string findKey, out string value)
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        /// <param name="findHeadle">头</param>
        /// <param name="findKey">键</param>
        /// <param name="value">值</param>
        /// <returns>是否取到结果<br/>是为true,否则为false</returns>
        public static bool FindValue(in string FilePath, in string findHeadle, in string findKey, out bool value)
        {
            string str;
            if (!FindValue(FilePath, findHeadle, findKey, out str))
            {
                value = false;
                return false;
            }
            else
            {
                return bool.TryParse(str.ToLower(), out value);
            }
        }
        /// <summary>
        ///  寻找对应头的对应键的对应值
        /// </summary>
        /// <param name="FilePath">要写入的文件路径</param>
        /// <param name="Headle">写入的头</param>
        /// <param name="Key">写入的键</param>
        /// <param name="value">写入的值</param>
        /// <returns>写入是否成功<br/>成功为true,否则为false</returns>
        public static bool WriteValue(in string FilePath, in string Headle, in string Key, in string value)
        {
            if (!System.IO.File.Exists(FilePath))
            {
                using (System.IO.FileStream fs = new System.IO.FileStream(FilePath, System.IO.FileMode.CreateNew, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite))
                {
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(Headle + ":\r\n{\r\n");
                    fs.Write(buffer,0,buffer.Length);
                    fs.Flush();
                    fs.Position = fs.Length;
                    buffer = System.Text.Encoding.UTF8.GetBytes(Key + "=" + value + "\r\n}");
                    fs.Write(buffer, 0, buffer.Length);
                    fs.Flush();
                    return true;
                }
            }
            bool Result;
            string[] FileLines = System.IO.File.ReadAllLines(FilePath, System.Text.Encoding.UTF8);
            string[] WriteFileLines;
            using (System.IO.FileStream fs = new System.IO.FileStream(FilePath, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite))
            {
                int EndLine = 0;
                int Writeline = 0;
                int Flag1=0;
                for (int Line = 0; Line < FileLines.Length&&Flag1 == 0; Line++)
                {
                    if (FileLines[Line][0] == '/' && FileLines[Line][1] == '/') continue;
                    if (FileLines[Line] == Headle + ":")
                    {
                        for (; Line < FileLines.Length&&Flag1 == 0; Line++)
                        {
                            if (FileLines[Line][0] == '/' && FileLines[Line][1] == '/') continue;
                            if (FileLines[Line].Split(new string[] { "=" }, System.StringSplitOptions.None)[0] == Key)
                            {
                                Writeline = Line;
                                Flag1 = 1;
                            }
                            if (FileLines[Line] == "}")
                            {
                                EndLine = Line;
                                Flag1 = 2;
                            }
                        }
                    }
                }
                if (Flag1 == 0) Flag1 = 3;
                switch (Flag1)
                {
                    case 1:
                        {
                            WriteFileLines = new string[FileLines.Length];
                            for (int i = 0; i < FileLines.Length; i++)
                            {
                                if (i < Writeline)
                                {
                                    WriteFileLines[i] = FileLines[i];
                                }
                                if (i == Writeline)
                                {
                                    WriteFileLines[i] = Key+"="+value;
                                }
                                else
                                {
                                    WriteFileLines[i] = FileLines[i];
                                }
                            }
                        }
                        break;
                    case 2:
                        {
                            WriteFileLines = new string[FileLines.Length + 1];
                            for(int i = 0; i < FileLines.Length + 1 ; i++)
                            {
                                if (i < EndLine)
                                {
                                    WriteFileLines[i] = FileLines[i];
                                }
                                else if (i == EndLine)
                                {
                                    WriteFileLines[i] = Key + "=" + value;
                                }
                                else
                                {
                                    WriteFileLines[i] = FileLines[i-1];
                                }
                            }
                        }
                        break;
                    case 3:
                        {
                            WriteFileLines = new string[FileLines.Length + 4];
                            for (int i = 0; i < FileLines.Length + 4; i++)
                            {
                                if (i < FileLines.Length)
                                {
                                    WriteFileLines[i] = FileLines[i];
                                }
                                else
                                {
                                    WriteFileLines[i] = Headle + ":";
                                    WriteFileLines[i + 1] = "{";
                                    WriteFileLines[i + 2] = Key + "=" + value;
                                    WriteFileLines[i + 3] = "}";
                                    i += 4;
                                }
                            }
                        }
                        break;
                    default:
                        WriteFileLines = FileLines;
                        break;
                }
                if (WriteFileLines == FileLines) return false;
                else
                {
                    string WriteFile = string.Empty;
                   for (int i = 0; i < WriteFileLines.Length; i++)
                    {
                        if (i < WriteFileLines.Length - 1)
                        {
                            WriteFile += WriteFileLines[i] + "\r\n";
                        }
                        if (i == WriteFileLines.Length - 1)
                        {
                            WriteFile += WriteFileLines[i];
                        }
                    }
                    byte[] byt = System.Text.Encoding.UTF8.GetBytes(WriteFile, 0, WriteFile.Length);
                    fs.Position = 0;
                    fs.Write(byt, 0, byt.Length);
                    return true;
                }
            }
            
        }
    }
}
