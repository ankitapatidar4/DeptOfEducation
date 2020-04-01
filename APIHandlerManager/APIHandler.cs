using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using QuickType.Models;

namespace QuickType.APIHandlerManager
{
    public class APIHandler
    {
        // Obtaining the API key is easy. The same key should be usable across the entire
        // data.gov developer network, i.e. all data sources on data.gov.
        // https://www.nps.gov/subjects/developer/get-started.htm

        static string BASE_URL = "http://api.data.gov/ed/collegescorecard/";
        static string API_KEY = "LZGXQNc9nEf15jyc4Sb0LX4816ZjgbUL8tJSKxxc"; //Add your API key here inside ""

        HttpClient httpClient;

        /// <summary>
        ///  Constructor to initialize the connection to the data source
        /// </summary>
        public APIHandler()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", API_KEY);
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public object JsonConvert { get; private set; }

        /// <summary>
        /// Method to receive data from API end point as a collection of objects
        /// 
        /// JsonConvert parses the JSON string into classes
        /// </summary>
        /// <returns></returns>
        public Scores GetScores()
        {
            string EDUCATION_DEPT_API_PATH = BASE_URL + "/v1/schools?limit=20";
            string scoresData = "";

            Scores scores = null;

            httpClient.BaseAddress = new Uri(EDUCATION_DEPT_API_PATH);

            // It can take a few requests to get back a prompt response, if the API has not received
            //  calls in the recent past and the server has put the service on hibernation
            try
            {
                HttpResponseMessage response = httpClient.GetAsync(EDUCATION_DEPT_API_PATH).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    scoresData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }

                if (!scoresData.Equals(""))
                {
                    // JsonConvert is part of the NewtonSoft.Json Nuget package
                    scores = JsonConvert.DeserializeObject<Scores>(scoresData);
                }
            }
            catch (Exception e)
            {
                // This is a useful place to insert a breakpoint and observe the error message
                Console.WriteLine(e.Message);
            }

            return scores;
        }
    }
}