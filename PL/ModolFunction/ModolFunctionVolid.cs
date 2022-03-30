using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BO;
using System.Windows.Shapes;
using System.Globalization;
using System.Collections.ObjectModel;
using BlApi;


namespace PL
{

    /// <summary>
    /// cheak if text box not empty
    /// </summary>
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            if (string.IsNullOrWhiteSpace((value ?? "").ToString()) || ((value ?? "0").ToString() == "0"))
                return new ValidationResult(false, "Field is required.");
            return ValidationResult.ValidResult;
        }
    }
    /// <summary>
    /// chake the uint not start with -
    /// </summary>
    public class MinusValidationRule : ValidationRule
    {

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            if ((value ?? "").ToString().StartsWith('-'))
                return new ValidationResult(false, "Input is Error!");
            return ValidationResult.ValidResult;
        }
    }
    /// <summary>
    /// check the combo box not empty
    /// </summary>
    public class NotEmptyByItemValidationRule : ValidationRule
    {



        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            if (value is null)
                return new ValidationResult(false, "Field is required.");
            return ValidationResult.ValidResult;
        }
    }

    /// <summary>
    /// convert visibluty to boolean 
    /// </summary>
    public class VisibilityToBooleanConverter : IValueConverter

    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility && (Visibility)value == Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }
    }
    /// <summary>
    /// convert the longitude the E or W
    /// </summary>
    public class LongPointEorWConverter : IValueConverter

    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value ?? "").ToString().StartsWith("-"))
                return 'W';
            else
                return 'E';
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
    /// <summary>
    /// convert the lutitude to s or N
    /// </summary>
    public class LutitudePointSorNConverter : IValueConverter

    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value ?? "").ToString().StartsWith("-"))
                return 'S';
            else
                return 'N';
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
    /// <summary>
    /// convert point to degree
    /// </summary>
    public class DegrreConverter : IValueConverter

    {
        double degree;
        public string Degree(double point)
        {
            point = (point < 0) ? point * (-1) : point;
            uint d = (uint)point;
            uint m = (uint)((point - d) * 60);
            double mph = (double)((double)m / 60);
            double s = (point - d - mph) * 3600;
            return $"{d}\x00B0 {m}' {s:0.0000}\"";
        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {


            Double.TryParse((value ?? "").ToString(), out degree);
            return Degree(degree);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {


            Double.TryParse((value ?? "").ToString(), out degree);
            return degree;
        }
    }
    /// <summary>
    /// check the input for point 
    /// </summary>
    public class InputERRORWithPointValidationRule : ValidationRule
    {

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string checking = (value ?? "").ToString();

            if (!convetDegreeChack(checking))
            {
              
                if (checking.ToString().Count(x => x == '.') > 1)
                    return new ValidationResult(false, "Cant by more the one point!");
                if (checking.ToString().StartsWith('.'))
                    return new ValidationResult(false, "You cant start with point!");
                if (checking.ToString().Any(x => x == '-') )
                    if( !(value ?? "").ToString().StartsWith('-'))
                    return new ValidationResult(false, "only a digit number!");
                if (checking.EndsWith('.'))
                    return new ValidationResult(false, "You cant end with point!");
                if (checking.Any(x => (x < '0' || x > '9') && x != '.'&&x!='-'))
                    return new ValidationResult(false, "only a digit number!");
            }
            return ValidationResult.ValidResult;
        }
        /// <summary>
        /// check if the input is degree
        /// </summary>
        /// <param name="pointDegree">the point in degree</param>
        /// <returns></returns>
        private bool convetDegreeChack(string pointDegree)
        {
            var b = pointDegree.SkipWhile(x => (x >= '0' && x <= '9'));
            if (b.FirstOrDefault() != '\x00B0')
                return false;
            b = b.Skip(2);
            b = b.SkipWhile(x => (x >= '0' && x <= '9'));
            if (b.FirstOrDefault() != '\'')
                return false;
            b = b.Skip(2);
            b = b.SkipWhile(x => (x >= '0' && x <='9') || x=='.');
            if (b.FirstOrDefault() != '\"')
                return false;
            return true;
        }
    }
    /// <summary>
    /// check the int number input
    /// </summary>
    public class InputERRORValidationRule : ValidationRule
    {

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            if ((value ?? "").ToString().Any(x => x < '0' || x > '9'))
                return new ValidationResult(false, "Input ERROR!");
            return ValidationResult.ValidResult;
        }
    }

}
