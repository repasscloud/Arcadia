#!/usr/bin/env zsh

# Function to generate a random string
generate_random_string() {
  local length=$1
  head -c "$length" /dev/urandom | base64 | tr -dc 'A-Za-z0-9' | head -c "$length"
}

# Define security levels for JWT secret
echo "Select security level for JWT Secret:"
echo "1) Normal (32 characters)"
echo "2) High (64 characters)"
echo "3) Extra High (128 characters)"
echo -n "Enter your choice (1-3): "
read security_level

case $security_level in
  1)
    JWT_SECRET=$(generate_random_string 32)
    ;;
  2)
    JWT_SECRET=$(generate_random_string 64)
    ;;
  3)
    JWT_SECRET=$(generate_random_string 128)
    ;;
  *)
    echo "Invalid choice. Exiting."
    exit 1
    ;;
esac

# Generate ISSUER and AUDIENCE
echo -n "Enter your application's domain or name (for ISSUER) (eg: \"https://default-issuer.example.com\"): "
read app_domain
echo -n "Enter your API's endpoint or name (for AUDIENCE): (eg: \"https://default-audience.example.com\"): "
read api_endpoint

JWT_ISSUER=${app_domain:-"https://default-issuer.example.com"}
JWT_AUDIENCE=${api_endpoint:-"https://default-audience.example.com"}

# Output to .env file
echo "JWTSETTINGS__SECRET=$JWT_SECRET" > jwt_settings.env
echo "JWTSETTINGS__ISSUER=$JWT_ISSUER" >> jwt_settings.env
echo "JWTSETTINGS__AUDIENCE=$JWT_AUDIENCE" >> jwt_settings.env

echo "JWT settings have been generated and saved to jwt_settings.env:"
cat jwt_settings.env
