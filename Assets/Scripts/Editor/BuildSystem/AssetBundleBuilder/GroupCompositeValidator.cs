using System.Linq;

namespace UnityBuilder.AssetBundle {
    internal class GroupCompositeValidator : IGroupValidator {
        //include all fields and properties here (private & public)
        #region Fields and Properties

        private IGroupValidator[] _rules = {
            new BigScreenGroupValidator(),
            new PlatformGroupValidator(),
            new SharedGroupValidator(),
            new SmallScreenGroupValidator(),
            new StandaloneGroupValidator()
        };

        #endregion



        //include all public methods here
        #region Public Methods
        public bool ShouldBeIncluded(Platform platform, string groupName)
            => _rules.Any(rule => rule.ShouldBeIncluded(platform, groupName));

        #endregion


        //include all private methods here
        #region Private Methods

        #endregion

        private class BigScreenGroupValidator : IGroupValidator {
            public bool ShouldBeIncluded(Platform platform, string groupName)
                => groupName.ToLower().Contains("bigscreen") && platform != Platform.SteamDeck;
        }

        private class PlatformGroupValidator : IGroupValidator {
            public bool ShouldBeIncluded(Platform platform, string groupName)
                => groupName.ToLower().Contains(platform.ToString().ToLower());
        }

        private class SharedGroupValidator : IGroupValidator {
            public bool ShouldBeIncluded(Platform platform, string groupName)
                => groupName.ToLower().Contains("shared");
        }

        private class SmallScreenGroupValidator : IGroupValidator {
            public bool ShouldBeIncluded(Platform platform, string groupName)
                => groupName.ToLower().Contains("smallscreen");
        }

        private class StandaloneGroupValidator : IGroupValidator {
            public bool ShouldBeIncluded(Platform platform, string groupName)
                => groupName.ToLower().Contains("standalone") &&
                    (platform == Platform.Windows
                    || platform == Platform.Mac
                    || platform == Platform.Linux);
        }
    }
}