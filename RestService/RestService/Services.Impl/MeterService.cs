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
            var meterdetails = this.GetMeterDetails(buildingId);
            var meterDailyConsumptionModel = new List<MeterDailyConsumptionModel>();

            if (meterdetails.Count() > 0)
            {
                DateTime today = ServiceUtil.GetCurrentDateTime(meterdetails.First().UTCConversionTime);

                foreach (var meterdetail in meterdetails)
                {
                    var dailyConsumptionDetail = this.dbContext.DailyConsumptionDetails.Where(data =>
                                            meterdetail.PowerScout.Equals(data.PowerScout, StringComparison.InvariantCultureIgnoreCase)
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
                        dailyConsumptionPredictions = this.GetDailyConsumtionPredictionForMonth(meterdetail.PowerScout, monthDate);
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
                        meterDailyConsumption = this.GetDailyConsumtionDetailsForMonth(meterDetail.PowerScout, monthDate);
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
                        dailyConsumptionPredictions = this.GetDailyConsumtionPredictionForMonth(meterdetail.PowerScout, monthDate);
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
            var meterDetailModels = new List<MeterDetailsModel>();

            foreach (var meterDetail in meterDetails.ToList())
            {
                var meterDetailModel = new MeterDetailsModel
                {
                    Id = meterDetail.Id,
                    PowerScout = meterDetail.PowerScout,
                    Name = meterDetail.Breaker_details,
                    MonthlyConsumption = this.GetMonthlyConsumption(new List<MeterDetails> { meterDetail }.AsQueryable()),
                    MonthlyPrediction = this.GetMonthlyPrediction(new List<MeterDetails> { meterDetail }.AsQueryable())
                };
                meterDetailModels.Add(meterDetailModel);
            }

            return meterDetailModels;
        }

        List<MeterMonthlyConsumptionModel> IMeterService.GetMeterMonthlyConsumption(int buildingId)
        {
            var meterDetails = this.GetMeterDetails(buildingId);
            List<MeterMonthlyConsumptionModel> meterMonthlyConsumptionModels = new List<MeterMonthlyConsumptionModel>();

            foreach (var meterDetail in meterDetails)
            {
                var currentDate = ServiceUtil.GetCurrentDateTime(meterDetail.UTCConversionTime);

                var monthlyConsumptionDetail = this.dbContext.MonthlyConsumptionDetails.FirstOrDefault(m => m.PowerScout.Equals(meterDetail.PowerScout, StringComparison.InvariantCultureIgnoreCase) && m.Month.Equals(currentDate.ToString("MMM"), StringComparison.InvariantCultureIgnoreCase) && m.Year.Equals(currentDate.Year.ToString()));

                MeterMonthlyConsumptionModel meterMonthlyConsumption = monthlyConsumptionDetail == null ? new MeterMonthlyConsumptionModel { Powerscout = meterDetail.PowerScout }
                                                                                    : new MeterMonthlyConsumptionModelMapping().Map(monthlyConsumptionDetail);

                meterMonthlyConsumption.Name = meterDetail.Breaker_details;
                meterMonthlyConsumptionModels.Add(meterMonthlyConsumption);
            }

            return meterMonthlyConsumptionModels;
        }

        ConsumptionPredictionModel IMeterService.GetMonthlyConsumptionPredictionPerPremise(int premiseID)
        {
            var meterDetails = this.GetMeterDetailsPerPremise(premiseID);
            return this.GetConsumptionPrediction(meterDetails);
        }

        ConsumptionPredictionModel IMeterService.GetMonthlyConsumptionPredictionPerBuildings(int buildingId)
        {
            var meterDetails = this.GetMeterDetails(buildingId);
            return this.GetConsumptionPrediction(meterDetails);
        }

        private ConsumptionPredictionModel GetConsumptionPrediction(IQueryable<MeterDetails> meterDetails)
        {
            DateTime startDate, endDate;
            QueryableExtention.GetStartAndEndDate(out startDate, out endDate);

            var inDateRange = startDate != DateTime.MinValue || endDate != DateTime.MinValue;

            var cunsumptionPrediction = new ConsumptionPredictionModel
            {
                Consumption = inDateRange ? this.GetGivenDateConsumption(meterDetails, startDate, endDate) : this.GetMonthlyConsumption(meterDetails),
                Prediction = inDateRange ? this.GetGivenDatePrediction(meterDetails, startDate, endDate) : this.GetMonthlyPrediction(meterDetails)
            };

            return cunsumptionPrediction;
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
                        dailyConsumptionDetailsForMonth = this.GetDailyConsumtionDetailsForMonth(meterDetail.PowerScout, monthDate);
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

        List<MeterMonthWiseConsumptionModel> IMeterService.GetMonthWiseConsumption(int buildingId, int year)
        {
            var meterDetails = this.GetMeterDetails(buildingId);
            List<MeterMonthWiseConsumptionModel> meterDataList = new List<MeterMonthWiseConsumptionModel>();

            foreach (var meterDetail in meterDetails)
            {
                var monthlyConsumptionDetails = this.dbContext.MonthlyConsumptionDetails.Where(m => m.PowerScout.Equals(meterDetail.PowerScout, StringComparison.InvariantCultureIgnoreCase) && m.Year.Equals(year.ToString()));

                MeterMonthWiseConsumptionModel meterMonthWiseConsumption = this.GetMeterMonthWiseConsumption(monthlyConsumptionDetails);

                if (meterMonthWiseConsumption != null)
                {
                    meterMonthWiseConsumption.PowerScout = meterDetail.PowerScout;
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
            var meterdetails = this.dbContext.MeterDetails.WhereActiveAccessibleMeterDetails(m => m.BuildingId == buildingId);
            return meterdetails;
        }

        private IQueryable<MeterDetails> GetMeterDetailsPerPremise(int premiseID)
        {
            var meterdetails = this.dbContext.MeterDetails.WhereActiveAccessibleMeterDetails(m => m.Building.Premise.PremiseID == premiseID);
            return meterdetails;
        }

        private double GetMonthlyConsumption(IQueryable<MeterDetails> meterDetails)
        {
            double monthly_KWH_Consumption = default(double);

            foreach (var meterDetail in meterDetails)
            {
                var currentMonth = ServiceUtil.GetCurrentDateTime(meterDetail.UTCConversionTime);

                var monthlyConsumptionDetail = this.dbContext.MonthlyConsumptionDetails.FirstOrDefault(m => m.PowerScout.Equals(meterDetail.PowerScout, StringComparison.InvariantCultureIgnoreCase) && m.Timestamp.HasValue && m.Timestamp.Value.Month == currentMonth.Month && m.Timestamp.Value.Year == currentMonth.Year);

                if (monthlyConsumptionDetail != null && monthlyConsumptionDetail.Monthly_KWH_System.HasValue)
                {
                    monthly_KWH_Consumption = monthly_KWH_Consumption + monthlyConsumptionDetail.Monthly_KWH_System.Value;
                }
            }

            return monthly_KWH_Consumption;
        }

        private double GetMonthlyPrediction(IQueryable<MeterDetails> meterDetails)
        {
            double monthly_KWH_Prediction = default(double);

            foreach (var meterDetail in meterDetails)
            {
                var currentMonth = ServiceUtil.GetCurrentDateTime(meterDetail.UTCConversionTime);

                var dailyPredictions = (from dailyConsumptionPrediction in this.dbContext.DailyConsumptionPrediction
                                        where meterDetail.PowerScout.Equals(dailyConsumptionPrediction.PowerScout, StringComparison.InvariantCultureIgnoreCase)
                                        && dailyConsumptionPrediction.Timestamp.HasValue && dailyConsumptionPrediction.Daily_Predicted_KWH_System.HasValue
                                        && currentMonth.Month == dailyConsumptionPrediction.Timestamp.Value.Month
                                        && currentMonth.Year == dailyConsumptionPrediction.Timestamp.Value.Year
                                        select dailyConsumptionPrediction.Daily_Predicted_KWH_System).Sum();

                if (dailyPredictions.HasValue)
                {
                    monthly_KWH_Prediction = monthly_KWH_Prediction + dailyPredictions.Value;
                }
            }

            return monthly_KWH_Prediction;
        }

        private double GetGivenDateConsumption(IQueryable<MeterDetails> meterDetails, DateTime startDate, DateTime endDate)
        {
            double daily_KWH_Consumption = default(double);

            foreach (var meterDetail in meterDetails)
            {
                var dailyConsumptionDetail = this.dbContext.DailyConsumptionDetails.Where(m => m.PowerScout.Equals(meterDetail.PowerScout, StringComparison.InvariantCultureIgnoreCase) && m.Daily_KWH_System.HasValue && m.Timestamp.HasValue);

                if (startDate != DateTime.MinValue)
                {
                    dailyConsumptionDetail = dailyConsumptionDetail.Where(s => DbFunctions.TruncateTime(s.Timestamp) >= startDate);
                }

                if (endDate != DateTime.MinValue)
                {
                    dailyConsumptionDetail = dailyConsumptionDetail.Where(s => DbFunctions.TruncateTime(s.Timestamp) <= endDate);
                }

                var consumptionSum = dailyConsumptionDetail.Sum(d => d.Daily_KWH_System);

                if (consumptionSum.HasValue)
                {
                    daily_KWH_Consumption = daily_KWH_Consumption + consumptionSum.Value;
                }
            }

            return daily_KWH_Consumption;
        }

        private double GetGivenDatePrediction(IQueryable<MeterDetails> meterDetails, DateTime startDate, DateTime endDate)
        {
            double daily_KWH_Prediction = default(double);

            foreach (var meterDetail in meterDetails)
            {
                var dailyPredictions = this.dbContext.DailyConsumptionPrediction.Where(dailyConsumptionPrediction =>
                                        meterDetail.PowerScout.Equals(dailyConsumptionPrediction.PowerScout, StringComparison.InvariantCultureIgnoreCase)
                                       && dailyConsumptionPrediction.Timestamp.HasValue && dailyConsumptionPrediction.Daily_Predicted_KWH_System.HasValue
                                        );

                if (startDate != DateTime.MinValue)
                {
                    dailyPredictions = dailyPredictions.Where(s => DbFunctions.TruncateTime(s.Timestamp) >= startDate);
                }

                if (endDate != DateTime.MinValue)
                {
                    dailyPredictions = dailyPredictions.Where(s => DbFunctions.TruncateTime(s.Timestamp) <= endDate);
                }

                var predictionSum = dailyPredictions.Sum(d => d.Daily_Predicted_KWH_System);

                if (predictionSum.HasValue)
                {
                    daily_KWH_Prediction = daily_KWH_Prediction + predictionSum.Value;
                }
            }

            return daily_KWH_Prediction;
        }
    }
}