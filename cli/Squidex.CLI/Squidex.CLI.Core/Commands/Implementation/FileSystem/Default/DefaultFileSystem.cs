﻿// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

namespace Squidex.CLI.Commands.Implementation.FileSystem.Default;

public sealed class DefaultFileSystem(DirectoryInfo directory) : IFileSystem
{
    public string FullName => directory.FullName;

    public bool Readonly { get; init; }

    public Task OpenAsync()
    {
        return Task.CompletedTask;
    }

    public IFile GetFile(FilePath path)
    {
        string fullPath = GetFullPath(path);

        var relativePath = Path.GetRelativePath(directory.FullName, fullPath);

        return new DefaultFile(new FileInfo(fullPath), relativePath)
        {
            Readonly = Readonly,
        };
    }

    public IEnumerable<IFile> GetFiles(FilePath path, string extension)
    {
        var fullPath = GetFullPath(path);

        if (Directory.Exists(fullPath))
        {
            foreach (var file in Directory.GetFiles(fullPath, $"*{extension}", SearchOption.AllDirectories))
            {
                var fileInfo = new FileInfo(file);

                if (fileInfo.Exists && MatchsExtension(fileInfo.FullName, extension))
                {
                    var relativePath = Path.GetRelativePath(directory.FullName, fileInfo.FullName);

                    yield return new DefaultFile(fileInfo, relativePath)
                    {
                        Readonly = Readonly,
                    };
                }
            }
        }
    }

    private static bool MatchsExtension(string fullName, string extension)
    {
        if (extension == ".*")
        {
            return true;
        }

        return fullName.EndsWith(extension, StringComparison.OrdinalIgnoreCase);
    }

    private string GetFullPath(FilePath path)
    {
        return Path.Combine(directory.FullName, Path.Combine(path.Elements.ToArray()));
    }

    public void Dispose()
    {
    }
}
