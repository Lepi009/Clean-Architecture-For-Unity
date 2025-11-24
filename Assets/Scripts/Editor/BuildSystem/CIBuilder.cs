using System;
using UnityEditor;
using UnityEditor.Build.Profile;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityBuilder.AssetBundle;

namespace UnityBuilder {
    public static class CIBuilder {
        public static void BuildFromCommandLine() {
            // Get platform from command line
            if(!Enum.TryParse(GetArgument("platform"), out Platform platform)) {
                Debug.LogError($"Cannot parse platform {GetArgument("platform")}");
                EditorApplication.Exit(1);
                return;
            }

            var currentProfile = AssetDatabase.LoadAssetAtPath<BuildProfile>($"Assets/Settings/Build Profiles/{platform}.asset");

            if(currentProfile == null) {
                Debug.LogError($"Build profile for {platform} not found.");
                EditorApplication.Exit(1);
                return;
            }

            //switch to that build profile
            try {
                BuildProfile.SetActiveBuildProfile(currentProfile);
            }
            catch(Exception e) {
                Debug.LogException(e);
                EditorApplication.Exit(1);
                return;
            }

            // Build Addressables for this platform
            if(!AssetBundleBuilder.TryBuildAssetBundlesForPlatform(platform)) {
                Debug.LogError("Addressables build failed!");
                EditorApplication.Exit(1);
                return;
            }

            //reassign current profile because it can be overwritten by BuildAddressables
            currentProfile = AssetDatabase.LoadAssetAtPath<BuildProfile>($"Assets/Settings/Build Profiles/{platform}.asset");

            try {
                BuildPlayerWithProfileOptions options = new() {
                    buildProfile = currentProfile,
                    locationPathName = platform.GetTargetPath(),
                    options = BuildOptions.CleanBuildCache | BuildOptions.Development,

                };
                var report = BuildPipeline.BuildPlayer(options);

                var summary = report.summary;

                if(summary.result == BuildResult.Succeeded) {
                    Debug.Log($"Build succeeded: {summary.totalSize / 1024 / 1024} MB");

                    StreamingAssetsCleaner.CleanStreamingAssetsFolder(platform);
                    CleanDoNotShip.Delete(platform.GetBuildFolder());
                    EditorApplication.Exit(0);
                }
                else {
                    Debug.LogError($"Build failed: {summary.result}");
                    EditorApplication.Exit(1);
                }
            }
            finally { }
        }

        private static string GetArgument(string name) {
            var args = Environment.GetCommandLineArgs();
            for(int i = 0; i < args.Length; i++) {
                if(args[i] == $"-{name}" && i + 1 < args.Length)
                    return args[i + 1];
            }
            return null;
        }

        public static void SetVersionFromCommandLine() {
            var args = System.Environment.GetCommandLineArgs();
            for(int i = 0; i < args.Length; i++) {
                if(args[i] == "-bundleVersion" && i + 1 < args.Length) {
                    string version = args[i + 1];
                    PlayerSettings.bundleVersion = version;
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    Debug.Log("Updated bundleVersion to " + version);
                    return;
                }
            }
            Debug.LogError("Missing -bundleVersion argument.");
        }

        public static void SetToDefaultProfile() {
            var profile = AssetDatabase.LoadAssetAtPath<BuildProfile>($"Assets/Settings/Build Profiles/Windows.asset");
            if(profile != null) {
                BuildProfile.SetActiveBuildProfile(profile);
            }
        }

    }

}
