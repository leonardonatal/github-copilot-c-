namespace MoreCoffee.ViewModels;

public interface INavigationAware
{
    void OnNavigatedTo();
    void OnNavigatedFrom();
}