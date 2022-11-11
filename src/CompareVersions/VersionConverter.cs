using Version = CompareVersions.Models.Version;

namespace CompareVersions.UI;

/// <summary>
///     Class for converting <see cref="Version"/> instances to other types.
/// </summary>
/// <seealso cref="System.ComponentModel.TypeConverter" />
public class VersionConverter : TypeConverter
{
    /// <summary>
    /// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
    /// <param name="sourceType">A <see cref="T:System.Type" /> that represents the type you want to convert from.</param>
    /// <returns>
    ///   <see langword="true" /> if this converter can perform the conversion; otherwise, <see langword="false" />.
    /// </returns>
    public override bool CanConvertFrom([AllowNull] ITypeDescriptorContext context, Type sourceType)
    {
        return sourceType == typeof(Version) || base.CanConvertFrom(context, sourceType);
    }

    /// <summary>
    /// Returns whether this converter can convert the object to the specified type, using the specified context.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
    /// <param name="destinationType">A <see cref="T:System.Type" /> that represents the type you want to convert to.</param>
    /// <returns>
    ///   <see langword="true" /> if this converter can perform the conversion; otherwise, <see langword="false" />.
    /// </returns>
    public override bool CanConvertTo([AllowNull] ITypeDescriptorContext context, [NotNullWhen(true)][AllowNull] Type destinationType)
    {
        if (destinationType == typeof(System.Version)) return true;
        else return base.CanConvertTo(context, destinationType);
    }

    /// <summary>
    /// Converts the given object to the type of this converter, using the specified context and culture information.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
    /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture.</param>
    /// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
    /// <returns>
    /// An <see cref="T:System.Object" /> that represents the converted value.
    /// </returns>
    public override object ConvertFrom([AllowNull] ITypeDescriptorContext context, [AllowNull] CultureInfo culture, object value)
    {
        if (value is System.Version systemVersion)
        {
            var localVersion = new Version(systemVersion.Major, systemVersion.Minor, systemVersion.Build, systemVersion.Revision);
            return localVersion;
        }
        else
        {
            return base.ConvertFrom(context, culture, value) ?? throw new InvalidOperationException();
        }
    }

    /// <summary>
    /// Converts the given value object to the specified type, using the specified context and culture information.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
    /// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" />. If <see langword="null" /> is passed, the current culture is assumed.</param>
    /// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
    /// <param name="destinationType">The <see cref="T:System.Type" /> to convert the <paramref name="value" /> parameter to.</param>
    /// <returns>
    /// An <see cref="T:System.Object" /> that represents the converted value.
    /// </returns>
    public override object ConvertTo([AllowNull] ITypeDescriptorContext context, [AllowNull] CultureInfo culture, [AllowNull] object value, Type destinationType)
    {
        if (destinationType == typeof(System.Version))
        {
            var localVersion = value as Version;
            if (localVersion is Version)
            {
                var systemVersion = new System.Version(localVersion.MajorSegment.Value,
                    localVersion.MinorSegment.Value,
                    localVersion.PatchSegment.Value,
                    localVersion.BuildSegment.Value);
                return systemVersion;
            }
        }

        return base.ConvertTo(context, culture, value, destinationType) ?? throw new InvalidOperationException();
    }
}