using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace UnityBuilder {
    internal static class StreamingAssetsCleaner {
        internal static void CleanStreamingAssetsFolder(Platform platform) {
            string streamingAssetsPath = platform.GetBuildStreamingAssetsFolder();

            if(!Directory.Exists(streamingAssetsPath)) {
                Debug.LogWarning($"StreamingAssets folder not found: {streamingAssetsPath}");
                return;
            }

            CleanFoldersRecursive(streamingAssetsPath, platform);
        }

        private static void CleanFoldersRecursive(string rootFolder, Platform platform) {
            foreach(string folder in Directory.GetDirectories(rootFolder)) {
                string configFile = Path.Combine(folder, "platforms.json");

                bool shouldDelete = false; // default: keep folder

                if(File.Exists(configFile)) {
                    try {
                        string json = File.ReadAllText(configFile);
                        PlatformConfig config = JsonConvert.DeserializeObject<PlatformConfig>(json);

                        if(config == null || config.platforms == null || config.platforms.Count == 0) {
                            // Empty config = default keep
                            shouldDelete = false;
                        }
                        else {
                            // If current platform is NOT listed → delete
                            bool listed = config.platforms
                                .Any(p => p.Equals(platform.ToString(), StringComparison.OrdinalIgnoreCase));

                            if(!listed)
                                shouldDelete = true;
                        }
                        File.Delete(configFile);
                    }
                    catch(Exception ex) {
                        Debug.LogError($"Error parsing {configFile}: {ex}");
                        // On parse error → keep folder (safe default)
                        shouldDelete = false;
                    }
                }

                if(shouldDelete) {
                    Directory.Delete(folder, true);
                }
                else {
                    // Recurse into subfolders
                    CleanFoldersRecursive(folder, platform);
                }
            }

        }


        [Serializable]
        private class PlatformConfig {
            public List<string> platforms;
        }
    }

}