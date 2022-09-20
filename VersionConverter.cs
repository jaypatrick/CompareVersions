
namespace CompareVersions.UI;

/// <summary>
///     Class for converting <see cref="Version"/> instances to other types.
/// </summary>
/// <seealso cref="System.ComponentModel.TypeConverter" />
public class VersionConverter : TypeConverter
{
    public override bool CanConvertFrom([AllowNull] ITypeDescriptorContext context, Type sourceType)
    {


        return base.CanConvertFrom(context, sourceType);
    }

    public override bool CanConvertTo([AllowNull] ITypeDescriptorContext context, [NotNullWhen(true)][AllowNull] Type destinationType)
    {
        return base.CanConvertTo(context, destinationType);
    }

    public override object ConvertFrom([AllowNull] ITypeDescriptorContext context, [AllowNull] CultureInfo culture, object value)
    {
        return base.ConvertFrom(context, culture, value);
    }

    public override object ConvertTo([AllowNull] ITypeDescriptorContext context, [AllowNull] CultureInfo culture, [AllowNull] object value, Type destinationType)
    {
        return base.ConvertTo(context, culture, value, destinationType);
    }

    public override object CreateInstance([AllowNull] ITypeDescriptorContext context, IDictionary propertyValues)
    {
        return base.CreateInstance(context, propertyValues);
    }

    public override bool Equals([AllowNull] object obj)
    {
        return base.Equals(obj);
    }

    public override bool GetCreateInstanceSupported([AllowNull] ITypeDescriptorContext context)
    {
        return base.GetCreateInstanceSupported(context);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override PropertyDescriptorCollection GetProperties([AllowNull] ITypeDescriptorContext context, object value, [AllowNull] Attribute[] attributes)
    {
        return base.GetProperties(context, value, attributes);
    }

    public override bool GetPropertiesSupported([AllowNull] ITypeDescriptorContext context)
    {
        return base.GetPropertiesSupported(context);
    }

    public override StandardValuesCollection GetStandardValues([AllowNull] ITypeDescriptorContext context)
    {
        return base.GetStandardValues(context);
    }

    public override bool GetStandardValuesExclusive([AllowNull] ITypeDescriptorContext context)
    {
        return base.GetStandardValuesExclusive(context);
    }

    public override bool GetStandardValuesSupported([AllowNull] ITypeDescriptorContext context)
    {
        return base.GetStandardValuesSupported(context);
    }

    public override bool IsValid([AllowNull] ITypeDescriptorContext context, [AllowNull] object value)
    {
        return base.IsValid(context, value);
    }

    public override string ToString()
    {
        return base.ToString();
    }
}