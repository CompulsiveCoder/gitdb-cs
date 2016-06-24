echo "Starting build for project"
echo "Dir: $PWD"

DIR=$PWD

cd mod/gitter-cs/
sh build.sh
cd $DIR

xbuild src/gitdb.sln /p:Configuration=Release
