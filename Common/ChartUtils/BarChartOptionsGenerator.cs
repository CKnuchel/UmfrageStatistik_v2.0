using BlazorBootstrap;
using Microsoft.IdentityModel.Tokens;

namespace Common.ChartUtils;

public class BarChartOptionsGenerator
{
    #region Fields
    private BarChartOptions _options;
    #endregion

    #region Constructors
    /// <summary>
    /// Optionen für das BarChart mit den nötigsten Default Werten
    /// </summary>
    /// <param name="strIndexAxis">x = vertikal, y = Horizontal</param>
    /// <param name="strTitleX">Titel der X-Achse</param>
    /// <param name="strTitleY">Titel der Y-Achse</param>
    /// <param name="bStacked">Gestappelte Balken</param>
    public BarChartOptionsGenerator(string strIndexAxis, string strTitleX, string strTitleY, bool bStacked = false)
    {
        if(strIndexAxis.IsNullOrEmpty()) throw new ArgumentNullException(nameof(strIndexAxis));

        CreateDefaultChartOptions();

        if(strIndexAxis.Contains('x')) _options!.Interaction = new Interaction { Mode = InteractionMode.X };
        if(strIndexAxis.Contains('y')) _options!.Interaction = new Interaction { Mode = InteractionMode.Y };

        _options!.IndexAxis = strIndexAxis.ToLower();
        _options.Scales.X!.Title!.Text = strTitleX;
        _options.Scales.Y!.Title!.Text = strTitleY;
        _options.Scales.X.Stacked = bStacked;
        _options.Scales.Y.Stacked = bStacked;
    }
    #endregion

    #region Publics
    public BarChartOptions GetOptions()
    {
        return _options;
    }
    #endregion

    #region Privates
    private void CreateDefaultChartOptions()
    {
        _options = new BarChartOptions
                   {
                       Responsive = true,
                       Interaction = new Interaction { Mode = InteractionMode.X },
                   };

        _options.Scales.X!.Title!.Font!.Size = 18;
        _options.Scales.X!.Title!.Display = true;

        _options.Scales.Y!.Title!.Font!.Size = 18;
        _options.Scales.Y!.Title!.Display = true;

        _options.Plugins.Legend.Display = false;
    }
    #endregion
}