#!/bin/sh

. ./.env

curl \
	-H "X-Shopify-Access-Token: ${SHOP_ACCESS_TOKEN}" \
	-X GET "https://freshcask-dev.myshopify.com/admin/api/2022-10/events.json?since_id=164748010" 
