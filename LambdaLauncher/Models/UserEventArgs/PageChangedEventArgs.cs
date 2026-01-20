using System;

namespace LambdaLauncher.Models.UserEventArgs;

public class PageChangedEventArgs : EventArgs
{
    public int CurrentPage { get; init; }

    #region Equals

    public override bool Equals(object? obj)
    {
        return Equals(obj as PageChangedEventArgs);
    }

    public bool Equals(PageChangedEventArgs? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return CurrentPage == other.CurrentPage;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(CurrentPage);
    }

    public static bool operator ==(PageChangedEventArgs? left, PageChangedEventArgs? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(PageChangedEventArgs? left, PageChangedEventArgs? right)
    {
        return !(left == right);
    }

    #endregion Equals
}