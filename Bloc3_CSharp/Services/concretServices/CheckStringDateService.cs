using Bloc3_CSharp.Services.abstractServices;

namespace Bloc3_CSharp.Services.concretServices
{
    public class CheckStringDateService : ICheckStringDateService
    {
        public CheckStringDateService() { }

        public bool DateIsAfterNow(string dateToCompare)
        {
            DateTime dateToCompareDT = DateTime.Parse(dateToCompare);
            return DateTime.Compare(dateToCompareDT, DateTime.Now) > 0;
        }

        public bool DateIsBeforeNow(string dateToCompare)
        {
            DateTime dateToCompareDT = DateTime.Parse(dateToCompare);
            return DateTime.Compare(dateToCompareDT, DateTime.Now) < 0;
        }

        public bool DateIsAfterOrEqualNow(string dateToCompare)
        {
            DateTime dateToCompareDT = DateTime.Parse(dateToCompare);
            return DateTime.Compare(dateToCompareDT, DateTime.Now) >= 0;
        }

        public bool DateIsBeforeOrEqualNow(string dateToCompare)
        {
            DateTime dateToCompareDT = DateTime.Parse(dateToCompare);
            return DateTime.Compare(dateToCompareDT, DateTime.Now) <= 0;
        }
    }
}
