using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Tesco.ClubcardProducts.MCA.API.Common.Utilities
{
    public static class SerializerUtility<T>
    {
        public static string GetSerializedString(T obj)
        {
            
            try
            {
                //NGCTrace.NGCTrace.TraceInfo("Start:SerializerUtility.GetSerializedString");
                //NGCTrace.NGCTrace.TraceDebug("Start Debug:SerializerUtility.GetSerializedString");
                XmlSerializer ser = new XmlSerializer(typeof(T));
                StringWriter writer = new StringWriter();
                ser.Serialize(writer, obj);
                string serializedValue = writer.ToString();
                writer.Dispose();
                //NGCTrace.NGCTrace.TraceInfo("End:SerializerUtility.GetSerializedString");
                //NGCTrace.NGCTrace.TraceDebug("End Debug:SerializerUtility.GetSerializedString");
                return serializedValue;
            }

            catch (Exception exp)
            {
                //NGCTrace.NGCTrace.TraceCritical("Critical:SerializerUtility.GetSerializedString- Error Message :" + exp.ToString());
                //NGCTrace.NGCTrace.TraceError("Error:SerializerUtility.GetSerializedString - Error Message :" + exp.ToString());
                //NGCTrace.NGCTrace.TraceWarning("Warning:SerializerUtility.GetSerializedString");
                //NGCTrace.NGCTrace.ExeptionHandling(exp);
                throw exp;
            }
            
        }

        public static string GetSerializedString(T obj, XmlAttributeOverrides overrider)
        {

            try
            {
                //NGCTrace.NGCTrace.TraceInfo("Start:SerializerUtility.GetSerializedString");
                //NGCTrace.NGCTrace.TraceDebug("Start Debug:SerializerUtility.GetSerializedString");
                XmlSerializer ser = new XmlSerializer(typeof(T), overrider);
                StringWriter writer = new StringWriter();
                ser.Serialize(writer, obj);
                string serializedValue = writer.ToString();
                writer.Dispose();
                //NGCTrace.NGCTrace.TraceInfo("End:SerializerUtility.GetSerializedString");
                //NGCTrace.NGCTrace.TraceDebug("End Debug:SerializerUtility.GetSerializedString");
                return serializedValue;
            }

            catch (Exception exp)
            {
                //NGCTrace.NGCTrace.TraceCritical("Critical:SerializerUtility.GetSerializedString- Error Message :" + exp.ToString());
                //NGCTrace.NGCTrace.TraceError("Error:SerializerUtility.GetSerializedString - Error Message :" + exp.ToString());
                //NGCTrace.NGCTrace.TraceWarning("Warning:SerializerUtility.GetSerializedString");
                //NGCTrace.NGCTrace.ExeptionHandling(exp);
                throw exp;
            }

        }
    }
}
