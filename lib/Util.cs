using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MikManager.Utility
{
    public static partial class Util
    {
        private static readonly HttpClient httpQueryClient = new() {
            BaseAddress = new Uri("https://api.modrinth.com")
        };

        private static readonly HttpClient httpDownloadClient = new() {
            BaseAddress = new Uri("https://cdn.modrinth.com/data")
        };

        public static ModDownloadDetails ReadModsTextFile() {
            Debug.LogInfo("Reading .tml files...");
            string cwd = Directory.GetCurrentDirectory();
            string[] files = Directory.GetFiles(cwd);
            string[] filtered = [.. files.Where((file) => file.ToLower().EndsWith(".tml"))];
            if (filtered.Length == 0) {
                Debug.LogError("No .tml file found");
                ConsolePauseAndExit();
            }
            // TODO: Change this so it uses all .tml files
            string modTextPath = filtered[0];
            Debug.LogInfo($"Parsing {Path.GetFileName(modTextPath)}...");
            string modTextContent = File.ReadAllText(modTextPath);
            modTextContent = CommentRegex().Replace(modTextContent, ""); 
            modTextContent = WhiteSpaceRegex().Replace(modTextContent, "");
            modTextContent = modTextContent.Replace("\"", "");
            return new ModDownloadDetails(modTextContent);
        }

        public static async Task<ModrinthModJSON?> GetModJSON(string slug, string version = "") {
            Debug.LogInfo($"Querying for {slug}...");
            string versionQuery = "";
            if (version.Length > 0) versionQuery = $"&game_versions=[\"{version}\"]";

            HttpResponseMessage response = await httpQueryClient.GetAsync($"v2/project/{slug}/version?loaders=[\"fabric\"]{versionQuery}");
            response.WriteRequestToConsole();

            try {
                var jsonList = await response.Content.ReadFromJsonAsync<List<ModrinthModJSON>>();
                if (jsonList == null || jsonList.Count == 0) return LogAndReturnNull(); 

                // jsonList.ForEach((json) => Console.WriteLine(json.version_type));
                List<ModrinthModJSON> filteredList = [.. jsonList.Where((json) => json.version_type == "release")];
                ModrinthModJSON json = (filteredList.Count > 0) ? filteredList[0] : jsonList[0];
                if (filteredList.Count == 0) 
                    Debug.LogWarning($"Unable to find release version for {slug}, found {json.version_type} version instead");

                return json;
            } catch { return LogAndReturnNull(); }
            
            ModrinthModJSON? LogAndReturnNull() {
                string errString = $"Could not find mod \"{slug}\"" + (version.Length > 0 ? $" for version {version}" : "");
                Debug.LogError(errString);
                return null;
            }
        }

        public static async Task DownloadModFromJSON(ModrinthModJSON json, string downloadDir) {
            string? downloadUrl = json.files[0].url;
            string? fileName = json.files[0].filename;
            if (downloadUrl == null || fileName == null) {
                Debug.LogError("Download url or file name was null");
                return;
            }
            Debug.LogInfo($"Downloading {fileName}...");
            HttpResponseMessage response = await httpDownloadClient.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead);
            response.WriteRequestToConsole();
            
            if (!downloadDir.EndsWith('/') && !downloadDir.EndsWith('\\')) downloadDir += '\\';
            await using FileStream fileStream = new(Path.Join(downloadDir + fileName), FileMode.Create, FileAccess.Write, FileShare.None);
            await response.Content.CopyToAsync(fileStream);
        }

        public static void ConsolePauseAndExit() {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
            Environment.Exit(0);
        }

        // Lines that start with a # get removed
        [GeneratedRegex(@"#.*(?:\n|$)")]
        private static partial Regex CommentRegex();
        // Any white space except that between quotation marks is removed
        [GeneratedRegex(@"\s+(?=(?:[^""]*""[^""]*"")*[^""]*$)")]
        private static partial Regex WhiteSpaceRegex();
    }

    public record class ModDownloadDetails
    {
        public readonly string version;
        public readonly string downloadDir;
        public readonly string[] slugs;

        public ModDownloadDetails(string parsedCleanedTextContent) {
            this.version = GetVariableValue("version", parsedCleanedTextContent);
            this.downloadDir = GetVariableValue("downloadDir", parsedCleanedTextContent);
            if (this.downloadDir == "." || this.downloadDir == "") this.downloadDir = Directory.GetCurrentDirectory();
            this.slugs = GetListValue("slugs", parsedCleanedTextContent);
        }
        private static string GetVariableValue(string key, string parsedCleanedTextContent) {
            return GetValueString(key, "=", parsedCleanedTextContent);
        }
        private static string[] GetListValue(string key, string parsedCleanedTextContent) {
            return GetValueString(key, ":", parsedCleanedTextContent).Split(',');
        }
        private static string GetValueString(string key, string assigner, string parsedCleanedTextContent)
        {
            key += assigner;
            if (!parsedCleanedTextContent.Contains(key)) return "";
            int valueStartIndex = parsedCleanedTextContent.IndexOf(key) + key.Length;
            int valueEndIndex = parsedCleanedTextContent.IndexOf(';', valueStartIndex);
            if (valueEndIndex == -1) valueEndIndex = parsedCleanedTextContent.Length;
            int valueLength = valueEndIndex - valueStartIndex;
            return parsedCleanedTextContent.Substring(valueStartIndex, valueLength);
        }
        public override string ToString()
        {
            // 20 is just a wildly picked number it has no math behind it
            StringBuilder build = new(20);
            build.Append($"{this.version}\n{this.downloadDir}");
            foreach (string slug in slugs)
                build.Append($"\n{slug}");
            return build.ToString();
        }
    }

    public record class ModrinthModJSON(
        string[]? game_versions,
        string[]? loaders,
        string? id,
        string? project_id,
        string? author_id,
        bool? featured,
        string? name,
        string? version_number,
        string? changelog,
        string? changelog_url,
        string? date_published,
        int? downloads,
        string? version_type,
        string? status,
        string? requested_status,
        ModrinthModFileJSON[] files,
        ModrinthModDependencyJSON[] dependencies
    );

    public record class ModrinthModFileJSON(
        ModrinthModFileHashesJSON hashes,
        string? url,
        string? filename,
        bool? primary,
        int? size,
        string? fileType
    );
    
    public record class ModrinthModDependencyJSON(
        string? version_id,
        string? project_id,
        string? file_name,
        string? dependency_type
    );

    public record class ModrinthModFileHashesJSON(string? sha512, string? sha1);
}
