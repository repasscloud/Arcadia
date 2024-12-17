#!/usr/bin/env zsh

mlnet classification \
    --dataset "intent_data.csv" \
    --label-col Label \
    --has-header true \
    --name IntentModelDev02 \
    --train-time 60 \
    --separator "\t"