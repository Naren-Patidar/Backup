using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    [Serializable]
    public class MCAViewModel
    {
        VouchersViewModel _vouchersViewModel = null;
        PersonalDetailsViewModel _personalDetailsViewModel = null;
        
        public MCAViewModel()
        {
            this._vouchersViewModel = new VouchersViewModel();
            this._personalDetailsViewModel = new PersonalDetailsViewModel();
        }

        public VouchersViewModel vouchersViewModel
        {
            get { return _vouchersViewModel; }
            set { _vouchersViewModel = value; }
        }

        public PersonalDetailsViewModel personalDetailsViewModel
        {
            get { return _personalDetailsViewModel; }
            set { _personalDetailsViewModel = value; }
        }
    }
}
