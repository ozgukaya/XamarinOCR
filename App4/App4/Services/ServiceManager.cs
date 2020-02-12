using App4.Models.Request;
using App4.Models.Response;
using App4.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace App4.Services
{
    public class ServiceManager
    {
        public async Task<string> GetTextListWithUrl(UrlRequest request)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", APIKeys.OCRAPIKey);

                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                var response =await client.PostAsync(EndPoints.OCRAPI, content);

                var result =await response.Content.ReadAsStringAsync();

                var result2 = JsonConvert.DeserializeObject<OCRAPIResponse>(result);

                StringBuilder stringBuilder = new StringBuilder();

                if (result != null && result2.regions != null)
                {
                    foreach (var item in result2.regions)
                    {

                        foreach (var line in item.lines)
                        {
                            foreach (var word in line.words)
                            {
                                stringBuilder.Append(word.text);
                                stringBuilder.Append(" ");
                            }
                            stringBuilder.AppendLine();
                        }
                        stringBuilder.AppendLine();
                    }
                }
                return stringBuilder.ToString();
            }
        }

        public async Task<string> GetTextListWithCamera(Stream image)
        {
            using ( var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", APIKeys.OCRAPIKey);
                
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));

                var content = new StreamContent(image);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                var response = await client.PostAsync(EndPoints.OCRAPI, content);
                var result = await response.Content.ReadAsStringAsync();
                
                var result2 = JsonConvert.DeserializeObject<OCRAPIResponse>(result);

                StringBuilder stringBuilder = new StringBuilder();

                if (result != null && result2.regions != null)
                {
                    foreach (var item in result2.regions)
                    {

                        foreach (var line in item.lines)
                        {
                            foreach (var word in line.words)
                            {
                                stringBuilder.Append(word.text);
                                stringBuilder.Append(" ");
                            }
                            stringBuilder.AppendLine();
                        }
                        stringBuilder.AppendLine();
                    }
                }
                return stringBuilder.ToString();

            }
        }
       
    }
}
