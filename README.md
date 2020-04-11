# ASPNETCore-Quartz.NET
This is a example  for  ASP.NETCore intergate the Quartz.NET

![](https://imgkr.cn-bj.ufileos.com/2f49f94a-4dbd-4738-9bc6-ea0ba0f9e998.png)


##  Why make the program?
  - The native quartz.net simpleJobFactory just support （no param contructor） Job,
  But many job depend  other  components/services. 
  - The Quartz.Net provider the IJobFactory interface to support custom the  contruct process,
     So we intergare the ASP.NET Core DI with  the  IJobFactory to implement a elegant  timer web.
     
 ## Tip
 The web example use quartz.net to  out logs every 30 second.
 Please pay attention to the app logs. 
 
## Usage
 1. define your own  Job class 
 2. register the job and trigger   in  QuartzStart.
 3. regiser the job in asp.netcore DI framework in Transient mode.
