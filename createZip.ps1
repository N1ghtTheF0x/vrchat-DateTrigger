$version = $args[0]
Compress-Archive -Force -Path ".\Packages\ntf.vrchat.datetrigger\*" -DestinationPath ".\DateTrigger-$version.zip"