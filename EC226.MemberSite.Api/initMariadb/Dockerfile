FROM mariadb:10.4.8

# Install openssl
RUN apt-get update && \
    apt-get install -y openssl && \
    apt-get install -y vim && \
    rm -rf /var/lib/apt/lists/*

# 初始化脚本（如果有）
COPY init.sql /docker-entrypoint-initdb.d/
COPY my.cnf /etc/mysql/conf.d/my.cnf
# 设置环境变量
# 請於 docker build 時以 --build-arg MYSQL_ROOT_PASSWORD=... 覆寫
ARG MYSQL_ROOT_PASSWORD=change_me
ENV MYSQL_ROOT_PASSWORD=${MYSQL_ROOT_PASSWORD}
ENV MYSQL_DATABASE=ec226

# 暴露3306端口用于连接
EXPOSE 3306

# 可选：如果你希望容器启动时自动创建SSL证书，可以添加一个shell脚本到`/docker-entrypoint-initdb.d/`
# 这个脚本将在容器初始化时执行，可以用来生成新的SSL证书。
# 只有在你不打算提供自己的证书时才使用。
# COPY create-ssl-cert.sh /docker-entrypoint-initdb.d/
