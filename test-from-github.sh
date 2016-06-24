echo "Testing gitdb project from github"
echo "  Current directory:"
echo "  $PWD"

BRANCH=$1

if [ -z "$BRANCH" ]; then
    BRANCH=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')
fi

if [ -z "$BRANCH" ]; then
    BRANCH="master"
fi

echo "  Branch: $BRANCH"

DIR=$PWD

# If the .tmp/gitdb directory exists then remove it
if [ -d ".tmp/gitdb" ]; then
    rm .tmp/gitdb -rf
fi

git clone https://github.com/CompulsiveCoder/gitdb-cs.git .tmp/gitdb --branch $BRANCH
cd .tmp/gitdb && \
sh init-build-test.sh && \
cd $DIR && \
rm .tmp/gitdb -rf
