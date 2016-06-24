BRANCH=$1

DIR=$PWD

if [ -z "$BRANCH" ]; then
    BRANCH=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')
fi

if [ -z "$BRANCH" ]; then
    BRANCH="master"
fi

echo "Branch: $BRANCH"

# If the .tmp/gitdb directory exists then remove it
if [ -d ".tmp/gitdb" ]; then
    rm .tmp/gitdb -rf
fi

git clone https://github.com/CompulsiveCoder/gitdb-cs.git .tmp/gitdb --branch $BRANCH
cd .tmp/gitdb && \
sh init-build.sh && \
cd $DIR && \
rm .tmp/gitdb -rf
