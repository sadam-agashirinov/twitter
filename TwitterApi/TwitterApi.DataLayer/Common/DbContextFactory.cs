namespace TwitterApi.DataLayer.Common
{
    public class DbContextFactory
    {
        public TwitterDbContext Create()
        {
            return new TwitterDbContext();
        }
    }
}