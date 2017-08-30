using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;


namespace Tesco.ClubcardProducts.MCA.Web
{
    public class UnityBootStrapper
    {
        public static void Initialize()
        {
            IUnityContainer _container = new UnityContainer();
            UnityConfigurationSection sectionunity = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            sectionunity.Configure(_container, "MCAContainer");
            UnityServiceLocator locator = new UnityServiceLocator(_container);
            ServiceLocator.SetLocatorProvider(() => locator);

        }
    }
}