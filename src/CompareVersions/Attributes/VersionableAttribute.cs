namespace CompareVersions.Attributes
{
    [AttributeUsage((AttributeTargets.Class))]
    internal class VersionableAttribute<T> : Attribute
    where T : IVersionable

    {
        /// <summary>
        /// Gets the type of the parameter.
        /// </summary>
        /// <value>
        /// The type of the parameter.
        /// </value>
        public T ParamType { get; }
    }

    internal interface IVersionable
    {
    }
}
