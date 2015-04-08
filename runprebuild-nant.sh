#!/bin/bash

cd `dirname "$0"`
mkdir bin 2>/dev/null
cp METAbolt/assemblies/* bin
cp plugins/METAbolt.Plugin.Alice/assemblies/*.dll bin

mono METAbolt/prebuild.exe /target nant

cp -f NullBuild.txt plugins/METAbolt.Plugin.Speech/RadSpeechWin/RadSpeechWin.dll.build
cp -f NullBuild.txt plugins/METAbolt.Plugin.Speech/RadSpeechMac/RadSpeechMac.dll.build
cp -f NullBuild.txt plugins/METAbolt.Plugin.Demo/METAbolt.Plugin.Demo.dll.build

if [ x$1 == xnant ]; then
    nant -buildfile:METAbolt.build
    RES=$?
    echo Build Exit Code: $RES

    if [ x$RES != x0 ]; then
	exit $RES
    fi

    if [ x$2 == xdist ]; then
        tar czvf METAbolt-latest.tgz bin
    fi
    
    exit $RES
fi
