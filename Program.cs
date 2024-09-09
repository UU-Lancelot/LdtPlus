using LdtPlus.Config;
using LdtPlus.Exceptions;
using LdtPlus.Gui;

InputHandler inputHandler = new();
using (Gui gui = new(inputHandler))
{
    try
    {
        gui.ShowHeader();

        /// load configuration ///
        using (LoaderComponent loader = gui.ShowLoader())
        {
            #warning TODO: load configuration
            Thread.Sleep(200);
        }

        bool ldtFileExists = true;
        bool needCreateConfiguration = false;
        if (!ldtFileExists) // ldt file 
        {
            gui.UsePathInput(out string path);

            if (needCreateConfiguration)
            {
                using (LoaderComponent loader = gui.ShowLoader())
                {
                    #warning TODO: create configuration
                    Thread.Sleep(200);
                }
            }
        }

        /// show menu ///
        gui.UseMenu(new ConfigData().Menu, out string command);

        #warning TODO: execute command
    }
    catch (ExitAppException)
    {
        return;
    }
    catch (Exception ex)
    {
        gui.ShowError(ex);
        Console.ReadKey(true);
    }
}
