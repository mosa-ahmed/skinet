namespace Core.Specifications
{
    //we have created this class to store all our parameters that we wnat to pass to our method in the controller, we can pass them individually but a better way is to use a class
    //so we will set up our parameters that we are going to take for our products controller when we get a list of products and we want to be filtering or paging or whatever
    public class ProductSpecParams
    {
        private const int MaxPageSize = 50;
        public int PageIndex { get; set; } = 1;
        private int _pageSize = 6;
        public int PageSize 
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string Sort { get; set; }

        //we want always to check against a lowercase, so we will add a backing field and the public property
        private string _search;
        public string Search 
        { 
            get => _search;
            set => _search = value.ToLower();
            //and this will make sure that whenever a search term comes into our API, we simply gonna convert it into lowercase
        } 
    }
}