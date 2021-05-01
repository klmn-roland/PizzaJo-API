PIZZAJO - API DOC.
------------------

REQUEST METHOD - MYSQL PROCEDURE NAME - ARGUMENTS

functions' arguments (& their names) are matching with mysql procedures' arguments. (except api/food)

######## api/order

- GET select_all_order - args: none
- POST create_orderWithCustomer - args: 11pcs
- POST create_order_food - args: 3pcs
- POST set_orderStatus - args: 2pc
- DELETE delete_order - args: 1pc 

######## api/food

- GET select_all_{food} - args: 1pc -> food_group_id (1=pizza;2=hamburger;3=gyros)
	 > this will return the image in a base64 encoded string

######## api/address

- GET getAddress - args: none

######## api/admin

- POST set_admin - args: 2pcs
- GET none - args: none - GET request will return all username/password (TEMPORARY)

