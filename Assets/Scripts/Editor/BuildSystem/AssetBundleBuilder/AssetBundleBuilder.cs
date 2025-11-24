using System;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

namespace UnityBuilder.AssetBundle {
    internal static class AssetBundleBuilder {
        internal static bool TryBuildAssetBundlesForPlatform(Platform platform) {

            var settings = AssetDatabase.LoadAssetAtPath<AddressableAssetSettings>("Assets/AddressableAssetsData/AddressableAssetSettings.asset");

            SetGroupInclusionForActiveProfile(settings, platform);

            try {
                AddressableAssetSettings.CleanPlayerContent();
                AddressableAssetSettings.BuildPlayerContent();

                Debug.Log($"✅ Addressables built successfully for {platform}");
                return true;
            }
            catch(Exception e) {
                Debug.LogException(e);
                return false;
            }
        }

        private static void SetGroupInclusionForActiveProfile(AddressableAssetSettings settings, Platform platform) {
            foreach(var group in settings.groups.ToList()) {
                if(group == null) continue;
                var validator = new GroupCompositeValidator();
                bool shouldInclude = validator.ShouldBeIncluded(platform, group.Name);

                // Get the BundledAssetGroupSchema (this is the schema that contains IncludeInBuild)
                var bundledSchema = group.GetSchema<BundledAssetGroupSchema>();
                if(bundledSchema == null) {
                    Debug.LogWarning($"Could not get/add BundledAssetGroupSchema for group {group.Name}");
                    return;
                }

                if(bundledSchema.IncludeInBuild != shouldInclude) {
                    bundledSchema.IncludeInBuild = shouldInclude;
                    Debug.Log($"Set IncludeInBuild={shouldInclude} for group '{group.Name}'");
                }
            }

            // Persist changes
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.BatchModification, settings, true);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"Updated group inclusion for platform '{platform}'.");
        }

    }
}
