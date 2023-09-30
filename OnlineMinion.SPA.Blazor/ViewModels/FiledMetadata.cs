using System.Runtime.InteropServices;

namespace OnlineMinion.SPA.Blazor.ViewModels;

/// <summary>Strongly typed metadata for single editor viewmodel's field.</summary>
/// <param name="Value">Optional, should pe provided, when it is important to have as a metadata in form's UI.</param>
/// <param name="IsReadOnly"></param>
/// <typeparam name="TValue">Type of value of filed.</typeparam>
[StructLayout(LayoutKind.Auto)]
public readonly record struct FiledMetadata<TValue>(TValue? Value = default, bool IsReadOnly = false);
