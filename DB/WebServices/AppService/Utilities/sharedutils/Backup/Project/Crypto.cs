using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Fujitsu.eCrm.Generic.SharedUtils
{
	#region Header
	///
	/// <department>Fujitsu, e-Innovation, eCRM</department>
	/// <copyright>(c) Fujitsu Consulting, 2002</copyright>
	/// 
	/// <summary>
	/// A class to provide encryption using MD5 and SHA1 one-way hash algorithms.
	/// </summary>
	/// <development> 
		///    <version number="1.10" day="16" month="01" year="2003">
		///			<developer>Tom Bedwell</developer>
		///			<checker>Steve Lang</checker>
		///			<work_packet>WP/Barcelona/046</work_packet>
		///			<description>Namespaces conform to standards</description>
	///	</version>
	///		<version number="1.00" day="16" month="09" year="2002">
	///			<developer>Mark Hart</developer>
	///			<checker></checker>
	///			<work_packet></work_packet>
	///			<description>Initial Implementation</description>
	///		</version>
	/// </development>
	/// 
	#endregion Header

	public class Crypto : IDisposable {

		#region Attributes
		// Ensure that the class is only disposed of once.
		private bool disposed = false;
		private static ICryptoTransform desEncryptor;
		private static ICryptoTransform desDecryptor;
		#endregion

		#region DES Cryptography Used for sensitive session variables
		static Crypto() {
			DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
			desProvider.GenerateIV();
			desProvider.GenerateKey();
			desEncryptor = desProvider.CreateEncryptor();
			desDecryptor = desProvider.CreateDecryptor();
		}

		/// <summary>
		/// Encrypt a string using DES
		/// </summary>
		/// <param name="s">String to encrypt</param>
		/// <returns>Encrypted string</returns>
		public static string DesEncrypt(string s) {
			return DesTransform(s,desEncryptor);
		}

		/// <summary>
		/// Decrypt a string using DES
		/// </summary>
		/// <param name="s">String to decrypt</param>
		/// <returns>Decrypted string</returns>
		public static string DesDecrypt(string s) {
			return DesTransform(s,desDecryptor);
		}

		private static string DesTransform(string s, ICryptoTransform cryptoTransform) {
			byte[] inBuffer = System.Text.UnicodeEncoding.Unicode.GetBytes(s);
			byte[] outBuffer = cryptoTransform.TransformFinalBlock(inBuffer,0,inBuffer.Length);
			return System.Text.UnicodeEncoding.Unicode.GetString(outBuffer,0,outBuffer.Length);
		}
		#endregion

		#region Public Class Methods
		/// <summary>
		/// Encrypt a string using one-way hashing.
		/// </summary>
		/// <param name="s">The string to be encrypted.</param>
		/// <returns>The encrypted string.</returns>
		public string Encrypt(string s) 
		{
			// Add a little salt ... determine using the string
			// to be encrypted and append to string.
			string saltedString = s + GetSaltString(s);

			// Generate the hashed byte array containing the hash.
			SHA512 sha512 = SHA512.Create();
			byte[] e1 = sha512.ComputeHash(StringToByteArray(saltedString));

			// Also generate a hashed byte array using just the original string.
			byte[] e2 = sha512.ComputeHash(StringToByteArray(s));

			// Join the hex string together and jumble.
			string j = JumbleString(ByteArrayToHexString(e1).ToUpper() + ByteArrayToHexString(e2).ToUpper());

			// Return the encrypted string.
			return j;
		}
		#endregion

		#region String Manipulation Utilities
		/// <summary>
		/// Convert a byte array to a hex string.
		/// </summary>
		/// <param name="array">The byte array to be converted.</param>
		/// <returns>A string containing a hex representation of the byte array.</returns>
		private static string ByteArrayToHexString(byte[] array) 
		{
			StringBuilder sb = new StringBuilder();
			// Loop round the byte array and convert each byte.
			for (int i = 0; i < array.Length; ++i) 
			{
				string s = String.Format("{0:x}", array[i]);
				// If the hex is a single character, then need to add a
				// zero to the front.
				if (s.Length == 1) 
				{
					s = "0" + s;
				}
				sb.Append(s);
			}
			
			// Return the complete hex string.
			return sb.ToString();
		}

		// <summary>
		// Convert a byte array to a string.
		// </summary>
		// <param name="array">The byte array to be converted.</param>
		// <returns>The string equivalent of the byte array.</returns>
		//private static string ByteArrayToString(byte[] array) 
		//{
		//	return (new System.Text.UnicodeEncoding()).GetString(array);
		//}

		/// <summary>
		/// Convert a string to a byte array.
		/// </summary>
		/// <param name="s">The string to be converted to a byte array.</param>
		byte[] StringToByteArray(string s) 
		{
			return (new System.Text.UnicodeEncoding()).GetBytes(s);
		}

		/// <summary>
		/// Rearrange the characters of a string depending upon their position.
		/// </summary>
		/// <param name="s">The string to be jumbled.</param>
		/// <returns>The jumbled string.</returns>
		private static string JumbleString(string s) 
		{
			// Loop round each character of the string and assign
			// to a string depending on whether it's position is
			// odd or even
			string odd = "";
			string even = "";
			for (int i = 0; i < s.Length; i++) 
			{
				// Obtain the current character from the string.
				string character = s.Substring(i,1);
				if (i%2 == 0)
				{
					// Position is even.
					even = even + character;
				}
				else
				{
					// Position is odd.
					odd = odd + character;
				}
			}

			// Append odd and even together and return.
			return odd + even;
		}
		/// <summary>
		/// Examine the string to be encrypted and determine a salt
		/// string.
		/// </summary>
		/// <param name="s">The string used to determine the salt.</param>
		/// <returns>The salt string.</returns>
		private static string GetSaltString(string s) 
		{
			// Set-up a new hashtable to hold the salt strings.
			Hashtable ht = new Hashtable();
			ht.Add("a",@"Ugsfgsdf£q*&£!(*&^£q");
			ht.Add("b",@"4twujhky&*£^*&q(^wrh");
			ht.Add("c",@"r£q(*£&q^*&q^sfhfgh£");
			ht.Add("d",@"q)(*&)_fghfhq(*!)_FG");
			ht.Add("e",@"HHGHG(*LKJSQHOIUEQWO");
			ht.Add("f",@"FNOFGIUWEVGMNOIVWEGM");
			ht.Add("g",@"O|VYOIER|YQGMWEOIVYM");
			ht.Add("h",@"G|O<IYgluhboiguybUYF");
			ht.Add("i",@"VBGIUGibugyuiybgugy9");
			ht.Add("j",@"87689769876BUYGBUKYG");
			ht.Add("k",@"YUG987TB3UC8TTCX5TN9");
			ht.Add("l",@"834KJK7988WE9KWERFWE");
			ht.Add("m",@"WE9,7TW8K,7TE9KV5M30");
			ht.Add("n",@"[94C98908VYUY94UY54-");
			ht.Add("o",@"398UXGMCRUGRMHWOIUPM");
			ht.Add("p",@"HOIUH*)(&^&(*g^&BUYG");
			ht.Add("q",@"NWDFGYXFGUYE8F70WQY0");
			ht.Add("r",@"97Y103945U7-39812982");
			ht.Add("s",@"1384210=9841(*)&()*&");
			ht.Add("t",@")(*MNIUWHFMNORUHFMEW");
			ht.Add("u",@"UOY908)(*_(*)_VOWIFV");
			ht.Add("v",@"RJWEOIPFG,WEORPIGJ,R");
			ht.Add("w",@"0-U9-0(*_*)_**SDOIAF");
			ht.Add("x",@"JWEPOJI,765765FTGGGA");
			ht.Add("y",@"90879402385702398457");
			ht.Add("z",@"23485SAOIF238LL/823R");
			ht.Add("A",@"402T8TCM5TGUD84UT-94");
			ht.Add("B",@"TXC8UYTUFT085F684986");
			ht.Add("C",@"4MN68N8VG65V6Y84F986");
			ht.Add("D",@"N8347698986-478F234P");
			ht.Add("E",@"YUCFU768F5729804N098");
			ht.Add("F",@"7J0987J)(*&mn_*m&n*7");
			ht.Add("G",@"MN608BNT76HNT8B7tnt6");
			ht.Add("H",@"ngt6mn98tynm87ymn7ym");
			ht.Add("I",@"n7ymn87ymn87yn8706n0");
			ht.Add("J",@"979087)(*N&)(*&MN)(*");
			ht.Add("K",@"&MN(*&(*0iuhniuerlhn");
			ht.Add("L",@"wemguxrhxoigrmhigrhw");
			ht.Add("M",@"e098(*)&)(&())(*&*)(");
			ht.Add("N",@"OLJHOPIJMOIPJ,XFGPJ3");
			ht.Add("O",@"980980-8098982348632");
			ht.Add("P",@"8496396olmihoipjmhoi");
			ht.Add("Q",@"psuhifgmoudhmuwoighr");
			ht.Add("R",@"mOPIMNHRXGWEMOIUHoiu");
			ht.Add("S",@"mh98-70987-98mu098uy");
			ht.Add("T",@"m(*_&M(*_M&(*)&lsjwe");
			ht.Add("U",@"irugmruwehgmwrughwei");
			ht.Add("V",@"uhi()*U(*)&*)(&*()*&");
			ht.Add("W",@"(IUHMOIUHMIOUHGMOWEI");
			ht.Add("X",@"RZREOIUHRMUIGERMWIOG");
			ht.Add("Y",@"URH980709&()*&)(*&)(");
			ht.Add("Z",@"*&*98UM098UM)(*&)(*7");
			ht.Add("!",@"0988097(*)980jmn79JN");
			ht.Add("£",@"m&T6G76NF^*%FN%nf685");
			ht.Add("$",@"FN856FN65FB658F*%^RN");
			ht.Add("%",@"8%^RBN685rn857rn*%7r");
			ht.Add("^",@"n68%RN857RN*%^RN^*%R");
			ht.Add("&",@"N865R856NR856RN85R68");
			ht.Add("*",@"5RN*^%rn*%^rn%*R58r8");
			ht.Add("(",@"r68r76r76r76nr76rnR7");
			ht.Add(")",@"66R8^r^nr&^RN4623N50");
			ht.Add(":",@"39847252349857M98098");
			ht.Add(";",@"7n098798098897((JJJJ");
			ht.Add("@",@"(*&(*&n^7NTGGYGUYGUY");
			ht.Add("#",@"GNGNUYGNUGNYguygnygn");
			ht.Add("<",@"987tn986t9698UYGUYGN");
			ht.Add(">",@"8R54EDB53ES3434VS32S");
			ht.Add("{",@"Vb3d5fm85g987hn,uoij");
			ht.Add("}",@",809U,098,Y897TMI7YG");
			ht.Add("[",@"BYTF76R5764r765rygn8");
			ht.Add("]",@"tgfn6TNTN8T67T8T68T8");
			ht.Add("|",@"t76tn7tnufbnfr5e45e5");
			ht.Add("default",@"687n9669n6(N^(N86fgn");

			// Determine which salt string to use depending on the first
			// character of the string to be encrypted.
			string firstChar = s.Substring(0,1);

			// Check to see if this character exists in the hastable.
			string salt;
			if (ht.ContainsKey(firstChar)) 
			{
				// Character exists, so get the salt key.
				salt = (string)ht[firstChar];
			} 
			else 
			{
				// Character doesn't exist, so use the default.
				salt = (string)ht["default"];
			}
			return salt;
		}

		#endregion

		#region Destructors
		/// <summary>
		/// Dispose of the object to free up any resource being used. The use
		/// of this method effectively indicates that the class will not be
		/// used again.
		/// </summary>
		public void Dispose() 
		{
			Dispose(true);
			Close();

			// No longer any need for the garbage collector to call the destructor
			GC.SuppressFinalize(this);	

		}

		/// <summary>
		/// The method to do the work to dispose of any resources being used.
		/// </summary>
		/// <param name="disposing">Indicate whether the object has already been disposed of.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)	// Only dispose once.
			{
				disposed = true;
			}
		}

		/// <summary>
		/// Tidly end use of the object.
		/// </summary>
		public void Close() 
		{
		}

		/// <summary>
		/// Destructor called by the Garbage Collector as the class is
		/// derived from IDisposable.
		/// </summary>
		~Crypto() 
		{
			Dispose(false);
		}
		#endregion
	}
}