namespace ToolsCommon
{
    public interface IPrintTemplate
    {
        string SiteName { get; set; }

        string SubnetAddr { get; set; }

        string FilePath { get; set; }
    }
}
