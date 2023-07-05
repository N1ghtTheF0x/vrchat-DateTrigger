$version = $args[0]
Compress-Archive -Path ".\Packages\ntf.vrchat.datetrigger\*" -DestinationPath ".\DateTrigger-$version.zip"