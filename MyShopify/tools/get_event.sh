#!/bin/sh

. ./.env

EVENT_ID="99755211522352"

curl \
	-H "X-Shopify-Access-Token: ${SHOP_ACCESS_TOKEN}" \
	-X GET "https://freshcask-dev.myshopify.com/admin/api/2022-10/events/${EVENT_ID}.json"
 
 
