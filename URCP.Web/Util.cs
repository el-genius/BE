using URCP.Web.Infrastructure.Encryption;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml.Linq;
using System.Xml.Serialization;
using MessagingToolkit.QRCode.Codec;
using URCP.Web.ViewModels;
using System.Text.RegularExpressions;

namespace URCP.Web
{
    public static class Util
    {
        #region DateTime Utilities

        /// <summary>
        /// Check if the string provided is Gregorian date
        /// </summary>
        /// <param name="greg">date string</param>
        /// <returns>True if string can be casted as gregorian date</returns>
        public static bool IsGreg(string greg)
        {
            string[] allFormats ={"yyyy/MM/dd","yyyy/M/d",
            "dd/MM/yyyy","d/M/yyyy",
            "dd/M/yyyy","d/MM/yyyy","yyyy-MM-dd",
            "yyyy-M-d","dd-MM-yyyy","d-M-yyyy",
            "dd-M-yyyy","d-MM-yyyy","yyyy MM dd",
            "yyyy M d","dd MM yyyy","d M yyyy",
            "dd M yyyy","d MM yyyy"};
            var enCul = new CultureInfo("en-GB");

            if (greg.Length <= 0)
                return false;
            try
            {
                DateTime tempDate = DateTime.ParseExact(greg, allFormats, enCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);

                if (tempDate.Year >= 1900 && tempDate.Year <= 2200)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Returns DateTime string with specific format using UmAlQuraCalendar
        /// </summary>
        /// <param name="dateTime">DateTime object</param>
        /// <param name="format">date time format to use</param>
        /// <returns></returns>
        public static string GetHigriDateWithFormat(DateTime? dateTime, string format)
        {
            string result = String.Empty;

            if (dateTime == null)
                return result;

            try
            {
                CultureInfo arCul = new CultureInfo("ar-SA");
                arCul.DateTimeFormat.Calendar = new System.Globalization.UmAlQuraCalendar();
                result = dateTime.Value.ToString(format, arCul);
            }
            catch { result = String.Empty; }
            return result;
        }

        /// <summary>
        /// Returns DateTime string with specific format using GregorianCalendar
        /// </summary>
        /// <param name="dateTime">DateTime object</param>
        /// <param name="format">date time format to use</param>
        /// <returns></returns>
        public static string GetGregorianDateWithFormat(DateTime? dateTime, string format)
        {
            string result = String.Empty;

            if (dateTime == null)
                return result;

            try
            {
                CultureInfo geCul = new CultureInfo("en-US");
                geCul.DateTimeFormat.Calendar = new System.Globalization.GregorianCalendar();
                result = dateTime.Value.ToString(format, geCul);
            }
            catch { result = String.Empty; }
            return result;
        }

        /// <summary>
        /// Returns datetime string in specific format after converting from UmAlQuraCalendar datetime string
        /// </summary>
        /// <param name="hijri">UmAlQuraCalendar datetime as string</param>
        /// <param name="Format">datetime format</param>
        /// <returns></returns>
        public static string HijriToGreg(string hijri, string Format)
        {
            string[] allFormats ={"yyyy/MM/dd","yyyy/M/d",
            "dd/MM/yyyy","d/M/yyyy",
            "dd/M/yyyy","d/MM/yyyy","yyyy-MM-dd",
            "yyyy-M-d","dd-MM-yyyy","d-M-yyyy",
            "dd-M-yyyy","d-MM-yyyy","yyyy MM dd",
            "yyyy M d","dd MM yyyy","d M yyyy","dd/MM/yyyy hh:mm",
            "dd M yyyy","d MM yyyy"};
            CultureInfo arCul = new CultureInfo("ar-SA");
            arCul.DateTimeFormat.Calendar = new UmAlQuraCalendar();
            CultureInfo enCul = new CultureInfo("en-US");
            DateTime tempDate = DateTime.ParseExact(hijri, allFormats, arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
            return tempDate.ToString(Format, enCul.DateTimeFormat);
        }

        /// <summary>
        /// Converts string to datetime using UmAlQuraCalendar
        /// </summary>
        /// <param name="DateTimeIntigerFormat">datetime string</param>
        /// <returns></returns>
        public static DateTime ConvertStringToDateTime(string DateTimeIntigerFormat)
        {
            string[] allFormats ={"yyyy/MM/dd","yyyy/M/d",
            "dd/MM/yyyy","d/M/yyyy",
            "dd/M/yyyy","d/MM/yyyy","yyyy-MM-dd",
            "yyyy-M-d","dd-MM-yyyy","d-M-yyyy",
            "dd-M-yyyy","d-MM-yyyy","yyyy MM dd",
            "yyyy M d","dd MM yyyy","d M yyyy",
            "dd M yyyy","d MM yyyy","yyyyMMdd"};
            CultureInfo arCul = new CultureInfo("ar-SA");
            arCul.DateTimeFormat.Calendar = new UmAlQuraCalendar();
            return DateTime.ParseExact(DateTimeIntigerFormat, allFormats, arCul.DateTimeFormat, DateTimeStyles.AssumeLocal);
        }

        public static DateTime? FromGregOrHijToDateTime(string date)
        {
            var geCul = new CultureInfo("en-US");
            geCul.DateTimeFormat.Calendar = new GregorianCalendar();

            try
            {
                if (IsGreg(date))
                {
                    return DateTime.ParseExact(date, "dd/MM/yyyy", geCul.DateTimeFormat, DateTimeStyles.AssumeLocal);
                    //ConvertStringToDateTime(date);
                }
                else
                {
                    return DateTime.ParseExact(HijriToGreg(date, "dd/MM/yyyy"), "dd/MM/yyyy", geCul.DateTimeFormat, DateTimeStyles.AssumeLocal);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region Serialize/Deserialize/Clone Utilities

        private static List<string> preTypes = new List<string>();
        /// <summary>
        /// Creates a clone of complex type object with all complex sub types avoiding the serialization infinit loop.
        /// </summary>
        /// <param name="objSource">Object instance to clone</param>
        /// <returns>the clone</returns>
        public static object CloneObject(object objSource)
        {
            //step : 1 Get the type of source object and create a new instance of that type
            Type typeSource = objSource.GetType();
            object objTarget = Activator.CreateInstance(typeSource);

            if (typeSource.IsGenericType)
            {
                int index = typeSource.GenericTypeArguments[0].Name.IndexOf('_');
                string typeName = index == -1 ? typeSource.GenericTypeArguments[0].Name : typeSource.GenericTypeArguments[0].Name.Substring(0, index);
                if (preTypes.Contains(typeName))
                    return objTarget;
                Convert.ChangeType(objTarget, typeSource);
                foreach (var item in (dynamic)Convert.ChangeType(objSource, typeSource))
                {
                    var value = CloneObject(item);
                    ((dynamic)objTarget).Add(value);
                }
            }

            //Step2 : Get all the properties of source object type
            PropertyInfo[] propertyInfo = typeSource.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            //Step : 3 Assign all source property to taget object 's properties
            foreach (PropertyInfo property in propertyInfo)
            {
                //Check whether property can be written to
                if (property.CanWrite)
                {
                    //Step : 4 check whether property type is value type, array, enum or string type
                    if (property.PropertyType.IsValueType || property.PropertyType.IsEnum || property.PropertyType.Equals(typeof(System.String)) || property.PropertyType.IsArray)
                    {
                        property.SetValue(objTarget, property.GetValue(objSource, null), null);
                    }
                    //else property type is object/complex types, so need to recursively call this method until the end of the tree is reached
                    else
                    {
                        object objPropertyValue = property.GetValue(objSource, null);
                        if (objPropertyValue == null)
                        {
                            property.SetValue(objTarget, null, null);
                        }
                        else
                        {
                            int index = typeSource.Name.IndexOf('_');
                            string typeName = index == -1 ? typeSource.Name : typeSource.Name.Substring(0, index);
                            if (!preTypes.Contains(typeName))
                                preTypes.Add(typeName);
                            property.SetValue(objTarget, CloneObject(objPropertyValue), null);
                        }
                    }
                }
            }
            return objTarget;
        }

        /// <summary>
        /// Serialize one flat object of T type to xml string
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="SerializedObject">Object to serialize</param>
        /// <returns>Empty string if any exception</returns>
        public static string SerializeXML<T>(T SerializedObject)
        {
            try
            {
                StringBuilder xml = new StringBuilder();
                Type objTyp = typeof(T);
                foreach (var prop in typeof(T).GetProperties().ToList())
                {
                    if (prop.PropertyType.Namespace != objTyp.Namespace &&
                        prop.PropertyType.Namespace != "System.Collections.Generic" &&
                        !prop.IsDefined(typeof(XmlIgnoreAttribute)))
                        if (prop.GetValue(SerializedObject, null) != null)
                        {
                            xml.AppendFormat("\n<{0}>{1}</{0}>", prop.Name, prop.GetValue(SerializedObject, null).ToString());
                        }
                }
                return string.Format(@"<?xml version='1.0' encoding='utf-16'?>
                   <{0} xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>{1}</{0}>", objTyp.Name, xml.ToString());
            }
            catch (Exception)
            {
                return "";
            }

        }

        /// <summary>
        /// Serialize list object of T type to xml string
        /// </summary>
        /// <typeparam name="T">List Type</typeparam>
        /// <param name="serializedObjectLst">The list object</param>
        /// <returns>Empty string if any exception</returns>
        public static string SerializeListXML<T>(List<T> serializedObjectLst)
        {
            try
            {
                StringBuilder xml = new StringBuilder();
                Type objTyp = typeof(T);
                foreach (T obj in serializedObjectLst)
                {
                    xml.AppendFormat("<{0}>", objTyp.Name);
                    foreach (var prop in typeof(T).GetProperties().ToList())
                    {
                        if (prop.PropertyType.Namespace != objTyp.Namespace &&
                            prop.PropertyType.Namespace != "System.Collections.Generic" &&
                            !prop.IsDefined(typeof(XmlIgnoreAttribute)))
                            if (prop.GetValue(obj, null) != null)
                                xml.AppendFormat("\n<{0}>{1}</{0}>", prop.Name, prop.GetValue(obj, null).ToString());
                    }
                    xml.AppendFormat("</{0}>", objTyp.Name);
                }

                return string.Format(@"<?xml version='1.0' encoding='utf-16'?>
                   <{0}s xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>{1}</{0}s>", objTyp.Name, xml.ToString());
            }
            catch
            {
                return "";
            }

        }

        /// <summary>
        /// Return object instance of type T deserialized from string
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="SerializedObj">The strialization string</param>
        /// <returns>T object</returns>
        public static T DeserializeItstring<T>(string SerializedObj)
        {
            T instance = (T)Activator.CreateInstance(typeof(T));
            byte[] byteArray = Encoding.Unicode.GetBytes(SerializedObj);
            MemoryStream stream = new MemoryStream(byteArray);
            XDocument doc = XDocument.Load(stream);
            Type objType = typeof(T);
            foreach (var prop in objType.GetProperties().ToList())
            {
                Type t = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                if (doc.Element(objType.Name).Element(prop.Name) != null)
                {
                    prop.SetValue(instance, Convert.ChangeType(doc.Element(objType.Name).Element(prop.Name).Value, t));
                }
            }
            return instance;
        }

        /// <summary>
        /// Return list object of type T deserialized from string
        /// </summary>
        /// <typeparam name="T">object type</typeparam>
        /// <param name="serializedLstObj">the serialization string of the list</param>
        /// <returns></returns>
        public static List<T> DeserializeLst<T>(string serializedLstObj)
        {
            List<T> collectionList = new List<T>();
            Type objType = typeof(T);
            T instance = default(T);
            byte[] byteArray = Encoding.Unicode.GetBytes(serializedLstObj);
            MemoryStream stream = new MemoryStream(byteArray);
            XDocument doc = XDocument.Load(stream);
            var list = doc.Root.Elements(objType.Name).ToList();
            foreach (XElement innerXMLObj in list)
            {
                instance = (T)Activator.CreateInstance(typeof(T));
                foreach (var prop in objType.GetProperties().ToList())
                {
                    Type t = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                    if (innerXMLObj.Element(prop.Name) != null)
                    {
                        prop.SetValue(instance, Convert.ChangeType(innerXMLObj.Element(prop.Name).Value, t));
                    }
                }
                collectionList.Add(instance);
            }

            return collectionList;
        }

        #endregion

        #region IO/ Temp folder Utilities

        /// <summary>
        /// Clean ~/Temp folder
        /// </summary>
        public static void CleanTempFolder()
        {
            string tempPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Temp");
            if (System.IO.Directory.Exists(tempPath))
            {
                string[] tempFiles = System.IO.Directory.GetFiles(tempPath);
                foreach (string tempFile in tempFiles)
                {
                    try
                    {
                        System.IO.File.Delete(tempFile);
                    }
                    catch { }
                }
            }
        }

        /// <summary>
        /// Returns byte array of the file and optionally delete it
        /// </summary>
        /// <param name="PathToFile">phizical path of the file</param>        
        /// <param name="deleteTheFile">If true delete the file before return</param>        
        /// <returns></returns>
        public static byte[] GetArrayFromFile(string PathToFile, bool deleteTheFile = true)
        {
            using (FileStream fs = new FileStream(PathToFile, FileMode.OpenOrCreate, FileAccess.Read))
            {
                byte[] matriz = new byte[fs.Length];
                fs.Read(matriz, 0, System.Convert.ToInt32(fs.Length));
                if (deleteTheFile)
                {
                    try { File.Delete(PathToFile); }
                    catch { }
                }
                return matriz;
            }
        }

        /// <summary>
        /// Create file with specific byteArray content
        /// </summary>
        /// <param name="fileName">file name to create</param>
        /// <param name="byteArray">file content</param>
        public static void ByteArrayToFile(string fileName, byte[] byteArray)
        {
            // Open file for reading
            using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                // Writes a block of bytes to this stream using data from
                // a byte array.
                fileStream.Write(byteArray, 0, byteArray.Length);
            }
        }

        /// <summary>
        /// Create file in temp folder with specific content and Extension. file name will be of SessionID__Guid[Ext]
        /// </summary>
        /// <param name="byteArray">File content</param>
        /// <param name="Ext">file extension</param>
        /// <returns>relative path of the saved file</returns>
        public static string ByteArrayToFileInTempFolder(byte[] byteArray, string Ext = ".pdf")
        {
            string fileName = string.Format("{0}__{1}{2}", HttpContext.Current.Session.SessionID, Guid.NewGuid().ToString(), Ext);
            string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Temp"), fileName);
            try
            {
                ByteArrayToFile(filePath, byteArray);
                return string.Format("~/Temp/{0}", fileName);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Method to resize photo to fit user profile info display. save the new image to the Temp folder
        /// </summary>
        /// <param name="originalImage">Image to resize</param>
        /// <returns>Temp file relative path</returns>
        public static string getProfilePhoto(byte[] originalImage)
        {
            string fileName = string.Format("{0}__{1}.Jpeg", HttpContext.Current.Session.SessionID, Guid.NewGuid().ToString());
            string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Temp"), fileName);
            try
            {
                //get bitmap from byte array
                Bitmap bmp;
                using (var ms = new MemoryStream(originalImage))
                {
                    bmp = new Bitmap(ms);
                    ResizeImage(bmp, 60, 60, 500, filePath);
                }

                return string.Format("~/Temp/{0}", fileName);
            }
            catch
            {
                return "";
            }

        }

        public static string ImageByteArrayToFileInThumbsFolder(string userName, byte[] byteArray)
        {
            if (byteArray == null) return "";
            string fileName = string.Format("{0}__thumb{1}", userName, ".jpg");
            string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Temp"), fileName);
            try
            {
                ByteArrayToFile(filePath, byteArray);
                return string.Format("~/Temp/{0}", fileName);
            }
            catch
            {
                return "";
            }
        }

        public static string GetUserThumbPhotoUrl(string userName)
        {
            string fileName = string.Format("{0}__thumb{1}", userName, ".jpg");
            string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Temp"), fileName);
            try
            {
                if (File.Exists(filePath))
                    return string.Format("~/Temp/{0}", fileName);
                return "";
            }
            catch
            {
                return "";
            }
        }

        #endregion

        #region Encryption

        public static RouteValueDictionary EncryptRouteValues(object routeValues)
        {
            RouteValueDictionary encryptedRouteValue = new RouteValueDictionary();
            string encryptedValue = string.Empty, queryString = string.Empty;
            String area = String.Empty;

            RouteValueDictionary d;
            if (routeValues is RouteValueDictionary)
                d = routeValues as RouteValueDictionary;
            else
                d = new RouteValueDictionary(routeValues);

            for (int i = 0; i < d.Keys.Count; i++)
            {
                if (d.Keys.ElementAt(i).ToLower() != "area")
                {
                    if (i > 0)
                        queryString += "&";

                    queryString += d.Keys.ElementAt(i) + "=" + d.Values.ElementAt(i);
                }
                else
                {
                    area = d.Values.ElementAt(i).ToString();
                }
            }

            encryptedValue = StringEncrypter.UrlEncrypter.Encrypt(queryString);

            encryptedRouteValue["q"] = HttpUtility.UrlEncode(encryptedValue);
            if (!String.IsNullOrWhiteSpace(area))
                encryptedRouteValue["Area"] = area;

            return encryptedRouteValue;
        }


        public static Dictionary<string, object> RouteValuesFromEncryptedQueryString(string encryptedQueryString)
        {
            Dictionary<string, object> decryptedParameters = new Dictionary<string, object>();

            string decryptedString = StringEncrypter.UrlEncrypter.Decrypt(encryptedQueryString);
            string[] paramsArrs = decryptedString.Split('?', '&');

            for (int i = 0; i < paramsArrs.Length; i++)
            {
                string[] paramArr = paramsArrs[i].Split('=');
                decryptedParameters.Add(paramArr[0].ToLower(), paramArr[1]);
            }

            return decryptedParameters;
        }
        #endregion

        #region Razor

        /// <summary>
        /// Returns the string represents rendered view/partial view using provided model
        /// </summary>
        /// <param name="thisController">Caller controller</param>
        /// <param name="viewName">View/partial view name</param>
        /// <param name="model">View model</param>
        /// <returns>String</returns>
        public static string RenderPartialViewToString(ControllerBase thisController, string viewName, object model)
        {
            // assign the model of the controller from which this method was called to the instance of the passed controller (a new instance, by the way)
            thisController.ViewData.Model = model;

            // initialize a string builder
            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindView(thisController.ControllerContext, viewName, null);
                // find and load the view or partial view, pass it through the controller factory

                if (viewResult.View == null)
                    viewResult = ViewEngines.Engines.FindPartialView(thisController.ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(thisController.ControllerContext, viewResult.View, thisController.ViewData, thisController.TempData, sw);

                // render it
                viewResult.View.Render(viewContext, sw);

                //return the razorized view/partial-view as a string
                return sw.ToString();
            }
        }

        /// <summary>
        /// Returns SelectList object generated from enum structure with optional DisplayName Attribute
        /// </summary>
        /// <typeparam name="T">Enum Name</typeparam>
        /// <returns></returns>
        public static SelectList EnumToSelectList<T>()
        {
            var items = Enum.GetValues(typeof(T)).Cast<T>().Select(
                enu => new SelectListItem() { Value = Convert.ToInt32(enu).ToString(), Text = GetDisplayName<T>(enu.ToString()) }).ToList().OrderBy(s => s.Text);
            return new SelectList(items, "Value", "Text");
        }

        public static SelectList EnumToSelectListFromResource<T>(Type resource)
        {
            var items = Enum.GetValues(typeof(T)).Cast<T>().Select(
                enu => new SelectListItem() { Value = Convert.ToInt32(enu).ToString(), Text = GetDisplayNameFromResource<T>(enu.ToString(), resource) });
            return new SelectList(items, "Value", "Text");
        }

        /// <summary>
        /// returns the displayName string if exists, or the property name for Enum
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="value">Enum value to get name for</param>
        /// <returns></returns>
        public static string GetDisplayName<T>(string value)
        {
            Type type = typeof(T);

            MemberInfo[] memInfo = type.GetMember(value);

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false);

                if (attrs != null && attrs.Length > 0)
                    return ((System.ComponentModel.DataAnnotations.DisplayAttribute)attrs[0]).Name;
            }

            return value;
        }


        public static string GetDisplayNameFromResource<T>(string value, Type resource)
        {
            var property = resource.GetProperty(GetDisplayName<T>(value), BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            return value = property.GetValue(null, null).ToString();
        }
        #endregion

        #region Others

        /// <summary>
        /// Generates simple random code of 5 degits
        /// </summary>
        /// <returns></returns>
        public static string GenerateRandomCode()
        {
            return (new System.Random().Next(10000, 99999).ToString());
        }

        /// <summary>
        /// Generates alphanumeric random code of specific length
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateComplexRandomCode(int length)
        {
            string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            string res = "";
            System.Random rnd = new System.Random();
            while (0 < length--)
                res += valid[rnd.Next(valid.Length)];
            return res;
        }

        /// <summary>
        /// Check if provided string is saudi number (started with 05, 009665 or +9665)
        /// </summary>
        /// <param name="mobileNo">string to check</param>
        /// <returns></returns>
        public static bool IsSaudiMobileNumber(string mobileNo)
        {
            var mobilePattern = @"^(009665|9665|\+9665|05|5)([0-9]{8})$";

            if (string.IsNullOrEmpty(mobileNo) || !Regex.IsMatch(mobileNo, mobilePattern))
                return false;

            return true;
        }

        /// <summary>
        /// Resize a given bitmap and save it to file
        /// </summary>
        /// <param name="image">bitmap to resize</param>
        /// <param name="maxWidth">Maximum width for the produced image</param>
        /// <param name="maxHeight">Maximum height for the produced image</param>
        /// <param name="quality">integer represents the quality of the output image</param>
        /// <param name="filePath">file path and name to save</param>
        private static void ResizeImage(Bitmap image, int maxWidth, int maxHeight, int quality, string filePath)
        {
            // Get the image's original width and height
            int originalWidth = image.Width;
            int originalHeight = image.Height;

            // To preserve the aspect ratio
            float ratioX = (float)maxWidth / (float)originalWidth;
            float ratioY = (float)maxHeight / (float)originalHeight;
            float ratio = Math.Min(ratioX, ratioY);

            // New width and height based on aspect ratio
            int newWidth = (int)(originalWidth * ratio);
            int newHeight = (int)(originalHeight * ratio);

            // Convert other formats (including CMYK) to RGB.
            Bitmap newImage = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);

            // Draws the image in the specified size with quality mode set to HighQuality
            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            // Get an ImageCodecInfo object that represents the JPEG codec.
            ImageCodecInfo imageCodecInfo = GetEncoderInfo(ImageFormat.Jpeg);

            // Create an Encoder object for the Quality parameter.
            System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.Quality;

            // Create an EncoderParameters object. 
            EncoderParameters encoderParameters = new EncoderParameters(1);

            // Save the image as a JPEG file with quality level.
            EncoderParameter encoderParameter = new EncoderParameter(encoder, quality);
            encoderParameters.Param[0] = encoderParameter;
            newImage.Save(filePath, imageCodecInfo, encoderParameters);
        }

        /// <summary>
        /// Method to get encoder infor for given image format.
        /// </summary>
        /// <param name="format">Image format</param>
        /// <returns>image codec info.</returns>
        private static ImageCodecInfo GetEncoderInfo(ImageFormat format)
        {
            return ImageCodecInfo.GetImageDecoders().SingleOrDefault(c => c.FormatID == format.Guid);
        }

        #endregion

        #region QR Handling

        private static Image QRGen(string input, int qrlevel)
        {
            try
            {
                string toenc = input;
                QRCodeEncoder qe = new QRCodeEncoder();
                qe.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                qe.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L; // - Using LOW for more storage
                qe.QRCodeForegroundColor = Color.Black;
                qe.QRCodeVersion = qrlevel;
                System.Drawing.Bitmap bm = qe.Encode(toenc);
                return bm;
            }
            catch (Exception ex)
            {
                throw new Exception("QR is not working", ex);
            }
        }

        public static SelectList GetDropDownData(List<SelectListViewModel> Data)
        {
            return new SelectList(Data, "Value", "Text");
        }

        public static byte[] QRGenerate(int examinationId)
        {
            try
            {
                Encryption encrypter = new Encryption();
                string url = "";
                url += FullyQualifiedApplicationPath();
                url += "VerifyCertificate/Print?q=" + encrypter.TEncrypt(examinationId.ToString()) + "";
                Image imageIn = QRGen(url, 7);
                MemoryStream ms = new MemoryStream();
                imageIn.Save(ms, ImageFormat.Bmp);
                byte[] r = ms.ToArray();

                //Save Image to check
                //var fs = new BinaryWriter(new FileStream(@"E:\\mmm.png", FileMode.Append, FileAccess.Write));
                //fs.Write(r);
                //fs.Close();
                ms.Close();
                ms.Dispose();
                return r;
            }
            catch (Exception ex)
            {
                throw new Exception("QR is not working", ex);
            }
        }

        //private static string GetQRUrl()
        //{
        //    string url = HttpContext.Current.Request.Url.AbsoluteUri;
        //    var xx = Regex.Split(url, "/Reports/FakeReport.aspx", RegexOptions.None);   //.GetLeftPart(UriPartial.Authority);// COO/Login/V // WebConfigurationManager.AppSettings["QRUrl"];
        //    return FullyQualifiedApplicationPath();//xx[0];
        //}
        private static string FullyQualifiedApplicationPath()
        {
            var appPath = string.Empty;

            //Getting the current context of HTTP request
            var context = HttpContext.Current;

            //Checking the current context content
            if (context != null)
            {
                //Formatting the fully qualified website url/name
                appPath = string.Format("{0}://{1}{2}{3}",
                                        context.Request.Url.Scheme,
                                        context.Request.Url.Host,
                                        context.Request.Url.Port == 80
                                            ? string.Empty
                                            : ":" + context.Request.Url.Port,
                                        context.Request.ApplicationPath);
            }

            if (!appPath.EndsWith("/"))
                appPath += "/";

            return appPath;
        }

        #endregion

        public static List<T> CreateNewListFromObject<T>(T item) where T : class
        {

            var list = new List<T>();
            if (item != null)
                list.Add(item);
            return list;
        }
    }
}