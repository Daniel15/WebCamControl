#!/bin/sh
set -ex

if ! command -v xgettext; then
	echo "Please install gettext"
	exit 1
fi

# Strings from .cs files
csharp_potfile=$(mktemp)
dotnet tool run GetText.Extractor --unixstyle --aliasgetstring '_' --source ../ --target "$csharp_potfile"

# Strings from .blp files
blp_file_list=$(mktemp)
blp_potfile=$(mktemp)
find ../src/ -name "*.blp" > "$blp_file_list"
xgettext --from-code=UTF-8 --keyword=_ -C --add-comments --files-from "$blp_file_list" --output "$blp_potfile" 

msgcat --use-first -o messages.pot "$csharp_potfile" "$blp_potfile" 
rm "$csharp_potfile" "$blp_potfile" "$blp_file_list"

# Normalize paths so they're relative to the src/ directory.
sed -Ei 's|(#: ).*/(WebCamControl\..*)|\1\2|' messages.pot
