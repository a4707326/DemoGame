@echo off

:: . = 當前路徑

:: 設置 proto 文件目錄
set PROTO_PATH=.

:: 設置輸出目錄
set OUTPUT_PATH=.

:: 設置 grpc 插件路徑
set GRPC_PLUGIN_PATH=C:\tools\grpc_csharp_plugin.exe

:: 設置 grpc 插件路徑
set PROTO_NAME=DemoGameService.proto

:: 生成 C# 和 gRPC 文件
protoc --proto_path=%PROTO_PATH% --csharp_out=%OUTPUT_PATH% --grpc_out=%OUTPUT_PATH% --plugin=protoc-gen-grpc=%GRPC_PLUGIN_PATH% %PROTO_NAME%

:: 暫停以查看輸出
pause
