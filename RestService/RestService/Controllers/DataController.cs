using RestService.Entities;
using RestService.Models;
using RestService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RestService.Controllers
{
    public class DataController : ApiController
    {
        DataService dataService;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DataController()
        {
            dataService = new DataService();
        }

        // GET: api/Data
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Data/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Data
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Data/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Data/5
        public void Delete(int id)
        {
        }

        [Route("api/getmeterlist/{Id}")]
        public HttpResponseMessage GetMeterList(int Id)
        {
            HttpResponseMessage response;
            try
            {
                log.Debug("GetMeterList API called");
                var data = dataService.GetMeterList(Id);
                response = data == null ? Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Invalid User") : Request.CreateResponse(HttpStatusCode.OK, data);
                return response;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetMeterList as: " + ex);
                response = Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ex);
                return response;
            }
        }

        [Route("api/getmonthlyconsumption/{Id}")]
        public HttpResponseMessage GetMeterMonthlyConsumption(int Id)
        {
            HttpResponseMessage response;
            try
            {
                log.Debug("GetMonthlyConsumption API called");
                var data = dataService.GetMeterMonthlyConsumption(Id);
                response = data == null ? Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Invalid User") : Request.CreateResponse(HttpStatusCode.OK, data);
                return response;
            }
            catch (Exception ex)
            {
                log.Debug("Exception occurred in GetMonthlyConsumption as: " + ex);
                response = Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ex);
                return response;
            }
        }

        [Route("api/getdailyconsumption/{Id}")]
        public HttpResponseMessage GetMeterDailyConsumption(int Id)
        {
            HttpResponseMessage response;
            try
            {
                log.Debug("GetMeterDailyConsumption API called");
                var data = dataService.GetMeterDailyConsumption(Id);
                response = data == null ? Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Invalid User") : Request.CreateResponse(HttpStatusCode.OK, data);
                return response;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetMeterDailyConsumption as: " + ex);
                response = Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ex);
                return response;
            }
        }

        [Route("api/getpowerbiurl/{Id}/{MeterSerial}")]
        public HttpResponseMessage GetPowerBIURL(int Id, string MeterSerial)
        {
            HttpResponseMessage response;
            try
            {
                log.Debug("GetPowerBIURL API called");
                var data = dataService.GetPowerBIUrl(Id, MeterSerial);
                response = data == null ? Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Invalid User") : Request.CreateResponse(HttpStatusCode.OK, data);
                return response;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetPowerBIURL as: " + ex);
                response = Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ex);
                return response;
            }

        }

        [Route("api/getmonthwiseconsumption/{Id}/{Year}")]
        public HttpResponseMessage GetMonthWiseConsumption(int Id, int Year)
        {
            HttpResponseMessage response;
            try
            {
                log.Debug("GetMonthWiseConsumption API called");
                var data = dataService.GetMonthWiseConsumption(Id, Year);
                response = data == null ? Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Invalid User") : Request.CreateResponse(HttpStatusCode.OK, data);
                return response;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetMonthWiseConsumption as: " + ex);
                response = Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ex);
                return response;
            }
        }

        [Route("api/getmonthwiseconsumptionforoffset/{Id}/{Month}/{Year}/{Offset}")]
        public HttpResponseMessage GetMonthWiseConsumptionForOffset(int Id, string Month, int Year, int Offset)
        {
            HttpResponseMessage response;
            try
            {
                log.Debug("GetMonthWiseConsumptionForOffset API called");
                var data = dataService.GetMonthWiseConsumptionForOffset(Id, Month, Year, Offset);
                response = data == null ? Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Invalid User") : Request.CreateResponse(HttpStatusCode.OK, data);
                return response;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetMonthWiseConsumptionForOffset as: " + ex);
                response = Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ex);
                return response;
            }
        }

        [Route("api/getweekwisemonthlyconsumption/{Id}/{Month}/{Year}")]
        public HttpResponseMessage GetWeekWiseMonthlyConsumption(int Id, string Month, int Year)
        {
            HttpResponseMessage response;
            try
            {
                log.Debug("GetWeekWiseMonthlyConsumption API called");
                var data = dataService.GetWeekWiseMonthlyConsumption(Id, Month, Year);
                response = data == null ? Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Invalid User") : Request.CreateResponse(HttpStatusCode.OK, data);
                return response;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetWeekWiseMonthlyConsumption as: " + ex);
                response = Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ex);
                return response;
            }
        }

        [Route("api/getweekwisemonthlyconsumptionforoffset/{Id}/{Month}/{Year}/{Offset}")]
        public HttpResponseMessage GetWeekWiseMonthlyConsumptionForOffset(int Id, string Month, int Year, int Offset)
        {
            HttpResponseMessage response;
            try
            {
                log.Debug("GetWeekWiseMonthlyConsumptionForOffset API called ");
                var data = dataService.GetWeekWiseMonthlyConsumptionForOffset(Id, Month, Year, Offset);
                response = data == null ? Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Invalid User") : Request.CreateResponse(HttpStatusCode.OK, data);
                return response;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetWeekWiseMonthlyConsumptionForOffset as: " + ex);
                response = Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ex);
                return response;
            }
        }

        [Route("api/getdaywisemonthlyconsumption/{Id}/{Month}/{Year}")]
        public HttpResponseMessage GetDayWiseMonthlyConsumption(int Id, string Month, int Year)
        {
            HttpResponseMessage response;
            try
            {
                log.Debug("GetDayWiseMonthlyConsumption called");
                var data = dataService.GetDayWiseMonthlyConsumption(Id, Month, Year);
                response = data == null ? Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Invalid User") : Request.CreateResponse(HttpStatusCode.OK, data);
                return response;
            }
            catch (Exception ex)
            {
                log.Debug("Exception occurred in the GetDayWiseMonthlyConsumption as: " + ex);
                response = Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ex);
                return response;
            }
        }

        [Route("api/getdaywisenextmonthprediction/{Id}/{Month}/{Year}")]
        public HttpResponseMessage GetDayWiseNextMonthConsumptionPrediction(int Id, string Month, int Year)
        {
            HttpResponseMessage response;
            try
            {
                log.Debug("GetDayWiseNextMonthConsumptionPrediction API called");
                var data = dataService.GetDayWiseNextMonthPrediction(Id, Month, Year);
                response = data == null ? Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Invalid User") : Request.CreateResponse(HttpStatusCode.OK, data);
                return response;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetDayWiseNextMonthConsumptionPrediction as: " + ex);
                response = Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ex);
                return response;
            }
        }

        [Route("api/getdaywisecurrentmonthprediction/{Id}/{Month}/{Year}")]
        public HttpResponseMessage GetDayWiseCurrentMonthConsumptionPrediction(int Id, string Month, int Year)
        {
            HttpResponseMessage response;
            try
            {
                log.Debug("GetDayWiseCurrentMonthConsumptionPrediction API called");
                var data = dataService.GetDayWiseCurrentMonthPrediction(Id, Month, Year);
                response = data == null ? Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Invalid User") : Request.CreateResponse(HttpStatusCode.OK, data);
                return response;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetDayWiseCurrentMonthConsumptionPrediction as: " + ex);
                response = Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ex);
                return response;
            }
        }

        [Route("api/getpowerbigeneralurl/{Id}")]
        public HttpResponseMessage GetPowerBIGeneralURL(int Id)
        {
            HttpResponseMessage response;
            try
            {
                log.Debug("GetPowerBIGeneralURL API called");
                var data = dataService.GetPowerBIGeneralURL(Id);
                response = data == null ? Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Invalid User") : Request.CreateResponse(HttpStatusCode.OK, data);
                return response;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetPowerBIGeneralURL as: " + ex);
                response = Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ex);
                return response;
            }
        }

        [Route("api/getallalerts/{Id}")]
        public HttpResponseMessage GetAllAlerts(int Id)
        {
            log.Debug("GetAllAlerts API Called");
            HttpResponseMessage response;
            try
            {
                var data = dataService.GetAllAlerts(Id);
                response = data == null ? Request.CreateResponse(HttpStatusCode.Forbidden, "Invalid User") : Request.CreateResponse(HttpStatusCode.OK, data);
                return response;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetAllAlerts as: " + ex);
                response = Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ex);
                return response;
            }
        }
    }
}
