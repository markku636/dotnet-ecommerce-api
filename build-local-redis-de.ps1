$ipV4 = Test-Connection -ComputerName (hostname) -Count 1  | Select -ExpandProperty IPV4Address 
$localRedis=$ipV4.IPAddressToString
$bindLocalPort="30006:6379"

$containerName="ec226-redis"

# stop container
docker ps -a -f ancestor=$containerName --no-trunc -q | foreach-object { docker stop $_ }
docker ps -a -f name=$containerName --no-trunc -q | foreach-object { docker stop $_ }

# remove container
docker ps -a -f ancestor=$containerName* --no-trunc -q | foreach-object { docker rm -f $_ }
docker ps -a -f name=$containerName* --no-trunc -q | foreach-object { docker rm -f $_ }

# pull & run 
docker pull redis
docker run --name $containerName -p $bindLocalPort -d redis
pause 


