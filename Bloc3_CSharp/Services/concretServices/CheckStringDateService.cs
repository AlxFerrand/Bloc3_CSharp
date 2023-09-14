using Bloc3_CSharp.Services.abstractServices;

namespace Bloc3_CSharp.Services.concretServices
{
    public class CheckStringDateService : ICheckStringDateService
    {
        public CheckStringDateService() { }

        public bool DateIsAfterNow(string dateToCompare)
        {
            DateTime toDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTime dateToCompareDT = DateTime.Parse(dateToCompare);
            return DateTime.Compare(dateToCompareDT, toDay) > 0;
        }

        public bool DateIsBeforeNow(string dateToCompare)
        {
            DateTime toDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTime dateToCompareDT = DateTime.Parse(dateToCompare);
            return DateTime.Compare(dateToCompareDT, toDay) < 0;
        }

        public bool DateIsAfterOrEqualNow(string dateToCompare)
        {
            DateTime toDay = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,0,0,0);
            DateTime dateToCompareDT = DateTime.Parse(dateToCompare);
            return DateTime.Compare(dateToCompareDT, toDay) >= 0;
        }

        public bool DateIsBeforeOrEqualNow(string dateToCompare)
        {
            DateTime toDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTime dateToCompareDT = DateTime.Parse(dateToCompare);
            return DateTime.Compare(dateToCompareDT, toDay) <= 0;
        }

        public bool DateOneIsAfterDateTwo(string dateOne, string dateTwo)
        {
            DateTime dateOneDt = DateTime.Parse(dateOne);
            DateTime dateTwoDt = DateTime.Parse(dateTwo);
            return DateTime.Compare(dateOneDt, dateTwoDt) > 0;
        }
    }
}
