CREATE TABLE UserProfilePicUpld (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Email TEXT NOT NULL DEFAULT '',
    ProfileImage BLOB NULL,
    BackgroundImage BLOB NULL
);
