using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Net.Http;
namespace OGCServiceCatalogue
{
    /// <summary>
    /// Manages creation of datastreams in OGS Sensorthings API
    /// </summary>
    public class ManageDataStreams
    {
        string baseUrl = "";
        string gostPrefix = "";


            /// <summary>
            /// Using the parameters it will create the necessery part in 
            /// </summary>
            /// <param name="searchAndResult"></param>
            /// <returns>True if a datastream was found or created. False if it failed</returns>
        public bool FindOrCreateDatastream(ref Models.OgcDataStreamInfo searchAndResult)
        {
            
                baseUrl = settings.GOSTServerAddress;
            
           
                gostPrefix = settings.GOSTPrefix;
          

            bool success = false;
            string thingID = FindOrCreateThing(searchAndResult.ExternalId, searchAndResult.Metadata, searchAndResult.fixedLatitude, searchAndResult.fixedLongitude,ref  success);
            if (!success)
                return false;
            string sensorid = FindOrCreateSensor(searchAndResult.SensorType, searchAndResult.UnitOfMeasurement, ref success);
            if (!success)
                return false;
            string observedPropertyId = FindOrCreateObservedProperty(searchAndResult.SensorType, searchAndResult.UnitOfMeasurement, ref success);
            if (!success)
                return false;
            string streamId = FindOrCreateDatastream(searchAndResult.ExternalId, searchAndResult.Metadata,searchAndResult.SensorType, searchAndResult.UnitOfMeasurement,thingID, observedPropertyId, sensorid, ref success);
            if (!success)
                return false;

            searchAndResult.MqttTopic = gostPrefix +"/Datastreams(" + streamId + ")/Observations";
            searchAndResult.MqttServer = settings.MQTTServerAddress;
            searchAndResult.DataStreamId = int.Parse(streamId);
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="externalid"></param>
        /// <param name="description"></param>
        /// <param name="fixedLat"></param>
        /// <param name="fixedLon"></param>
        /// <param name="success"></param>
        /// <returns></returns>
        string FindOrCreateThing(string externalid, string description, double? fixedLat, double? fixedLon,ref bool success)
        {
            string retVal;
            success = false;
            bool foundThing = false;
            string JsonResult = "";
           
            //First search if it exists
            WebClient client = new WebClient();
            try
            {
                string url = baseUrl + "Things?$filter=name eq '" + externalid + "'";
                client.Encoding = System.Text.Encoding.UTF8;
                client.Headers["Accept"] = "application/json";
                client.Headers["Content-Type"] = "application/json";
                JsonResult = client.DownloadString(url);
                dynamic jsonRes = JValue.Parse(JsonResult);
                if(jsonRes.value.Count > 0)
                {
                    retVal = jsonRes.value[0]["@iot.id"];
                    success = true;
                    return retVal;
                }
                else
                    foundThing = false;
            }
            catch (WebException exception)
            {
              
                foundThing = false;
            }

            dynamic json = new JObject();
            json.name = externalid;
            json.description = description;
            // Create Thing


            string payload = json.ToString();


     
            client = new WebClient();
            try
            {
                string url = baseUrl + "Things";
                System.Console.WriteLine("Payload:" + payload);
                client.Encoding = System.Text.Encoding.UTF8;
                client.Headers["Accept"] = "application/json";
                client.Headers["Content-Type"] = "application/json";

                JsonResult = client.UploadString(url, payload);
                dynamic jsonRes = JValue.Parse(JsonResult);
                retVal = jsonRes["@iot.id"];

            }
            catch (WebException exception)
            {
                System.Console.WriteLine("Create Thing failed:" + exception.Message);
                success = false;
                return "";
            }

            json = new JObject();
            json.name = externalid;
            json.description = description;
            json.encodingType = "application/vnd.geo+json";

            if (fixedLon == null) fixedLon = 18.034640;
            if (fixedLat == null) fixedLat = 59.399364;
            dynamic loc = new JObject();
            loc.type = "Point";
            JArray coord = new JArray();
            JValue lon = new JValue(fixedLon);
            JValue lat = new JValue(fixedLat);
            coord.Add(lon);
            coord.Add(lat);

            loc.coordinates = coord;
           
            //Create Location
            json.location = loc;
            payload = json.ToString();
//            


            client = new WebClient();
            try
            {
                string url = baseUrl + "Things("+retVal+")/Locations";
                System.Console.WriteLine("Payload:" + payload);
                client.Encoding = System.Text.Encoding.UTF8;
                client.Headers["Accept"] = "application/json";
                client.Headers["Content-Type"] = "application/json";
                JsonResult = client.UploadString(url, payload);
             
            }
            catch (WebException exception)
            {
                System.Console.WriteLine("Create Thing failed:" + exception.Message);
                success = false;
                return "";
            }



            success = true;
            return retVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SensorType"></param>
        /// <param name="UnitOfMeasurement"></param>
        /// <param name="success"></param>
        /// <returns></returns>
        string FindOrCreateSensor(string SensorType, string UnitOfMeasurement, ref bool success)
        {
            string retVal;
            success = false;
            bool foundSensor = false;
            string JsonResult = "";

            //First search if it exists
            WebClient client = new WebClient();
            try
            {
                string url = baseUrl + "Sensors?$filter=name eq '" + SensorType + "'";
                client.Encoding = System.Text.Encoding.UTF8;
                client.Headers["Accept"] = "application/json";

                JsonResult = client.DownloadString(url);
                dynamic jsonRes = JValue.Parse(JsonResult);
                if (jsonRes.value.Count > 0)
                {
                    retVal = jsonRes.value[0]["@iot.id"];
                    success = true;
                    return retVal;
                }
                else
                    foundSensor = false;
            }
            catch (WebException exception)
            {

                foundSensor = false;
            }

            dynamic json = new JObject();
            json.name = SensorType;
            json.description = UnitOfMeasurement;
            json.encodingType = "application/pdf";
            json.metadata = "https://en.wikipedia.org/wiki/"+ SensorType;
            // Create Thing
            string payload = json.ToString();
                




            client = new WebClient();
            try
            {
                string url = baseUrl + "Sensors";
                System.Console.WriteLine("Payload:" + payload);
                client.Encoding = System.Text.Encoding.UTF8;
                client.Headers["Accept"] = "application/json";
                client.Headers["Content-Type"] = "application/json";
       
                JsonResult = client.UploadString(url, payload);
                dynamic jsonRes = JValue.Parse(JsonResult);
                retVal = jsonRes["@iot.id"];
           
            }
            catch (WebException exception)
            {
                System.Console.WriteLine("Create Thing failed:" + exception.Message);
                success = false;
                return "";
            }

     



            success = true;
            return retVal;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="SensorType"></param>
        /// <param name="UnitOfMeasurement"></param>
        /// <param name="success"></param>
        /// <returns></returns>
        string FindOrCreateObservedProperty(string SensorType, string UnitOfMeasurement, ref bool success)
        {
            string retVal;
            success = false;
            bool foundSensor = false;
            string JsonResult = "";

            //First search if it exists
            WebClient client = new WebClient();
            try
            {
                string url = baseUrl + "ObservedProperties?$filter=name eq '" + SensorType + "'";
                client.Encoding = System.Text.Encoding.UTF8;
                client.Headers["Accept"] = "application/json";
                client.Headers["Content-Type"] = "application/json";

              
                JsonResult = client.DownloadString(url);
                dynamic jsonRes = JValue.Parse(JsonResult);
                if (jsonRes.value.Count > 0)
                {
                    retVal = jsonRes.value[0]["@iot.id"];
                    success = true;
                    return retVal;
                }
                else
                    foundSensor = false;
            }
            catch (WebException exception)
            {

                foundSensor = false;
            }

            dynamic json = new JObject();
            json.name = SensorType;
            json.description = UnitOfMeasurement;
            json.definition = "http://mmisw.org/ont/ioos/parameter/" + SensorType;
            // Create Thing
            string payload = json.ToString();
//  

            client = new WebClient();
            try
            {
                string url = baseUrl + "ObservedProperties";
                System.Console.WriteLine("Payload:" + payload);
                client.Encoding = System.Text.Encoding.UTF8;
                client.Headers["Accept"] = "application/json";
                client.Headers["Content-Type"] = "application/json";
             

                JsonResult = client.UploadString(url, payload);

                dynamic jsonRes = JValue.Parse(JsonResult);
                retVal = jsonRes["@iot.id"];

            }
            catch (WebException exception)
            {
                System.Console.WriteLine("Create Thing failed:" + exception.Message);
                success = false;
                return "";
            }





            success = true;
            return retVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ExternalId"></param>
        /// <param name="Metadata"></param>
        /// <param name="SensorType"></param>
        /// <param name="UnitOfMeasurement"></param>
        /// <param name="thingid"></param>
        /// <param name="ObservedPropertyid"></param>
        /// <param name="Sensorid"></param>
        /// <param name="success"></param>
        /// <returns></returns>
        string FindOrCreateDatastream(string ExternalId, string Metadata, string SensorType, string UnitOfMeasurement, string thingid, string ObservedPropertyid, string Sensorid, ref bool success)
        {
            string retVal;
            success = false;
            bool foundDatastream = false;
            string JsonResult = "";

            //First search if it exists
            WebClient client = new WebClient();
            try
            {
                string searchstring = ExternalId + ":" + SensorType;
                string url = baseUrl + "Datastreams?$filter=name eq '" + searchstring + "'";
                client.Encoding = System.Text.Encoding.UTF8;
                client.Headers["Accept"] = "application/json";
                client.Headers["Content-Type"] = "application/json";
                
                JsonResult = client.DownloadString(url);
                dynamic jsonRes = JValue.Parse(JsonResult);
                if (jsonRes.value.Count > 0)
                {
                    
                    retVal = jsonRes.value[0]["@iot.id"];
                    success = true;
                    return retVal;
                }
                else
                    foundDatastream = false;
            }
            catch (WebException exception)
            {

                foundDatastream = false;
            }

            dynamic json = new JObject();
            json.name = ExternalId + ":" + SensorType;
            json.description = ExternalId + ":" + SensorType;
            json.observationType = "http://www.opengis.net/def/observationType/OGC-OM/2.0/OM_Observation";

            dynamic uom = new JObject();
            uom.symbol = UnitOfMeasurement;
            uom.name = SensorType;
            uom.definition = "http://unitsofmeasure.org/ucum.html#para-30";

            json.unitOfMeasurement = uom;

            dynamic thing = new JObject();
            thing["@iot.id"] = int.Parse(thingid);

            json.Thing = thing;

            dynamic ObservedProperty = new JObject();
            ObservedProperty["@iot.id"] = int.Parse(ObservedPropertyid);

            json.ObservedProperty = ObservedProperty;

            dynamic Sensor = new JObject();
            Sensor["@iot.id"] = int.Parse(Sensorid);

            json.Sensor = Sensor;
            // Create Thing
            string payload = json.ToString();
                

            client = new WebClient();
            try
            {
                string url = baseUrl + "Datastreams";
                System.Console.WriteLine("Payload:" + payload);
                client.Encoding = System.Text.Encoding.UTF8;
                client.Headers["Accept"] = "application/json";
                client.Headers["Content-Type"] = "application/json";
              
                JsonResult = client.UploadString(url, payload);
                dynamic jsonRes = JValue.Parse(JsonResult);
                retVal = jsonRes["@iot.id"];

            }
            catch (WebException exception)
            {
                System.Console.WriteLine("Create Thing failed:" + exception.Message);
                success = false;
                return "";
            }





            success = true;
            return retVal;
        }
    }
}
