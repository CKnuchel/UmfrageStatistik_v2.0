using Microsoft.AspNetCore.Components;

namespace Web.Pages;

public partial class Semester : ComponentBase
{
    public List<string> test = new List<string>();
    protected override void OnInitialized()
    {
        for(int i = 1; i <= 20; i++)
        {
            test.Add($"Ich bin die {i}. Zahl in der Liste.");
        }
    }
}