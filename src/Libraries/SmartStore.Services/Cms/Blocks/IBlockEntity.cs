namespace SmartStore.Services.Cms.Blocks
{
    /// <summary>
    /// Represents a block instance's storage data.
    /// </summary>
    public interface IBlockEntity
    {
        #region Public Properties

        int? BindEntityId { get; set; }

        string BindEntityName { get; set; }

        string BlockType { get; set; }

        string Body { get; set; }

        string Custom1 { get; set; }

        string Custom2 { get; set; }

        string Custom3 { get; set; }

        string Custom4 { get; set; }

        string Custom5 { get; set; }

        int Id { get; set; }

        string Model { get; set; }

        int StoryId { get; }

        string SubTitle { get; set; }

        string TagLine { get; set; }

        string Template { get; set; }

        string Title { get; set; }

        #endregion Public Properties
    }
}