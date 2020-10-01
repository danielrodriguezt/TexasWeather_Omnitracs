# TexasWeather_Omnitracs
Exercise for Omnitracs
This program reads Texas weather every 5 minutes
The API used for this purpose was Clima Cell (with the restriction of 100 request per day, so once you achieve this number you need wait 24 hours in oder the company give you another 100 reuquest)
Is necessary to install RestSharp package v106.11.7 (Install-Package RestSharp -Version 106.6.10)
NewtonSoft.Json v12.0.3
Due the restriction of 100 request per day I included a counter with only 10 repetitions per execution in order do not waste the 100 reuqest in only one run
