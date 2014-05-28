#region User/License
// Copyright (c) 2005-2013 tfwroble
// 
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion
#if NET35 || NET40
/*
 * User: oIo
 * Date: 11/15/2010 – 2:33 AM
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Xml.Serialization;

namespace System
{
	/// <summary>
	/// This FieldExtention class is generally designed to aid in parsing Field-Template strings.
	/// </summary>
	static public class FieldStringExtension
	{
		/// <summary>
		/// it seems that this extention method is not used.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		static public string SOr(this string input)
		{
			return string.Format("{0}|",input);
		}

		#region FormatList
		/// <summary>
		/// 
		/// </summary>
		/// <param name="input"></param>
		/// <param name="format"></param>
		/// <param name="fieldFormat"></param>
		/// <param name="values"></param>
		/// <returns></returns>
		static public string FormatList(this string input, string format, string fieldFormat, params object[] values)
		{
			return input.FormatList(false,format,fieldFormat,values);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="input"></param>
		/// <param name="addLineSep"></param>
		/// <param name="format"></param>
		/// <param name="fieldFormat"></param>
		/// <param name="values"></param>
		/// <returns></returns>
		static public string FormatList(this string input, bool addLineSep, string format, string fieldFormat, params object[] values)
		{
			var list = new string[values.Length];
			for (int i=0; i < values.Length; i++) list[i] = string.Format(fieldFormat,values[i]);
			string output = string.Format(format,string.Join(addLineSep ? "\r\n" : string.Empty, list));
			return output;
		}
		#endregion
		#region Capitolize
		/// <summary>
		/// 
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		static public string CapitolN(this string input) { return string.Concat(input,"C"); }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		static public string ToStringCapitolize(this object input)
		{
			string output = string.Format("{0}",input);
			try {
				output = string.Concat(char.ToUpper(output[0]).ToString(),output.Substring(1));
			} catch {
			}
			return output;
		}

		#endregion
		#region CamelClean
		/// <summary>
		/// removes dashes and converts the string to CamelCase
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		static public string CamelCleanDash(this string input)
		{
			if (string.IsNullOrEmpty(input)) return "";
			string newName = string.Empty;
			if (input.Contains("-"))
			{
				string[] nn = input.Split('-');
				if (nn.Length<=1)
				{
					return input;
				}
				newName = nn[0];
				for (int i = 1; i < nn.Length; i++)
				{
					newName += string.Concat(char.ToUpper(nn[i][0]),nn[i].Substring(1));
				}
			}
			else return input;
			return newName;
		}
		/// <summary>
		/// removes dashes and converts the string to CamelCase
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		static public string CamelCleanUnder(this string input)
		{
			string newName = string.Empty;
			if (input.Contains("-"))
			{
				string[] nn = input.Split('_');
				if (nn.Length<=1)
				{
					return input;
				}
				newName = nn[0];
				for (int i = 1; i < nn.Length; i++)
				{
					newName += string.Concat(char.ToUpper(nn[i][0]),nn[i].Substring(1));
				}
			}
			else return input;
			return newName;
		}
		/// <summary>
		/// removes dashes and converts the string to CamelCase
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		static public string CamelClean(this string input)
		{
			return input.CamelCleanDash().CamelCleanUnder();
		}
		#endregion

		/// <summary>
		/// removes dashes and converts the string to CamelCase
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		static public string Clean(this string input)
		{
			return input.CamelClean();
		}

		#region ftag
		/// <summary>
		/// provides a curly variable name
		/// </summary>
		/// <param name="input"></param>
		/// <returns>${input}</returns>
		static public string FTagCurly(this string input) { return string.Format("${{{0}}}",input); }
		/// <summary>
		/// returns paranthesized variable name
		/// </summary>
		/// <param name="input"></param>
		/// <returns>$(input)</returns>
		static public string FTagParen(this string input) { return string.Format("$({0})",input); }
		#endregion		
		
		#region Replace (){}
		/// <summary>
		/// This is used to replace all occurences of “$(input)” given the input tagname.
		/// if ‘tag’ is “this name” then you get “$(this name)”
		/// </summary>
		/// <param name="input"></param>
		/// <param name="tag"></param>
		/// <param name="replace"></param>
		/// <returns></returns>
		static public string ReplaceP(this string input, string tag, object replace)
		{
			return input.Replace(tag.FTagParen(),string.Format("{0}",replace));
		}
		/// <summary>
		/// </summary>
		/// <param name="input"></param>
		/// <param name="tag"></param>
		/// <param name="replace"></param>
		/// <returns>${input}</returns>
		static public string ReplaceC(this string input, string tag, object replace)
		{
			return input.Replace(tag.FTagCurly(),string.Format("{0}", replace));
		}

		#endregion

	}

}
#endif