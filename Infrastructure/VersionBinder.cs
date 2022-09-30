using System.CommandLine.Binding;
using CompareVersions.Models;
using Version = CompareVersions.Models.Version;

namespace CompareVersions.Infrastructure
{
    public class VersionBinder : BinderBase<Version>
    {
        private readonly Option<Segment> major;
        private readonly Option<Segment> minor;
        private readonly Option<Segment> patch;
        private readonly Option<Segment> build;

        private readonly Option<Version> version;

        private readonly Option<IList<Version>> versions;
        private readonly Option<IList<string>> versionStrings;

        public VersionBinder(Option<Segment> major, Option<Segment> minor, Option<Segment> patch, Option<Segment> build)
        {
            this.major = major;
            this.minor = minor;
            this.patch = patch;
            this.build = build;
        }

        public VersionBinder(Option<Version> version)
        {
            this.version = version;
        }

        public VersionBinder(Option<IList<Version>> versions)
        {
            this.versions = versions;
        }

        public VersionBinder(Option<IList<string>> versionStrings)
        {
            this.versionStrings = versionStrings;
        }

        protected override Version GetBoundValue(BindingContext bindingContext) =>
            new Version()
            {
                // This is only needed if we parse out each SEGMENT from the commandline
                MajorSegment = bindingContext.ParseResult.GetValueForOption(major),
                MinorSegment = bindingContext.ParseResult.GetValueForOption(minor),
                PatchSegment = bindingContext.ParseResult.GetValueForOption(patch),
                BuildSegment = bindingContext.ParseResult.GetValueForOption(build)

            };
    }
}
