using Laboration2_Music.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

           
            
            if (SelectedPlaylist == null)
            {
                DisplayTracks = GetTracks(1);
            }else if(SelectedPlaylist != null)
            {

            DisplayTracks = GetTracks(SelectedPlaylist.PlaylistId);
            }
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
            /*            int pLId = 3;
                        using var context = new EveryloopContext();

                        var playListTracksQuery = context.PlaylistTracks;
                        var tracksQuery = context.Tracks;

                        ObservableCollection<PlaylistTrack> AllPlaylistTracks = new ObservableCollection<PlaylistTrack>(playListTracksQuery.ToList());
                        ObservableCollection<Track> AllTracks = new ObservableCollection<Track>(tracksQuery.ToList());

                        var filteredTracks = new ObservableCollection<Track>(
                            AllTracks.Join(
                                AllPlaylistTracks.Where(pt => pt.PlaylistId == playlistId),
                                track => track.TrackId,
                                playlistTrack => playlistTrack.TrackId,
                                (track, _) => track 
                            )
                        );
                        return filteredTracks;*/

            using var context = new EveryloopContext();

            ObservableCollection<PlaylistTrack> fpt = new ObservableCollection<PlaylistTrack>(context.PlaylistTracks.Where(pt=> pt.PlaylistId == SelectedPlaylist.PlaylistId).ToList());

            ObservableCollection<Track> filteredTracks = new ObservableCollection<Track>
                (
                from t in AllTracks
                where (t.TrackId == fpt.TrackId  )
                select t


                ); ;

            
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
