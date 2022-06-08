#!/usr/bin/env bash
docker build -t auth0-javascript-sample-01-login .
docker run --init -p 3002:3002 -it auth0-javascript-sample-01-login