namespace SportCommentary.Helpers
{
    public class PaginationHelper : IPaginationHelper
    {
        public int CalculateTotalPages(int count, int pageSze)
        {
            return (int)Math.Ceiling(decimal.Divide(count, pageSze));
        }
    }
}
