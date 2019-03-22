﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using System.Xml;
using Abp.Reflection.Extensions;
using ICSharpCode.SharpZipLib.Checksum;

namespace CjzDataBase
{
    public class CommUtils
    {
        public static double GetAttributeDouble(XmlElement node, string attrName)
        {
            double ret = 0;
            double.TryParse(node.GetAttribute(attrName), out ret);
            return ret;
        }

        public static bool GetAttributeBoolean(XmlElement node, string attrName)
        {
            bool ret = false;
            bool.TryParse(node.GetAttribute(attrName), out ret);
            return ret;
        }

        public static int GetAttributeInteger(XmlElement node, string attrName)
        {
            int ret = 0;
            int.TryParse(node.GetAttribute(attrName), out ret);
            return ret;
        }

        public static DateTimeFormatInfo GetDateTimeFormatInfo936()
        {
            DateTimeFormatInfo dateTimeFormat = new CultureInfo("zh-CN").DateTimeFormat;
            dateTimeFormat.DateSeparator = "-";
            return dateTimeFormat;
        }
        public static DateTime StringToDateTime(string str)
        {
            DateTime minValue;
            if (!DateTime.TryParse(str, GetDateTimeFormatInfo936(), DateTimeStyles.None, out minValue))
            {
                minValue = DateTime.MinValue;
            }
            return minValue;
        }
        public static string DateTimeToString(DateTime Value)
        {
            return Value.ToString(GetDateTimeFormatInfo936());
        }

        public static string ArraybyteToStr(byte[] Value)
        {
            if (Value == null || Value.Length == 0)
            {
                return string.Empty;
            }
            return Convert.ToBase64String(Value);
        }
        public static byte[] StrToArraybyte(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return new byte[0];
            }
            return Convert.FromBase64String(str);
        }

        /// <summary>
        /// Base64加码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string EncodeBase64(string code)
        {
            string result = "";
            byte[] bytes = Encoding.GetEncoding(936).GetBytes(code);
            try
            {
                result = Convert.ToBase64String(bytes);
            }
            catch
            {
                result = code;
            }
            return result;
        }

        /// <summary>
        /// Base64解码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string DecodeBase64(string code)
        {
            string result = "";
            byte[] bytes = Convert.FromBase64String(code);
            try
            {
                result = Encoding.GetEncoding(936).GetString(bytes);
            }
            catch
            {
                result = code;
            }
            return result;
        }

        public static string GetObjectTypeName(object obj)
        {
            string assemblyQualifiedName = obj.GetType().AssemblyQualifiedName;
            return assemblyQualifiedName.Substring(0, assemblyQualifiedName.IndexOf(',', assemblyQualifiedName.IndexOf(',') + 1));
        }
        public static string GetTypeName(Type type)
        {
            string assemblyQualifiedName = type.AssemblyQualifiedName;
            return assemblyQualifiedName.Substring(0, assemblyQualifiedName.IndexOf(',', assemblyQualifiedName.IndexOf(',') + 1));
        }
        public static object CreateObjectByType(string typeName)
        {
            string assemblyName = null;
            int num = typeName.IndexOf(',');
            if (num >= 0)
            {
                assemblyName = typeName.Substring(num + 1);
                typeName = typeName.Substring(0, num);

            }
            var coreAssemblyDirectoryPath = typeof(CommUtils).GetAssembly().GetDirectoryPathOrNull();
            var filePath = $"{coreAssemblyDirectoryPath}\\CjzDataBase.dll";
            var assembly = System.Reflection.Assembly.LoadFile(filePath);

            object obj = assembly.CreateInstance(typeName);
            return obj;
            //Type type = assembly.GetType(typeName, true);
            //return Activator.CreateInstance(type);

        }

        /// <summary>
        /// 删除非空文件夹
        /// </summary>
        /// <param name="path">要删除的文件夹目录</param>
        public static void DeleteDirectory(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            if (dir.Exists)
            {
                DirectoryInfo[] childs = dir.GetDirectories();
                foreach (DirectoryInfo child in childs)
                {
                    child.Delete(true);
                }
                dir.Delete(true);
            }
        }

        /// <summary>
        /// 压缩单个文件
        /// </summary>
        /// <param name="fileToZip">要压缩的文件</param>
        /// <param name="zipedFile">压缩后的文件</param>
        /// <param name="compressionLevel">压缩等级</param>
        /// <param name="blockSize">每次写入大小</param>
        public static void ZipFile(string fileToZip, string zipedFile, int compressionLevel, int blockSize)
        {
            //如果文件没有找到，则报错
            if (!System.IO.File.Exists(fileToZip))
            {
                throw new System.IO.FileNotFoundException("指定要压缩的文件: " + fileToZip + " 不存在!");
            }

            using (System.IO.FileStream ZipFile = System.IO.File.Create(zipedFile))
            {
                using (ZipOutputStream ZipStream = new ZipOutputStream(ZipFile))
                {
                    using (System.IO.FileStream StreamToZip = new System.IO.FileStream(fileToZip, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        string fileName = fileToZip.Substring(fileToZip.LastIndexOf("\\") + 1);

                        ZipEntry ZipEntry = new ZipEntry(fileName);

                        ZipStream.PutNextEntry(ZipEntry);

                        ZipStream.SetLevel(compressionLevel);

                        byte[] buffer = new byte[blockSize];

                        int sizeRead = 0;

                        try
                        {
                            do
                            {
                                sizeRead = StreamToZip.Read(buffer, 0, buffer.Length);
                                ZipStream.Write(buffer, 0, sizeRead);
                            }
                            while (sizeRead > 0);
                        }
                        catch (System.Exception ex)
                        {
                            throw ex;
                        }

                        StreamToZip.Close();
                    }

                    ZipStream.Finish();
                    ZipStream.Close();
                }

                ZipFile.Close();
            }
        }

        /// <summary>
        /// 压缩单个文件
        /// </summary>
        /// <param name="fileToZip">要进行压缩的文件名</param>
        /// <param name="zipedFile">压缩后生成的压缩文件名</param>
        public static void ZipFile(string fileToZip, string zipedFile)
        {
            //如果文件没有找到，则报错
            if (!File.Exists(fileToZip))
            {
                throw new System.IO.FileNotFoundException("指定要压缩的文件: " + fileToZip + " 不存在!");
            }

            using (FileStream fs = File.OpenRead(fileToZip))
            {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();

                using (FileStream ZipFile = File.Create(zipedFile))
                {
                    using (ZipOutputStream ZipStream = new ZipOutputStream(ZipFile))
                    {
                        string fileName = fileToZip.Substring(fileToZip.LastIndexOf("\\") + 1);
                        ZipEntry ZipEntry = new ZipEntry(fileName);
                        ZipStream.PutNextEntry(ZipEntry);
                        ZipStream.SetLevel(5);

                        ZipStream.Write(buffer, 0, buffer.Length);
                        ZipStream.Finish();
                        ZipStream.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 压缩多层目录
        /// </summary>
        /// <param name="strDirectory">The directory.</param>
        /// <param name="zipedFile">The ziped file.</param>
        public static void ZipFileDirectory(string strDirectory, string zipedFile)
        {
            using (System.IO.FileStream ZipFile = System.IO.File.Create(zipedFile))
            {
                using (ZipOutputStream s = new ZipOutputStream(ZipFile))
                {
                    ZipSetp(strDirectory, s, "");
                }
            }
        }

        /// <summary>
        /// 递归遍历目录
        /// </summary>
        /// <param name="strDirectory">The directory.</param>
        /// <param name="s">The ZipOutputStream Object.</param>
        /// <param name="parentPath">The parent path.</param>
        private static void ZipSetp(string strDirectory, ZipOutputStream s, string parentPath)
        {
            if (strDirectory[strDirectory.Length - 1] != Path.DirectorySeparatorChar)
            {
                strDirectory += Path.DirectorySeparatorChar;
            }
            Crc32 crc = new Crc32();

            string[] filenames = Directory.GetFileSystemEntries(strDirectory);

            foreach (string file in filenames)// 遍历所有的文件和目录
            {

                if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                {
                    string pPath = parentPath;
                    pPath += file.Substring(file.LastIndexOf("\\") + 1);
                    pPath += "\\";
                    ZipSetp(file, s, pPath);
                }

                else // 否则直接压缩文件
                {
                    //打开压缩文件
                    using (FileStream fs = File.OpenRead(file))
                    {

                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, buffer.Length);

                        string fileName = parentPath + file.Substring(file.LastIndexOf("\\") + 1);
                        ZipEntry entry = new ZipEntry(fileName);

                        entry.DateTime = DateTime.Now;
                        entry.Size = fs.Length;

                        fs.Close();

                        crc.Reset();
                        crc.Update(buffer);

                        entry.Crc = crc.Value;
                        s.PutNextEntry(entry);

                        s.Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }

        /// <summary>
        /// 解压缩一个 zip 文件。
        /// </summary>
        /// <param name="zipedFile">The ziped file.</param>
        /// <param name="strDirectory">The STR directory.</param>
        /// <param name="password">zip 文件的密码。</param>
        /// <param name="overWrite">是否覆盖已存在的文件。</param>
        public static bool UnZip(string zipedFile, string strDirectory, string password, bool overWrite)
        {

            if (strDirectory == "")
                strDirectory = Directory.GetCurrentDirectory();
            if (!strDirectory.EndsWith("\\"))
                strDirectory = strDirectory + "\\";

            using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipedFile)))
            {
                s.Password = password;
                ZipEntry theEntry;

                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = "";
                    string pathToZip = "";
                    pathToZip = theEntry.Name;

                    if (pathToZip != "")
                        directoryName = Path.GetDirectoryName(pathToZip) + "\\";

                    string fileName = Path.GetFileName(pathToZip);

                    Directory.CreateDirectory(strDirectory + directoryName);

                    if (fileName != "")
                    {
                        if ((File.Exists(strDirectory + directoryName + fileName) && overWrite) || (!File.Exists(strDirectory + directoryName + fileName)))
                        {
                            using (FileStream streamWriter = File.Create(strDirectory + directoryName + fileName))
                            {
                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);

                                    if (size > 0)
                                        streamWriter.Write(data, 0, size);
                                    else
                                        break;
                                }
                                streamWriter.Close();
                            }
                        }
                    }
                }

                s.Close();

            }
            return true;
        }
    }
}
