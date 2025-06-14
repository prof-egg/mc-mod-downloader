# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).


## [Unreleased]

### Added
- New internal variable "loader" for specifying what type of mod loader is used. 
- New internal variable "clear" for optionally specifying whether or not the client should clear all .jar files in the download dir before commencing.
- New internal variable "tmlPath" for optionally specifying the path of a .tml file.
- Able to automatically resolve mod dependencies.
  - New internal variable "updateTmlOnDependencyResolution" for optionally specifying if you want the client to spit out a new .tml file that includes the dependencies that it found in the original .tml file.
- If the client finds more than one tml file, give the user an option to choose which file to use as the config.


## [1.0.0] - 2025-02-27

_First release._


[1.0.0]: https://github.com/prof-egg/mc-mod-downloader/releases/tag/v1.0.0
