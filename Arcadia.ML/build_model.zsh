#!/usr/bin/env zsh

# mlnet classification \
#     --dataset "intent_master_data.csv" \
#     --label-col Label \
#     --has-header true \
#     --name IntentModelDev03 \
#     --train-time 600


mlnet classification \
    --dataset "intent_master_data.csv" \
    --label-col 2 \
    --has-header false \
    --name IntentModelDev03 \
    --train-time 600
