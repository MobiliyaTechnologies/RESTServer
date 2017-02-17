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

        [Route("api/getalertdetails/{Id}/{LogId}")]
        public HttpResponseMessage GetAlertDetails(int Id, int LogId)
        {
            HttpResponseMessage response;
            try
            {
                log.Debug("GetAlertDetails API called");
                var data = dataService.GetAlertDetails(Id, LogId);
                response = data == null ? Request.CreateResponse(HttpStatusCode.Forbidden, "Invalid User") : Request.CreateResponse(HttpStatusCode.OK, data);
                return response;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetAlertDetails API as: " + ex);
                response = Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ex);
                return response;
            }
        }

        [Route("api/getclassrooms/{Id}")]
        public HttpResponseMessage GetAllClassrooms(int Id)
        {
            log.Debug("GetAllClassrooms API Called");
            HttpResponseMessage response;
            try
            {
                var data = dataService.GetAllClassrooms(Id);
                response = data == null ? Request.CreateResponse(HttpStatusCode.Forbidden, "Invalid User") : Request.CreateResponse(HttpStatusCode.OK, data);
                return response;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetAllClassrooms as: " + ex);
                response = Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ex);
                return response;
            }
        }

        [Route("api/acknowledgealert/{Id}")]
        [HttpPost]
        public HttpResponseMessage AcknowledgeAlert(int Id, [FromBody] AlertModel alertDetail)
        {
            HttpResponseMessage response;
            try
            {
                log.Debug("AcknowledgeAlert API called");
                if (!string.IsNullOrEmpty(alertDetail.Acknowledged_By.Replace(" ", string.Empty)))
                {
                    var data = dataService.AcknowledgeAlert(Id, alertDetail);
                    response = Request.CreateResponse(HttpStatusCode.OK, data);
                    return response;
                }
                //Create an error message for returning
                response = Request.CreateResponse(HttpStatusCode.BadRequest, "Please enter User name for Acknowledgement");
                return response;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in AcknowledgeAlert API as: " + ex);
                response = Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ex);
                return response;
            }
        }
        [Route("api/deletefeedback/{Id}")]
        [HttpPost]
        public HttpResponseMessage FeedbackDelete(int Id, [FromBody] FeedbackModel feedbackdetail)
        {
            log.Debug("DeleteFeedback API called");
            HttpResponseMessage response;
            try
            {
                if (ModelState.IsValid)
                {
                    var data = dataService.FeedbackDelete(Id, feedbackdetail);
                    response = data == null ? Request.CreateResponse(HttpStatusCode.Forbidden, "Invalid User") : Request.CreateResponse(HttpStatusCode.OK, data);
                    return response;
                }
                //Create an error message for returning
                string messages = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                response = Request.CreateResponse(HttpStatusCode.BadRequest, messages);
                return response;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in DeleteFeedback API as: " + ex);
                response = Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ex);
                return response;
            }
        }
        [Route("api/updatefeedback/{Id}")]
        [HttpPost]
        public HttpResponseMessage FeedbackUpdate(int Id, [FromBody] FeedbackModel feedbackdetail)
        {
            log.Debug("UpdateFeedback API called");
            HttpResponseMessage response;
            try
            {
                if (ModelState.IsValid)
                {
                    var data = dataService.FeedbackUpdate(Id, feedbackdetail);
                    response = data == null ? Request.CreateResponse(HttpStatusCode.Forbidden, "Invalid User") : Request.CreateResponse(HttpStatusCode.OK, data);
                    return response;
                }
                //Create an error message for returning
                string messages = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                response = Request.CreateResponse(HttpStatusCode.BadRequest, messages);
                return response;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in UpdateFeedback API as: " + ex);
                response = Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ex);
                return response;
            }
        }
        [Route("api/storefeedback/{Id}")]
        [HttpPost]
        public HttpResponseMessage StoreFeedback(int Id, [FromBody] Feedback feedbackdetail)
        {
            log.Debug("StoreFeedback API called");
            HttpResponseMessage response;
            try
            {
                if (ModelState.IsValid) 
                {
                    if (feedbackdetail.AnswerID == null && feedbackdetail.FeedbackDesc == null)
                    //Create an error message for returning
                    {
                        response = Request.CreateResponse(HttpStatusCode.BadRequest, "Please choose any answer or provide description");
                        return response;
                    }
                    else
                    {
                        var data = dataService.StoreFeedback(Id, feedbackdetail);
                        response = data == null ? Request.CreateResponse(HttpStatusCode.Forbidden, "Invalid User") : Request.CreateResponse(HttpStatusCode.OK, data);
                        return response;
                    }
                }
                //Create an error message for returning
                string messages = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                response = Request.CreateResponse(HttpStatusCode.BadRequest, messages);
                return response;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in StoreFeedback API as: " + ex);
                response = Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ex);
                return response;
            }
        }
        [Route("api/getallfeedback/{Id}")]
        public HttpResponseMessage GetAllFeedback(int Id)
        {
            log.Debug("GetAllFeedback API Called");
            HttpResponseMessage response;
            try
            {
                var data = dataService.GetAllFeedback(Id);
                response = data == null ? Request.CreateResponse(HttpStatusCode.Forbidden, "Invalid User") : Request.CreateResponse(HttpStatusCode.OK, data);
                return response;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetAllFeedback as: " + ex);
                response = Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ex);
                return response;
            }
        }

        [Route("api/getallsensors/{Id}")]
        public HttpResponseMessage GetAllSensors(int Id)
        {
            log.Debug("GetAllSensors API Called");
            HttpResponseMessage response;
            try
            {
                var data = dataService.GetAllSensors(Id);
                response = data == null ? Request.CreateResponse(HttpStatusCode.Forbidden, "Invalid User") : Request.CreateResponse(HttpStatusCode.OK, data);
                return response;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetAllSensors as: " + ex);
                response = Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ex);
                return response;
            }
        }

        [Route("api/mapsensortoclass/{Id}")]
        [HttpPost]
        public HttpResponseMessage MapSensor(int Id, [FromBody] SensorModel sensorDetail)
        {
            HttpResponseMessage response;
            try
            {
                log.Debug("MapSensor API called");
                if (sensorDetail.Class_Id != null && sensorDetail.Class_Id > 0)
                {
                    var data = dataService.MapSensor(Id, sensorDetail);
                    response = Request.CreateResponse(HttpStatusCode.OK, data);
                    return response;
                }
                //Create an error message for returning
                response = Request.CreateResponse(HttpStatusCode.BadRequest, "Please enter Class Id to map sensor");
                return response;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in MapSensor API as: " + ex);
                response = Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ex);
                return response;
            }
        }

        [Route("api/getquestionanswers/{Id}")]
        public HttpResponseMessage GetQuestionAnswers(int Id)
        {
            log.Debug("GetQuestionAnswers API Called");
            HttpResponseMessage response;
            try
            {
                var data = dataService.GetQuestionAnswers(Id);
                response = data == null ? Request.CreateResponse(HttpStatusCode.Forbidden, "Invalid User") : Request.CreateResponse(HttpStatusCode.OK, data);
                return response;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetQuestionAnswers as: " + ex);
                response = Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ex);
                return response;
            }
        }

        //[Route("api/getsensorDetails/{Id}")]
        //[HttpPost]
        //public HttpResponseMessage GetSensorDetails(int Id, [FromBody] SensorDataModel sensorData)
        //{
        //    log.Debug("GetSensorDetails API Called");
        //    HttpResponseMessage response;
        //    try
        //    {
        //        var data = dataService.GetSensorDetails(Id, sensorData);
        //        response = data == null ? Request.CreateResponse(HttpStatusCode.Forbidden, "Invalid User") : Request.CreateResponse(HttpStatusCode.OK, data);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Exception occurred in GetSensorDetails as: " + ex);
        //        response = Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ex);
        //        return response;
        //    }
        //}

        [Route("api/getfeedbackcount/{Id}")]
        public HttpResponseMessage GetFeedbackCount(int Id)
        {
            log.Debug("GetFeedbackCount API Called");
            HttpResponseMessage response;
            try
            {
                var data = dataService.GetFeedbackCount(Id);
                response = data == null ? Request.CreateResponse(HttpStatusCode.Forbidden, "Invalid User") : Request.CreateResponse(HttpStatusCode.OK, data);
                return response;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetFeedbackCount as: " + ex);
                response = Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ex);
                return response;
            }
        }
    }
}
