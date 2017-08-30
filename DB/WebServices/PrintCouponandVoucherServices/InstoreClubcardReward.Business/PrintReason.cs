using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstoreClubcardReward.Business
{
    [Serializable]
    public class PrintReason : BaseClass
    {


        #region Member Variables
        protected int? _printReasonId;
        protected int? _displayOrder;
        protected string _printReason;
        protected bool? _enabled;
        #endregion

        #region Public Properties
        public int? PrintReasonId
        {
            get { return _printReasonId; }
            set { _printReasonId = value; }
        }

        public int? DisplayOrder
        {
            get { return _displayOrder; }
            set { _displayOrder = value; }
        }

        public string PrintReasonText
        {
            get { return _printReason; }
            set { _printReason = value; }
        }

        public bool? Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }
        #endregion

        public PrintReason()
        {
        }

        /// <summary>
        /// Constructor for getting reason from database
        /// </summary>
        /// <param name="printReasonId"></param>
        /// <param name="displayOrder"></param>
        /// <param name="printReasonText"></param>
        /// <param name="enabled"></param>
        public PrintReason(int? printReasonId, int? displayOrder, string printReasonText, bool? enabled)
        {
            this.PrintReasonId = printReasonId;
            this.DisplayOrder = displayOrder;
            this.PrintReasonText = printReasonText;
            this.Enabled = enabled;

        }

    }

}
