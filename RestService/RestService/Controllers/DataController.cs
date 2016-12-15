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
                var data = dataService.GetMeterList(Id);
                response = data == null ? Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid User") : Request.CreateResponse(HttpStatusCode.OK, data);
                return response;
            }
            catch (Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ex);
                return response;
            }
        }

        //public List<MonthlyConsumptionModel> GetMeterMonthlyConsumption()
        //{
        //    return dataService.GetMeterMonthlyConsumption();
        //}
    }
}
