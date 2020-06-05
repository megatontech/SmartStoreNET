namespace SmartStore.Services.Cms
{
    public class MenuInfo
    {
        #region Public Properties

        public int DisplayOrder { get; set; }

        public int Id { get; set; }

        public string SystemName { get; set; }

        public string Template { get; set; }

        public string[] WidgetZones { get; set; }

        #endregion Public Properties
    }
}