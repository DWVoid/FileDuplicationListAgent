# FileDuplicationListAgent

![](https://img.shields.io/github/languages/code-size/DWVoid/FileDuplicationListAgent.svg?style=flat) ![](https://img.shields.io/github/repo-size/DWVoid/FileDuplicationListAgent.svg?style=flat)

A small tool for discovering duplicated files in a directory and list duplications as files

查找并以文件形式呈现文件夹内重复文件的工具

## Usage 使用方法

1. Command Line 命令行

Arguments 参数: TargetDirectory OutputDirectory 目标文件夹 输出文件夹

2. Interactive 交互式

Run the program directly and follow the instructions

直接运行程序并按照提示操作

## Output Format 输出格式

One file is written under the output directory for each group of duplication found

输出文件夹下每个文件一组重复的文件

File Name: TheNameOfTheFirstFileDiscoveredByTheProgram.txt

文件名：程序第一个找到的文件的名称.txt

Content: Name of duplicated files (exclude the one in the file name), one per line

内容：重复文件的文件名（文件名中的文件除外），每行一个文件名

## Hint 提示

Make sure your output directory either does not exist or being empty

请确保输出文件夹不包含任何文件或不存在
