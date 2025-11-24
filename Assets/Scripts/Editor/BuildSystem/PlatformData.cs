using System.IO;
using UnityEditor;

namespace UnityBuilder {
    internal static class PlatformExtensions {

        internal static string GetBuildFolder(this Platform platform) {
            string projectFolder = Directory.GetParent(UnityEngine.Application.dataPath).FullName;
            return Path.Combine(projectFolder, "Builds", platform.ToString());
        }

        internal static string GetTargetPath(this Platform platform) {
            string product = PlayerSettings.productName;
            var platformSpecific = platform switch
            {
                Platform.Windows => $@"{product}.exe",
                Platform.Mac => $@"{product}.app",
                Platform.Linux => $@"{product}.x86_64",
                Platform.WebGL => $@"",
                Platform.SteamDeck => $@"{product}.x86_64",
                _ => throw new System.NotImplementedException(),
            };
            return Path.Combine(platform.GetBuildFolder(), platformSpecific);
        }

        internal static string GetBuildStreamingAssetsFolder(this Platform platform) {
            string product = PlayerSettings.productName;
            var platformSpecific = platform switch
            {
                Platform.Windows => $"{product}_Data",
                Platform.Mac => $@"{product}.app\Contents\Resources\Data",
                Platform.Linux => $@"{product}_Data",
                Platform.WebGL => @"",
                Platform.SteamDeck => $@"{product}_Data",
                _ => throw new System.NotImplementedException(),
            };

            return Path.Combine(platform.GetBuildFolder(), platformSpecific, "StreamingAssets");
        }
    }

    internal enum Platform {
        Windows, Mac, Linux, WebGL, SteamDeck
    }
}