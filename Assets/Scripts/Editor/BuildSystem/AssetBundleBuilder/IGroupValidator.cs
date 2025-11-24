namespace UnityBuilder.AssetBundle {
    internal interface IGroupValidator {
        public bool ShouldBeIncluded(Platform platform, string groupName);
    }
}