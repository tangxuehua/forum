forum
=====

A simple forum implemented by enode framework.

How to run this forum
======================

先决条件
--------
Visual Studio 2013 + Sql Server 2005/2008;
确保5000,5001,6000,6001这四个端口已开；5000,5001这两个端口是论坛运行所需，6000,6001这两个端口是单元测试时用到；
如果你是win8，那么可以在控制面板->高级设置->入站规则->新建规则->选择端口->点击下一步->在文本框中输入你要开通的端口，一直下一步，最后输入一个名称，保存即可；

编译运行
--------
1. 打开Forum.sln解决方案，启用Nuget还原，编译解决方案；
2. 新建一个空的数据库，叫Forum；
3. 在新建的数据库中运行Forum.Infrastructure项目中的InstallDB.sql脚本，创建所有的表；
4. 修改Forum.BrokerService、Forum.CommandService、Forum.EventService、Forum.Web、Forum.Domain.Tests这些项目中配置文件中的connectionString；
5. 通过命令InstallUtil安装Forum.BrokerService、Forum.CommandService、Forum.EventService这三个Windows服务，安装完成后先启动BrokerService，再启动后两个；
   重要：由于这三个Windows服务是使用NT AUTHORITY\LOCAL SERVICE这个账号启动的，而该账号没有访问Sql Server的权限，
   所以connectionString中请不要以集成模式访问数据库，最好以sa账号来访问。
6. 将Forum.Web设置为启动项目，然后F5启动网站即可；

目前Forum.Web实现了注册、登录、注销三个功能，以及提供了两个Web API接口，提供查询帖子列表和帖子详情。
