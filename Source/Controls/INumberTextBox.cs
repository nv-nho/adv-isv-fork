using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OMS.Controls
{
    /// <summary>
    /// Input number Textbox
    /// -------------------------
    /// Author : ISV-PHUONG
    /// Date   : 2014/07/24
    /// Ver    : 0.0.0.1
    /// -------------------------
    /// </summary>
    public class INumberTextBox : ITextBox
    {
        #region Constant
        /// <summary>
        /// 最大小数桁
        /// </summary>
        private const int MaximumDecimalDigit = 3;
        /// <summary>
        /// 最少小数桁
        /// </summary>
        private const int MinimumDecimalDigit = 0;
        /// <summary>
        /// 最大整数桁
        /// </summary>
        private const int MaximumIntegerDigit = 12;
        /// <summary>
        /// 最大値
        /// </summary>
        private const decimal ValueMaximum = (decimal)999999999999999.999;
        /// <summary>
        /// 最小値
        /// </summary>
        private const decimal ValueMinimum = (decimal)-999999999999999.999;
        #endregion

        #region Constructor
        public INumberTextBox()
            : base()
        {
            this.InitProperties();
            this.InitDefaultStyle();
        }

        #endregion

        #region Properties
        /// <summary>
        /// Cho phép nhập số âm
        /// </summary>
        public bool IsAllowNegative
        {
            get
            {
                return this.GetValueViewState<bool>("IsAllowNegative");
            }
            set
            {
                base.ViewState[ViewStateKey("IsAllowNegative")] = value;
            }
        }

        /// <summary>
        /// Giá trị nhỏ nhất được nhập
        /// </summary>
        public decimal? MinimumValue
        {
            get
            {
                return this.GetValueViewState<decimal?>("MinimumValue");
            }
            set
            {
                //nullの場合
                if (value == null)
                {
                    value = this.GetDecimalCut(ValueMinimum);
                }
                ////MaximumValueより大きい場合
                //if (value > MaximumValue)
                //{
                //    throw new ArgumentOutOfRangeException();
                //}
                ////小数桁がDecimalDigitより多い場合
                //if (this.GetDigit(value, false) > this.DecimalDigit)
                //{
                //    throw new ArgumentOutOfRangeException();
                //}
                ////整数値が5桁より多い場合
                //if (this.GetDigit(value, true) > MaximumIntegerDigit)
                //{
                //    throw new ArgumentOutOfRangeException();
                //}
                ////小数値が3桁より多い場合
                //if (this.GetDigit(value, false) > MaximumDecimalDigit)
                //{
                //    throw new ArgumentOutOfRangeException();
                //}

                base.ViewState[ViewStateKey("MinimumValue")] = value;
            }

        }

        /// <summary>
        /// Giá trị lớn nhất được nhập
        /// </summary>
        public decimal? MaximumValue
        {
            get
            {
                return this.GetValueViewState<decimal?>("MaximumValue");
            }
            set
            {
                //nullの場合
                if (value == null)
                {
                    value = GetDecimalCut(ValueMaximum);
                }
                ////MinimumValueより小さい場合
                //if (value < MinimumValue)
                //{
                //    throw new ArgumentOutOfRangeException();
                //}
                ////小数桁がDecimalDigitより多い場合
                //if (GetDigit(value, false) > DecimalDigit)
                //{
                //    throw new ArgumentOutOfRangeException();
                //}
                ////整数値が5桁より多い場合
                //if (GetDigit(value, true) > MaximumIntegerDigit)
                //{
                //    throw new ArgumentOutOfRangeException();
                //}
                ////小数値が3桁より多い場合
                //if (GetDigit(value, false) > MaximumDecimalDigit)
                //{
                //    throw new ArgumentOutOfRangeException();
                //}
                base.ViewState[ViewStateKey("MaximumValue")] = value;
            }
        }

        /// <summary>
        /// Phần giới hạn số lẻ
        /// </summary>
        public int DecimalDigit
        {
            get
            {
                return this.GetValueViewState<int>("DecimalDigit");
            }
            set
            {
                base.ViewState[ViewStateKey("DecimalDigit")] = value;
                this.Text = this.DispValue;
            }
        }

        private decimal? value_;
        /// <summary>
        /// Giá trị của NumberBox
        /// </summary>
        new public decimal? Value
        {
            get
            {               
                if (base.IsEmpty)
                {
                    this.value_ = null;
                    return null;
                }
                decimal val = 0m;
                if (decimal.TryParse(base.Text, out val))
                {
                    this.value_ = val;
                    return this.value_;
                }
                return null;
            }
            set
            {
                this.value_ = value;
                this.Text = this.DispValue;
            }
        }

        /// <summary>
        /// giá trị hiển thị
        /// </summary>
        private string DispValue
        {
            get
            {
                if (this.value_.HasValue)
                {
                    var format = string.Format("N{0}", this.DecimalDigit);
                    return this.value_.GetValueOrDefault().ToString(format);
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Css Class
        /// </summary>
        public override string CssClass
        {
            get
            {
                return base.CssClass;
            }
            set
            {
                base.CssClass = value;
                base.CssClass += " input-num disable-ime";
            }
        }

        #endregion

        #region Override
       
        protected override void Render(HtmlTextWriter writer)
        {            
            var minimumValue = string.Format("{0}", this.MinimumValue);
            var maximumValue = string.Format("{0}", this.MaximumValue);
            var decimalDigit = string.Format("{0}", this.DecimalDigit);
            var isAllowNegative = string.Format("{0}", this.IsAllowNegative);

            var maxInt = 0m;            
            if (this.MaximumValue.HasValue)
            {
                maxInt = Math.Truncate(this.MaximumValue.GetValueOrDefault());
            }

            var maxIntLength = maxInt.ToString().Length;
            if (maxIntLength % 3 == 0)
            {
                maxIntLength += (maxIntLength / 3) - 1;
            }
            else
            {
                maxIntLength += (maxIntLength / 3);
            }
            if (this.IsAllowNegative)
            {
                var indexOfNegative = this.Text.IndexOf("-");
                if (indexOfNegative != -1)
                {
                    this.Attributes.Add("class", " negative-num");
                }
                else
                {
                    this.Attributes.Remove("negative-num");
                }
                //maxIntLength += 1;
            }           
            
            this.Attributes.Add("minimum-value", minimumValue);
            this.Attributes.Add("maximum-value", maximumValue);
            this.Attributes.Add("decimal-digit", decimalDigit);
            this.Attributes.Add("allow-negative", isAllowNegative);
            this.Attributes.Add("value", this.DispValue);
            this.Attributes.Add("int-max-length", string.Format("{0}", maxIntLength));
            

            string strError = this.ApplyValidation(writer);
            if (string.IsNullOrEmpty(strError))
            {
                this.Attributes.Remove("validation-error");
            }
            else
            {
                this.Attributes.Add("validation-error", strError);
            }
            base.Render(writer);
        }
        
        #endregion

        #region Method
        /// <summary>
        /// Khởi tạo giá trị mặc định cho các property
        /// </summary>
        private void InitProperties()
        {
            this.IsAllowNegative = false;
            this.MinimumValue = 0;
            this.MaximumValue = 999999999999;
            this.DecimalDigit = 0;
            this.Value = null;
            this.AutoPostBack = false;
        }

        /// <summary>
        /// Khởi tạo giá trị css mặc định cho numbertextbox
        /// </summary>
        private void InitDefaultStyle()
        {
            this.Attributes.Add("class", "input-num disable-ime");
            this.Style.Add("text-align", "right");

        }

        /// <summary>
        /// Validation
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        private string ApplyValidation(HtmlTextWriter writer)
        {
            string errorMsg = string.Empty;

            var textVal = this.Text;

            //入力値の制限
            if (this.InputLimitTextValue(ref textVal))
            {
                //入力値の検証
                errorMsg = InputCheckTextValue(textVal);
            }
            return errorMsg;
        }

        /// <summary>
        /// 小数点切り捨て
        /// </summary>
        /// <param name="value">入力値</param>
        /// <returns>小数点が切り捨てられた値</returns>
        private decimal GetDecimalCut(decimal value)
        {
            decimal result = value;
            //入力値の小数桁
            int decimalDigit = this.GetDigit(value, false);
            if (decimalDigit > this.DecimalDigit)
            {
                //マイナスの場合、符号を削除する。
                if (value < 0)
                {
                    result = result * -1;
                }
                //入力可能な小数桁より多い場合
                //入力の小数桁を切り捨てる
                int integerDigit = this.GetDigit(result, true);
                string decimalCut = result.ToString();
                decimalCut = decimalCut.Substring(0, integerDigit + DecimalDigit + 1);
                if (!decimal.TryParse(decimalCut, out result))
                {
                    result = 0;
                }
                //マイナスの場合、符号を戻す。
                if (value < 0)
                {
                    result = result * -1;
                }
            }
            return result;
        }

        /// <summary>
        /// 小数点切り捨て
        /// </summary>
        /// <param name="value">入力値</param>
        /// <returns>小数点が切り捨てられた値</returns>
        private string GetDecimalCut(string value)
        {
            string result = value;
            //入力値の小数桁
            int decimalDigit = this.GetDigit(value, false);
            if (decimalDigit > this.DecimalDigit)
            {
                //入力可能な小数桁より多い場合
                //入力の小数桁を切り捨てる
                int integerDigit = this.GetDigit(value, true);
                result = value.Substring(0, integerDigit + this.DecimalDigit + 1);
            }
            else if ((value.IndexOf('.') == -1) &&
                     (value != "-0"))
            {
                //小数点がない場合
                //先頭"0"消去対応
                //例："09"→"9"にする
                result = decimal.Parse(value).ToString();
            }
            if (this.DecimalDigit > 0)
            {
                //入力可能な小数桁の書式
                //-0対応
                //"-"を削除して後で追加する
                bool minus = false;
                if (result.IndexOf('-') != -1)
                {
                    minus = true;
                    result = result.Replace("-", "");
                }
                var format = string.Format("N{0}", this.DecimalDigit);
                result = decimal.Parse(result).ToString(format);
                if (minus)
                {
                    result = "-" + result;
                }
            }
            else
            {
                result = result.TrimEnd('.');
            }
            return result;
        }

        /// <summary>
        /// 整数桁または小数桁を返す
        /// </summary>
        /// <param name="value">値</param>
        /// <param name="integer">true:整数桁 false:小数桁</param>
        /// <returns>桁数</returns>
        private int GetDigit(decimal? value, bool integer)
        {
            int integerDigit = 0;   //整数桁
            int decimalDigit = 0;   //小数桁
            if (value == null)
            {
                return 0;
            }
            //マイナスの場合、符号を削除する。
            if (value < 0)
            {
                value = value * -1;
            }

            int posion = value.ToString().IndexOf('.');

            if (posion == -1)
            {
                //整数のみ
                integerDigit = value.ToString().Length;
            }
            else
            {
                decimalDigit = value.ToString().TrimEnd('0').Substring(posion + 1).Length;
                integerDigit = value.ToString().Length - decimalDigit - 1;
            }

            if (integer)
            {
                return integerDigit;
            }
            else
            {
                return decimalDigit;
            }
        }

        /// <summary>
        /// 整数桁または小数桁を返す
        /// </summary>
        /// <param name="value">値</param>
        /// <param name="integer">true:整数桁 false:小数桁</param>
        /// <returns>桁数</returns>
        private int GetDigit(string value, bool integer)
        {
            int integerDigit = 0;   //整数桁
            int decimalDigit = 0;   //小数桁

            if (string.IsNullOrEmpty(value))
            {
                //空白の場合、0桁を返す
                return 0;
            }
            else
            {
                int posion = value.ToString().IndexOf('.');

                if (posion == -1)
                {
                    //整数のみ
                    integerDigit = value.ToString().Length;
                }
                else
                {
                    integerDigit = value.Length - value.ToString().Substring(posion).Length;
                    if (posion != value.Length)
                    {
                        //最終文字が小数点ではないとき
                        decimalDigit = value.Substring(posion + 1).Length;
                    }
                }

                if (integer)
                {
                    return integerDigit;
                }
                else
                {
                    return decimalDigit;
                }
            }
        }

        /// <summary>
        /// 入力値の制限
        /// </summary>
        /// <param name="value">入力値</param>
        private bool InputLimitTextValue(ref string value)
        {
            bool result = false;
            decimal valueDecimal = 0;
            if (string.IsNullOrEmpty(value))
            {
                //Emptyの場合そのまま返す
                result = true;
            }
            else if (value == "-")
            {
                //"-"の場合 Emptyで返す
                value = string.Empty;
                result = true;
            }
            else if (value == ".")
            {
                //"."の場合 Emptyで返す
                value = string.Empty;
                result = true;
            }
            else if (decimal.TryParse(value, out valueDecimal))
            {
                if (this.IsPointZero(value))
                {
                    //".000" ".00" ".0"の場合 Emptyで返す
                    value = string.Empty;
                    result = true;
                }
                else if ((ValueMinimum <= valueDecimal) &&
                         (ValueMaximum >= valueDecimal))
                {
                    //"-99999.999～99999.999"の場合、小数値をDecimalDigitの桁数で返す（切り捨て）
                    value = this.GetDecimalCut(value);
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// 入力値の検証
        /// </summary>
        /// <param name="value">入力値</param>
        private string InputCheckTextValue(string value)
        {
            string errorMsg = string.Empty;
            if (string.IsNullOrEmpty(value))
            {
                //Emptyの場合
                //値が最小値と最大値の範囲内の場合
                return null;
            }
            else if ((MinimumValue <= decimal.Parse(value)) &&
                     (decimal.Parse(value) <= MaximumValue))
            {
                //値が最小値と最大値の範囲内の場合
                return null;
            }
            else
            {
                errorMsg = MinimumValue + "～" + MaximumValue + "の範囲内で入力してください。";
                //値が最小値と最大値の範囲外の場合エラー
                return errorMsg;
            }
        }

        /// <summary>
        /// 整数値がなくて小数値が0の場合は true を返す
        /// </summary>
        /// <param name="value">入力値</param>
        /// <returns>整数値がなくて小数値が0の場合は true を返す</returns>
        private bool IsPointZero(string value)
        {
            bool result = true;
            value = value.Replace("-", "");
            int posion = value.IndexOf('.');
            //先頭が"."以外の場合
            if (posion != 0)
            {
                //整数値あり
                result = false;
            }
            else
            {
                if (int.Parse(value.Substring(posion + 1)) > 0)
                {
                    //小数値が"0"以外
                    result = false;
                }
            }
            return result;
        }
        
        #endregion
    }
}
