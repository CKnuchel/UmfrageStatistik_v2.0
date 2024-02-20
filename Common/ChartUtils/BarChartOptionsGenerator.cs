using BlazorBootstrap;
using Microsoft.IdentityModel.Tokens;

namespace Common.ChartUtils;

public class BarChartOptionsGenerator
{
    #region Fields
    private BarChartOptions options;
    #endregion

    #region Constructors
    /// <summary>
    /// Optionen für das BarChart mit den nötigsten Default Werten
    /// </summary>
    /// <param name="indexAxis">x = vertikal, y = Horizontal</param>
    /// <param name="xTitle">Titel der X-Achse</param>
    /// <param name="yTitle">Titel der Y-Achse</param>
    public BarChartOptionsGenerator(string indexAxis, string xTitle, string yTitle)
    {
        if(indexAxis.IsNullOrEmpty()) throw new ArgumentNullException(nameof(indexAxis));
        if(xTitle.IsNullOrEmpty()) throw new ArgumentNullException(nameof(xTitle));
        if(yTitle.IsNullOrEmpty()) throw new ArgumentNullException(nameof(yTitle));

        CreateDefaultChartOptions();

        options!.IndexAxis = indexAxis.ToLower();
        options.Scales.X!.Title!.Text = xTitle;
        options.Scales.Y!.Title!.Text = yTitle;
    }
    #endregion

    #region Publics
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
                      Interaction = new Interaction { Mode = InteractionMode.X },
                  };

        options.Scales.X!.Title!.Font!.Size = 24;
        options.Scales.X!.Title!.Display = true;

        options.Scales.Y!.Title!.Font!.Size = 24;
        options.Scales.Y!.Title!.Display = true;

        options.Plugins.Legend.Display = false;
    }
    #endregion
}