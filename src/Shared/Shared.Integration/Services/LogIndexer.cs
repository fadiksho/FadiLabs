namespace Shared.Integration.Services;

public class LogIndexer
{
  private int _index = 0;
  public int Index { get { return _index; } }

  public void Increament()
  {
    _index++;
  }
}
