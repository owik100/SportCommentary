namespace SportCommentary.Helpers
{
    public interface IPaginationHelper
    {
        int CalculateTotalPages(int count, int pageSze);
    }
}
