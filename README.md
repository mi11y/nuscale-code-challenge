# nuscale-code-challenge

Project uses the following third party packages:
- Xceed WPF extensions
- MySqlConnector

To use a database instead of a mocked data store:
1. Open `BookService.cs` and modify the `US_DB` constant to true.
2. Replace the `CONNECTION_STRING` constant with your connection string. Change the hostname, user, password and database to your use case.

I have created a mariaDB hosted on Digital Ocean with a schema as follows:
```
MariaDB [inventory]> SHOW COLUMNS FROM books;
+------------+---------+------+-----+---------+----------------+
| Field      | Type    | Null | Key | Default | Extra          |
+------------+---------+------+-----+---------+----------------+
| book_id    | int(11) | NO   | PRI | NULL    | auto_increment |
| author     | text    | NO   |     | NULL    |                |
| title      | text    | NO   |     | NULL    |                |
| page_count | int(11) | NO   |     | 0       |                |
+------------+---------+------+-----+---------+----------------+
```
