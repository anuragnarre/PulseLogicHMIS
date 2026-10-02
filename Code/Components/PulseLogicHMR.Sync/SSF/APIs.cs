using PulseLogicHMR.ServerModel.SSFModels;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace PulseLogicHMR.Sync.SSF
{
    public class APIs
    {
        public static async System.Threading.Tasks.Task<SSF_RealTimeBookingServiceResponse> BookClaimAsync(object claimBooking, SSFCredentials ssfCred)
        {
            using (var client = new HttpClient())
            {
                var ISO_8859_1 = Encoding.GetEncoding("ISO-8859-1");
                var svcCredentials = Convert.ToBase64String(ISO_8859_1.GetBytes(ssfCred.SSFUsername + ":" + ssfCred.SSFPassword));
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + svcCredentials);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add(ssfCred.SSFRemotekey, ssfCred.SSFRemoteValue);
                client.BaseAddress = new Uri(ssfCred.SSFurl);
                var jsonContent = JsonConvert.SerializeObject(claimBooking);
                StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"BookingService", content);
                SSF_RealTimeBookingServiceResponse realTimeBookingServiceResponse = new SSF_RealTimeBookingServiceResponse();
                if (response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    realTimeBookingServiceResponse.ResponseData = message;
                    realTimeBookingServiceResponse.BookingStatus = true;
                    return realTimeBookingServiceResponse;
                }
                else
                {
                    var errorString = await response.Content.ReadAsStringAsync();
                    realTimeBookingServiceResponse.ResponseData = errorString;
                    realTimeBookingServiceResponse.BookingStatus = false;
                    return realTimeBookingServiceResponse;
                }
            }
        } 
    }
}
