forum
=====

a forum implemented by enode framework.

Run Forum.Web steps:
1. Build the whole solution;
2. Start MongoDB server to make sure mongodb://localhost/ForumDB can be accessed.
3. Run SetupQueryDB.sql script to setup the query db.
4. Please check and modify the query db connection string in Global.asax.cs file. The QuerySideConnectionString const.
5. F5 to run the Forum.Web web project.
