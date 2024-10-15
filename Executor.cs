using System.Diagnostics;

namespace LdtPlus;
public class Executor
{
    public Executor(string ldtPath)
    {
        _ldtPath = ldtPath;
    }

    private readonly string _ldtPath;

    public void Run(Gui.Gui gui, string command)
    {
        using (Process process = new Process())
        {
            process.StartInfo.FileName = _ldtPath;
            process.StartInfo.Arguments = command;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.CreateNoWindow = true; //not diplay a windows
            process.EnableRaisingEvents = true;

            gui.Show(b => b
                .ShowProcess(process));

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
        }
    }

    public string RunAndGetOutput(string command)
    {
        using (Process process = new Process())
        {
            process.StartInfo.FileName = _ldtPath;
            process.StartInfo.Arguments = command;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.CreateNoWindow = true; //not diplay a windows

            process.Start();
            string output = process.StandardOutput.ReadToEnd(); //The output result
            process.WaitForExit();

            return output;
        }
    }
}