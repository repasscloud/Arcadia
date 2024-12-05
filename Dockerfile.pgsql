# syntax=docker/dockerfile:1

# Create a stage for building the application.
FROM --platform=$BUILDPLATFORM postgres:17.2-alpine3.20

# Install necessary packages for locale support
RUN apk add --no-cache \
    bash \
    musl-locales \
    musl-locales-lang

# Set locale environment variables
ENV LANG=en_US.UTF-8 \
    LANGUAGE=en_US.UTF-8 \
    LC_ALL=en_US.UTF-8

# Persist locale settings
RUN echo 'export LANG=en_US.UTF-8' >> /etc/profile && \
    echo 'export LANGUAGE=en_US.UTF-8' >> /etc/profile && \
    echo 'export LC_ALL=en_US.UTF-8' >> /etc/profile
