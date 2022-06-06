# About This Repository

This repository contains common code used by Ari.

# Changelog

## Version 0.0.10

- Move Debug() sink outside of #if DEBUG.

## Version 0.0.9

- UseSerilog() and CreateLogger() now accepts a path parameter.
- Added Debug() sink for logs when using debug mode in Visual Studio.

## Version 0.0.8

- Add Unit Testing Libarary.

## Version 0.0.7

- Add .ToAsync() to convert IEnumerable<Task<T>> to IAsyncEnumerable<T>.
- Github Workflow -- remove publish to Nuget on pull request.

## Version 0.0.6

- Add EnumerableExtensions, AddRange and RemoveRange extention methods.

## Version 0.0.5

- Add base class for INotifyPropertyChanged implementations.
- Remove JSON format type for logging (SeriLog already has its own serializer.)

## Version 0.0.4
- asagiv.common.mongodb: Add support for environment variables in MongoClient.
- add README.md