using PiWord.Models;
using Label = Microsoft.Maui.Controls.Label;

namespace PiWord.Pages;

public class HomePage : ContentPage
{
    private static readonly Dictionary Dictionary = new();

    private readonly Label _currentWordLabel;
    private readonly Label _currentWordDefinition;
    private readonly CollectionView _favoriteCollection = new();

    private Word _word = Dictionary.Random();
    private readonly List<Word> _favorites = new();

    public HomePage()
    {
        var container = new ContentView();

        var verticalStackLayout = new VerticalStackLayout
        {
            Spacing = 25,
            Padding = 30,
            VerticalOptions = LayoutOptions.Center
        };
        container.Content = verticalStackLayout;

        _currentWordLabel = new Label
        {
            Text = _word.Value.ToUpper(),
            FontSize = 30,
            FontAttributes = FontAttributes.Bold,
            Padding = 25,
            HorizontalOptions = LayoutOptions.Center,
        };
        SemanticProperties.SetHeadingLevel(_currentWordLabel, SemanticHeadingLevel.Level1);
        verticalStackLayout.Children.Add(_currentWordLabel);

        _currentWordDefinition = new Label
        {
            Text = _word.Definition,
            FontSize = 12,
            FontAttributes = FontAttributes.Italic,
            HorizontalOptions = LayoutOptions.Center
        };
        verticalStackLayout.Children.Add(_currentWordDefinition);

        // Buttons

        var horizontalStackLayout = new HorizontalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center
        };

        var favoriteButton = new Button
        {
            Text = "Like",
            HorizontalOptions = LayoutOptions.Center
        };
        SemanticProperties.SetHint(favoriteButton, "Add this my favorite.");
        horizontalStackLayout.Children.Add(favoriteButton);
        favoriteButton.Clicked += OnFavorite;

        var nextButton = new Button
        {
            Text = "Next",
            HorizontalOptions = LayoutOptions.Center
        };
        nextButton.SetValue(SemanticProperties.HintProperty, "Next word");
        horizontalStackLayout.Children.Add(nextButton);
        nextButton.Clicked += OnNext;

        verticalStackLayout.Children.Add(horizontalStackLayout);

        // List of favorites 

        if (_favorites.Count > 0)
        {
            _favoriteCollection.ItemsSource = _favorites;
        }
        else
        {
            var emptyLabel = new Label
            {
                Text = "No favorite!"
            };
            _favoriteCollection.EmptyView = emptyLabel;
        }

        verticalStackLayout.Children.Add(_favoriteCollection);

        Content = container;
    }

    private void OnNext(object sender, EventArgs e)
    {
        _word = Dictionary.Random();
        _currentWordLabel.Text = _word.Value.ToUpper();
        _currentWordDefinition.Text = !string.IsNullOrEmpty(_word.Definition) ? _word.Definition : string.Empty;

        SemanticScreenReader.Announce(_currentWordLabel.Text);
        SemanticScreenReader.Announce(_currentWordDefinition.Text);
    }

    private void OnFavorite(object sender, EventArgs e)
    {
        if (_favorites.Contains(_word))
            _favorites.Remove(_word);
        else 
            _favorites.Add(_word);

        _favoriteCollection.ItemsSource = _favorites;
        if (_favorites.Count > 0) 
            SemanticScreenReader.Announce(_favoriteCollection.ItemsSource.GetEnumerator().ToString() ?? throw new InvalidOperationException());
    }
}

