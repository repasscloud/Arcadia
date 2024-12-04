#!/usr/bin/env zsh

# Define common variables
CERT_PASSWORD="your-password"
DAYS_VALID=365
CERT_DIR="./certificates"

# Create the certificates directory if it doesn't exist
rm -rf $CERT_DIR > /dev/null 2>&1
mkdir -p $CERT_DIR

# Function to generate a certificate
generate_certificate() {
  local CERT_NAME=$1

  echo "Generating certificate for $CERT_NAME..."

  # Generate a private key
  openssl genrsa -out $CERT_DIR/$CERT_NAME.key 2048

  # Create a self-signed certificate
  openssl req -new -x509 -key $CERT_DIR/$CERT_NAME.key -out $CERT_DIR/$CERT_NAME.crt -days $DAYS_VALID -subj "/CN=localhost"

  # Convert to PFX format
  openssl pkcs12 -export -out $CERT_DIR/$CERT_NAME.pfx -inkey $CERT_DIR/$CERT_NAME.key -in $CERT_DIR/$CERT_NAME.crt -password pass:$CERT_PASSWORD

  echo "PFX certificate generated at: $CERT_DIR/$CERT_NAME.pfx"
}

# Function to create a Docker volume and copy a file into it
create_volume_and_add_file() {
  local VOLUME_NAME=$1
  local FILE_TO_COPY=$2

  echo "Creating Docker volume: $VOLUME_NAME..."
  docker volume create --name $VOLUME_NAME

  echo "Copying file $FILE_TO_COPY into volume $VOLUME_NAME..."
  docker run --rm \
    -v $VOLUME_NAME:/data \
    -v $(pwd)/$CERT_DIR:/certificates \
    alpine:3.20.3 sh -c "cp /certificates/$FILE_TO_COPY /data"

  echo "File $FILE_TO_COPY has been added to volume $VOLUME_NAME."
}

# Generate certificates for API and WebApp
generate_certificate "arcadia_api_cert"
generate_certificate "arcadia_webapp_cert"

# Create volumes and add respective files
create_volume_and_add_file "arcadia_api_dataprotection_keys" "arcadia_api_cert.pfx"
create_volume_and_add_file "arcadia_webapp_dataprotection_keys" "arcadia_webapp_cert.pfx"

echo "All certificates and volumes are set up successfully."
