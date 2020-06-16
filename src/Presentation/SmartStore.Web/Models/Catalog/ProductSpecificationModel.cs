using SmartStore.Services.Localization;
using SmartStore.Web.Framework.Modelling;

namespace SmartStore.Web.Models.Catalog
{
    public partial class ProductSpecificationModel : ModelBase
    {
        public int SpecificationAttributeId { get; set; }
        public LocalizedValue<string> SpecificationAttributeName { get; set; }
        public LocalizedValue<string> SpecificationAttributeOption { get; set; }
    }
    public partial class DeclarationProductSpecificationModel : ModelBase
    {
        public int SpecificationAttributeId { get; set; }
        public LocalizedValue<string> SpecificationAttributeName { get; set; }
        public LocalizedValue<string> SpecificationAttributeOption { get; set; }
    }
    
}