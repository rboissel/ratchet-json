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
            Decimal _Value = 0;
            public Decimal Value { get { return _Value; } set { _Value = value; } }
            public static implicit operator Number(int Value) { Number n = new Number(); n._Value = Value; return n; }
            public static implicit operator int(Number Number) { return (int)Number._Value; }
            public override string ToString()
            {
                string result = _Value.ToString().Replace(",", ".");
                if (result.EndsWith(".0")) { return result.Replace(".0", ""); }
                return result;
            }
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
            if (Data[Position] == '{')
            {
                Position++;
                if (Position >= Data.Length || Position < 0) { return null; }
                StringBuilder chunk = new StringBuilder();
                int count = 1;
                for (; Position < Data.Length; Position++)
                {
                    if (Data[Position] == '{') { count++; }
                    if (Data[Position] == '}') { count--; if (count == 0) { break; } }
                    chunk.Append(Data[Position]);
                }
                if (Position >= Data.Length || Position < 0) { return null; }
                if (Data[Position] == '}') { Position++; }
                int SubPosition = 0;
                return _ParseObject(chunk.ToString(), ref SubPosition);
            }
            if (Data[Position] == '[') { return _ParseArray(Data, ref Position); }
            if (Data[Position] == '"' || Data[Position] == '\'' /* Permissive */ ) { return _ParseString(Data, ref Position); }
            if (Data[Position] == 'n') { return ParseNull(Data, ref Position); }
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
            for (; Position < Data.Length && char.IsWhiteSpace(Data[Position]); Position++) ;
            int value = 0;
            for (; Position < Data.Length; Position++)
            {
                if (!char.IsDigit(Data[Position])) { return value; }
                value *= 10;
                value += (int)(Data[Position] - '0');
            }
            return value;
        }
        private static Object _ParseObject(string Data, ref int Position)
        {
            Object obj = new Object();
            if (Position >= Data.Length || Position < 0) { return new Object(); }
            while (Position < Data.Length)
            {
                for (; Position < Data.Length && char.IsWhiteSpace(Data[Position]); Position++) ;
                if (Position >= Data.Length) { break; }
                if (Data[Position] != '"')
                {
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
