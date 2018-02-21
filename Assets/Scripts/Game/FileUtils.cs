using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace WsUtils
{
	public class FileUtils
	{

		private static FileUtils _instance = null;
		private string _syspath = "";

		public FileUtils()
		{
			_syspath = Application.persistentDataPath;
		}

		public static FileUtils getInstance()
		{
			if (_instance == null)
			{
				_instance = new FileUtils();
			}
			return _instance;
		}
		/**
		 * 用于读写可读写空间的文件
		 */
		public string readStreamingFile(string name)
		{
			string data = "";
			Debug.Log(data);
			string filePath = Application.dataPath + "/" + name;
			FileInfo fi = new FileInfo(filePath);
			if (!fi.Exists)
			{//Application.platform == RuntimePlatform.Android
				Debug.Log("A2 Search: " + filePath + ", datapath=" + Application.dataPath + ", name=" + name);
				WWW www = new WWW(filePath);
				while (!www.isDone)
				{
				}
				//if (www.error.Length < 1)
				data = www.text + " ERROR:" + www.error;
				//StartCoroutine(LoadWWW());
			}
			else
			{
				//sockectLogger.doLog("not android");
				StreamReader sr = null;
				sr = fi.OpenText();
				data = sr.ReadToEnd();
				sr.Close();
				sr.Dispose();
			}

			//sockectLogger.doLog("Read done: " + data);
			return data;
		}

		public void WriteFile(string name, string info)
		{
			WriteFile(name, info, true);
		}
		/**   用于写可读写空间的文件
		   * name：文件的名称   
		   *  info：写入的内容   
		   */
		public void WriteFile(string name, string info, bool overwrite)
		{
			//文件流信息
			StreamWriter sw;
			FileInfo t = new FileInfo(_syspath + "//" + name);
			if (!t.Exists)
			{
				//如果此文件不存在则创建
				sw = t.CreateText();
			}
			else
			{
				if (overwrite)
					sw = t.CreateText();
				else
					sw = t.AppendText();
			}
			//以行的形式写入信息
			sw.WriteLine(info);
			//关闭流
			sw.Close();
			//销毁流
			sw.Dispose();
		}

		/**   用于写可读写空间的文件
	   * name：读取文件的名称   
	   */
		public string ReadFile(string name)
		{
			//使用流的形式读取
			StreamReader sr = null;
			try
			{
				sr = File.OpenText(_syspath + "//" + name);
			}
			catch (Exception )
			{
				//路径与名称未找到文件则直接返回空
				return null;
			}
			string data = sr.ReadToEnd();

			//关闭流
			sr.Close();
			//销毁流
			sr.Dispose();
			//将数组链表容器返回   
			return data;
		}

		/**   用于删除可读写空间的文件
	   * name：删除文件的名称   
	   */

		void DeleteFile(string name)
		{
			File.Delete(_syspath + "//" + name);
		}
	}
}
