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
        public RelayCommand GetDisplayTracksCommand { get; }
        
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

        private Playlist? _selectedPlaylist;
        public Playlist? SelectedPlaylist
        {
            get { return _selectedPlaylist; }
            set {
                _selectedPlaylist = value;
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
           
            AllTracks = GetAllTracks();

            using var context = new EveryloopContext();
            PlayLists =GetPlaylists();
            if (SelectedPlaylist != null)
            {
                DisplayTracks = GetTracks(PlaylistIdentifier);
                
            }else if( SelectedPlaylist == null )
            {
                DisplayTracks = GetTracks(PlaylistIdentifier);
                
            }
            
            //PlaylistIdentifier = SelectedPlaylist.PlaylistId;
             
           // DisplayTracks = GetTracks(SelectedPlaylist.PlaylistId);
            
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
                
                var query = context.PlaylistTracks
                    .FirstOrDefault(pt => pt.TrackId == SelectedTrack.TrackId && pt.PlaylistId == SelectedPlaylist.PlaylistId);

                if (query == null)
                {
                    
                    var newTrack = new PlaylistTrack
                    {
                        TrackId = SelectedTrack.TrackId,
                        PlaylistId = SelectedPlaylist.PlaylistId
                    };

                   
                    context.PlaylistTracks.Add(newTrack);

                    
                    context.SaveChanges();

                    
                    DisplayTracks = GetTracks(SelectedPlaylist.PlaylistId);
                }
                else
                {
                    MessageBox.Show("The selected track is already in the playlist.", "Duplicate Track", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add the track to the playlist. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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



    }
}
