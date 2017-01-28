using System;
using System.Reflection; //Test if Executable is Win32 or .Net
using System.Diagnostics; //For launching the executable.
using System.Linq;	//Easy way to get first value of Array.
using System.IO;	//Used to test if the file exists.

namespace WvM
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			string location = "";
			string arguments = "";
			try
			{
				location = args.First();
				//Assuming this hasn't already errored (No Arguments, Automatic Failure and Try/Catch)
				//we need to check that the file Exists, and that it is in fact a Windows Executable file.
				if (!File.Exists(location))
				{
					throw new Exception();
				}
				if (location.EndsWith(".EXE", true, System.Globalization.CultureInfo.CurrentCulture) == false)
				{
					throw new Exception();
				}
				//We need to skip the first argument, since that is the exectuable, and put together our list
				//of arguments that were passed, since we are passing those to the application that runs
				//at the end of all this.
				bool first = true;
				foreach (var arg in args)
				{
					if (!first)
					{
						arguments += arg + " ";
					}
					else
					{
						first = false; //Skipped first value!
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("Nothing was passed, File does not exist, or it is not an EXE file.");
				//Console.WriteLine(e.ToString()); //Left for Debugging Purposes.
				Environment.Exit(1);

				//If this was executed, then any of the above checks failed.
			}
				
			try
			{
				var testAssembly = AssemblyName.GetAssemblyName(location); 
				//If this is NOT a .Net Application, the above command will fail
				//and the try/catch will assume that the specified application is
				//a Win32 Application.
				Console.WriteLine("Launching Mono!");
				var Proc = new ProcessStartInfo();
				Proc.FileName = "mono";
				Proc.Arguments = location + " " + arguments;
				Process.Start(Proc);
			}
			catch
			{
				Console.WriteLine("Launching Wine!");
				var Proc = new ProcessStartInfo();
				Proc.FileName = "wine";
				Proc.Arguments = location + " " + arguments;
				Process.Start(Proc);
			}
			//At this point we have finished executing. Congratulations! Your Application is now running on
			//what is hopefully the most appropriate runtime available to it.
		}
	}
}