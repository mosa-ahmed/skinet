using Core.Entities;

namespace Core.Specifications
{
    //we just using all of that just to get the count the count of items so that we can populate that in our Pagination class so we are gonna make use of this inside our controller
    public class ProductWithFiltersForCountSpecfication : BaseSpecification<Product>
    {
        public ProductWithFiltersForCountSpecfication(ProductSpecParams productParams)
                : base(x =>
                 (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
                 (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) &&
                 (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId)
            )
        {
        }
    }
    //What we are gonna return from this class: ProductsWithTypesAndBrandsSpecification is the paged results, so we are not gonna know from this class: ProductsWithTypesAndBrandsSpecification alone how many items are gonna be inside that particular list as a whole
    //What we are gonna know is how many items returned after the paging has been applied, so we have this specification class : ProductWithFiltersForCountSpecfication that just handles the filteirng and gives us a count of all the items after the filters have been applied
}