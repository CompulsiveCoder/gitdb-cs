echo "Initializing project"
echo "  Dir: $PWD"

git submodule update --init --recursive

DIR=$PWD

cd lib
sh get-libs.sh
cd $DIR

cd mod/gitter-cs/
INIT_FILE="init.sh"
if [ ! -f "$INIT_FILE" ]; then
  echo "gitter init file not found: $PWD/$INIT_FILE. Did the submodule fail to initialize?"
else
  echo "gitter submodule found"
  sh init.sh
  cd $DIR
fi
