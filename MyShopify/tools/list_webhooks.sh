#!/bin/sh

. ./.env

if [ "$DEBUG" = "1" ]; then
	DEBUG_OPT="-v"
else
	DEBUG_OPT=""
fi

SHOP_URL="https://freshcask-dev.myshopify.com"
URL="${SHOP_URL}/admin/api/2022-10/webhooks.json?since_id=901431826"

curl \
	$DEBUG_OPT \
	-H "X-Shopify-Access-Token: ${SHOP_ACCESS_TOKEN}" \
	-X GET \
	$URL

