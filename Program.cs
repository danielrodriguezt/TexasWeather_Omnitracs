using System;
using System.IO;
using System.Linq;
using System.Web;
using RestSharp;
using System.Xml;
using System.Threading.Tasks;
using System.Timers;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Web.Script.Serialization;


namespace Texas_Weather_DRT_Omnitracs
{
    class Program
    {
        static void Main(string[] args)
        {
            

                // timer callback has been reached.
                var autoEvent = new AutoResetEvent(false);
                var statusChecker = new StatusChecker(10);
                // Create a timer that invokes CheckStatus after one second and every 1/4 second thereafter.
            Console.WriteLine("Program to read Texas Weather every 5 minutes");
            var stateTimer = new System.Threading.Timer(statusChecker.CheckStatus, autoEvent, 10, 300000);// 300000 milisegundos = 5 min
            
            //TIMER DE PRUEBA var stateTimer = new System.Threading.Timer(statusChecker.CheckStatus, autoEvent, 10, 300);
            autoEvent.WaitOne();
            stateTimer.Dispose();
                //Console.WriteLine("\nDestroying timer.");
        }
    }

    class StatusChecker
    {
        private int invokeCount;
        private int maxCount;

        public StatusChecker(int count)
        {
            invokeCount = 0;
            maxCount = count;
        }

        // This method is called by the timer delegate.
        public void CheckStatus(Object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
            new_row();
            ++invokeCount;
            if (invokeCount == maxCount)
            {
                invokeCount = 0;
                autoEvent.Set();
            }
        }
        public void new_row()
        {
            //Creando el archivo
            string path = @"Texas_Weather.csv";
            string delimiter = ", ";
            if (!File.Exists(path))
            {
                string createText = "Clima en el estado de Texas" + delimiter + "Unidad" + delimiter + "Precipitacion" + delimiter + Environment.NewLine;
                File.WriteAllText(path, createText);
            }

            //Incluir secuencia de lectura de clima
             var client = new RestSharp.RestClient("https://climacell-microweather-v1.p.rapidapi.com/weather/realtime?unit_system=si&fields=temp%2Chail_binary&lat=29.7633&lon=-95.3633");//hail_binary es precipitacion
             var request = new RestSharp.RestRequest(Method.GET);
             request.AddHeader("x-rapidapi-host", "climacell-microweather-v1.p.rapidapi.com");
            // request.AddHeader("x-rapidapi-key", "4e1d142f50mshb8f07c2c55a0ea2p141785jsn95f293a27f88"); GMAIL
            request.AddHeader("x-rapidapi-key", "87c5b0a539msh427fca5c94da8abp1971e7jsn1a03e03f5a67");//MSN
            IRestResponse response = client.Execute(request);
            var jObject = JObject.Parse(response.Content);
            Temp ReadtempJSon = JsonConvert.DeserializeObject<Temp>(jObject.GetValue("temp").ToString());
            Temp ReadprepJSon = JsonConvert.DeserializeObject<Temp>(jObject.GetValue("hail_binary").ToString());
            string temptest = ReadtempJSon.Value.ToString();
            string tempUnitest = ReadtempJSon.Units.ToString();
            string unitstest = ReadprepJSon.Value.ToString();
            Boolean Precipitacion;

            if (unitstest == "1")
            {Precipitacion= true; }
            else { Precipitacion= false;}
            

            Console.WriteLine(temptest + delimiter + tempUnitest + delimiter + Precipitacion.ToString() + delimiter+ "Line number:" + invokeCount.ToString());

            string appendText = temptest + delimiter + tempUnitest + delimiter + Precipitacion.ToString() + delimiter + Environment.NewLine;
            File.AppendAllText(path, appendText);



        }

    }


    class Temp
    {
        public float Value { get; set; }
        public string Units { get; set; }
    }
}
