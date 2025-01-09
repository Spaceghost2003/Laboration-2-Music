using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Laboration_2_Music.Migrations
{
    /// <inheritdoc />
    public partial class DeployMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_playlist_track_playlists",
                table: "PlaylistTracks");

            migrationBuilder.DropForeignKey(
                name: "FK_playlist_track_tracks",
                table: "PlaylistTracks");

            migrationBuilder.DropTable(
                name: "PlaylistTrack",
                schema: "music");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlaylistTracks",
                table: "PlaylistTracks");

            migrationBuilder.RenameTable(
                name: "PlaylistTracks",
                newName: "playlist_track",
                newSchema: "music");

            migrationBuilder.RenameIndex(
                name: "IX_PlaylistTracks_TrackId",
                schema: "music",
                table: "playlist_track",
                newName: "IX_playlist_track_TrackId");

            migrationBuilder.AlterColumn<int>(
                name: "PlaylistId",
                schema: "music",
                table: "playlists",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_playlist_track",
                schema: "music",
                table: "playlist_track",
                columns: new[] { "PlaylistId", "TrackId" });

            migrationBuilder.AddForeignKey(
                name: "FK_playlist_track_playlists_PlaylistId",
                schema: "music",
                table: "playlist_track",
                column: "PlaylistId",
                principalSchema: "music",
                principalTable: "playlists",
                principalColumn: "PlaylistId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_playlist_track_tracks_TrackId",
                schema: "music",
                table: "playlist_track",
                column: "TrackId",
                principalSchema: "music",
                principalTable: "tracks",
                principalColumn: "TrackId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_playlist_track_playlists_PlaylistId",
                schema: "music",
                table: "playlist_track");

            migrationBuilder.DropForeignKey(
                name: "FK_playlist_track_tracks_TrackId",
                schema: "music",
                table: "playlist_track");

            migrationBuilder.DropPrimaryKey(
                name: "PK_playlist_track",
                schema: "music",
                table: "playlist_track");

            migrationBuilder.RenameTable(
                name: "playlist_track",
                schema: "music",
                newName: "PlaylistTracks");

            migrationBuilder.RenameIndex(
                name: "IX_playlist_track_TrackId",
                table: "PlaylistTracks",
                newName: "IX_PlaylistTracks_TrackId");

            migrationBuilder.AlterColumn<int>(
                name: "PlaylistId",
                schema: "music",
                table: "playlists",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlaylistTracks",
                table: "PlaylistTracks",
                columns: new[] { "PlaylistId", "TrackId" });

            migrationBuilder.CreateTable(
                name: "PlaylistTrack",
                schema: "music",
                columns: table => new
                {
                    PlaylistsPlaylistId = table.Column<int>(type: "int", nullable: false),
                    TracksTrackId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistTrack", x => new { x.PlaylistsPlaylistId, x.TracksTrackId });
                    table.ForeignKey(
                        name: "FK_PlaylistTrack_playlists_PlaylistsPlaylistId",
                        column: x => x.PlaylistsPlaylistId,
                        principalSchema: "music",
                        principalTable: "playlists",
                        principalColumn: "PlaylistId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaylistTrack_tracks_TracksTrackId",
                        column: x => x.TracksTrackId,
                        principalSchema: "music",
                        principalTable: "tracks",
                        principalColumn: "TrackId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistTrack_TracksTrackId",
                schema: "music",
                table: "PlaylistTrack",
                column: "TracksTrackId");

            migrationBuilder.AddForeignKey(
                name: "FK_playlist_track_playlists",
                table: "PlaylistTracks",
                column: "PlaylistId",
                principalSchema: "music",
                principalTable: "playlists",
                principalColumn: "PlaylistId");

            migrationBuilder.AddForeignKey(
                name: "FK_playlist_track_tracks",
                table: "PlaylistTracks",
                column: "TrackId",
                principalSchema: "music",
                principalTable: "tracks",
                principalColumn: "TrackId");
        }
    }
}
