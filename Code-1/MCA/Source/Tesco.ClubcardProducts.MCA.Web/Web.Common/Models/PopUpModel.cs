using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    public class PopUpModel
    {
        public PopUpModel()
        {
            this.PrimaryButton = new ModalButton();
            this.SecondaryButton = new ModalButton();
        }

        public string Template { get; set; }
        public ModalButton PrimaryButton { get; set; }
        public ModalButton SecondaryButton { get; set; }
        public string trigger { get; set; }
    }
   
    public class ModalButton
    {
        public string Text { get; set; }
        public string Link { get; set; }
        public bool IsVisible { get; set; }
    }
}

