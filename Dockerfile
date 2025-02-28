# Build: docker build --tag shards-of-minax --progress auto .
# Run:   docker run -it -p 2593:2593 -v ./Backups:/opt/Shards-of-Minax/Backups -v ./Client_files:/opt/Shards-of-Minax/Client_files -v ./Logs:/opt/Shards-of-Minax/Logs -v ./Saves:/opt/Shards-of-Minax/Saves shards-of-minax

# Tested on mono:6.12.0.182
FROM mono:6.12

# Save time
RUN echo "force-unsafe-io" > /etc/dpkg/dpkg.cfg.d/02apt-speedup
# Save space
RUN echo "Acquire::http {No-Cache=True;};" > /etc/apt/apt.conf.d/no-cache
# Add the zlib1g-dev package
RUN echo "deb http://security.debian.org/debian-security buster/updates main" >> /etc/apt/sources.list
RUN apt-get -qq update && apt-get -qq --yes install zlib1g-dev

# Take only what the server needs
COPY ./Config/ /opt/Shards-of-Minax/Config/
COPY ./Data/ /opt/Shards-of-Minax/Data/
COPY ./RevampedSpawns/ /opt/Shards-of-Minax/RevampedSpawns/
# If you'd like to be able to modify the scripts without having to rebuild the image, then remove the copy of the scripts
# and bind mount your scripts folder from the host - like the Logs & Saves.
COPY ./Scripts/ /opt/Shards-of-Minax/Scripts/
COPY ./Server/ /opt/Shards-of-Minax/Server/
COPY ./Spawns/ /opt/Shards-of-Minax/Spawns/
COPY ./Ultima/ /opt/Shards-of-Minax/Ultima/
COPY ./VitaNexCore/ /opt/Shards-of-Minax/VitaNexCore/
COPY ./*.config /opt/Shards-of-Minax/
COPY ./*.dll /opt/Shards-of-Minax/
COPY ./*.exe /opt/Shards-of-Minax/

WORKDIR /opt/Shards-of-Minax

CMD ["mono", "./ServUO.exe"]

# Port is defined in "Config/Server.cfg"
EXPOSE 2593/tcp
