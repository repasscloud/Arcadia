# Arcadia: The All-in-One IT Utility Toolbelt

## Overview

Arcadia is a comprehensive IT utility tool designed to be the ultimate all-in-one toolbelt for IT professionals. Whether you’re a systems administrator, developer, network engineer, or DevOps practitioner, Arcadia provides a suite of features tailored to streamline your workflows and solve common challenges.

This document outlines the capabilities of Arcadia and provides an overview of the various versions and their unique features.

---

### Key Features

Arcadia aims to simplify IT operations with a modular design, offering tools for:

- **Systems Monitoring and Diagnostics:** Quickly identify and resolve system bottlenecks or failures.
- **Network Management:** Tools for network scanning, troubleshooting, and performance tuning.
- **Deployment and Automation:** Simplify CI/CD pipelines and infrastructure management.
- **Utility Functions:** Everyday tasks like log parsing, file compression, and checksum generation.
- **Custom Integrations:** Easily integrate with existing IT ecosystems using APIs or extensions.

---

## Getting Started

1. Installation:
  - Download the latest version of Arcadia from the releases page.
  - Follow the installation guide included in the package.

2. Usage:
  - Run the CLI or Web API to explore the available tools.
  - Refer to the User Guide for detailed instructions on each feature.

3. Configuration:
  - Modify the `appsettings.json` file to match your environment.
  - Use configuration profiles for multi-environment support.

---

## Community and Support

- **Contribute:** Arcadia is an actively developed project. Contributions are welcome! Fork the repository and submit a pull request.
- **Support:** If you encounter issues, visit the Support Page or open a ticket on the GitHub repository.
- **Documentation:** Full documentation is available here.

### Normal Start

```sh
docker compose up --build"
```

### RESET DB START

```sh
docker compose up --build -e RESET_DB=true"
```
