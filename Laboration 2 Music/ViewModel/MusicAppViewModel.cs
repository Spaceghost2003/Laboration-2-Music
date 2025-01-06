using Laboration2_Music.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration_2_Music.ViewModel
{
    class MusicAppViewModel:ViewModelBase
    {

        public RelayCommand InsertTrackCommand { get; }

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


        public MusicAppViewModel() 
        {

            InsertTrackCommand = new RelayCommand(InsertTrack);



            using var context = new EveryloopContext();
            PlayLists =GetPlaylists();

            if(SelectedPlaylist != null)
            { 
            DisplayTracks = GetTracks(SelectedPlaylist.PlaylistId);
            }
            else if (SelectedPlaylist == null)
            {
                DisplayTracks = GetTracks(1);
            }
        }

        public static void TrackManager()
        {
            //skriv kod här efter lunch
        }

        public static ObservableCollection<Playlist> GetPlaylists()
        {
            using var context = new EveryloopContext();

            var query = context.Playlists;

            query.ToList();

            ObservableCollection<Playlist> playlists = new ObservableCollection<Playlist>(query);
            return playlists;
        }

        public static ObservableCollection<Track> GetTracks(int playlistId)
        {
            int pLId = 3;
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
            return filteredTracks;
        }

        public static void InsertTrack(object obj)
        {
            using var context = new EveryloopContext();

            var track = new Track
            {
              TrackId = 3
            };
            var query = context.PlaylistTracks.Where(pt => pt.TrackId == 3 && pt.PlaylistId == 1).FirstOrDefault();
            
            if (query != null)
            {
                context.PlaylistTracks.Remove(query);
            }

            context.SaveChanges();
        }

    }
}
