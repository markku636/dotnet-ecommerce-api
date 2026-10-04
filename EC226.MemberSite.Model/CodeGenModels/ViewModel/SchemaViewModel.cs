namespace EC226.MemberSite.Model.CodeGenModels.Models.ViewModel
{
    public class CreateSchemaKeyReqBody
    {
        public string Key { get; set; }
    }
    public class CreateSchemaKeyResponseData
    {
        public string CreatedKey { get; set; }
    }

    public class ModifySchemaReqBody
    {
        public string Description { get; set; }
        public string FormSchema { get; set; }
        public string TableColumnData { get; set; }
    }
}
