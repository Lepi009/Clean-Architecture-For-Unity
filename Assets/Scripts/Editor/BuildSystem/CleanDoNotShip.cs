using System;
using System.IO;
using System.Linq;

namespace UnityBuilder {
    internal static class CleanDoNotShip {
        static readonly string[] EXCLUDED_FOLDERS = new string[] { "DoNotShip", "DontShipIt" };

        internal static void Delete(string buildPath) {
            var directories = Directory.GetDirectories(buildPath, "*", SearchOption.TopDirectoryOnly)
                                       .Where(d => {
                                           string name = Path.GetFileName(d);
                                           return name.Contains(EXCLUDED_FOLDERS[0]) || name.Contains(EXCLUDED_FOLDERS[1]);
                                       })
                                       .ToList();

            for(int i = directories.Count - 1; i >= 0; i--) {
                if(Directory.Exists(directories[i])) {
                    Directory.Delete(directories[i], true);
                }
            }
        }
    }
}