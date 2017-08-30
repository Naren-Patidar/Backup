using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace Tesco.NGC.Utils {

	/// <summary>
	/// Class to support pre-compilation of data from any source and in any format
	/// </summary>
	public class PrecompiledStream : Stream {

		#region Enumerations
		/// <summary>The source of the input</summary>
		public enum Source {
			/// <summary>The input is from a file</summary>
			File,
			/// <summary>The input is from a string</summary>
			String
		}

		/// <summary>The format of the input</summary>
		public enum Format {
			/// <summary>The input contains SQL</summary>
			Sql,
			/// <summary>The input contains XML</summary>
			Xml
		}
		#endregion

		#region Attributes
		private StringCollection defineCollection;
		private char[] startTag;
		private char[] endTag;
		private TextReader inputReader;
		private StringBuilder output;
		private StringReader outputReader;
		#endregion

		#region Properties
		/// <summary>Always support Read</summary>
		public override bool CanRead { get { return true; } }

		/// <summary>Never support Seek</summary>
		public override bool CanSeek { get { return false; } }

		/// <summary>Never support Write</summary>
		public override bool CanWrite {	get { return false; } }

		/// <summary>NOT SUPPORTED</summary>
		public override long Length { get { return 0; } }

		/// <summary>NOT SUPPORTED</summary>
		public override long Position {
			get { return 0; }
			set { }
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Process the source following precompilation instructions
		/// </summary>
		public PrecompiledStream(Source source, string input, Format format, params string[] symbols) {

			// Read data from string or from file
			switch (source) {
				case Source.File:
					this.inputReader = new StreamReader(input);
					break;
				case Source.String:
					this.inputReader = new StringReader(input);
					break;
			}

			// Handle Sql or Xml data
			switch (format) {
				case Format.Sql:
					startTag = "/*".ToCharArray();
					endTag = "*/".ToCharArray();
					break;
				case Format.Xml:
					startTag = "<!--".ToCharArray();
					endTag = "-->".ToCharArray();
					break;
			}

			// Setup collection of Symbols
			this.defineCollection = new StringCollection();
			foreach (string symbol in symbols) {
				string lowerSymbol = symbol.ToLower();
				if (!this.defineCollection.Contains(lowerSymbol)) {
					this.defineCollection.Add(lowerSymbol);
				}
			}

			// Precompile data and write to a string reader
			this.output = new StringBuilder();
			StackFrame(String.Empty,true,true);
			this.outputReader = new StringReader(this.output.ToString());
		}

		#region Precompile
		/// <summary>
		/// Process current stack frame 
		/// </summary>
		/// <param name="outerKeyword">The command calling this stack frame</param>
		/// <param name="canBeTrue">Can conditional sibling commands be evaluated as true</param>
		/// <param name="isTrue">Is the current command true, i.e. should its contents be written to output</param>
		private void StackFrame(string outerKeyword, bool canBeTrue, bool isTrue) {

			bool endOfStack = false;

			while (true) {
				int nextEntry = this.inputReader.Peek();
				if (nextEntry == -1) {
					break;
				}

				StringBuilder buffer = new StringBuilder();

				// Check for a start tag
				if (!ReadToken(buffer,this.startTag)) {
					if (isTrue) {
						this.output.Append(buffer.ToString());
					}
					continue;
				}
				// Read White Spaces
				ReadWhiteSpaces(buffer);

				// Read Word Characters
				string keyword = ReadWordCharacters(buffer);
				// Read White Spaces
				ReadWhiteSpaces(buffer);
				
				string symbol = String.Empty;
				bool found = false;
				switch (keyword) {
					case "if":
						// Read Word Characters
						symbol = ReadWordCharacters(buffer).ToLower();
						found = (symbol != String.Empty);
						break;
					case "elif":
						// Read Word Characters
						symbol = ReadWordCharacters(buffer).ToLower();
						found = (((outerKeyword == "if") || (outerKeyword == "elif")) && (symbol != String.Empty));
						break;
					case "else":
						// Read Word Characters
						found = ((outerKeyword == "if") || (outerKeyword == "elif"));
						break;
					case "endif":
						// Read Word Characters
						found = ((outerKeyword == "if") || (outerKeyword == "elif") || (outerKeyword == "else"));
						break;
					case "define":
					case "undef":
						// Read Word Characters
						symbol = ReadWordCharacters(buffer).ToLower();
						found = (symbol != String.Empty);
						break;
				}
				if (!found) {
					if (isTrue) {
						this.output.Append(buffer.ToString());
					}
					continue;
				}
				// Read White Spaces
				ReadWhiteSpaces(buffer);

				// Check for a end tag
				if (!ReadToken(buffer,this.endTag)) {
					if (isTrue) {
						this.output.Append(buffer.ToString());
					}
					continue;
				}

				// An instruction has really been found, now process it
				switch (keyword) {
					case "if":
						if (isTrue) {
							if (this.defineCollection.Contains(symbol)) {
								StackFrame("if",false,true);
							} else {
								StackFrame("if",true,false);
							}
						} else {
							StackFrame("if",false,false);
						}
						break;
					case "elif":
						if ((canBeTrue) && (this.defineCollection.Contains(symbol))) {
							canBeTrue = false;
							isTrue = true;
						} else {
							isTrue = false;
						}
						outerKeyword = "elif";
						break;
					case "else":
						if (canBeTrue) {
							canBeTrue = false;
							isTrue = true;
						} else {
							isTrue = false;
						}
						outerKeyword = "else";
						break;
					case "endif":
						endOfStack = true;
						break;
					case "define":
						if (!this.defineCollection.Contains(symbol)) {
							this.defineCollection.Add(symbol);
						}
						break;
					case "undef":
						if (this.defineCollection.Contains(symbol)) {
							this.defineCollection.Remove(symbol);
						}
						break;
				}

				// stop if at the end of the stack frame
				if (endOfStack) {
					break;
				}
			}
		}

		private string ReadWhiteSpaces(StringBuilder buffer) {

			StringBuilder output = new StringBuilder();

			int nextEntry = this.inputReader.Peek();
			while (nextEntry != -1) {
				if (Char.IsWhiteSpace((char)nextEntry)) {
					int entry = this.inputReader.Read();
					nextEntry = this.inputReader.Peek();
					output.Append((char)entry);
				} else {
					break;
				}
			}

			string outputValue = output.ToString();
			buffer.Append(outputValue);
			return outputValue;
		}

		private string ReadWordCharacters(StringBuilder buffer) {

			StringBuilder output = new StringBuilder();

			int nextEntry = this.inputReader.Peek();
			while (nextEntry != -1) {
				char nextCharEntry = (char)nextEntry;
				if (Char.IsLetterOrDigit(nextCharEntry) || (nextCharEntry == '_')) {
					int entry = this.inputReader.Read();
					nextEntry = this.inputReader.Peek();
					output.Append(nextCharEntry);
				} else {
					break;
				}
			}

			string outputValue = output.ToString();
			buffer.Append(outputValue);
			return outputValue;
		}

		private bool ReadToken(StringBuilder buffer,char[] token) {

			bool found = true;
			for (int i = 0; i < token.Length ; i++) {
				int entry = this.inputReader.Read();
				buffer.Append((char)entry);
				if (entry != token[i]) {
					found = false;
					break;
				}
			}
			return found;
		}
		#endregion
		#endregion

		#region Supported Methods
		/// <summary>
		/// Read the Part of the Output, only supports ASCII at the moment
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public override int Read (byte[] buffer, int offset, int count) {
			char[] buffer2 = new char[buffer.Length];
			int result = this.outputReader.Read(buffer2, offset, count);
			for (int i = 0; i < result; i++) {
				buffer[i] = Convert.ToByte(buffer2[i]);
			}
			return result;
		}

		/// <summary>
		/// Read the Whole Output
		/// </summary>
		/// <returns></returns>
		public string ReadToEnd() {
			return this.outputReader.ReadToEnd();
		}
		#endregion

		#region Unsupported Methods
		/// <summary>
		/// NOT SUPPORTED, AS STREAM IS FLUSHED
		/// </summary>
		public override void Flush() {}

		/// <summary>
		/// NOT SUPPORTED, AS STREAM IS READ FORWARD ONLY
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		public override void Write (byte[] buffer, int offset, int count) {
		}

		/// <summary>
		/// NOT SUPPORTED, AS STREAM IS READ FORWARD ONLY
		/// </summary>
		/// <param name="offset"></param>
		/// <param name="origin"></param>
		/// <returns></returns>
		public override long Seek (long offset, SeekOrigin origin) {
			return 0;
		}

		/// <summary>
		/// NOT SUPPORTED, AS STREAM IS READ FORWARD ONLY
		/// </summary>
		/// <param name="value">The desired length of the current stream in bytes</param>
		public override void SetLength (long value) {
		}
		#endregion
	}
}