#!/bin/sh
set -ex

if ! command -v msgfmt; then
	echo "Please install gettext"
	exit 1
fi

po_dir=$(dirname "$(realpath "$0")")
output_dir="$po_dir/../src/WebCamControl.Core/Locales"

echo $po_dir
echo $output_dir

for po_file in "$po_dir"/*.po; do
	lang=$(basename "$po_file" .po)
	out_dir="$output_dir/$lang/LC_MESSAGES"
	mkdir -p "$out_dir"
	msgfmt "$po_file" -o "$out_dir/webcamcontrol.mo" 
done
