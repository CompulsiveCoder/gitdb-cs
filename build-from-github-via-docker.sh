BRANCH=$1

if [ -z "$BRANCH" ]; then
    BRANCH=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')
fi

if [ -z "$BRANCH" ]; then
    BRANCH="master"
fi


docker run -it compulsivecoder/ubuntu-mono /bin/bash -c "curl https://raw.githubusercontent.com/CompulsiveCoder/gitdb/$BRANCH/build-from-github.sh | sh -s $BRANCH"
