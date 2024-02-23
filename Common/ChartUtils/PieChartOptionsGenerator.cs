using BlazorBootstrap;

namespace Common.ChartUtils;

public class PieChartOptionsGenerator
{
    #region Fields
    private PieChartOptions _options;
    #endregion

    #region Constructors
    public PieChartOptionsGenerator(string strDefaultTitle)
    {
        _options = new PieChartOptions();
        CreateDefaultChartOptions();
        _options.Plugins.Title!.Text = strDefaultTitle;
    }
    #endregion

    #region Publics
    public PieChartOptions GetOptions()
    {
        return _options;
    }
    #endregion

    #region Privates
    private void CreateDefaultChartOptions()
    {
        _options = new PieChartOptions
                   {
                       Responsive = true,
                       Plugins = new PieChartPlugins
                                 {
                                     Title = new ChartPluginsTitle
                                             {
                                                 Display = true
                                             },
                                     Legend =
                                     {
                                         Position = "bottom",
                                         Align = "left"
                                     }
                                 }
                   };
        _options.Plugins.Title.Font!.Size = 30;
    }
    #endregion
}