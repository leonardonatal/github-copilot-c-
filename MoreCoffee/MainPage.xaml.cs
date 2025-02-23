using MoreCoffee.ViewModels;

namespace MoreCoffee;

public partial class MainPage : ContentPage
{
    private readonly MainPageViewModel viewModel;
    
    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        this.viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        viewModel.OnNavigatedTo();
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
        viewModel.OnNavigatedFrom();
    }
}

