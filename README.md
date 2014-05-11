forum
=====

A simple forum implemented by enode framework.

How to run this forum
======================

先决条件
--------
Visual Studio 2013 + Sql Server 2005/2008

编译运行
--------
1. 打开Forum.sln解决方案，启用Nuget还原，编译解决方案；
2. 新建一个空的数据库，叫Forum；
3. 在新建的数据库中运行Forum.Infrastructure项目中的InstallDB.sql脚本，创建所有的表；
4. 修改Forum.BrokerService、Forum.CommandService、Forum.EventService、Forum.Web、Forum.Domain.Tests这些项目中配置文件中的connectionString；
5. 通过命令InstallUtil安装Forum.BrokerService、Forum.CommandService、Forum.EventService这三个Windows服务，安装完成后先启动BrokerService，再启动后两个；
6. 将Forum.Web设置为启动项目，然后F5启动网站即可；

目前Forum.Web实现了注册、登录、注销三个功能，以及提供了两个Web API接口，提供查询帖子列表和帖子详情。
