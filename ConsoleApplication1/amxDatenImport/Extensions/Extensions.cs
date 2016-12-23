using System;
using System.Linq.Expressions;

namespace amxArtikelAnlageAssistent.Utility
{
    public static class ErrorChecks
    {
        /// <summary>
        /// Checks the Type T for Condition exp. If true, an InvalidValueExeption is returned or immidiatly thrown
        /// </summary>
        /// <typeparam name="T"> The T item</typeparam>
        /// <param name="source"> The Extended Item</param>
        /// <param name="exp">The Lamba Expression that defines the Boolean Operation</param>
        /// <param name="autothrow">Bool if an Exception should be thrown directly, if Boolean is violated</param>
        /// <returns> Null or Exception</returns> 
        public static InvalidValueException ConditionalErrorCheck<T>(this T source, Expression<Func<T, bool>> exp, bool autothrow = false)
        {
            if (!exp.Compile()(source))
            {
                return null;
            }

            var ex = new InvalidValueException("The Value " + source + " violated the predicate Function: " + exp.Body);
            if (autothrow)
            {
                throw ex;
            }
            return ex;
        }

        public static int ToInt(this int? value, int defaultIfNull = 0) => value == null
                                                                      ? defaultIfNull
                                                                      : Convert.ToInt32(value);

        public static short ToShort(this short? value, short defaultIfNull = 0) => value == null
                                                                                       ? defaultIfNull
                                                                                       : Convert.ToInt16(value);

        public static decimal ToDecimal(this decimal? value, decimal defaultIfNull = decimal.Zero) => value == null
                                                                                                          ? defaultIfNull
                                                                                                          : Convert
                                                                                                                .ToDecimal
                                                                                                                (value);

        public static bool ToBool(this bool? value, bool defaultIfNull = false) => value == null
                                                                                       ? defaultIfNull
                                                                                       : Convert.ToBoolean(value);

        public static double ToDouble(this double? value, double defaultIfNull = 0) => value == null
                                                                                         ? defaultIfNull
                                                                                         : Convert.ToDouble(value);
        public static string DefaultIfNull(this string value, string Default = "") => value ?? Default;

        public static DateTime ToDateTime(this DateTime? value, DateTime defaultIfNull = new DateTime())
    => value ?? defaultIfNull;

    }

    /// <summary>
    /// PlaceHolder Exception Object for Later Use
    /// </summary>
    public class InvalidValueException : Exception
    {
        public InvalidValueException()
        {
        }

        public InvalidValueException(string message)
        : base(message)
        {
        }

        public InvalidValueException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
