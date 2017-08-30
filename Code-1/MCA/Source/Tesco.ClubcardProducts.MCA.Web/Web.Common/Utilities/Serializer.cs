using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using JWT;
using JWT.Serializers;
using JWT.Algorithms;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Security;
using System.Web;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using System.Collections;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Utilities
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

    public static class JWTSerializerUtility
    {
        public static SecurityDefinition JWTDeserialize(string token)
        {
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();

            IJwtValidator validator = new JwtValidator(serializer, provider);
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
            return decoder.DecodeToObject<SecurityDefinition>(token, JWTSerializerUtility.GetSecretKey(), true);
        }

        public static string JWTSserialize(SecurityDefinition payload)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var token = encoder.Encode(payload, JWTSerializerUtility.GetSecretKey());

            return token;
        }

        public static string GetSecretKey()
        {
            StringBuilder secretBuilder = new StringBuilder();
            string jwtKey = System.Configuration.ConfigurationManager.AppSettings[AppConfigEnum.jwtkey.ToString()];

            var accessToken = HttpContext.Current.Request.Cookies[ParameterNames.OAUTH_TOKEN];
            var uuidToken = HttpContext.Current.Request.Cookies[ParameterNames.UUID]; //"ebdd55b0-5dba-453f-b40a-418a27569600";

            if (accessToken != null && !String.IsNullOrWhiteSpace(accessToken.Value))
            {
                secretBuilder.Append(accessToken.Value);
            }
            else
            {
                throw new Exception("Secret service unavailable - a");
            }
            if (!String.IsNullOrWhiteSpace(jwtKey))
            {
                secretBuilder.Append(CryptoUtility.DecryptTripleDES(jwtKey));
            }
            else
            {
                throw new Exception("Secret service unavailable - s");
            }
            if (uuidToken != null && !String.IsNullOrWhiteSpace(uuidToken.Value))
            {
                secretBuilder.Append(uuidToken.Value);
            }
            else
            {
                throw new Exception("Secret service unavailable - u");
            }

            return secretBuilder.ToString();
        }
    }
}
