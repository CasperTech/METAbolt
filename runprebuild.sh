#!/bin/bash

cd `dirname "$0"`
mkdir bin 2>/dev/null

mono METAbolt/prebuild.exe /target vs2010 /exclude plug_speech

if [ x$1 == xbuild ]; then
    xbuild /p:Configuration=Release METAbolt.sln
    RES=$?
    echo Build Exit Code: $RES

    if [ x$RES != x0 ]; then
	exit $RES
    fi

    if [ x$2 == xdist ]; then
        tar czvf METAbolt-latest.tgz bin
    fi
    
    exit $RES
else
    echo "Now run:"
    echo
    echo "xbuild METAbolt.sln"
fi
