forum
=====

A simple forum implemented by enode framework.

How to run this forum
======================

先决条件
--------
1. Visual Studio 2013 + Sql Server数据库;

编译运行
--------
1. 打开Forum.sln解决方案，启用Nuget还原，编译解决方案；
2. 新建一个空的数据库，叫Forum；
3. 在新建的数据库中运行Scripts目录下的InstallDB.sql脚本，创建所有的表；
4. 修改Forum.BrokerService、Forum.CommandService、Forum.EventService、Forum.Web、Forum.Domain.Tests这些项目的配置文件中的connectionString；
   确保能连接上面新建的Forum数据库；
5. 将Forum.Web设置为启动项目；
5. 依次启动Forum.BrokerService、Forum.CommandService、Forum.EventService这三个工程；
6. 然后F5启动网站即可；
7. 如果要管理论坛版块，需要注册一个名称为admin的账号，注册登录成功后，在右上角就有一个版块管理了；
