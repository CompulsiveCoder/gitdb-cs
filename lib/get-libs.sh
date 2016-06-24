echo "Getting library files"
echo "  Dir: $PWD"

LIB_DIR=$PWD

PARENT_DIR=../../..
PARENT_LIB_DIR=$PARENT_DIR/lib

echo "Parent libs directory: $PARENT_LIB_DIR"

if [ -f $PARENT_LIB_DIR/nuget.exe ]; then
   cd $PARENT_LIB_DIR
   echo "Copying libs from parent project"
   echo "  Dir: $PWD"
   cp -v nuget.exe $LIB_DIR/
   for d in */ ; do
    cp -vr $d $LIB_DIR/$d
   done

   cd $DIR
fi

NUGET_FILE="nuget.exe"

if [ ! -f "$NUGET_FILE" ];
then
   wget http://nuget.org/nuget.exe
fi

mono nuget.exe install newtonsoft.json -version 8.0.3
mono nuget.exe install nunit -version 2.6.4
mono nuget.exe install nunit.runners -version 2.6.4
