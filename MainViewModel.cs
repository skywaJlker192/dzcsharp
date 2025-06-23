using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace FavoriteMoviesApp
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _newMovieName;
        private Movie _selectedMovie;

        public ObservableCollection<Movie> AllMovies { get; set; } = new ObservableCollection<Movie>();
        public ObservableCollection<Movie> FavoriteMovies { get; set; } = new ObservableCollection<Movie>();
        public string NewMovieName
        {
            get => _newMovieName;
            set
            {
                _newMovieName = value;
                OnPropertyChanged();
            }
        }

        public Movie SelectedMovie
        {
            get => _selectedMovie;
            set
            {
                _selectedMovie = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddMovieCommand { get; }
        public ICommand AddToFavoritesCommand { get; }
        public ICommand RemoveFromFavoritesCommand { get; }

        public MainViewModel()
        {
            AddMovieCommand = new RelayCommand(AddMovie, CanAddMovie);
            AddToFavoritesCommand = new RelayCommand(AddToFavorites, CanAddToFavorites);
            RemoveFromFavoritesCommand = new RelayCommand(RemoveFromFavorites, CanRemoveFromFavorites);
        }

        private void AddMovie(object parameter)
        {
            if (!string.IsNullOrWhiteSpace(NewMovieName))
            {
                AllMovies.Add(new Movie { Name = NewMovieName });
                NewMovieName = string.Empty;
            }
        }

        private bool CanAddMovie(object parameter) => !string.IsNullOrWhiteSpace(NewMovieName);

        private void AddToFavorites(object parameter)
        {
            if (parameter is Movie movie && !FavoriteMovies.Contains(movie))
            {
                FavoriteMovies.Add(movie);
            }
        }

        private bool CanAddToFavorites(object parameter) => parameter is Movie movie && !FavoriteMovies.Contains(movie);

        private void RemoveFromFavorites(object parameter)
        {
            if (parameter is Movie movie)
            {
                FavoriteMovies.Remove(movie);
            }
        }

        private bool CanRemoveFromFavorites(object parameter) => parameter is Movie;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    // Реализация ICommand
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object parameter) => _execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
