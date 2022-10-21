#!/bin/sh

. ./.env

if [ "$DEBUG" = "1" ]; then
	DEBUG_OPT="-v"
else
	DEBUG_OPT=""
fi

curl \
	$DEBUG_OPT \
	-X POST \
	-F 'key1=value1' \
	-H 'x-functions-key: ${FUNCTION_KEY_WEBHOOK1}' \
	https://func-lab-myshopify.azurewebsites.net/api/webhook1 
echo ""
