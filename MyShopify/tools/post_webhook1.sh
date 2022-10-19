#!/bin/sh

. ./.env

curl \
	-vv \
	-X POST \
	-F 'key1=value1' \
	-H 'x-functions-key: ${FUNCTION_KEY_WEBHOOK1}' \
	https://func-lab-myshopify.azurewebsites.net/api/webhook1 
echo ""
