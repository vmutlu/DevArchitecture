﻿using System;
using System.IO;
using System.Text;

namespace UiPreparation
{
	class Program
	{
		static void Main(string[] args)
		{
			var path = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.IndexOf("bin"));
			var exePath = Path.Combine(path, "DevArchitectureUI");
			var deletePath = exePath + @"\node_modules\selenium-webdriver\lib\test\data";
			StringBuilder bld = new StringBuilder();

			bld.Append("npm install -g @angular/cli@latest&");
			bld.Append("npm install&");
			bld.Append("npm prune&");
			bld.Append("code .&");
			bld.Append($"RD /S /Q {deletePath} &");
			bld.Append("ng serve --open&");			
			

			System.Diagnostics.Process cmd = new System.Diagnostics.Process();
			cmd.StartInfo.UseShellExecute = false;
			cmd.StartInfo.FileName = "cmd.exe";
			cmd.StartInfo.WorkingDirectory = exePath;
			cmd.StartInfo.Arguments = @"/c " + bld.ToString();
			cmd.Start();
			Console.ReadLine();			
			cmd.WaitForExit();			
		}		
	}

}
