Param($path = "output")

New-Item -ItemType d -Force $path -ErrorAction SilentlyContinue
src\.nuget\nuget.exe pack src\navigation-routes-mvc4.nuspec -o $path
$o= resolve-path $path

"To test these packages add a nuget source (in visual studio) to:"
"$o" 
" "
$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")