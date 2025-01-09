using Laboration2_Music.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Laboration_2_Music.ViewModel
{
    class MusicAppViewModel:ViewModelBase
    {
        
        public RelayCommand InsertTrackCommand { get; }
        public RelayCommand RemoveTrackCommand { get; }
        public RelayCommand InsertPlaylistCommand { get; }
        public RelayCommand RemovePlaylistCommand { get; }  
        public RelayCommand GetDisplayTracksCommand { get; }
        public RelayCommand OpenCreatePlaylistCommand { get; }
      
        
        private ObservableCollection<Playlist> _playLists;
        public ObservableCollection<Playlist> PlayLists
        {
            get { return _playLists; }
            set 
            { 
                _playLists = value;
                OnPropertyChanged();
            }
        }

        private string _insertedPlaylistName;

        public string InsertedPlaylistName
        {
            get { return _insertedPlaylistName; }
            set 
            { 
                _insertedPlaylistName = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<PlaylistTrack> _playlistTracks;

        public ObservableCollection<PlaylistTrack> PlaylistTracks
        {
            get { return _playlistTracks; }
            set 
            { 
                _playlistTracks = value;
                OnPropertyChanged();
            }
        }

        private Track _insertedTrack;

        public Track InsertedTrack
        {
            get { return _insertedTrack; }
            set { 
                _insertedTrack = value;
                OnPropertyChanged();
            }
        }


        private Playlist _selectedPlaylist;
        public Playlist SelectedPlaylist
        {
            get { return _selectedPlaylist; }
            set
            {
                if (_selectedPlaylist != value)
                {
                    _selectedPlaylist = value;
                    OnPropertyChanged();

                    
                    if (_selectedPlaylist != null)
                    {
                        DisplayTracks = GetTracks(_selectedPlaylist.PlaylistId);
                    }
                    else
                    {
                        
                        DisplayTracks = new ObservableCollection<Track>();
                    }
                }
            }
        }
        private int _tester;

        public int Tester
        {
            get { return _tester; }
            set { 
                _tester = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Track> _allTracks;

        public ObservableCollection<Track> AllTracks
        {
            get { return _allTracks; }
            set 
            {
                _allTracks = value;
                OnPropertyChanged();
            }
        }

        private int _playlistIdentifier = 3;
        
        public int PlaylistIdentifier
        {
            get { return _playlistIdentifier; }
            set
            {
                _playlistIdentifier = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<Track> _displayTracks;
        
        public ObservableCollection<Track> DisplayTracks
        {
            get { return _displayTracks; }
            set
            {
                _displayTracks = value;
                OnPropertyChanged();
            }
        }

        private Track _selectedTrack;

        public Track SelectedTrack
        {
            get { return _selectedTrack; }
            set 
            { 
                _selectedTrack = value;
                OnPropertyChanged();
            }
        }


        public MusicAppViewModel() 
        {

            
            RemoveTrackCommand = new RelayCommand(RemoveTrack);
            InsertTrackCommand = new RelayCommand(InsertTrack);
            RemovePlaylistCommand = new RelayCommand(RemovePlaylist);
            OpenCreatePlaylistCommand = new RelayCommand(OpenCreatePlaylist);
            InsertPlaylistCommand = new RelayCommand(InsertPlaylist);
            using var context = new EveryloopContext();
            AllTracks = GetAllTracks();
            PlaylistIdentifier = Selector(SelectedPlaylist);

        
            DisplayTracks = GetTracks(PlaylistIdentifier);
            PlayLists = GetPlaylists();
            
        }

        public int Selector(Playlist playlist)
        {
            int selector = 0;
            if (playlist == null)
            {
                selector = 3;
            }else if (playlist != null) 
            {
                selector = playlist.PlaylistId;
            }

            return selector;

        }

        public static ObservableCollection<Track> GetAllTracks()
        {
            using var context = new EveryloopContext();

            var query = context.Tracks;

            query.ToList();

            ObservableCollection<Track> tracks = new ObservableCollection<Track>(query);
            return tracks;
        }

        public static ObservableCollection<Playlist> GetPlaylists()
        {
            using var context = new EveryloopContext();

            var query = context.Playlists;

            query.ToList();

            ObservableCollection<Playlist> playlists = new ObservableCollection<Playlist>(query);
            return playlists;
        }

        public ObservableCollection<Track> GetTracks(int playlistId)
        {

            using var context = new EveryloopContext();

      
            var playlistTracks = context.PlaylistTracks
                .Where(pt => pt.PlaylistId == playlistId)
                .ToList();
            var trackIds = playlistTracks.Select(pt => pt.TrackId).ToList();
            var tracks = context.Tracks
                .Where(t => trackIds.Contains(t.TrackId))
                .ToList();

          
            return new ObservableCollection<Track>(tracks);
        }



        public void GetDisplayTracks(object sender, RoutedEventArgs e)
        {
            using var context = new EveryloopContext();


            var playlistTracks = context.PlaylistTracks
                .Where(pt => pt.PlaylistId == SelectedPlaylist.PlaylistId)
                .ToList();
            var trackIds = playlistTracks.Select(pt => pt.TrackId).ToList();
            var tracks = context.Tracks
                .Where(t => trackIds.Contains(t.TrackId))
                .ToList();
            DisplayTracks = new ObservableCollection<Track>(tracks);
        }




            public  void InsertTrack(object obj)
            {
            using var context = new EveryloopContext();

            try
            {
                if (InsertedTrack == null)
                {
                    MessageBox.Show("Please select a track and a playlist before adding.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var query = context.PlaylistTracks
                    .FirstOrDefault(pt => pt.TrackId == InsertedTrack.TrackId && pt.PlaylistId == SelectedPlaylist.PlaylistId);
                if (query == null)
                {
                    var newTrack = new PlaylistTrack
                    {
                        TrackId = InsertedTrack.TrackId,
                        PlaylistId = SelectedPlaylist.PlaylistId
                    };
                    context.PlaylistTracks.Add(newTrack);
                    context.SaveChanges();
                    DisplayTracks = GetTracks(SelectedPlaylist.PlaylistId);
                    MessageBox.Show("Track added to playlist successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("The selected track is already in the playlist.", "Duplicate Track", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add the track to the playlist. Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void RemoveTrack(object obj)
        {
            using var context = new EveryloopContext();
            try
            {
                var query = context.PlaylistTracks.Where(pt => pt.TrackId == SelectedTrack.TrackId && pt.PlaylistId == SelectedPlaylist.PlaylistId).FirstOrDefault();

                if (query != null)
                {
                    context.PlaylistTracks.Remove(query);

                    context.SaveChanges();
                }
                DisplayTracks = GetTracks(SelectedPlaylist.PlaylistId);
            }catch(Exception r)
            {
                MessageBox.Show("Failed to remove song, please try again");
                return;
            }
        }

        public void RemovePlaylist(object obj)
        {
            using var context = new EveryloopContext();

            try
            {
                var query = context.Playlists.Where(pl => pl.PlaylistId == SelectedPlaylist.PlaylistId).FirstOrDefault();
                var relatedTracks = context.PlaylistTracks.Where(pt => pt.PlaylistId == SelectedPlaylist.PlaylistId).ToList();
                context.PlaylistTracks.RemoveRange(relatedTracks);
                context.Playlists.Remove(query); 
                context.Playlists.Remove(query);
                context.SaveChanges();
                PlayLists = GetPlaylists();
            }catch(Exception ex)
            {
                MessageBox.Show($"Failed to remove playlist, Error: {ex.Message}, please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        public void InsertPlaylist(object obj)
        {
            using var context = new EveryloopContext();

            try
            {
                if (string.IsNullOrWhiteSpace(InsertedPlaylistName))
                {
                    MessageBox.Show("Please enter a name for the playlist before adding.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var existingPlaylist = context.Playlists.FirstOrDefault(pl => pl.Name == InsertedPlaylistName);
                if (existingPlaylist != null)
                {
                    MessageBox.Show("A playlist with this name already exists.", "Duplicate Playlist", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                var newPlaylist = new Playlist
                {
                    Name = InsertedPlaylistName
                };
              
                context.Playlists.Add(newPlaylist);
                
                context.SaveChanges();
                context.Entry(newPlaylist).Reload();
                InsertedPlaylistName = string.Empty;
                PlayLists = GetPlaylists();
                MessageBox.Show("Playlist added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add the playlist. Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void OpenCreatePlaylist(object obj)
        {
            var newWindow = new NewPlaylist();
            newWindow.Show();
            
        }

    }
}
