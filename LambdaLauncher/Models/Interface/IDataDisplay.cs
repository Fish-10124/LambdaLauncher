namespace LambdaLauncher.Models.Interface;

public interface IDataDisplay
{
    /// <summary>
    /// 应通过Utils.ResourceLoader.GetString()方法从资源文件里获取
    /// </summary>
    public string DisplayText { get; init; }
}