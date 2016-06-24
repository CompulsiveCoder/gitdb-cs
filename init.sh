echo "Initializing project"
echo "  Dir: $PWD"

DIR=$PWD

cd lib
sh get-libs.sh
cd $DIR
