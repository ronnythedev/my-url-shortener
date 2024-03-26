# my-url-shortener

WebApi to shorten large urls.

Build with `VSCode` and the `C# DevKit` extension.

Since Docker projects are not supported by `C# DevKit` in `VSCode`, `Dockerfile` might look different than the ones created from VS; as well as the `docker-compose.yml` file.

## Dependencies

You need the following global dependencies on your host machine.

- Dotnet SDK 8 https://dotnet.microsoft.com/en-us/download/dotnet/8.0
- docker https://docs.docker.com/get-docker/
- docker desktop https://docs.docker.com/desktop/release-notes/ (optional)
- task manager https://taskfile.dev/#/installation (optional)

## Why task manager?

To make easier the execution of Docker commands which are not supported natively in `VSCode`.

E.g: running `task boot` is the same as typing and running `docker-compose up -d --build`

For more commands (shortcuts) take a look at `Taskfile.yml`
