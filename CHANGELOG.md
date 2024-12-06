# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [v1.0.0] - 2024-12-06

### Added

- Scripts to manage the deployment/setup [[#8](https://github.com/repasscloud/Arcadia/issues/8)]
- Initialization for database with init migration [[#7](https://github.com/repasscloud/Arcadia/issues/7)]
- Nuclear docker script to wipe everything out of the system [[#6](https://github.com/repasscloud/Arcadia/issues/6)]
- Run script to give menu selection when executing [[#4](https://github.com/repasscloud/Arcadia/issues/4)]
- Health check for Docker or Caddy to blazor [[#3](https://github.com/repasscloud/Arcadia/issues/3)]

### Changed

- Using volumes for certificates and XML security files [[#5](https://github.com/repasscloud/Arcadia/issues/5)]
- Updated db-init scripts [[#2](https://github.com/repasscloud/Arcadia/issues/2)]

### Removed

- Old scripts [issues/8](https://github.com/repasscloud/Arcadia/issues/8)
- Built-in CSS for Blazor [[#1](https://github.com/repasscloud/Arcadia/issues/1)]

## [v20241202] - 2024-12-02

### Added

- db-init scripts for Docker Compose [issue-2](https://github.com/repasscloud/Arcadia/issues/2)
- Blazor.Bootstrap added to UI [issue-1](https://github.com/repasscloud/Arcadia/issues/1) [9b01276](https://github.com/repasscloud/Arcadia/commit/9b012765fcd28a10ddbc1adff2d91e06451a517f)