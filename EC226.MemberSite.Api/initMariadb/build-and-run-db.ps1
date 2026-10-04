$dockerfile = './Dockerfile'
$imageName = "ec226-marine-db:latest"
$containerName = 'ec226-marine-db'
$port="33070:3306"
docker build -t $imageName . -f $dockerfile
docker run -d --name $containerName -p $port $imageName  
docker update $containerName --restart=always


PAUSE