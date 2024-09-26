namespace LdtPlus;
public class Executor
{
    public Executor(string ldtPath)
    {
        _ldtPath = ldtPath;
    }

    private readonly string _ldtPath;

    public void Run(string command)
    {
        using (System.Diagnostics.Process process = new System.Diagnostics.Process())
        {
            process.StartInfo.FileName = _ldtPath;
            process.StartInfo.Arguments = command;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            process.StartInfo.CreateNoWindow = true; //not diplay a windows

            process.Start();
            process.StandardOutput.BaseStream.CopyTo(Console.OpenStandardOutput()); //The output result
            process.WaitForExit();
        }
    }

    public string RunAndGetOutput(string command)
    {
        using (System.Diagnostics.Process process = new System.Diagnostics.Process())
        {
            process.StartInfo.FileName = _ldtPath;
            process.StartInfo.Arguments = command;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            process.StartInfo.CreateNoWindow = true; //not diplay a windows

            process.Start();
            string output = process.StandardOutput.ReadToEnd(); //The output result
            process.WaitForExit();

            return output;
        }
    }
}