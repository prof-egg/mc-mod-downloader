using System.Threading.Tasks;
using MikManager.Utility;

namespace MikManager
{
    public class App
    {
        public static async Task Main(string[] args)
        {

            Console.WriteLine("             _                            _        ");
            Console.WriteLine("           _| |                   _      | |       ");
            Console.WriteLine(" _ __ ___ ( ) | __  ___  ___ _ __( )_ __ | |_      ");
            Console.WriteLine("| '_ ` _ \\| | |/ / / __|/ __| '__| | '_ \\| __|   ");
            Console.WriteLine("| | | | | | |   <  \\__ \\ (__| |  | | |_) | |_    ");
            Console.WriteLine("|_| |_| |_|_|_|\\_\\ |___/\\___|_|  |_| .__/ \\__| ");
            Console.WriteLine("                                   | |             ");
            Console.WriteLine("                                   |_|             ");

            ModDownloadDetails mdd = Util.ReadModsTextFile();

            if (mdd.version == "")
            {
                Debug.LogError("No game version specified");
                Util.ConsolePauseAndExit();
            }
            if (!Directory.Exists(mdd.downloadDir)) 
            {
                Debug.LogError($"Directory \"{mdd.downloadDir}\" does not exist.");
                Util.ConsolePauseAndExit();
            }
            if (mdd.slugs.Length == 0 || (mdd.slugs.Length == 1 && mdd.slugs[0] == ""))
            {
                Debug.LogError($"No slugs specified");
                Util.ConsolePauseAndExit();
            }

            Debug.LogInfo($"Downloading {mdd.slugs.Length} mods for version {mdd.version} to {mdd.downloadDir}");
            foreach (string slug in mdd.slugs) 
            {
                var stuff = await Util.GetModJSON(slug, mdd.version);
                if (stuff == null) continue;
                await Util.DownloadModFromJSON(stuff, mdd.downloadDir);
            }
            Debug.LogInfo("Done!");
            Util.ConsolePauseAndExit();
            // await Util.GetModJSON("particular", "1.21.1");
        }   
    }
}