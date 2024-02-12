using BlazorBootstrap;

namespace Common.ChartUtils;

public class BarChartOptionsGenerator
{
    #region Fields
    private BarChartOptions options;
    #endregion

    #region Constructors
    public BarChartOptionsGenerator(string indexAxis, string xTitle, string yTitle)
    {
        CreateDefaultChartOptions();
        options.IndexAxis = indexAxis.ToLower();
        options.Scales.X.Title.Text = xTitle;
        options.Scales.Y.Title.Text = yTitle;
    }

    public BarChartOptions GetOptions()
    {
        return options;
    }
    #endregion

    #region Privates
    private void CreateDefaultChartOptions()
    {
        options = new BarChartOptions
                  {
                      Responsive = true,
                      Interaction = { Mode = InteractionMode.X },
                  };

        options.Scales.X.Title.Font!.Size = 24;
        options.Scales.X.Title.Display = true;

        options.Scales.Y.Title.Font!.Size = 24;
        options.Scales.Y.Title.Display = true;

        options.Plugins.Legend.Display = false;
    }

    #endregion
}