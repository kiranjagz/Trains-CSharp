Trains by Michael Graf
Written in C# w/ Visual C# 2010 Express

I basically took every chance I could to try fun things with this project and to showcase bits that weren't entirely necessary but also fun to do.

One assumption I made; the text of the email said that stops would be from letters A-D but the sample data had A-E ... I assumed A-E were valid. You may want to update the email if this is a typo.

The main system starts by instantiating a new SampleData object. This object represents the sample data and questions found within the email. You can replace the file loaded by the TestFileReader if you like, or just use the default sample data. 

Next we utilize a builder class (GraphBuilder) to build our Graph object.

Next, we instantiate a Finder object and tell it work over the graph we've built. The finder object implements most of the real work and logic of the project. Finder.DoJob(...) switches on an enumeration allowing it to know which kind of job to perform on the test data Tuple. Route Distance calculations are done in a fairl straight forward procedure, simply follow the routes as given. 
Max Three stops and Exactly four stops utilize a function which recursively builds all routes and selects the one's it'd like to return.
The Shortest Route and shortest cycle utilize another class "Dijkstra" we utilize Dijkstra's algorithm (slightly modified) to allow us to find the shortest path and shortest cycle. Finally, When calculating routes less than 30 we shoudl have used the bellman ford algorithm, but alas I was short on time. I realized that because I was able to recursively generate all routes of N stops, a 29 cost route must be < 30/Min edge stops long. For example, if the minimum edge length is 9 then the maximum number of stops is 3 (3x9 = 27). That is there is some ceiling on the stop count in which all 29 cost routes must exist. I simply find all 29/Min Edge routes, filter out those which have a cost > 29, and filter out those which are not cycles. If we have too many users of the kiwiland rail, we can refactor to implement the more efficient algorithm (or scale out with machines calculating routes :P ). 

finally we return the answer and the main reads the sample data tuple to see what the expected answer was. In theory I could have used NUnit and had the sample data and expected values as my test cases. Maybe next time :). 

Im not sure what else to say about the design or how it all works. If you have any questions, ask away!

