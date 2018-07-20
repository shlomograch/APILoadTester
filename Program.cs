using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RestSharp;

namespace APILoadTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Debug.WriteLine("Starting test...");


            Class1 t = new Class1();
            t.MainMethod();

            //int CurCnt = 1;
            //int MaxCnt = 5;


            //var taskTimer = Task.Factory.StartNew(async () =>
            //    {
            //        var result = ApiServices.ApiGet("http://dev.intsvc.nelnet.net:4106/api/v1/states/validate/", "USA/CO");
            //        Debug.WriteLine("Result: " + result.StatusCode + "   Count = " + CurCnt);
            //        CurCnt++;

            //    },
            //    TaskCreationOptions.LongRunning);


            Debug.WriteLine("Ending test...");
            Console.ReadLine();
        }
    }


    public static class ApiServices
    {
        internal static string UriBase { get; set; }
        internal static string UriMethodPath { get; set; }
        private static RestClient Client { get; set; }

        public static IRestResponse ApiGet(string baseUri, string uriMethodPath)
        {
            UriBase = baseUri;
            UriMethodPath = uriMethodPath;

            try
            {
                Client = new RestClient(UriBase);
                var restRequest = new RestRequest(UriMethodPath, Method.GET);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.AddHeader("UserName", "");
                restRequest.AddHeader("StarKey", "");
                return Client.Execute(restRequest);
            }

            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }

            return null;
        }
    }
}
