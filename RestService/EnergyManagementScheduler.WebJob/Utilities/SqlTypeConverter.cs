namespace EnergyManagementScheduler.WebJob.Utilities
{
    using System;

    public static class SqlTypeConverter
    {
        // Summary:
        //     Converts the value of the specified object to its equivalent string representation.
        //
        // Parameters:
        //   value:
        //     An object that supplies the value to convert, or null.
        //
        // Returns:
        //     The string representation of value, or System.String.Empty if value is an object
        //     whose value is null. If value is null, the method returns null.
        public static string ToString(object value)
        {
            return value == DBNull.Value ? default(string) : Convert.ToString(value);
        }

        // Summary:
        //     Converts the value of the specified object to a double-precision floating-point
        //     number.
        //
        // Parameters:
        //   value:
        //     An object that implements the System.IConvertible interface, or null.
        //
        // Returns:
        //     A double-precision floating-point number that is equivalent to value, or zero
        //     if value is null.
        //
        // Exceptions:
        //   T:System.FormatException:
        //     value is not in an appropriate format for a System.Double type.
        //
        //   T:System.InvalidCastException:
        //     value does not implement the System.IConvertible interface. -or-The conversion
        //     is not supported.
        //
        //   T:System.OverflowException:
        //     value represents a number that is less than System.Double.MinValue or greater
        //     than System.Double.MaxValue.
        public static double ToDouble(object value)
        {
            return value == DBNull.Value ? default(double) : Convert.ToDouble(value);
        }

        // Summary:
        //     Converts the value of the specified object to a System.DateTime object.
        //
        // Parameters:
        //   value:
        //     An object that implements the System.IConvertible interface, or null.
        //
        // Returns:
        //     The date and time equivalent of the value of value, or a date and time equivalent
        //     of System.DateTime.MinValue if value is null.
        //
        // Exceptions:
        //   T:System.FormatException:
        //     value is not a valid date and time value.
        //
        //   T:System.InvalidCastException:
        //     value does not implement the System.IConvertible interface. -or-The conversion
        //     is not supported.
        public static DateTime ToDateTime(object value)
        {
            return value == DBNull.Value ? default(DateTime) : Convert.ToDateTime(value);
        }

        // Summary:
        //     Converts the value of the specified object to a 32-bit signed integer.
        //
        // Parameters:
        //   value:
        //     An object that implements the System.IConvertible interface, or null.
        //
        // Returns:
        //     A 32-bit signed integer equivalent to value, or zero if value is null.
        //
        // Exceptions:
        //   T:System.FormatException:
        //     value is not in an appropriate format.
        //
        //   T:System.InvalidCastException:
        //     value does not implement the System.IConvertible interface. -or-The conversion
        //     is not supported.
        //
        //   T:System.OverflowException:
        //     value represents a number that is less than System.Int32.MinValue or greater
        //     than System.Int32.MaxValue.
        public static int ToInt32(object value)
        {
            return value == DBNull.Value ? default(int) : Convert.ToInt32(value);
        }
    }
}
