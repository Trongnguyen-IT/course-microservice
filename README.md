# course-microservice

docker-compose -f .\docker-compose.yml -f .\docker-compose.override.yml up -d
docker-compose -f .\docker-compose.yml -f .\docker-compose.override.yml down -d

# mongo

docker run -d -p 27017:27017 --name shopping-mongo mongo
docker logs -f shopping-mongo
docker exec -it shopping-mongo /bin/bash
ls: list information
mongosh
show dbs
use CatalogDb
db.createCollection('Products')
db.Products.insertMany(
	[
		{
			"Name": "Asus Laptop",
			"Category": "Computers",
			"Summary": "Summary",
			"Description": "Description",
			"ImageFile": "ImageFile",
			"Price": 54.93
		},
		{
			"Name": "HP Laptop",
			"Category": "Computers",
			"Summary": "Summary",
			"Description": "Description",
			"ImageFile": "ImageFile",
			"Price": 84.93
		}
	]
)

db.Products.find({}).pretty()


# redis

docker pull redis
docker run -d -p 6379:6379 --name aspnetrun-redis redis
docker logs -f aspnetrun-redis
docker exec -it aspnetrun-redis /bin/bash