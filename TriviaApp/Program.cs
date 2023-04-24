using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json; // Importing Newtonsoft.Json for JSON serialization
using System.IO;
using System.Net;
using System.Web;

namespace TriviaApp
{
    // This class represents the result of a single trivia question
    class TriviaResult
    {
        public string category; // The category of the trivia question
        public string type; // The type of the trivia question (e.g. multiple choice)
        public string difficulty; // The difficulty of the trivia question
        public string question; // The question text
        public string correct_answer; // The correct answer to the question
        public List<string> incorrect_answers; // A list of incorrect answers
    }

    // This class represents the entire response from the trivia API
    class Trivia
    {
        public int response_code; // A response code indicating success or failure
        public List<TriviaResult> results; // A list of trivia results
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Declare variables
            string url = null;
            string s = null;
            HttpWebRequest request;
            HttpWebResponse response;
            StreamReader reader;

            // Set up the API URL to fetch a single random trivia question with multiple choices
            url = "https://opentdb.com/api.php?amount=1&type=multiple";

            // Send a HTTP GET request to the API endpoint
            request = (HttpWebRequest)WebRequest.Create(url);
            response = (HttpWebResponse)request.GetResponse();

            // Read the response from the API endpoint
            reader = new StreamReader(response.GetResponseStream());
            s = reader.ReadToEnd();
            reader.Close();

            // Deserialize the JSON response from the API into a Trivia object
            Trivia trivia = JsonConvert.DeserializeObject<Trivia>(s);

            // Decode any HTML entities in the incorrect answer options
            for (int i = 0; i < trivia.results[0].incorrect_answers.Count; ++i)
            {
                trivia.results[0].incorrect_answers[i] = HttpUtility.HtmlDecode(trivia.results[0].incorrect_answers[i]);
            }
        }
    }
}
