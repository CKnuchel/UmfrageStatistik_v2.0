using BlazorBootstrap;

namespace Common.ChartUtils;

public class PieChartOptionsGenerator
{
    #region Properties
    private PieChartOptions options { set; get; }
    #endregion

    #region Constructors
    public PieChartOptionsGenerator(string defaultTitle)
    {
        CreateDefaultChartOptions();
        this.options.Plugins.Title.Text = defaultTitle;
    }
    #endregion

    #region Publics
    public PieChartOptions GetOptions()
    {
        return this.options;
    }
    #endregion

    #region Privates
    private void CreateDefaultChartOptions()
    {
        this.options = new PieChartOptions
                       {
                           Responsive = true,
                           Plugins = new PieChartPlugins
                                     {
                                         Title = new ChartPluginsTitle
                                                 {
                                                     Display = true,
                                                     Font =
                                                     {
                                                         Size = 30
                                                     }
                                                 },
                                         Legend =
                                         {
                                             Position = "bottom"
                                         }
                                     }
                       };

        this.options.Plugins.Legend.Align = "left";
    }
    #endregion
}