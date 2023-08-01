namespace Bloc3_CSharp.Services.abstractServices
{
    public interface ICheckStringDateService
    {
        public bool DateIsAfterNow(string dateToCompare);
        public bool DateIsBeforeNow(string dateToCompare);
        public bool DateIsAfterOrEqualNow(string dateToCompare);
        public bool DateIsBeforeOrEqualNow(string dateToCompare);
        public bool DateOneIsAfterDateTwo(string dateOne, string dateTwo);
    }   
}
