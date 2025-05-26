CREATE TABLE chat (
  cID INTEGER PRIMARY KEY AUTOINCREMENT,
  idr VARCHAR(20),
  id1 INTEGER,
  id2 INTEGER,
  message TEXT,
  data TEXT,
  date VARCHAR(20),
  read INTEGER DEFAULT 0
);

CREATE TABLE room (
  rID VARCHAR(20),
  id1 INTEGER,
  id2 INTEGER
);

CREATE TABLE user (
  uID INTEGER PRIMARY KEY AUTOINCREMENT, 
  photo VARCHAR(100) DEFAULT 'profile.png',
  fname VARCHAR(20),
  lname VARCHAR(20),
  telp VARCHAR(20),
  email VARCHAR(20) UNIQUE,
  password VARCHAR(100)
);
