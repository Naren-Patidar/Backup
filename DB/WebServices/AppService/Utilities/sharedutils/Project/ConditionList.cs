using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;

namespace Fujitsu.eCrm.Generic.SharedUtils {

	#region ConditionList
	/// <summary>
	/// Class for generating conditions sent to CrmServer's Find Web Method
	/// Find Web Method also uses this class for receiving the conditions
	/// </summary>
	public class ConditionList {

		#region Attributes
		private static XmlSchemaCollection conditionListXsdCollection;
		private static string conditionListXsd;
		private XmlDocument conditionListDoc;
		private XmlElement conditionListNode;
		private XmlAttribute opNode;
		private XmlAttribute notNode;
		private ArrayList childConditions;
		private ArrayList childConditionLists;
		#endregion

		#region Properties
		/// <summary>Get the XML Schema for condition list schema as used by the CrmServer's Find Method</summary>
		public static string Xsd { get { return ConditionList.conditionListXsd; } }

		/// <summary>Get the XML Document describing this objects list of conditions</summary>
		internal XmlDocument Document { get { return this.conditionListDoc; } }
		
		/// <summary>Get the XML Node describing this objects list of conditions</summary>
		internal XmlElement Node { get { return this.conditionListNode; } }
		
		/// <summary>Get the list of conditions belonging to this list</summary>
		public ArrayList ChildConditions { get { return this.childConditions; } }
		
		/// <summary>Get the list of condition lists belonging to this list</summary>
		public ArrayList ChildConditionLists { get { return this.childConditionLists; } }

		/// <summary>Get or Set the conjunction joining the components of this list</summary>
		public ConditionList.OperatorType Operator {
			get {
				if (this.opNode == null) {
					return OperatorType.And;
				}
				return this.opNode.Value == "and" ? OperatorType.And : OperatorType.Or;
			}
			set { 
				if (this.opNode == null) {
					this.opNode = this.conditionListDoc.CreateAttribute("op");
					this.conditionListNode.Attributes.Append(this.opNode);
				}
				this.opNode.Value = value == (OperatorType.And) ? "and" : "or";
			}
		}

		/// <summary>Get or Set the negation of this list</summary>
		public bool Not {
			get {
				if (this.notNode == null) {
					return false;
				}
				return (this.notNode.Value == "true");
			}
			set {
				if (this.notNode == null) {
					this.notNode = this.conditionListDoc.CreateAttribute("not");
					this.conditionListNode.Attributes.Append(this.notNode);
				}
				this.notNode.Value = value ? "true" : "false";
			}
		}
		#endregion

		#region Enumerator
		/// <summary>
		/// Enumeration of permissible conjunctions
		/// </summary>
		public enum OperatorType {
			/// <summary>SQLs AND</summary>
			And, 

			/// <summary>SQLs OR</summary>
			Or
		};
		#endregion

		#region Constructors
		/// <summary>
		/// Initialise the static variables
		/// </summary>
		public static void Initialise() {
			ConditionList.conditionListXsdCollection = new XmlSchemaCollection();
			Stream xsdStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Fujitsu.eCrm.Generic.SharedUtils.Resources.ConditionList.xsd");
			try {
				StreamReader reader = new System.IO.StreamReader(xsdStream);
				ConditionList.conditionListXsd = reader.ReadToEnd();

				xsdStream.Seek(0,SeekOrigin.Begin);

				ConditionList.conditionListXsdCollection.ValidationEventHandler += new ValidationEventHandler(XmlSyntaxError.Handle);
				ConditionList.conditionListXsdCollection.Add("urn:Fujitsu.eCrm.Generic.CrmService/xsd/condition_list",new XmlTextReader(xsdStream));
			} finally {
				xsdStream.Close();
			}
			if (XmlSyntaxError.HasError) {
				throw (new CrmServiceException("Library","XmlError","Condition.InvalidXsd",XmlSyntaxError.ErrorMessage));
			}
		}

		static ConditionList() {
			if (ConditionList.conditionListXsdCollection == null) {
				Initialise();
			}
		}

		/// <summary>
		/// Construct a root condition list
		/// </summary>
		public ConditionList() {
			this.conditionListDoc = new XmlDocument();
			this.conditionListNode = this.conditionListDoc.CreateElement("condition_list");
			this.conditionListDoc.AppendChild(this.conditionListNode);
			this.childConditionLists = new ArrayList();
			this.childConditions = new ArrayList();
		}

		/// <summary>
		/// Construct a root condition list
		/// </summary>
		/// <param name="op">The conjunction of the condition list</param>
		public ConditionList(ConditionList.OperatorType op) : this() {
			this.Operator = op;
		}

		/// <summary>
		/// Construct a root condition list
		/// </summary>
		/// <param name="not">The disjunction of the condition list</param>
		public ConditionList(bool not) : this() {
			this.Not = not;
		}

		/// <summary>
		/// Construct a root condition list
		/// </summary>
		/// <param name="op">The conjunction of the condition list</param>
		/// <param name="not">The disjunction of the condition list</param>
		public ConditionList(ConditionList.OperatorType op, bool not) : this() {
			this.Operator = op;
			this.Not = not;
		}

		private ConditionList(ConditionList parent) {
			this.conditionListDoc = parent.conditionListDoc;
			this.conditionListNode = this.conditionListDoc.CreateElement("condition_list",parent.Node.NamespaceURI);
			parent.conditionListNode.AppendChild(this.conditionListNode);
			this.childConditions = new ArrayList();
			this.childConditionLists = new ArrayList();
		}

		private ConditionList(ConditionList parent, ConditionList.OperatorType op) : this(parent) {
			this.Operator = op;
		}

		private ConditionList(ConditionList parent, bool not) : this(parent) {
			this.Not = not;
		}

		private ConditionList(ConditionList parent, ConditionList.OperatorType op, bool not) : this(parent) {
			this.Operator = op;
			this.Not = not;
		}

		/// <summary>
		/// Construct a root condition list, from an XML Document following the 
		/// condition_list schema
		/// </summary>
		public ConditionList(string xmlString) {

			Regex r = new Regex("xmlns");
			if (!r.IsMatch(xmlString)) {
				r = new Regex("condition_list");
				xmlString = r.Replace(xmlString,"condition_list xmlns=\"urn:Fujitsu.eCrm.Generic.CrmService/xsd/condition_list\"",1);
			}

			XmlValidatingReader reader = new XmlValidatingReader(xmlString,XmlNodeType.Document,null);
			reader.ValidationType = ValidationType.Schema;
			reader.Schemas.Add(ConditionList.conditionListXsdCollection);
			reader.ValidationEventHandler += new ValidationEventHandler(XmlSyntaxError.Handle);

			this.conditionListDoc = new XmlDocument();
			this.conditionListDoc.Load(reader);

			if (XmlSyntaxError.HasError) {
				throw (new CrmServiceException("Library","XmlError","Condition.InvalidXml",XmlSyntaxError.ErrorMessage));
			}

			this.conditionListNode = this.conditionListDoc.DocumentElement;
			ParseXml();
		}

		private ConditionList(ConditionList parent, XmlNode egoNode) {
			this.conditionListDoc = parent.conditionListDoc;
			this.conditionListNode = (XmlElement)egoNode;
			ParseXml();
		}

		private void ParseXml() {
			this.opNode = (XmlAttribute)this.conditionListNode.Attributes.GetNamedItem("op");
			this.notNode = (XmlAttribute)this.conditionListNode.Attributes.GetNamedItem("not");
			this.childConditionLists = new ArrayList();
			this.childConditions = new ArrayList();
			foreach (XmlNode childNode in this.conditionListNode.ChildNodes) {
				if (childNode.Name == "condition_list") {
					this.childConditionLists.Add(new ConditionList(this,childNode));
				} else {
					this.childConditions.Add(new Condition(this,childNode));
				}
			}
		}
		#endregion

		#region Populate Sub Condition List
		/// <summary>
		/// Add a new list of conditions to this condition list
		/// </summary>
		/// <returns>The new list of conditions</returns>
		public ConditionList AddConditionList() {
			ConditionList child = new ConditionList(this);
			this.childConditionLists.Add(child);
			return child;
		}

		/// <summary>
		/// Add a new list of conditions to this condition list
		/// </summary>
		/// <param name="op">The conjunction to be used by the new condition list</param>
		/// <returns>The new list of conditions</returns>
		public ConditionList AddConditionList(ConditionList.OperatorType op) {
			ConditionList child = new ConditionList(this, op);
			this.childConditionLists.Add(child);
			return child;
		}

		/// <summary>
		/// Add a new list of conditions to this condition list
		/// </summary>
		/// <param name="not">The disjunction to be used by the new condition list</param>
		/// <returns>The new list of conditions</returns>
		public ConditionList AddConditionList(bool not) {
			ConditionList child = new ConditionList(this, not);
			this.childConditionLists.Add(child);
			return child;
		}

		/// <summary>
		/// Add a new list of conditions to this condition list
		/// </summary>
		/// <param name="op">The conjunction to be used by the new condition list</param>
		/// <param name="not">The disjunction to be used by the new condition list</param>
		/// <returns>The new list of conditions</returns>
		public ConditionList AddConditionList(ConditionList.OperatorType op, bool not) {
			ConditionList child = new ConditionList(this, op, not);
			this.childConditionLists.Add(child);
			return child;
		}
		#endregion

		#region Populate Condition
		/// <summary>
		/// Add a condition to this list, assume the comparison will be Condition.OpType.Equals
		/// </summary>
		/// <param name="operandList">The operands to be used by this condition</param>
		/// <returns>The new condition</returns>
		public Condition AddCondition(params Condition.IOperand[] operandList) {
			Condition child = new Condition(this, operandList);
			this.childConditions.Add(child);
			return child;
		}

		/// <summary>
		/// Add a condition to this list
		/// </summary>
		/// <param name="op">The comparison to be used by this condition</param>
		/// <param name="operandList">The operands to be used by this condition</param>
		/// <returns>The new condition</returns>
		public Condition AddCondition(Condition.OperatorType op, params Condition.IOperand[] operandList) {
			Condition child = new Condition(this, op, operandList);
			this.childConditions.Add(child);
			return child;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Generate XML describing the condition list
		/// </summary>
		/// <returns></returns>
		public new string ToString() {
			return this.conditionListDoc.OuterXml;
		}
		#endregion
	}
	#endregion

	#region Condition
	/// <summary>
	/// A basic condition with a list
	/// </summary>
	public class Condition {
		
		#region Attributes
		private ConditionList parent;
		private XmlElement conditionNode;
		private XmlAttribute opNode;
		private ArrayList childList;
		#endregion

		#region Properties
		/// <summary>
		/// Get the list of elements of this condition
		/// </summary>
		public ArrayList Children { get { return this.childList; } }

		/// <summary>
		/// Get or Set the comparison used by this condition
		/// </summary>
		public Condition.OperatorType Operator {
			get {
				if (this.opNode == null) {
					return OperatorType.Equals;
				}
				switch (this.opNode.Value) {
					case "equals":
						return OperatorType.Equals;
					case "not equal":
						return OperatorType.NotEqual;
					case "is null":
						return OperatorType.IsNull;
					case "is not null":
						return OperatorType.IsNotNull;
					case "like":
						return OperatorType.Like;
					case "not like":
						return OperatorType.NotLike;
					case "less than":
						return OperatorType.LessThan;
					case "not less than":
						return OperatorType.NotLessThan;
					case "greater than":
						return OperatorType.GreaterThan;
					case "not greater than":
						return OperatorType.NotGreaterThan;
					case "between":
						return OperatorType.Between;
					case "not between":
						return OperatorType.NotBetween;
					case "in":
						return OperatorType.In;
					case "not in":
						return OperatorType.NotIn;
				}
				return OperatorType.Equals;
			}
			set {
				if (this.opNode == null) {
					this.opNode = this.parent.Document.CreateAttribute("op");
					this.conditionNode.Attributes.Append(this.opNode);
				}
				switch (value) {
					case OperatorType.Equals:
						this.opNode.Value = "equals";
						break;
					case OperatorType.NotEqual:
						this.opNode.Value = "not equal";
						break;
					case OperatorType.IsNull:
						this.opNode.Value = "is null";
						break;
					case OperatorType.IsNotNull:
						this.opNode.Value = "is not null";
						break;
					case OperatorType.Like:
						this.opNode.Value = "like";
						break;
					case OperatorType.NotLike:
						this.opNode.Value = "not like";
						break;
					case OperatorType.LessThan:
						this.opNode.Value = "less than";
						break;
					case OperatorType.NotLessThan:
						this.opNode.Value = "not less than";
						break;
					case OperatorType.GreaterThan:
						this.opNode.Value = "greater than";
						break;
					case OperatorType.NotGreaterThan:
						this.opNode.Value = "not greater than";
						break;
					case OperatorType.Between:
						this.opNode.Value = "between";
						break;
					case OperatorType.NotBetween:
						this.opNode.Value = "not between";
						break;
					case OperatorType.In:
						this.opNode.Value = "in";
						break;
					case OperatorType.NotIn:
						this.opNode.Value = "not in";
						break;
				}
			}
		}
		#endregion

		#region Constructors
		internal Condition(ConditionList parent, XmlNode egoNode) {
			this.parent = parent;
			this.conditionNode = (XmlElement)egoNode;
			this.opNode = (XmlAttribute)this.conditionNode.Attributes.GetNamedItem("op");
			this.childList = new ArrayList();
			foreach (XmlNode childNode in this.conditionNode.ChildNodes) {
				if (childNode.Name == "value") {
					childList.Add(new Condition.Value(this,childNode));
				} else {
					childList.Add(new Condition.Field(this,childNode));
				}
			}
		}
		
		/// <summary>
		/// Construct a condition
		/// </summary>
		/// <param name="conditionList">The parent under which this condition belongs</param>
		/// <param name="op">The comparison for comparing the list of elements</param>
		/// <param name="operandList">The list of elements used in the condition</param>
		public Condition(ConditionList conditionList, Condition.OperatorType op, params IOperand[] operandList) {
			this.parent = conditionList;
			this.conditionNode = this.parent.Document.CreateElement("condition",this.parent.Node.NamespaceURI);
			this.parent.Node.AppendChild(this.conditionNode);
			this.childList = new ArrayList();
			this.Operator = op;
			foreach (IOperand operand in operandList) {
				operand.Add(this);
				childList.Add(operand);
			}
		}

		/// <summary>
		/// Construct a condition, assumming the comparison is Condition.OpType.Equals
		/// </summary>
		/// <param name="conditionList">The parent under which this condition belongs</param>
		/// <param name="operandList">The list of elements used in the condition</param>
		public Condition(ConditionList conditionList, params IOperand[] operandList) : this(conditionList, Condition.OperatorType.Equals, operandList) {
		}
		#endregion
	
		#region Populate
		/// <summary>
		/// Add a field element to the condition
		/// </summary>
		/// <param name="fieldName">The name of the field element</param>
		public Condition.Field AddField(string fieldName) {
			Condition.Field child = new Condition.Field(this, fieldName);
			this.childList.Add(child);
			return child;
		}

		/// <summary>
		/// Add a value element to the condition
		/// </summary>
		/// <param name="literal">The value of the value element</param>
		public Condition.Value AddValue(string literal) {
			Condition.Value child = new Condition.Value(this, literal);
			this.childList.Add(child);
			return child;
		}
		#endregion

		#region Sub-components
		#region Operators
		/// <summary>
		/// Enumeration of permissible comparisons
		/// </summary>
		public enum OperatorType {
			/// <summary>SQLs =</summary>
			Equals, 

			/// <summary>SQLs !=</summary>
			NotEqual,

			/// <summary>SQLs IS NULL</summary>
			IsNull,

			/// <summary>SQLs IS NOT NULL</summary>
			IsNotNull,

			/// <summary>SQLs LIKE</summary>
			Like,

			/// <summary>SQLs NOT LIKE</summary>
			NotLike,

			/// <summary>SQLs &lt;</summary>
			LessThan,

			/// <summary>SQLs &gt;=</summary>
			NotLessThan,

			/// <summary>SQLs &gt;</summary>
			GreaterThan,

			/// <summary>SQLs &lt;=</summary>
			NotGreaterThan,

			/// <summary>SQLs BETWEEN</summary>
			Between,

			/// <summary>SQLs NOT BETWEEN</summary>
			NotBetween,

			/// <summary>SQLs IN</summary>
			In,

			/// <summary>SQLs NOT IN</summary>
			NotIn
		};

		/// <summary>
		/// Enumeration of permissible operands
		/// </summary>
		public enum OperandType {
			/// <summary>XML Field</summary>
			Field,

			/// <summary>Values</summary>
			Value
		}
		#endregion

		#region Operands
		/// <summary>
		/// An interface which all basic operands of a condition must inherit
		/// </summary>
		public interface IOperand {

			/// <summary>Get the type of this operand</summary>
			Condition.OperandType Type { get; }

			/// <summary>Get the value of this operand</summary>
			string Text { get; }

			/// <summary>
			/// Link this operand to the condition
			/// </summary>
			/// <param name="condition">The condition to which this operand will belong to</param>
			/// <returns></returns>
			void Add(Condition condition);
		}

		/// <summary>
		/// Class Field is a basic operand used by the condition class
		/// </summary>
		public class Field : IOperand {

			private Condition parent;
			private string fieldName;

			/// <summary>Get the type of this operand</summary>
			public Condition.OperandType Type { get { return Condition.OperandType.Field; } }

			/// <summary>Get the name of this field</summary>
			public string Text { get { return this.fieldName; } }

			/// <summary>
			/// Construct a basic field operand
			/// </summary>
			/// <param name="fieldName">The name of this field</param>
			public Field(string fieldName) {
				this.fieldName = fieldName;
			}

			internal Field(Condition condition, string fieldName) : this(fieldName) {
				this.Add(condition);
			}

			internal Field(Condition condition, XmlNode egoNode) {
				this.parent = condition;
				this.fieldName = egoNode.InnerText;
			}

			/// <summary>
			/// Link this field to the condition
			/// </summary>
			/// <param name="condition">The condition to which this field will belong to</param>
			/// <returns></returns>
			public void Add(Condition condition) {
				this.parent = condition;
				XmlElement fieldNode = condition.parent.Document.CreateElement("field",condition.parent.Node.NamespaceURI);
				condition.conditionNode.AppendChild(fieldNode);
				fieldNode.InnerText = this.fieldName;
			}
		}

		/// <summary>
		/// Class Value is a basic operand used by the condition class
		/// </summary>
		public class Value : IOperand {

			private Condition parent;
			private string literal;

			/// <summary>Get the type of this operand</summary>
			public Condition.OperandType Type { get { return Condition.OperandType.Value; } }

			/// <summary>Get the literal of this value</summary>
			public string Text { 
				get { return this.literal; } 
				set { this.literal = value; }
			}

			/// <summary>
			/// Construct a basic value operand
			/// </summary>
			/// <param name="literal">The literal of this value</param>
			public Value(string literal) {
				this.literal = literal;
			}

			internal Value(Condition condition, string literal) : this(literal) {
				this.Add(condition);
			}

			internal Value(Condition condition, XmlNode egoNode) {
				this.parent = condition;
				this.literal = egoNode.InnerText;
			}

			/// <summary>
			/// Link this value to the condition
			/// </summary>
			/// <param name="condition">The condition to which this field will belong to</param>
			/// <returns></returns>
			public void Add(Condition condition) {
				this.parent = condition;
				XmlElement valueNode = condition.parent.Document.CreateElement("value",condition.parent.Node.NamespaceURI);
				condition.conditionNode.AppendChild(valueNode);
				valueNode.InnerText = this.literal;
			}
		}
		#endregion
		#endregion
	}
	#endregion
}