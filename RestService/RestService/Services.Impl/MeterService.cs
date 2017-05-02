namespace RestService.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using RestService.Entities;
    using RestService.Mappings;
    using RestService.Models;
    using RestService.Utilities;

    public sealed class MeterService : IMeterService, IDisposable
    {
        private readonly PowerGridEntities dbContext;
        private readonly IContextInfoAccessorService context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MeterService"/> class.
        /// </summary>
        public MeterService()
        {
            this.dbContext = new PowerGridEntities();
            this.context = new ContextInfoAccessorService();
        }

        List<MeterDailyConsumptionModel> IMeterService.GetMeterDailyConsumption(int buildingId)
        {
            var utcOffset = Convert.ToDouble(ConfigurationManager.AppSettings["UTCOffset"]);
            var meterdetails = this.GetMeterDetails(buildingId);
            var meterDailyConsumptionModel = new List<MeterDailyConsumptionModel>();
            DateTime today = DateTime.UtcNow.AddHours(utcOffset);

            if (meterdetails.Count() > 0)
            {
                foreach (var meterdetail in meterdetails)
                {
                    var dailyConsumptionDetail = this.dbContext.DailyConsumptionDetails.Where(data =>
                                            meterdetail.Serial.Equals(data.PowerScout, StringComparison.InvariantCultureIgnoreCase)
                                           && data.Timestamp.HasValue
                                           && today.Day == data.Timestamp.Value.Day
                                           && today.Month == data.Timestamp.Value.Month
                                           && today.Year == data.Timestamp.Value.Year).OrderByDescending(d => d.Timestamp).FirstOrDefault();

                    if (dailyConsumptionDetail != null)
                    {
                        meterDailyConsumptionModel.Add(new MeterDailyConsumptionModelMapping().Map(dailyConsumptionDetail));
                    }
                    else
                    {
                        meterDailyConsumptionModel.Add(new MeterDailyConsumptionModel
                        {
                            PowerScout = meterdetail.PowerScout,
                            Breaker_details = meterdetail.Breaker_details
                        });
                    }
                }
            }

            return meterDailyConsumptionModel;
        }

        List<MeterDayWiseMonthlyConsumptionPredictionModel> IMeterService.GetDayWiseCurrentMonthPrediction(int buildingId, string month, int year)
        {
            var meterdetails = this.GetMeterDetails(buildingId);
            var dayWisePredictionList = new List<MeterDayWiseMonthlyConsumptionPredictionModel>();

            DateTime monthDate;
            var isVaslidDate = DateTime.TryParse("01-" + month + "-" + year, out monthDate);

            if (meterdetails.Count() > 0)
            {
                foreach (var meterdetail in meterdetails)
                {
                    IQueryable<DailyConsumptionPrediction> dailyConsumptionPredictions = null;

                    if (isVaslidDate)
                    {
                        dailyConsumptionPredictions = this.GetDailyConsumtionPredictionForMonth(meterdetail.Serial, monthDate);
                    }

                    if (dailyConsumptionPredictions != null && dailyConsumptionPredictions.Count() > 0)
                    {
                        dayWisePredictionList.Add(this.GetMeterDayWiseMonthlyConsumptionPrediction(dailyConsumptionPredictions));
                    }
                    else
                    {
                        dayWisePredictionList.Add(new MeterDayWiseMonthlyConsumptionPredictionModel { PowerScout = meterdetail.PowerScout, Name = meterdetail.Breaker_details });
                    }
                }
            }

            return dayWisePredictionList;
        }

        List<MeterDayWiseMonthlyConsumptionModel> IMeterService.GetDayWiseMonthlyConsumption(int buildingId, string month, int year)
        {
            var meterDetails = this.GetMeterDetails(buildingId);
            List<MeterDayWiseMonthlyConsumptionModel> meterDayWiseMonthlyConsumptionModelList = new List<MeterDayWiseMonthlyConsumptionModel>();

            DateTime monthDate;
            var isVaslidDate = DateTime.TryParse("01-" + month + "-" + year, out monthDate);

            if (meterDetails.Count() > 0)
            {
                foreach (var meterDetail in meterDetails)
                {
                    IQueryable<DailyConsumptionDetails> meterDailyConsumption = null;

                    if (isVaslidDate)
                    {
                        meterDailyConsumption = this.GetDailyConsumtionDetailsForMonth(meterDetail.Serial, monthDate);
                    }

                    if (meterDailyConsumption != null && meterDailyConsumption.Count() > 0)
                    {
                        var dayWiseConsumption = this.GetMeterDayWiseMonthlyConsumptionModel(meterDailyConsumption);
                        meterDayWiseMonthlyConsumptionModelList.Add(dayWiseConsumption);
                    }
                    else
                    {
                        meterDayWiseMonthlyConsumptionModelList.Add(new MeterDayWiseMonthlyConsumptionModel { PowerScout = meterDetail.PowerScout, Name = meterDetail.Breaker_details });
                    }
                }
            }

            return meterDayWiseMonthlyConsumptionModelList;
        }

        List<MeterDayWiseMonthlyConsumptionPredictionModel> IMeterService.GetDayWiseNextMonthPrediction(int buildingId, string month, int year)
        {
            var meterdetails = this.GetMeterDetails(buildingId);
            var dayWisePredictionList = new List<MeterDayWiseMonthlyConsumptionPredictionModel>();

            DateTime monthDate;
            var isVaslidDate = DateTime.TryParse("01-" + month + "-" + year, out monthDate);

            if (isVaslidDate)
            {
                monthDate = monthDate.AddMonths(1);
            }

            if (meterdetails.Count() > 0)
            {
                foreach (var meterdetail in meterdetails)
                {
                    IQueryable<DailyConsumptionPrediction> dailyConsumptionPredictions = null;

                    if (isVaslidDate)
                    {
                        dailyConsumptionPredictions = this.GetDailyConsumtionPredictionForMonth(meterdetail.Serial, monthDate);
                    }

                    if (dailyConsumptionPredictions != null && dailyConsumptionPredictions.Count() > 0)
                    {
                        dayWisePredictionList.Add(this.GetMeterDayWiseMonthlyConsumptionPrediction(dailyConsumptionPredictions));
                    }
                    else
                    {
                        dayWisePredictionList.Add(new MeterDayWiseMonthlyConsumptionPredictionModel { PowerScout = meterdetail.PowerScout, Name = meterdetail.Breaker_details });
                    }
                }
            }

            return dayWisePredictionList;
        }

        List<MeterDetailsModel> IMeterService.GetMeterList(int buildingId)
        {
            var meterDetails = this.GetMeterDetails(buildingId);
            return new MeterDetailsModelMapping().Map(meterDetails).ToList();
        }

        List<MeterMonthlyConsumptionModel> IMeterService.GetMeterMonthlyConsumption(int buildingId)
        {
            var meterDetails = this.GetMeterDetails(buildingId);
            List<MeterMonthlyConsumptionModel> meterMonthlyConsumptionModels = new List<MeterMonthlyConsumptionModel>();
            string currentMonth = DateTime.UtcNow.ToString("MMM");

            foreach (var meterDetail in meterDetails)
            {
                var monthlyConsumptionDetail = this.dbContext.MonthlyConsumptionDetails.FirstOrDefault(m => m.PowerScout.Equals(meterDetail.Serial, StringComparison.InvariantCultureIgnoreCase)
                                                                      && m.Month.Equals(currentMonth, StringComparison.InvariantCultureIgnoreCase));
                MeterMonthlyConsumptionModel meterMonthlyConsumption = monthlyConsumptionDetail == null ? new MeterMonthlyConsumptionModel { Powerscout = meterDetail.PowerScout }
                                                                                    : new MeterMonthlyConsumptionModelMapping().Map(monthlyConsumptionDetail);

                meterMonthlyConsumption.Name = meterDetail.Breaker_details;
                meterMonthlyConsumptionModels.Add(meterMonthlyConsumption);
            }

            return meterMonthlyConsumptionModels;
        }

        double IMeterService.GetMonthlyConsumptionPerCampus(int campusId)
        {
            var meterDetails = this.GetMeterDetailsPerCampus(campusId);
            string currentMonth = DateTime.UtcNow.ToString("MMM");

            double monthly_KWH_Consumption = default(double);

            foreach (var meterDetail in meterDetails)
            {
                var monthlyConsumptionDetail = this.dbContext.MonthlyConsumptionDetails.FirstOrDefault(m => m.PowerScout.Equals(meterDetail.Serial, StringComparison.InvariantCultureIgnoreCase) && m.Month.Equals(currentMonth, StringComparison.InvariantCultureIgnoreCase));

                if (monthlyConsumptionDetail != null && monthlyConsumptionDetail.Monthly_KWH_System.HasValue)
                {
                    monthly_KWH_Consumption = monthly_KWH_Consumption + monthlyConsumptionDetail.Monthly_KWH_System.Value;
                }
            }

            return monthly_KWH_Consumption;
        }

        double IMeterService.GetMonthlyConsumptionPerBuildings(int buildingId)
        {
            var meterDetails = this.GetMeterDetails(buildingId);
            string currentMonth = DateTime.UtcNow.ToString("MMM");

            double monthly_KWH_Consumption = default(double);

            foreach (var meterDetail in meterDetails)
            {
                var monthlyConsumptionDetail = this.dbContext.MonthlyConsumptionDetails.FirstOrDefault(m => m.PowerScout.Equals(meterDetail.Serial, StringComparison.InvariantCultureIgnoreCase) && m.Month.Equals(currentMonth, StringComparison.InvariantCultureIgnoreCase));

                if (monthlyConsumptionDetail != null && monthlyConsumptionDetail.Monthly_KWH_System.HasValue)
                {
                    monthly_KWH_Consumption = monthly_KWH_Consumption + monthlyConsumptionDetail.Monthly_KWH_System.Value;
                }
            }

            return monthly_KWH_Consumption;
        }

        List<MeterMonthWiseConsumptionModel> IMeterService.GetMonthWiseConsumptionForOffset(int buildingId, string month, int year, int offset)
        {
            var meterDetails = this.GetMeterDetails(buildingId);
            var meterMonthWiseConsumptions = new List<MeterMonthWiseConsumptionModel>();

            DateTime endDate;
            DateTime.TryParse("01-" + month + "-" + year, out endDate);
            endDate = endDate.AddMonths(1).AddDays(-1);
            DateTime startDate = endDate.AddMonths(-offset);

            foreach (var meterDetail in meterDetails)
            {
                IQueryable<MonthlyConsumptionDetails> monthlyConsumptionDetails = from monthlyConsumptionDetail in this.dbContext.MonthlyConsumptionDetails
                                                                                  where monthlyConsumptionDetail.PowerScout.Equals(meterDetail.PowerScout, StringComparison.InvariantCultureIgnoreCase)
                                                                                  && (monthlyConsumptionDetail.Year.Equals(startDate.Year.ToString())
                                                                                  || monthlyConsumptionDetail.Year.Equals(endDate.Year.ToString()))
                                                                                  && DbFunctions.TruncateTime(monthlyConsumptionDetail.Timestamp)
                                                                                  > startDate.Date
                                                                                  && DbFunctions.TruncateTime(monthlyConsumptionDetail.Timestamp)
                                                                                  <= endDate.Date
                                                                                  orderby monthlyConsumptionDetail.Id descending
                                                                                  select monthlyConsumptionDetail;

                if (monthlyConsumptionDetails.Count() > 0)
                {
                    if (monthlyConsumptionDetails.Count() > offset)
                    {
                        monthlyConsumptionDetails = monthlyConsumptionDetails.Take((monthlyConsumptionDetails.Count() - offset) + 1);
                    }

                    var meterMonthWiseConsumption = this.GetMeterMonthWiseConsumption(monthlyConsumptionDetails);

                    if (monthlyConsumptionDetails.Count() > 0)
                    {
                        meterMonthWiseConsumption.PowerScout = monthlyConsumptionDetails.First().PowerScout;
                        meterMonthWiseConsumption.Name = monthlyConsumptionDetails.First().Breaker_details;
                    }

                    meterMonthWiseConsumptions.Add(meterMonthWiseConsumption);
                }
                else
                {
                    meterMonthWiseConsumptions.Add(new MeterMonthWiseConsumptionModel { PowerScout = meterDetail.PowerScout, Name = meterDetail.Breaker_details });
                }
            }

            return meterMonthWiseConsumptions;
        }

        List<MeterWeekWiseMonthlyConsumptionModel> IMeterService.GetWeekWiseMonthlyConsumption(int buildingId, string month, int year)
        {
            var meterDetails = this.GetMeterDetails(buildingId);
            List<MeterWeekWiseMonthlyConsumptionModel> meterWeekWiseMonthlyConsumptions = new List<MeterWeekWiseMonthlyConsumptionModel>();

            DateTime monthDate;
            var isVaslidDate = DateTime.TryParse("01-" + month + "-" + year, out monthDate);

            if (meterDetails.Count() > 0)
            {
                foreach (var meterDetail in meterDetails)
                {
                    IQueryable<DailyConsumptionDetails> dailyConsumptionDetailsForMonth = null;

                    if (isVaslidDate)
                    {
                        dailyConsumptionDetailsForMonth = this.GetDailyConsumtionDetailsForMonth(meterDetail.Serial, monthDate);
                    }

                    if (dailyConsumptionDetailsForMonth != null && dailyConsumptionDetailsForMonth.Count() > 0)
                    {
                        var meterWeekWiseMonthlyConsumption = this.GetWeekWiseConsumptionFromMonthly(dailyConsumptionDetailsForMonth);
                        meterWeekWiseMonthlyConsumptions.Add(meterWeekWiseMonthlyConsumption);
                    }
                    else
                    {
                        meterWeekWiseMonthlyConsumptions.Add(new MeterWeekWiseMonthlyConsumptionModel { PowerScout = meterDetail.PowerScout, Name = meterDetail.Breaker_details });
                    }
                }
            }

            return meterWeekWiseMonthlyConsumptions;
        }

        List<MeterWeekWiseMonthlyConsumptionModel> IMeterService.GetWeekWiseMonthlyConsumptionForOffset(int buildingId, string month, int year, int offset)
        {
            var meterDetails = this.GetMeterDetails(buildingId);
            List<MeterWeekWiseMonthlyConsumptionModel> weekWiseConsumption = new List<MeterWeekWiseMonthlyConsumptionModel>();

            DateTime monthDate;
            var isVaslidDate = DateTime.TryParse("01-" + month + "-" + year, out monthDate);

            foreach (var meterDetail in meterDetails)
            {
                MeterWeekWiseMonthlyConsumptionModel meterWeekWiseConsumption = new MeterWeekWiseMonthlyConsumptionModel();
                while (meterWeekWiseConsumption.WeekWiseConsumption.Count < offset)
                {
                    IQueryable<DailyConsumptionDetails> dailyConsumptionDetailsForMonth = null;

                    if (isVaslidDate)
                    {
                        dailyConsumptionDetailsForMonth = this.GetDailyConsumtionDetailsForMonth(meterDetail.Serial, monthDate);
                    }

                    if (dailyConsumptionDetailsForMonth == null || dailyConsumptionDetailsForMonth.Count() < 1)
                    {
                        weekWiseConsumption.Add(new MeterWeekWiseMonthlyConsumptionModel { PowerScout = meterDetail.PowerScout, Name = meterDetail.Breaker_details });
                        break;
                    }

                    var weekWiseList = this.GetWeekWiseConsumptionFromMonthly(dailyConsumptionDetailsForMonth).WeekWiseConsumption;
                    if (weekWiseList.Count + meterWeekWiseConsumption.WeekWiseConsumption.Count > offset)
                    {
                        weekWiseList.Reverse();
                        meterWeekWiseConsumption.WeekWiseConsumption.AddRange(weekWiseList.Take(offset - meterWeekWiseConsumption.WeekWiseConsumption.Count));
                    }
                    else
                    {
                        meterWeekWiseConsumption.WeekWiseConsumption.AddRange(weekWiseList);
                    }

                    monthDate = monthDate.AddMonths(-1);
                }

                meterWeekWiseConsumption.PowerScout = meterDetail.PowerScout;
                meterWeekWiseConsumption.Name = meterDetail.Breaker_details;

                weekWiseConsumption.Add(meterWeekWiseConsumption);
            }

            return weekWiseConsumption;
        }

        List<MeterMonthWiseConsumptionModel> IMeterService.GetMonthWiseConsumption(int buildingId, int year)
        {
            var meterDetails = this.GetMeterDetails(buildingId);
            List<MeterMonthWiseConsumptionModel> meterDataList = new List<MeterMonthWiseConsumptionModel>();

            foreach (var meterDetail in meterDetails)
            {
                var monthlyConsumptionDetails = this.dbContext.MonthlyConsumptionDetails.Where(m => m.PowerScout.Equals(meterDetail.Serial, StringComparison.InvariantCultureIgnoreCase) && m.Year.Equals(year.ToString()));

                MeterMonthWiseConsumptionModel meterMonthWiseConsumption = this.GetMeterMonthWiseConsumption(monthlyConsumptionDetails);

                if (meterMonthWiseConsumption != null)
                {
                    meterMonthWiseConsumption.PowerScout = meterDetail.Serial;
                    meterMonthWiseConsumption.Name = meterDetail.Breaker_details;
                    meterDataList.Add(meterMonthWiseConsumption);
                }
                else
                {
                    meterDataList.Add(new MeterMonthWiseConsumptionModel { PowerScout = meterDetail.PowerScout, Name = meterDetail.Breaker_details });
                }
            }

            return meterDataList;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            if (this.dbContext != null)
            {
                this.dbContext.Dispose();
            }
        }

        private MeterDayWiseMonthlyConsumptionModel GetMeterDayWiseMonthlyConsumptionModel(IQueryable<DailyConsumptionDetails> dailyConsumptionList)
        {
            MeterDayWiseMonthlyConsumptionModel dayWiseConsumption = new MeterDayWiseMonthlyConsumptionModel();
            dayWiseConsumption.PowerScout = dailyConsumptionList.First().PowerScout;
            dayWiseConsumption.Name = dailyConsumptionList.First().Breaker_details;
            dayWiseConsumption.DayWiseConsumption = dailyConsumptionList.Select(x => x.Daily_KWH_System ?? default(double)).ToList();

            return dayWiseConsumption;
        }

        private MeterDayWiseMonthlyConsumptionPredictionModel GetMeterDayWiseMonthlyConsumptionPrediction(IQueryable<DailyConsumptionPrediction> dailyConsumptionPredictionList)
        {
            MeterDayWiseMonthlyConsumptionPredictionModel dayWiseConsumptionPrediction = new MeterDayWiseMonthlyConsumptionPredictionModel();
            dayWiseConsumptionPrediction.PowerScout = dailyConsumptionPredictionList.First().PowerScout;
            dayWiseConsumptionPrediction.Name = dailyConsumptionPredictionList.First().Breaker_details;
            dayWiseConsumptionPrediction.DayWiseConsumptionPrediction = dailyConsumptionPredictionList.Select(x => x.Daily_Predicted_KWH_System ?? default(double)).ToList();

            return dayWiseConsumptionPrediction;
        }

        private MeterWeekWiseMonthlyConsumptionModel GetWeekWiseConsumptionFromMonthly(IQueryable<DailyConsumptionDetails> dailyConsumptionDetails)
        {
            MeterWeekWiseMonthlyConsumptionModel meterWeekWiseMonthlyConsumption = new MeterWeekWiseMonthlyConsumptionModel();

            if (dailyConsumptionDetails != null && dailyConsumptionDetails.Count() > 0)
            {
                var dailyConsumptionDetail = dailyConsumptionDetails.FirstOrDefault(d => d.Timestamp.HasValue);

                if (dailyConsumptionDetail == null)
                {
                    return meterWeekWiseMonthlyConsumption;
                }

                DateTime startDate = dailyConsumptionDetail.Timestamp.Value.Date;
                int counter = 0;

                while (counter < dailyConsumptionDetails.Count())
                {
                    int range = 8 - ServiceUtil.GetDayOfWeek(startDate.ToString("ddd"));
                    if (counter + range > dailyConsumptionDetails.Count())
                    {
                        range = dailyConsumptionDetails.Count() - counter;
                    }

                    var weekList = dailyConsumptionDetails.Skip(counter).Take(range).ToList();

                    if (!weekList.Any())
                    {
                        break;
                    }

                    meterWeekWiseMonthlyConsumption.WeekWiseConsumption.Add(weekList.Sum(data => data.Daily_KWH_System ?? default(double)));

                    dailyConsumptionDetail = weekList.LastOrDefault(w => w.Timestamp.HasValue);

                    if (dailyConsumptionDetail != null)
                    {
                        startDate = dailyConsumptionDetail.Timestamp.Value.AddDays(1);
                        counter = counter + weekList.Count();
                    }
                    else
                    {
                        break;
                    }
                }

                meterWeekWiseMonthlyConsumption.PowerScout = dailyConsumptionDetails.First().PowerScout;
            }

            return meterWeekWiseMonthlyConsumption;
        }

        private IQueryable<DailyConsumptionDetails> GetDailyConsumtionDetailsForMonth(string meterSerial, DateTime monthDate)
        {
            return from dailyConsumptionDetail in this.dbContext.DailyConsumptionDetails
                   where meterSerial.Equals(dailyConsumptionDetail.PowerScout, StringComparison.InvariantCultureIgnoreCase)
                   && dailyConsumptionDetail.Timestamp.HasValue
                   && monthDate.Month == dailyConsumptionDetail.Timestamp.Value.Month
                   && monthDate.Year == dailyConsumptionDetail.Timestamp.Value.Year
                   orderby dailyConsumptionDetail.Timestamp
                   select dailyConsumptionDetail;
        }

        private IQueryable<DailyConsumptionPrediction> GetDailyConsumtionPredictionForMonth(string meterSerial, DateTime monthDate)
        {
            return from dailyConsumptionPrediction in this.dbContext.DailyConsumptionPrediction
                   where meterSerial.Equals(dailyConsumptionPrediction.PowerScout, StringComparison.InvariantCultureIgnoreCase)
                   && dailyConsumptionPrediction.Timestamp.HasValue
                   && monthDate.Month == dailyConsumptionPrediction.Timestamp.Value.Month
                   && monthDate.Year == dailyConsumptionPrediction.Timestamp.Value.Year
                   select dailyConsumptionPrediction;
        }

        private MeterMonthWiseConsumptionModel GetMeterMonthWiseConsumption(IQueryable<MonthlyConsumptionDetails> monthlyConsumptionDetails)
        {
            MeterMonthWiseConsumptionModel meterMonthWiseConsumption = new MeterMonthWiseConsumptionModel();

            foreach (var monthlyConsumptionDetail in monthlyConsumptionDetails)
            {
                switch (monthlyConsumptionDetail.Month.ToLower())
                {
                    case "jan":
                        meterMonthWiseConsumption.MonthWiseConsumption.Jan = meterMonthWiseConsumption.MonthWiseConsumption.Jan + monthlyConsumptionDetail.Monthly_KWH_System ?? default(double);
                        break;

                    case "feb":
                        meterMonthWiseConsumption.MonthWiseConsumption.Feb = meterMonthWiseConsumption.MonthWiseConsumption.Feb + monthlyConsumptionDetail.Monthly_KWH_System ?? default(double);
                        break;

                    case "mar":
                        meterMonthWiseConsumption.MonthWiseConsumption.Mar = meterMonthWiseConsumption.MonthWiseConsumption.Mar + monthlyConsumptionDetail.Monthly_KWH_System ?? default(double);
                        break;

                    case "apr":
                        meterMonthWiseConsumption.MonthWiseConsumption.Apr = meterMonthWiseConsumption.MonthWiseConsumption.Apr + monthlyConsumptionDetail.Monthly_KWH_System ?? default(double);
                        break;

                    case "may":
                        meterMonthWiseConsumption.MonthWiseConsumption.May = meterMonthWiseConsumption.MonthWiseConsumption.May + monthlyConsumptionDetail.Monthly_KWH_System ?? default(double);
                        break;

                    case "jun":
                        meterMonthWiseConsumption.MonthWiseConsumption.Jun = meterMonthWiseConsumption.MonthWiseConsumption.Jun + monthlyConsumptionDetail.Monthly_KWH_System ?? default(double);
                        break;

                    case "jul":
                        meterMonthWiseConsumption.MonthWiseConsumption.Jul = meterMonthWiseConsumption.MonthWiseConsumption.Jul + monthlyConsumptionDetail.Monthly_KWH_System ?? default(double);
                        break;

                    case "aug":
                        meterMonthWiseConsumption.MonthWiseConsumption.Aug = meterMonthWiseConsumption.MonthWiseConsumption.Aug + monthlyConsumptionDetail.Monthly_KWH_System ?? default(double);
                        break;

                    case "sep":
                        meterMonthWiseConsumption.MonthWiseConsumption.Sep = meterMonthWiseConsumption.MonthWiseConsumption.Sep + monthlyConsumptionDetail.Monthly_KWH_System ?? default(double);
                        break;

                    case "oct":
                        meterMonthWiseConsumption.MonthWiseConsumption.Oct = meterMonthWiseConsumption.MonthWiseConsumption.Oct + monthlyConsumptionDetail.Monthly_KWH_System ?? default(double);
                        break;

                    case "nov":
                        meterMonthWiseConsumption.MonthWiseConsumption.Nov = meterMonthWiseConsumption.MonthWiseConsumption.Nov + monthlyConsumptionDetail.Monthly_KWH_System ?? default(double);
                        break;

                    case "dec":
                        meterMonthWiseConsumption.MonthWiseConsumption.Dec = meterMonthWiseConsumption.MonthWiseConsumption.Dec + monthlyConsumptionDetail.Monthly_KWH_System ?? default(double);
                        break;
                }
            }

            return meterMonthWiseConsumption;
        }

        private IQueryable<MeterDetails> GetMeterDetails(int buildingId)
        {
            var meterdetails = this.dbContext.MeterDetails.Where(m => m.BuildingId == buildingId && m.Building.Campus.Role.Any(r => r.Id == this.context.Current.RoleId));

            return meterdetails;
        }

        private IQueryable<MeterDetails> GetMeterDetailsPerCampus(int campusId)
        {
            var meterdetails = this.dbContext.MeterDetails.Where(m => m.Building.Campus.CampusID == campusId && m.Building.Campus.Role.Any(r => r.Id == this.context.Current.RoleId));

            return meterdetails;
        }
    }
}