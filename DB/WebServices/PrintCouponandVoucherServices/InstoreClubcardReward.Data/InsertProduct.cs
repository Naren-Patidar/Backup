
//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by CodeSmith.
//
//     Date:    4/6/2009
//     Time:    2:36 
//     Version: 5.0.0.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

using System;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;

namespace InstoreClubcardReward.Data
{
	#region InsertProduct Wrapper
    /// <summary>
    /// This class is a wrapper for the InsertProduct stored procedure.
    /// </summary>
    public class InsertProduct
    {
		#region Member Variables
		protected string _connectionString = String.Empty;
        protected SqlConnection _connection = null;
        protected SqlTransaction _transaction = null;
		protected bool _ownsConnection = true;
		protected int _recordsAffected = -1;
		protected int _returnValue = 0;
		protected SqlString _productCode = SqlString.Null;
		protected bool _productCodeSet = false;
		protected SqlString _description = SqlString.Null;
		protected bool _descriptionSet = false;
		protected SqlMoney _unitPrice = SqlMoney.Null;
		protected bool _unitPriceSet = false;
		protected SqlString _country = SqlString.Null;
		protected bool _countrySet = false;
		protected SqlString _validUntil = SqlString.Null;
		protected bool _validUntilSet = false;
		protected SqlDateTime _enableDate = SqlDateTime.Null;
		protected bool _enableDateSet = false;
		protected SqlDateTime _disableDate = SqlDateTime.Null;
		protected bool _disableDateSet = false;
		protected SqlString _tokenLayout = SqlString.Null;
		protected bool _tokenLayoutSet = false;
		protected SqlString _barCode = SqlString.Null;
		protected bool _barCodeSet = false;
		protected SqlString _barCodeFlag = SqlString.Null;
		protected bool _barCodeFlagSet = false;
		protected SqlInt32 _tokenType = SqlInt32.Null;
		protected bool _tokenTypeSet = false;
		protected SqlString _tokenTitle = SqlString.Null;
		protected bool _tokenTitleSet = false;
		protected SqlString _tokenTitleShortPt1 = SqlString.Null;
		protected bool _tokenTitleShortPt1Set = false;
		protected SqlString _tokenTitleShortPt2 = SqlString.Null;
		protected bool _tokenTitleShortPt2Set = false;
		protected SqlString _tokenCurrencySymbol = SqlString.Null;
		protected bool _tokenCurrencySymbolSet = false;
		protected SqlString _tokenCurrencyWord = SqlString.Null;
		protected bool _tokenCurrencyWordSet = false;
		#endregion
		
		#region Constructors
		public InsertProduct()
		{
		}
		
		public InsertProduct(string connectionString)
		{
			this.ConnectionString = connectionString;
		}
		
		public InsertProduct(SqlConnection connection)
		{
			this.Connection = connection;
		}
		
		public InsertProduct(SqlConnection connection, SqlTransaction transaction)
		{
			this.Connection = connection;
			this.Transaction = transaction;
		}
		#endregion
		
		#region Public Properties
		/// <summary>
		/// The connection string to use when executing the InsertProduct stored procedure.
		/// </summary>
		public string ConnectionString
		{
			get {return _connectionString;}
			set {_connectionString = value;}
		}
        
        /// <summary>
        /// The connection to use when executing the InsertProduct stored procedure.
        /// If this is not null, it will be used instead of creating a new connection.
        /// </summary>
        public SqlConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }
		
        /// <summary>
        /// The transaction to use when executing the InsertProduct stored procedure.
        /// If this is not null, the stored procedure will be executing within the transaction.
        /// </summary>
        public SqlTransaction Transaction
        {
            get { return _transaction; }
            set { _transaction = value; }
        }
		
		/// <summary>
		/// Gets the return value from the InsertProduct stored procedure.
		/// </summary>
		public int ReturnValue
		{
			get {return _returnValue;}
		}
		
		/// <summary>
		/// Gets the number of rows changed, inserted, or deleted by execution of the InsertProduct stored procedure.
		/// </summary>
		public int RecordsAffected
		{
			get {return _recordsAffected;}
		}
		
		/// <summary>
		/// 
		/// </summary>
		public SqlString ProductCode
		{
			get {return _productCode;}
			set
			{
				_productCode = value;
				_productCodeSet = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SqlString Description
		{
			get {return _description;}
			set
			{
				_description = value;
				_descriptionSet = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SqlMoney UnitPrice
		{
			get {return _unitPrice;}
			set
			{
				_unitPrice = value;
				_unitPriceSet = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SqlString Country
		{
			get {return _country;}
			set
			{
				_country = value;
				_countrySet = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SqlString ValidUntil
		{
			get {return _validUntil;}
			set
			{
				_validUntil = value;
				_validUntilSet = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SqlDateTime EnableDate
		{
			get {return _enableDate;}
			set
			{
				_enableDate = value;
				_enableDateSet = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SqlDateTime DisableDate
		{
			get {return _disableDate;}
			set
			{
				_disableDate = value;
				_disableDateSet = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SqlString TokenLayout
		{
			get {return _tokenLayout;}
			set
			{
				_tokenLayout = value;
				_tokenLayoutSet = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SqlString BarCode
		{
			get {return _barCode;}
			set
			{
				_barCode = value;
				_barCodeSet = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SqlString BarCodeFlag
		{
			get {return _barCodeFlag;}
			set
			{
				_barCodeFlag = value;
				_barCodeFlagSet = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SqlInt32 TokenType
		{
			get {return _tokenType;}
			set
			{
				_tokenType = value;
				_tokenTypeSet = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SqlString TokenTitle
		{
			get {return _tokenTitle;}
			set
			{
				_tokenTitle = value;
				_tokenTitleSet = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SqlString TokenTitleShortPt1
		{
			get {return _tokenTitleShortPt1;}
			set
			{
				_tokenTitleShortPt1 = value;
				_tokenTitleShortPt1Set = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SqlString TokenTitleShortPt2
		{
			get {return _tokenTitleShortPt2;}
			set
			{
				_tokenTitleShortPt2 = value;
				_tokenTitleShortPt2Set = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SqlString TokenCurrencySymbol
		{
			get {return _tokenCurrencySymbol;}
			set
			{
				_tokenCurrencySymbol = value;
				_tokenCurrencySymbolSet = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SqlString TokenCurrencyWord
		{
			get {return _tokenCurrencyWord;}
			set
			{
				_tokenCurrencyWord = value;
				_tokenCurrencyWordSet = true;
			}
		}
		#endregion
		
		#region Helper Methods
        private SqlConnection GetConnection()
        {
            if (this.Connection != null)
            {
				_ownsConnection = false;
                return this.Connection;
            }
            else
            {
 				System.Diagnostics.Debug.Assert(this.ConnectionString.Length != 0, "You must first set the ConnectioString property before calling an Execute method.");
            	return new SqlConnection(this.ConnectionString);
            }
        }
		#endregion
		
		#region Execute Methods
		/// <summary>
		/// This method calls the InsertProduct stored procedure.
		/// </summary>
		public virtual void Execute()
		{
			SqlCommand cmd = new SqlCommand();
			
			SqlConnection cn = this.GetConnection();
			
			try
			{
				cmd.Connection = cn;
				cmd.Transaction = this.Transaction;
				cmd.CommandText = "[dbo].[InsertProduct]";
				cmd.CommandType = CommandType.StoredProcedure;
				
				#region Populate Parameters
				SqlParameter prmReturnValue = cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
				prmReturnValue.Direction = ParameterDirection.ReturnValue;
				
				SqlParameter prmProductCode = cmd.Parameters.Add("@ProductCode", SqlDbType.VarChar);
				prmProductCode.Direction = ParameterDirection.Input;
					prmProductCode.Size = 15;
				if (_productCodeSet == true || this.ProductCode.IsNull == false)
				{
					prmProductCode.Value = this.ProductCode;
				}
				
				SqlParameter prmDescription = cmd.Parameters.Add("@Description", SqlDbType.VarChar);
				prmDescription.Direction = ParameterDirection.Input;
					prmDescription.Size = 50;
				if (_descriptionSet == true || this.Description.IsNull == false)
				{
					prmDescription.Value = this.Description;
				}
				
				SqlParameter prmUnitPrice = cmd.Parameters.Add("@UnitPrice", SqlDbType.Money);
				prmUnitPrice.Direction = ParameterDirection.Input;
					prmUnitPrice.Precision = 19;
					prmUnitPrice.Scale = 4;
				if (_unitPriceSet == true || this.UnitPrice.IsNull == false)
				{
					prmUnitPrice.Value = this.UnitPrice;
				}
				
				SqlParameter prmCountry = cmd.Parameters.Add("@Country", SqlDbType.VarChar);
				prmCountry.Direction = ParameterDirection.Input;
					prmCountry.Size = 3;
				if (_countrySet == true || this.Country.IsNull == false)
				{
					prmCountry.Value = this.Country;
				}
				
				SqlParameter prmValidUntil = cmd.Parameters.Add("@ValidUntil", SqlDbType.VarChar);
				prmValidUntil.Direction = ParameterDirection.Input;
					prmValidUntil.Size = 50;
				if (_validUntilSet == true || this.ValidUntil.IsNull == false)
				{
					prmValidUntil.Value = this.ValidUntil;
				}
				
				SqlParameter prmEnableDate = cmd.Parameters.Add("@EnableDate", SqlDbType.DateTime);
				prmEnableDate.Direction = ParameterDirection.Input;
				if (_enableDateSet == true || this.EnableDate.IsNull == false)
				{
					prmEnableDate.Value = this.EnableDate;
				}
				
				SqlParameter prmDisableDate = cmd.Parameters.Add("@DisableDate", SqlDbType.DateTime);
				prmDisableDate.Direction = ParameterDirection.Input;
				if (_disableDateSet == true || this.DisableDate.IsNull == false)
				{
					prmDisableDate.Value = this.DisableDate;
				}
				
				SqlParameter prmTokenLayout = cmd.Parameters.Add("@TokenLayout", SqlDbType.VarChar);
				prmTokenLayout.Direction = ParameterDirection.Input;
					prmTokenLayout.Size = 255;
				if (_tokenLayoutSet == true || this.TokenLayout.IsNull == false)
				{
					prmTokenLayout.Value = this.TokenLayout;
				}
				
				SqlParameter prmBarCode = cmd.Parameters.Add("@BarCode", SqlDbType.VarChar);
				prmBarCode.Direction = ParameterDirection.Input;
					prmBarCode.Size = 20;
				if (_barCodeSet == true || this.BarCode.IsNull == false)
				{
					prmBarCode.Value = this.BarCode;
				}
				
				SqlParameter prmBarCodeFlag = cmd.Parameters.Add("@BarCodeFlag", SqlDbType.Char);
				prmBarCodeFlag.Direction = ParameterDirection.Input;
					prmBarCodeFlag.Size = 1;
				if (_barCodeFlagSet == true || this.BarCodeFlag.IsNull == false)
				{
					prmBarCodeFlag.Value = this.BarCodeFlag;
				}
				
				SqlParameter prmTokenType = cmd.Parameters.Add("@TokenType", SqlDbType.Int);
				prmTokenType.Direction = ParameterDirection.Input;
				if (_tokenTypeSet == true || this.TokenType.IsNull == false)
				{
					prmTokenType.Value = this.TokenType;
				}
				
				SqlParameter prmTokenTitle = cmd.Parameters.Add("@TokenTitle", SqlDbType.VarChar);
				prmTokenTitle.Direction = ParameterDirection.Input;
					prmTokenTitle.Size = 50;
				if (_tokenTitleSet == true || this.TokenTitle.IsNull == false)
				{
					prmTokenTitle.Value = this.TokenTitle;
				}
				
				SqlParameter prmTokenTitleShortPt1 = cmd.Parameters.Add("@TokenTitleShortPt1", SqlDbType.VarChar);
				prmTokenTitleShortPt1.Direction = ParameterDirection.Input;
					prmTokenTitleShortPt1.Size = 20;
				if (_tokenTitleShortPt1Set == true || this.TokenTitleShortPt1.IsNull == false)
				{
					prmTokenTitleShortPt1.Value = this.TokenTitleShortPt1;
				}
				
				SqlParameter prmTokenTitleShortPt2 = cmd.Parameters.Add("@TokenTitleShortPt2", SqlDbType.VarChar);
				prmTokenTitleShortPt2.Direction = ParameterDirection.Input;
					prmTokenTitleShortPt2.Size = 20;
				if (_tokenTitleShortPt2Set == true || this.TokenTitleShortPt2.IsNull == false)
				{
					prmTokenTitleShortPt2.Value = this.TokenTitleShortPt2;
				}
				
				SqlParameter prmTokenCurrencySymbol = cmd.Parameters.Add("@TokenCurrencySymbol", SqlDbType.Char);
				prmTokenCurrencySymbol.Direction = ParameterDirection.Input;
					prmTokenCurrencySymbol.Size = 1;
				if (_tokenCurrencySymbolSet == true || this.TokenCurrencySymbol.IsNull == false)
				{
					prmTokenCurrencySymbol.Value = this.TokenCurrencySymbol;
				}
				
				SqlParameter prmTokenCurrencyWord = cmd.Parameters.Add("@TokenCurrencyWord", SqlDbType.VarChar);
				prmTokenCurrencyWord.Direction = ParameterDirection.Input;
					prmTokenCurrencyWord.Size = 10;
				if (_tokenCurrencyWordSet == true || this.TokenCurrencyWord.IsNull == false)
				{
					prmTokenCurrencyWord.Value = this.TokenCurrencyWord;
				}
				#endregion
				
				#region Execute Command
				if (cn.State != ConnectionState.Open) cn.Open();
				_recordsAffected = cmd.ExecuteNonQuery();
				#endregion
				
				#region Get Output Parameters
				if (prmReturnValue.Value != null && prmReturnValue.Value != DBNull.Value)
				{
					_returnValue = (int)prmReturnValue.Value;
				}
				
				#endregion
			}
			finally
			{
				if (_ownsConnection)
				{
					if (cn.State == ConnectionState.Open)
					{
						cn.Close();
					}
					
					cn.Dispose();
				}
				cmd.Dispose();
			}
		}
		
		/// <summary>
		/// This method calls the InsertProduct stored procedure.
		/// </summary>
		/// <param name="connectionString">The connection string to use</param>
		/// <param name="productCode"></param>
		/// <param name="description"></param>
		/// <param name="unitPrice"></param>
		/// <param name="country"></param>
		/// <param name="validUntil"></param>
		/// <param name="enableDate"></param>
		/// <param name="disableDate"></param>
		/// <param name="tokenLayout"></param>
		/// <param name="barCode"></param>
		/// <param name="barCodeFlag"></param>
		/// <param name="tokenType"></param>
		/// <param name="tokenTitle"></param>
		/// <param name="tokenTitleShortPt1"></param>
		/// <param name="tokenTitleShortPt2"></param>
		/// <param name="tokenCurrencySymbol"></param>
		/// <param name="tokenCurrencyWord"></param>
		public static void Execute(
				#region Parameters
				string connectionString,
				SqlString productCode,
				SqlString description,
				SqlMoney unitPrice,
				SqlString country,
				SqlString validUntil,
				SqlDateTime enableDate,
				SqlDateTime disableDate,
				SqlString tokenLayout,
				SqlString barCode,
				SqlString barCodeFlag,
				SqlInt32 tokenType,
				SqlString tokenTitle,
				SqlString tokenTitleShortPt1,
				SqlString tokenTitleShortPt2,
				SqlString tokenCurrencySymbol,
				SqlString tokenCurrencyWord
				#endregion
		    )
		{
			InsertProduct insertProduct = new InsertProduct();
			
			#region Assign Property Values
			insertProduct.ConnectionString = connectionString;
			insertProduct.ProductCode = productCode;
			insertProduct.Description = description;
			insertProduct.UnitPrice = unitPrice;
			insertProduct.Country = country;
			insertProduct.ValidUntil = validUntil;
			insertProduct.EnableDate = enableDate;
			insertProduct.DisableDate = disableDate;
			insertProduct.TokenLayout = tokenLayout;
			insertProduct.BarCode = barCode;
			insertProduct.BarCodeFlag = barCodeFlag;
			insertProduct.TokenType = tokenType;
			insertProduct.TokenTitle = tokenTitle;
			insertProduct.TokenTitleShortPt1 = tokenTitleShortPt1;
			insertProduct.TokenTitleShortPt2 = tokenTitleShortPt2;
			insertProduct.TokenCurrencySymbol = tokenCurrencySymbol;
			insertProduct.TokenCurrencyWord = tokenCurrencyWord;
			#endregion
			
			insertProduct.Execute();
			
			#region Get Property Values
			
			#endregion
		}
		#endregion
	}
	#endregion
}

