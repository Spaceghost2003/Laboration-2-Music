using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Laboration_2_Music.Migrations
{
    /// <inheritdoc />
    public partial class addedPK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_playlist_track_PlaylistId",
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

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlaylistTracks",
                table: "PlaylistTracks",
                columns: new[] { "PlaylistId", "TrackId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_playlist_track_PlaylistId",
                schema: "music",
                table: "playlist_track",
                column: "PlaylistId");
        }
    }
}
