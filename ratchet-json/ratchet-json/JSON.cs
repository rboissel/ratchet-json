/*                                                                           *
 * Copyright © 2019, Raphaël Boissel                                         *
 * Permission is hereby granted, free of charge, to any person obtaining     *
 * a copy of this software and associated documentation files, to deal in    *
 * the Software without restriction, including without limitation the        *
 * rights to use, copy, modify, merge, publish, distribute, sublicense,      *
 * and/or sell copies of the Software, and to permit persons to whom the     *
 * Software is furnished to do so, subject to the following conditions:      *
 *                                                                           *
 * - The above copyright notice and this permission notice shall be          *
 *   included in all copies or substantial portions of the Software.         *
 * - The Software is provided "as is", without warranty of any kind,         *
 *   express or implied, including but not limited to the warranties of      *
 *   merchantability, fitness for a particular purpose and noninfringement.  *
 *   In no event shall the authors or copyright holders. be liable for any   *
 *   claim, damages or other liability, whether in an action of contract,    *
 *   tort or otherwise, arising from, out of or in connection with the       *
 *   software or the use or other dealings in the Software.                  *
 * - Except as contained in this notice, the name of Raphaël Boissel shall   *
 *   not be used in advertising or otherwise to promote the sale, use or     *
 *   other dealings in this Software without prior written authorization     *
 *   from Raphaël Boissel.                                                   *
 *                                                                           */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratchet.IO.Format
{
    public class JSON
    {
        abstract public class Value
        {
            public static implicit operator Value(string value) { return (String)value; }
            public static implicit operator Value(int[] array) { return (Array)array; }
            public static implicit operator Value(int value) { return (Number)value; }
            public static implicit operator Value(bool value) { return (Boolean)value; }
        }

        public class Array : Value
        {
            List<Value> _Values = new List<Value>();
            public List<Value> Values { get { return _Values; } set { _Values = value; } }

            public static implicit operator Array(int[] Value)
            {
                Array a = new Array();
                for (int n = 0; n < Value.Length; n++) { a._Values.Add(Value[n]); }
                return a;
            }
            public override string ToString()
            {
                StringBuilder builder = new StringBuilder();
                bool first = true;
                builder.Append('[');
                foreach (Value value in _Values)
                {
                    if (!first)
                    { builder.Append(','); }
                    else
                    { first = false; }
                    builder.Append(value.ToString());
                }
                builder.Append(']');
                return builder.ToString();
            }
        }
        public class String : Value
        {
            string _Value = "";
            public string Value { get { return _Value; } set { _Value = value; } }
            public override string ToString()
            {
                StringBuilder builder = new StringBuilder();
                builder.Append('"');
                builder.Append(_Value);
                builder.Append('"');
                return builder.ToString();
            }

            public static implicit operator String(string Value)
            {
                String str = new String();
                str.Value = Value;
                return str;
            }
        }
        public class Number : Value
        {
            string _Value = "0";

            bool IsValidNumber(string Number)
            {
                return true;
            }
       
            long Pow10Long(long value)
            {
                long ret = 1;
                while (value > 0) { ret *= 10; value--; }
                return ret;
            }

            long ParseAsLong()
            {
                long value = 0;
                long exponent = 0;
                long decimalPart = 0;
                long decimalPartExponent = 0;
                int position = 0;
                bool negative = false;
                bool negativeExp = false;

                if (_Value[0] == '-') { negative = true; position = 1; }
                for (; position < _Value.Length; position++)
                {
                    if (!char.IsDigit(_Value[position]))
                    {
                        if (_Value[position] == '.')
                        {
                            position++;
                            for (; position < _Value.Length; position++)
                            {
                                if (!char.IsDigit(_Value[position])) { break; }
                                decimalPart *= 10;
                                decimalPart += ((int)(_Value[position] - '0'));
                                decimalPartExponent++;
                            }
                        }

                        if (position < _Value.Length && (_Value[position] == 'e' || _Value[position] == 'E'))
                        {
                            position++;
                            if (position < _Value.Length && _Value[position] == '-') { negativeExp = true; position++; }

                            for (; position < _Value.Length; position++)
                            {
                                if (!char.IsDigit(_Value[position])) { break; }
                                exponent *= 10;
                                exponent += ((int)(_Value[position] - '0'));
                            }
                        }
                        break;
                    }

                    value *= 10;
                    value += ((int)(_Value[position] - '0'));
                }

                if (negative) { value = -value; }
                if (exponent > 0 & decimalPartExponent > 0 && !negativeExp)
                {
                    long decimalExp = decimalPartExponent - exponent;
                    if (decimalExp > 0) { decimalPart = decimalPart / Pow10Long(decimalExp); }
                    else { decimalPart = decimalPart * Pow10Long(decimalExp); }
                    if (negative) { return value * Pow10Long(exponent) - decimalPart; }
                    return value * Pow10Long(exponent) + decimalPart;
                }
                if (negativeExp) { return value / Pow10Long(exponent); } else { return value * Pow10Long(exponent); }
            }


            double ParseAsDouble()
            {
                double value = 0;
                long exponent = 0;
                double decimalPart = 0;
                long decimalPartExponent = 0;
                int position = 0;
                bool negative = false;
                bool negativeExp = false;

                if (_Value[0] == '-') { negative = true; position = 1; }
                for (; position < _Value.Length; position++)
                {
                    if (!char.IsDigit(_Value[position]))
                    {
                        if (_Value[position] == '.')
                        {
                            position++;
                            for (; position < _Value.Length; position++)
                            {
                                if (!char.IsDigit(_Value[position])) { break; }
                                decimalPart *= 10;
                                decimalPart += ((int)(_Value[position] - '0'));
                                decimalPartExponent++;
                            }
                        }

                        if (position < _Value.Length && (_Value[position] == 'e' || _Value[position] == 'E'))
                        {
                            position++;
                            if (position < _Value.Length && _Value[position] == '-') { negativeExp = true; position++; }

                            for (; position < _Value.Length; position++)
                            {
                                if (!char.IsDigit(_Value[position])) { break; }
                                exponent *= 10;
                                exponent += ((int)(_Value[position] - '0'));
                            }
                        }
                        
                        break;
                    }

                    value *= 10;
                    value += ((int)(_Value[position] - '0'));
                }

                if (negative)
                {
                    if (negativeExp) { return (-value - decimalPart * System.Math.Pow(10.0, -decimalPartExponent)) * System.Math.Pow(10.0, -exponent); }
                    else { return (-value - decimalPart * System.Math.Pow(10.0, -decimalPartExponent)) * System.Math.Pow(10.0, exponent); }

                }
                else
                {
                    if (negativeExp) { return (value + decimalPart * System.Math.Pow(10.0, -decimalPartExponent)) * System.Math.Pow(10.0, -exponent); }
                    else { return (value + decimalPart * System.Math.Pow(10.0, -decimalPartExponent)) * System.Math.Pow(10.0, exponent); }
                }
            }

            internal static Number __TrustedCreateValue(string Value)
            {
                Number n = new Number();
                if (Value == "") { n._Value = "0"; }
                n._Value = Value;
                return n;
            }
            public string Value { get { return _Value; } set { if (!IsValidNumber(_Value)) { throw new Exception("Number is not valid"); } _Value = value; } }
            public static implicit operator Number(int Value) { Number n = new Number(); n._Value = Value.ToString(System.Globalization.CultureInfo.InvariantCulture); return n; }
            public static implicit operator Number(Decimal Value) { Number n = new Number(); n._Value = Value.ToString(System.Globalization.CultureInfo.InvariantCulture); return n; }
            public static implicit operator long(Number Number) { return Number.ParseAsLong(); }
            public static implicit operator int(Number Number) { return (int)Number.ParseAsLong(); }
            public static implicit operator double(Number Number) { return Number.ParseAsDouble(); }
            public static implicit operator float(Number Number) { return (float)Number.ParseAsDouble(); }

        }
        public class Boolean : Value
        {
            bool _State = false;
            public bool State { get { return _State; } set { _State = value; } }
            public static implicit operator Boolean(bool value) { Boolean b = new Boolean(); b.State = value; return b; }
            public override string ToString()
            {
                return _State ? "true" : "false";
            }
        }
        public class Null : Value
        {
            public override string ToString()
            {
                return "null";
            }
        }
        public class Object : Value
        {
            Dictionary<string, Value> _Properties = new Dictionary<string, Value>();

            public Value this[string Key] { get { return _Properties[Key]; } set { _Properties[Key] = value; } }

            public override string ToString()
            {
                bool first = true;
                StringBuilder Builder = new StringBuilder();
                Builder.Append('{');
                foreach (string property in _Properties.Keys)
                {
                    if (!first)
                    { Builder.Append(','); }
                    else
                    { first = false; }
                    Builder.Append("\"");
                    Builder.Append(property);
                    Builder.Append("\":");
                    Builder.Append(_Properties[property]);
                }
                Builder.Append('}');
                return Builder.ToString();
            }
        }

        public static Value Parse(string Data)
        {
            int Position = 0;
            return _ParseValue(Data, ref Position);
        }

        private static Array _ParseArray(string Data, ref int Position)
        {
            if (Position >= Data.Length || Position < 0) { return null; }
            for (; Position < Data.Length && char.IsWhiteSpace(Data[Position]); Position++) ;
            if (Data[Position] == '[')
            {
                Array array = new Array();

                Position++;
                if (Data[Position] == ']') { Position++; return array; }
                for (; Position < Data.Length; Position++)
                {

                    Value value = _ParseValue(Data, ref Position);
                    array.Values.Add(value);
                    for (; Position < Data.Length && char.IsWhiteSpace(Data[Position]); Position++) ;
                    if (Position >= Data.Length || Position < 0) { return array; }

                    if (Data[Position] == ',') { continue; }
                    else if (Data[Position] == ']')
                    {
                        Position++;
                        return array;
                    }
                    else { return array; }
                }
            }
            return null;
        }

        private static Null ParseNull(string Data, ref int Position)
        {
            if (Position >= Data.Length || Position < 0) { return null; }
            for (; Position < Data.Length && char.IsWhiteSpace(Data[Position]); Position++) ;
            if (Position >= Data.Length || Position < 0) { return null; }
            if (Data[Position] == 'n' || Data[Position] == 'N') { Position++; } else { return null; }
            if (Position >= Data.Length || Position < 0) { return new Null(); }
            if (Data[Position] == 'u' || Data[Position] == 'U') { Position++; } else { return new Null(); }
            if (Position >= Data.Length || Position < 0) { return new Null(); }
            if (Data[Position] == 'l' || Data[Position] == 'L') { Position++; } else { return new Null(); }
            if (Position >= Data.Length || Position < 0) { return new Null(); }
            if (Data[Position] == 'l' || Data[Position] == 'L') { Position++; } else { return new Null(); }
            return new Null();
        }

        private static Value _ParseValue(string Data, ref int Position)
        {
            if (Position >= Data.Length || Position < 0) { return null; }
            for (; Position < Data.Length && char.IsWhiteSpace(Data[Position]); Position++) ;
            if (Data[Position] == '{') { return _ParseObject(Data, ref Position); }
            if (Data[Position] == '[') { return _ParseArray(Data, ref Position); }
            if (Data[Position] == '"' || Data[Position] == '\'' /* Permissive */ ) { return _ParseString(Data, ref Position); }
            if (Data[Position] == 'n') { return ParseNull(Data, ref Position); }
            if (Data[Position] == '-') { return _ParseNumber(Data, ref Position); }
            if (char.IsDigit(Data[Position])) { return _ParseNumber(Data, ref Position); }
            if (Data[Position] == 't' || Data[Position] == 'f') { return _ParseBoolean(Data, ref Position); }

            return new Null();
        }
        private static Boolean _ParseBoolean(string Data, ref int Position)
        {
            bool result = false;
            for (; Position < Data.Length && char.IsWhiteSpace(Data[Position]); Position++) ;
            if (Position >= Data.Length) { return false; }
            if (Data[Position] == 't') { result = true; }
            for (; Position < Data.Length && !char.IsWhiteSpace(Data[Position]) && Data[Position] != ','; Position++) ;
            return result;
        }
        private static Number _ParseNumber(string Data, ref int Position)
        {
            StringBuilder builder = new StringBuilder();
            for (; Position < Data.Length && char.IsWhiteSpace(Data[Position]); Position++) ;
            if (Position < Data.Length && Data[Position] == '-') { builder.Append('-'); Position++; }

            for (; Position < Data.Length; Position++)
            {
                if (!char.IsDigit(Data[Position])) { break; }
                builder.Append(Data[Position]);
            }

            if (Position < Data.Length && Data[Position] == '.')
            {
                builder.Append('.'); Position++;
                for (; Position < Data.Length; Position++)
                {
                    if (!char.IsDigit(Data[Position])) { break; }
                    builder.Append(Data[Position]);
                }
            }

            if (Position < Data.Length && (Data[Position] == 'e' || Data[Position] == 'E'))
            {
                builder.Append('E'); Position++;
                if (Position < Data.Length && Data[Position] == '-') { builder.Append('-'); Position++; }

                for (; Position < Data.Length; Position++)
                {
                    if (!char.IsDigit(Data[Position])) { break; }
                    builder.Append(Data[Position]);
                }
            }

            return Number.__TrustedCreateValue(builder.ToString());
        }
        private static Object _ParseObject(string Data, ref int Position)
        {
            Object obj = new Object();
            if (Data[Position] == '{') { Position++; }
            if (Position >= Data.Length || Position < 0) { return new Object(); }
            while (Position < Data.Length)
            {
                for (; Position < Data.Length && char.IsWhiteSpace(Data[Position]); Position++) ;
                if (Position >= Data.Length) { break; }
                if (Data[Position] != '"')
                {
                    if (Data[Position] == '}') { Position++; }
                    return obj;
                }
                String key = _ParseString(Data, ref Position);
                for (; Position < Data.Length && char.IsWhiteSpace(Data[Position]); Position++) ;
                if (Position >= Data.Length) { break; }

                if (Data[Position] == '=' || Data[Position] == ':') { Position++; }
                for (; Position < Data.Length && char.IsWhiteSpace(Data[Position]); Position++) ;
                if (Position >= Data.Length) { break; }
                Value value = _ParseValue(Data, ref Position);
                obj[key.Value] = value;
                for (; Position < Data.Length && char.IsWhiteSpace(Data[Position]); Position++) ;
                if (Position >= Data.Length) { break; }
                if (Data[Position] == ',') { Position++; }

            }
            return obj;
        }

        private static String _ParseString(string Data, ref int Position)
        {
            try
            {
                if (Position >= Data.Length || Position < 0) { return null; }
                if (Data[Position] != '"') { return null; }
                Position++;
                bool escape = false;
                StringBuilder builder = new StringBuilder();
                for (;
                    Position < Data.Length &&
                    (Data[Position] != '"' || escape);
                    Position++)
                {
                    if (escape)
                    {
                        switch (Data[Position])
                        {
                            case 'n': builder.Append('\n'); break;
                            case 'r': builder.Append('\r'); break;
                            case 't': builder.Append('\t'); break;
                            case '"': builder.Append('\"'); break;
                            case '\\': builder.Append('\\'); break;
                            case 'b': builder.Append('\b'); break;
                            default: builder.Append('\\'); builder.Append(Data[Position]); break;
                        }

                        escape = false;
                    }
                    else
                    {
                        if (Data[Position] == '\\') { escape = true; }
                        else { builder.Append(Data[Position]); }
                    }
                }

                Position++;
                String result = new String();
                result.Value = builder.ToString();
                return result;
            }
            catch
            {
                throw new Exception("JSON Parse error");
            }
        }
    }
}
