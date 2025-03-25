namespace OnlineShop.Web.Helpers
{
    public class AppsettingsConfigService
    {
        private IConfiguration _configuration;
        public AppsettingsConfigService(IConfiguration config)
        {
                _configuration = config;

            APIPath = config["API:Path"];
            APIPathProductPicturesPath = APIPath + config["API:ProductPicturesPath"];
        }

        public string APIPath { get; set; }
        public string APIPathProductPicturesPath { get; set; }
    }
}
