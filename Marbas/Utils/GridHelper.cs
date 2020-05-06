using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Marbas.Utils
{
    /// <summary>
    /// Utility class that allows to set Grid layouts with simple(r) attached properties directly on the Grid XML element instead of using the child elements Grid.RowDefinitions and Grid.ColumnDefinitions.
    /// Format:
    ///		- comma separated list of rows/columns:             GridHelper.Rows="ROW1,ROW2,ROW3,..."
    ///		- possible values: "*", "Auto" or number of pixels: GridHelper.Rows="*,Auto,200"
    ///     - options can be provided in parenthesis:           GridHelper.Rows="ROW1(OPTIONS1),ROW2(OPTIONS2),ROW3(OPTIONS3),..."
    ///     - possible options: min/max limits:                 GridHelper.Rows="*(Min=100,Max=200),Auto(Min=100),200"
    /// Samples:
    ///		View:GridHelper.Rows="Auto,*,200"
    ///		View:GridHelper.Columns="Auto(max=500),*(min=300),100"
    /// </summary>
    public static class GridHelper
    {
        #region Rows

        public static string GetRows(DependencyObject obj) => (string)obj.GetValue(RowsProperty);
        public static void SetRows(DependencyObject obj, string value) => obj.SetValue(RowsProperty, value);

        public static DependencyProperty RowsProperty = DependencyProperty.RegisterAttached(
            "Rows",
            typeof(string),
            typeof(GridHelper),
            new PropertyMetadata("", OnRowsPropertyChanged));

        private static void OnRowsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is Grid grid)) return;

            var rows = ParseXmlAttribute(e.NewValue as string);
            if (rows == null || rows.Length == 0) return;

            grid.RowDefinitions.Clear();
            foreach (var row in rows)
            {
                RowDefinition newRowDefinition;

                switch (row.Type)
                {
                    case RowColumnType.Auto:
                        newRowDefinition = new RowDefinition { Height = GridLength.Auto };
                        break;
                    case RowColumnType.Exact:
                        {
                            if (row.ExactValue is double val)
                            {
                                newRowDefinition = new RowDefinition { Height = new GridLength(val) };
                            }
                            else
                            {
                                newRowDefinition = new RowDefinition();
                            }
                        }
                        break;
                    case RowColumnType.Star:
                        {
                            if (row.ExactValue is double val)
                            {
                                newRowDefinition = new RowDefinition { Height = new GridLength(val, GridUnitType.Star) };
                            }
                            else
                            {
                                newRowDefinition = new RowDefinition();
                            }
                        }
                        break;
                    default:
                        newRowDefinition = new RowDefinition();
                        break;
                }

                //var newRowDefinition = row.Type switch
                //{
                //    RowColumnType.Auto => new RowDefinition { Height = GridLength.Auto },
                //    RowColumnType.Exact when row.ExactValue is double val => new RowDefinition { Height = new GridLength(val) },
                //    RowColumnType.Star when row.ExactValue is double val => new RowDefinition { Height = new GridLength(val, GridUnitType.Star) },
                //    RowColumnType.Star => new RowDefinition(),
                //    _ => new RowDefinition()
                //};

                if (row.MinValue is double min)
                {
                    newRowDefinition.MinHeight = min;
                }

                if (row.MaxValue is double max)
                {
                    newRowDefinition.MinHeight = max;
                }

                grid.RowDefinitions.Add(newRowDefinition);
            }
        }

        #endregion Rows

        #region Columns

        public static string GetColumns(DependencyObject obj) => (string)obj.GetValue(ColumnsProperty);

        public static void SetColumns(DependencyObject obj, string value) => obj.SetValue(ColumnsProperty, value);

        public static DependencyProperty ColumnsProperty = DependencyProperty.RegisterAttached(
            "Columns",
            typeof(string),
            typeof(GridHelper),
            new PropertyMetadata("", OnColumnsPropertyChanged));

        private static void OnColumnsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is Grid grid)) return;

            var columns = ParseXmlAttribute(e.NewValue as string);
            if (columns == null || columns.Length == 0) return;

            grid.ColumnDefinitions.Clear();
            foreach (var column in columns)
            {
                ColumnDefinition newColumnDefinition;

                switch (column.Type)
                {
                    case RowColumnType.Auto:
                        newColumnDefinition = new ColumnDefinition { Width = GridLength.Auto };
                        break;
                    case RowColumnType.Exact:
                        {
                            if (column.ExactValue is double val)
                            {
                                newColumnDefinition = new ColumnDefinition { Width = new GridLength(val) };
                            }
                            else
                            {
                                newColumnDefinition = new ColumnDefinition();
                            }
                        }
                        break;
                    case RowColumnType.Star:
                        {
                            if (column.ExactValue is double val)
                            {
                                newColumnDefinition = new ColumnDefinition { Width = new GridLength(val, GridUnitType.Star) };
                            }
                            else
                            {
                                newColumnDefinition = new ColumnDefinition();
                            }
                        }
                        break;
                    default:
                        newColumnDefinition = new ColumnDefinition();
                        break;
                }

                //var newColumnDefinition = column.Type switch
                //{
                //    RowColumnType.Auto => new ColumnDefinition { Width = GridLength.Auto },
                //    RowColumnType.Exact when column.ExactValue is double val => new ColumnDefinition { Width = new GridLength(val) },
                //    RowColumnType.Star when column.ExactValue is double val => new ColumnDefinition { Width = new GridLength(val, GridUnitType.Star) },
                //    RowColumnType.Star => new ColumnDefinition(),
                //    _ => new ColumnDefinition()
                //};

                if (column.MinValue is double min)
                {
                    newColumnDefinition.MinWidth = min;
                }

                if (column.MaxValue is double max)
                {
                    newColumnDefinition.MinWidth = max;
                }

                grid.ColumnDefinitions.Add(newColumnDefinition);
            }
        }

        #endregion Columns

        #region Utils

        private enum RowColumnType
        {
            Star,
            Auto,
            Exact
        }

        private class RowColumnDefinition
        {
            public RowColumnType Type { get; set; }
            public double? ExactValue { get; set; }
            public double? MinValue { get; set; }
            public double? MaxValue { get; set; }
        }

        private static RowColumnDefinition[] ParseXmlAttribute(string xmlAttribute)
        {
            if (string.IsNullOrEmpty(xmlAttribute)) return null;

            var result = new List<RowColumnDefinition>();

            var allSubValues = xmlAttribute.Split(',');
            foreach (var subValue in allSubValues)
            {
                var subValueCopy = subValue;

                // has options?
                string options = null;
                var optionsBegin = subValueCopy.IndexOf('(');

                if (optionsBegin != -1)
                {
                    var optionsEnd = subValueCopy.IndexOf(')');

                    if (optionsEnd != -1 && optionsBegin < optionsEnd)
                    {
                        options = subValueCopy.Substring(optionsBegin + 1, optionsEnd - optionsBegin - 1);
                        subValueCopy = subValueCopy.Substring(0, optionsBegin);
                    }
                }

                // parse value
                RowColumnDefinition rowColumnDefinition = null;
                if (subValueCopy == "*")
                {
                    rowColumnDefinition = new RowColumnDefinition { Type = RowColumnType.Star };
                }
                else if (string.Equals(subValueCopy, "auto", StringComparison.OrdinalIgnoreCase))
                {
                    rowColumnDefinition = new RowColumnDefinition { Type = RowColumnType.Auto };
                }
                else
                {
                    if (double.TryParse(subValueCopy, out var exactValue))
                    {
                        rowColumnDefinition = new RowColumnDefinition { Type = RowColumnType.Exact, ExactValue = exactValue };
                    }
                    else
                    {
                        if (subValueCopy.EndsWith("*"))
                        {
                            var starNumberStr = subValueCopy.Substring(0, subValueCopy.Length - 1);

                            if (double.TryParse(starNumberStr, out var starNumber))
                            {
                                rowColumnDefinition = new RowColumnDefinition { Type = RowColumnType.Star, ExactValue = starNumber };
                            }
                        }
                    }
                }
                if (rowColumnDefinition == null)
                {
                    continue;
                }

                // parse options
                if (options != null)
                {
                    var allOptions = options.Split(',');

                    foreach (var option in allOptions)
                    {
                        var keyValuePair = option.Split('=');
                        if (keyValuePair.Length != 2) continue;

                        if (string.Compare(keyValuePair[0], "min", StringComparison.OrdinalIgnoreCase) == 0 && double.TryParse(keyValuePair[1], out var v))
                        {
                            rowColumnDefinition.MinValue = v;
                        }
                        else if (string.Compare(keyValuePair[0], "max", StringComparison.OrdinalIgnoreCase) == 0 && double.TryParse(keyValuePair[1], out v))
                        {
                            rowColumnDefinition.MaxValue = v;
                        }
                    }
                }

                result.Add(rowColumnDefinition);
            }

            return result.ToArray();
        }

        #endregion Utils
    }
}
