using System;

namespace OMS.Utilities
{
    /// <summary>
    /// Class Fraction
    /// Crete Date: 2014/08/18
    /// Create Author: ISV-HUNG
    /// </summary>
    public sealed class Fraction
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Fraction()
        {

        }
        #endregion

        #region Method

        public static double Round(FractionType type, double value, int digits)
        {
            double ret = 0;

            switch (type)
            {
                case FractionType.RoundDown:
                    ret = RoundDown(value, digits);

                    break;
                case FractionType.Round4Down5Up:
                    ret = Round4Down5Up(value, digits);

                    break;
                case FractionType.Round5Down6Up:
                    ret = Round5Down6Up(value, digits);

                    break;
                case FractionType.RoundUp:
                    ret = RoundUp(value, digits);

                    break;
                case FractionType.RoundBankers:
                    ret = RoundBankers(value, digits);
                    break;
                default:
                    ret = value;
                    break;
            }

            return ret;
        }

        public static double Round(FractionType type, double value)
        {
            return Round(type, value, 0);
        }

        public static decimal Round(FractionType type, decimal value, int digits)
        {
            decimal ret = decimal.Zero;

            switch (type)
            {
                case FractionType.RoundDown:
                    ret = RoundDown(value, digits);

                    break;
                case FractionType.Round4Down5Up:
                    ret = Round4Down5Up(value, digits);

                    break;
                case FractionType.Round5Down6Up:
                    ret = Round5Down6Up(value, digits);

                    break;
                case FractionType.RoundUp:
                    ret = RoundUp(value, digits);

                    break;
                case FractionType.RoundBankers:
                    ret = RoundBankers(value, digits);
                    break;
                default:
                    ret = value;
                    break;
            }

            return ret;
        }

        public static decimal Round(FractionType type, decimal value)
        {
            return Round(type, value, 0);
        }
        #endregion

        #region RoundDown Method
        static internal double RoundDown(double value, int digits)
        {
            double coef = Coefficient(digits);

            return Fix(value * coef) / coef;
        }

        static internal decimal RoundDown(decimal value, int digits)
        {
            decimal coef = new decimal(Coefficient(digits));

            return Fix(value * coef) / coef;
        }
        #endregion

        #region Round4Down5Up Method
        static internal double Round4Down5Up(double value, int digits)
        {
            //if (value >= 0)
            //{
            //    return RoundDown(value + 0.1 * Coefficient(-digits) * Convert.ToInt32(FractionType.Round4Down5Up), digits);
            //}
            //else
            //{
            //    return RoundDown(value - 0.1 * Coefficient(-digits) * Convert.ToInt32(FractionType.Round4Down5Up), digits);
            //}

            return Math.Round(value, digits, MidpointRounding.AwayFromZero);
        }

        static internal decimal Round4Down5Up(decimal value, int digits)
        {
            //if (value >= 0)
            //{
            //    value = decimal.Add(value, decimal.Multiply(decimal.Multiply(0.1m, Convert.ToDecimal(Coefficient(-digits))), Convert.ToDecimal(FractionType.Round4Down5Up)));
            //}
            //else
            //{
            //    value = decimal.Add(value, decimal.Multiply(decimal.Multiply(-0.1m, Convert.ToDecimal(Coefficient(-digits))), Convert.ToDecimal(FractionType.Round4Down5Up)));
            //}

            //return RoundDown(value, digits);
            return Math.Round(value, digits, MidpointRounding.AwayFromZero);
        }
        #endregion

        #region Round5Down6Up Method
        static internal double Round5Down6Up(double value, int digits)
        {
            if (value >= 0)
            {
                return RoundDown(value + 0.1 * Coefficient(-digits) * Convert.ToInt32(FractionType.Round5Down6Up), digits);
            }
            else
            {
                return RoundDown(value - 0.1 * Coefficient(-digits) * Convert.ToInt32(FractionType.Round5Down6Up), digits);
            }
        }

        static internal decimal Round5Down6Up(decimal value, int digits)
        {
            if (value >= 0)
            {
                value = decimal.Add(value, decimal.Multiply(decimal.Multiply(0.1m, Convert.ToDecimal(Coefficient(-digits))), Convert.ToDecimal(FractionType.Round5Down6Up)));
            }
            else
            {
                value = decimal.Add(value, decimal.Multiply(decimal.Multiply(-0.1m, Convert.ToDecimal(Coefficient(-digits))), Convert.ToDecimal(FractionType.Round5Down6Up)));
            }

            return RoundDown(value, digits);
        }
        #endregion

        #region RoundUp Method
        static internal double RoundUp(double value, int digits)
        {
            double coef = Coefficient(digits);

            return Convert.ToDouble(((value >= 0) ? Math.Ceiling(value * coef) / coef : Math.Floor(value * coef) / coef));
        }

        static internal decimal RoundUp(decimal value, int digits)
        {
            decimal coef = new decimal(Coefficient(digits));

            return Convert.ToDecimal(((value >= 0) ? decimal.Negate(decimal.Divide(decimal.Floor(decimal.Negate(decimal.Multiply(value, coef))), coef)) : decimal.Divide(decimal.Floor(decimal.Multiply(value, coef)), coef)));
        }
        #endregion

        #region RoundBankers Method
        static internal double RoundBankers(double value, int digits)
        {
            return Math.Round(value, digits);
        }

        static internal decimal RoundBankers(decimal value, int digits)
        {
            return Math.Round(value, digits);
        }
        #endregion

        #region Coefficient Method
        /// <summary>
        /// Coefficient value
        /// </summary>
        /// <param name="digits"></param>
        /// <returns></returns>
        private static double Coefficient(int digits)
        {
            try
            {
                return Math.Pow(10, digits);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Fix Method
        /// <summary>
        /// Fix value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static internal double Fix(double value)
        {
            return Convert.ToDouble(((value >= 0) ? Math.Floor(value) : -Math.Floor(-value)));
        }

        /// <summary>
        /// Fix value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static internal decimal Fix(decimal value)
        {
            return Convert.ToDecimal(((value >= decimal.Zero) ? decimal.Floor(value) : decimal.Negate(decimal.Floor(decimal.Negate(value)))));
        }
        #endregion
    }
}