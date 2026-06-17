using System;
using System.Collections.Generic;

namespace PulseLogicHMR.Core.Utilities
{
    //class for convert English Date to Nepali Date and vice versa
    public class PulseLogicDateConvertor
    {
        public PulseLogicDateConvertor()
        {

        }
        //all Metadata contain by this class
        // private NepaliDateModel nepaliDateModel = new NepaliDateModel();
        //Model for Nepali Date
        private NepaliDateType nepaliDateType = new NepaliDateType();

        //Convert Nepali Date to English Date
        //Get Nepali Date(12 hours format)
        //Return English Date (DateTime format)24 hours format
        public static DateTime ConvertNepToEngDate(NepaliDateType nepaliDate)
        {
            try
            {
                DateTime returnEnglishDate = new DateTime();
                //if anything is empty then return null value.
                if (nepaliDate.Year > 0 && nepaliDate.Month > 0 && nepaliDate.Day > 0)
                {
                    DateTime calYearStartEng = DateTime.Parse(NepaliDateModel.calYear.Find(a => a.CYear == nepaliDate.Year).YearStart);
                    List<int> yrMth = NepaliDateModel.yr_mth[nepaliDate.Year];
                    int daysToAdd = 0;
                    for (int i = 0; i < nepaliDate.Month - 1; i++)
                    {
                        daysToAdd += yrMth[i];
                    }
                    daysToAdd += nepaliDate.Day - 1;
                    string retEngDate = calYearStartEng.AddDays(daysToAdd).ToShortDateString();
                    if (nepaliDate.AMPM == "AM")
                    {
                        if (nepaliDate.Hours == 12) { nepaliDate.Hours = 0; }
                    }
                    else
                    {
                        if (nepaliDate.Hours != 12)
                        {
                            nepaliDate.Hours = nepaliDate.Hours + 12;
                        }
                    }
                    string hrsStr = nepaliDate.Hours.ToString().Length == 1 ? "0" + nepaliDate.Hours.ToString() : nepaliDate.Hours.ToString();
                    string minStr = nepaliDate.Minutes.ToString().Length == 1 ? "0" + nepaliDate.Minutes.ToString() : nepaliDate.Minutes.ToString();
                    string hrsMNTStr = hrsStr + ":" + minStr;
                    returnEnglishDate = Convert.ToDateTime(retEngDate + " " + hrsMNTStr);

                }
                return returnEnglishDate;
            }
            catch (Exception)
            {
                throw;
            }

        }

        //Convert English Date to Nepali Date
        //Get Englis Date(24 hours format) and Return Nepali Date(12 hours format)
        //English Date -DateTime.Now- format
        public static NepaliDateType ConvertEngToNepDate(DateTime engDate)
        {
            // NepaliDateType nepaliDate = new NepaliDateType();
            NepaliDateType retNepDate = new NepaliDateType();
            try
            {
                if (engDate.Year > 1900 && engDate.Year < 2032)
                {
                    string AMPM = (engDate.Hour > 11) ? "PM" : "AM";
                    int hours = Convert.ToInt32(engDate.ToString("hh"));
                    string yrStartInBs = NepaliDateModel.engYearsHash.Find(a => a.engYear == engDate.Year).yStartInBS;
                    string[] bsDate = yrStartInBs.Split('-');
                    int bsYr = Convert.ToInt32(bsDate[0]);
                    int bsMth = Convert.ToInt32(bsDate[1]);
                    int bsDay = Convert.ToInt32(bsDate[2]);
                    string ipEngDate = engDate.Year.ToString() + "-" + engDate.Month + "-" + engDate.Day;
                    string ipEngYearStart = engDate.Year.ToString() + "-01-01";
                    DateTime ipEngdt = DateTime.Parse(ipEngDate);
                    DateTime ipEngYearStartDt = DateTime.Parse(ipEngYearStart);
                    TimeSpan diffDays = ipEngdt - ipEngYearStartDt;
                    int daysToAdd = diffDays.Days;
                    List<int> nepCalMth = NepaliDateModel.GetDaysInMonthOfNext13NepaliMonthsIncludingCurrentMth(bsYr);
                    //if days to add is lesser than remaining days in currentnepalimonth, that means days will be in current nepali month only.
                    //for which Just add the remaining days.
                    if (daysToAdd <= (nepCalMth[0] - bsDay))
                    {
                        bsDay = bsDay + daysToAdd;
                    }
                    else
                    {
                        //if daystoadd is going over current month's max days, increment Month, re-calculate daysToAdd and reset bsDays.
                        if ((bsDay + daysToAdd) > nepCalMth[0])
                        {
                            bsMth += 1;
                            daysToAdd = daysToAdd - (nepCalMth[0] - bsDay) - 1;
                            bsDay = 1;

                        }
                        //loop from nextmonth of nepalicalendar.
                        //LOOP FROM 1 to 12th Index (There are fixed 13items in nepCalMths Array)
                        for (var i = 1; i < 13; i++)
                        {
                            if (daysToAdd >= (nepCalMth[i]))
                            {
                                daysToAdd = daysToAdd - (nepCalMth[i]);
                                bsMth += 1;
                                //reset year and month once month goes above 12.
                                if (bsMth > 12)
                                {
                                    bsYr += 1;
                                    bsMth = 1;
                                }
                            }
                            else
                            {
                                bsDay = bsDay + daysToAdd;
                                break;
                            }
                        }

                    }
                    int bsHours = 0;
                    int bsMinutes = 0;
                    string bsAMPM = "";
                    if (hours > 0 && hours < 13)
                    {
                        bsHours = NepaliDateModel.NepaliHoursList.Find(a => a.HoursNumberEng == hours).HoursNumberEng;
                    }
                    if (engDate.Minute >= 0 && engDate.Minute <= 59)
                    {
                        bsMinutes = NepaliDateModel.NepaliMinutesList.Find(a => a.MinutesNumberEng == engDate.Minute).MinutesNumberEng;
                    }
                    if (AMPM.Length > 0)
                    {
                        bsAMPM = AMPM;
                    }
                    retNepDate.Day = bsDay;
                    retNepDate.Month = bsMth;
                    retNepDate.Year = bsYr;
                    retNepDate.Hours = bsHours;
                    retNepDate.Minutes = bsMinutes;
                    retNepDate.AMPM = bsAMPM;

                }
                else
                {

                }
                return retNepDate;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Return todays nepali date
        public static NepaliDateType GetTodaysNepDate()
        {
            try
            {
                DateTime engDateToday = DateTime.Now;
                NepaliDateType nepDateToday = PulseLogicDateConvertor.ConvertEngToNepDate(engDateToday);
                return nepDateToday;
            }
            catch (Exception)
            {
                throw;
            }

        }
        //Convert NepaliDateType to String (yyyy.MM.dd)
        public static string NepDateToString(NepaliDateType nepDate)
        {
            if (nepDate == null) return string.Empty;
            string mth = nepDate.Month.ToString().PadLeft(2, '0');
            string day = nepDate.Day.ToString().PadLeft(2, '0');
            return $"{nepDate.Year}.{mth}.{day}";
        }
    }
}
