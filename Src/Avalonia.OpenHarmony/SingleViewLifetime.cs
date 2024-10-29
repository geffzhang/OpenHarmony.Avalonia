using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Embedding;

namespace Avalonia.OpenHarmony;

public class SingleViewLifetime : ISingleViewApplicationLifetime
{
    public EmbeddableControlRoot? Root;
    public SingleViewLifetime()
    {

    }
    private Control? _mainView;
    public Control? MainView 
    {
        get => _mainView;
        set
        {
            _mainView = value;
            Root!.Content = value;
        }
    }
}