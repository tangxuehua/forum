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
   确保能连接上上面新建的Forum数据库；
5. 1）通过命令InstallUtil安装Forum.BrokerService、Forum.CommandService、Forum.EventService这三个Windows服务；
   2）安装完之后，将每个服务的登陆账号设置为你当前系统的登陆用户，否则服务无法启动；因为服务启动时会访问上面建立的Forum数据库，而服务默认的启动账号(LocalService)权限不够无法访问数据库；
   3）安装完成后先启动BrokerService，再启动后两个；
6. 将Forum.Web设置为启动项目，然后F5启动网站即可；
