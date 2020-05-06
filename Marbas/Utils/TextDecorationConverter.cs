using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Marbas.Utils
{
    [ValueConversion(typeof(object), typeof(TextDecorationCollection))]
    public sealed class TextDecorationConverter : IValueConverter
    {
        public TextDecorationCollection TrueTextDecoration { get; set; } = TextDecorations.Strikethrough;
        public TextDecorationCollection FalseTextDecoration { get; set; } = TextDecorations.Baseline;

        [SuppressMessage("ReSharper", "InvokeAsExtensionMethod")]
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var reverse = false;

            if (parameter is string s && Enum.TryParse<Parameter>(s, out var param))
            {
                if (param == Parameter.Inverted)
                {
                    reverse = true;
                }
            }

            if (reverse)
            {
                return IsTruthy(value) ? FalseTextDecoration : TrueTextDecoration;
            }

            return IsTruthy(value) ? TrueTextDecoration : FalseTextDecoration;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;

        private enum Parameter
        {
            // ReSharper disable once UnusedMember.Local
            // ReSharper disable once UnusedMember.Global
            Normal, Inverted,
        }

        private static bool IsTruthy(object? @this) =>
            @this switch
            {
                bool b => b,
                null => false,
                sbyte i => i != 0,
                short i => i != 0,
                int i => i != 0,
                long i => i != 0,
                byte i => i != 0,
                ushort i => i != 0,
                uint i => i != 0,
                ulong i => i != 0,
                char c => c != '\0',
                string s => s.Length != 0,
#if NETCOREAPP
                float f => MathF.Abs(f) >= float.Epsilon,
#else
                float f => Math.Abs(f) >= float.Epsilon,
#endif
                double d => Math.Abs(d) >= double.Epsilon,
                decimal d => d != decimal.Zero,
                ICollection collection => collection.Count != 0,
                _ => true,
            };
    }
}
