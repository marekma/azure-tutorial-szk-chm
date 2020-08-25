using Company.Function.Abstracts;
using System;

namespace Company.Function.Implementations
{
    class FileNameGenerator : IFileNameGenerator
    {
        public string ForJson()
        {
            return Guid.NewGuid().ToString() + ".json";
        }
    }
}
