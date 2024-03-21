# AviaSearcher
With docker compose 
3 microservices + redis + postgresql ( 5 containers)
Auth- Identity has 2 methods : RegisterOrUpdateUser and Login.
After successfull login you get Bearer token that you can use in all microsrvices.

AviaData -service with all data of Flights.To simplify code both source is in that project in diffferent controllers.

AviaSercher -service aggregator by http get data from AviaData and set reservation to choosen flight.


