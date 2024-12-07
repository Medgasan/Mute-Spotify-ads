using NAudio.CoreAudioApi;

namespace PruebasConNAudio
{
    internal static class Program
    {

        static public string sqldbname = "Data Source=blacklist.db";

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());

        }


    }
}